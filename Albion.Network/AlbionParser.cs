using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Albion.Common;
using Albion.Network.Handlers;
using Albion.Network.Messages;
using PcapDotNet.Core;
using PcapDotNet.Packets;
using PhotonPackageParser;

namespace Albion.Network
{
    public class AlbionParser : PhotonParser, IDisposable
    {
        private readonly Dictionary<EventCodes, BaseHandler> _eventHandlers = new Dictionary<EventCodes, BaseHandler>();

        private readonly Dictionary<OperationCodes, BaseHandler> _operationHandlers =
            new Dictionary<OperationCodes, BaseHandler>();

        private readonly Dictionary<OperationCodes, BaseHandler> _requestHandlers =
            new Dictionary<OperationCodes, BaseHandler>();

        private Thread[] _treads;
        private CancellationTokenSource _cts;

        public void AddRequestHandler<T>(Action<T> action) where T : BaseOperation, new()
        {
            AddHandler(action, _requestHandlers);
        }

        public void AddOperationHandler<T>(Action<T> action) where T : BaseOperation, new()
        {
            AddHandler(action, _operationHandlers);
        }

        public void AddEventHandler<T>(Action<T> action) where T : BaseEvent, new()
        {
            AddHandler(action, _eventHandlers);
        }

        public void Dispose()
        {
            _cts?.Cancel();
        }

        protected override void OnEvent(byte code, Dictionary<byte, object> parameters)
        {
            if (code == 2) parameters.Add(252, (short) EventCodes.Move);

            var eventCode = ParseEventCode(parameters);

            Debug.WriteLine($"case EventCodes.{eventCode.ToString()}:");

            if (_eventHandlers.TryGetValue(eventCode, out var handler))
                handler.Run(parameters);
        }

        protected override void OnRequest(byte code, Dictionary<byte, object> parameters)
        {
            var operationCode = ParseOperationCode(parameters);

            Debug.WriteLine($"case OperationCodes.{operationCode.ToString()}: //request");

            if (_requestHandlers.TryGetValue(operationCode, out var handler))
                handler.Run(parameters);
        }

        protected override void OnResponse(byte code, short returnCode, string debugMessage,
            Dictionary<byte, object> parameters)
        {
            var operationCode = ParseOperationCode(parameters);

            Debug.WriteLine($"case OperationCodes.{operationCode.ToString()}:");

            if (_operationHandlers.TryGetValue(operationCode, out var handler))
                handler.Run(parameters);
        }

        private OperationCodes ParseOperationCode(Dictionary<byte, object> parameters)
        {
            if (!parameters.TryGetValue(253, out var value)) throw new AlbionException();

            return (OperationCodes) value;
        }

        private EventCodes ParseEventCode(Dictionary<byte, object> parameters)
        {
            if (!parameters.TryGetValue(252, out var value)) throw new AlbionException();

            return (EventCodes) value;
        }

        private static void AddHandler<T, TCode>(Action<T> action, IDictionary<TCode, BaseHandler> handlers)
            where T : BaseMessage<TCode>, new()
        {
            var obj = new T();
            var handler = new BaseHandler<T>(action);
            handlers.Add(obj.Code, handler);
        }

        public void Start()
        {
            _cts?.Cancel();

            var devices = LivePacketDevice.AllLocalMachine;
            _cts = new CancellationTokenSource();
            _treads = devices.Select(device =>
            {
                var token = _cts.Token;
                var tread = new Thread(() =>
                {
                    using (var communicator = device.Open(65536, PacketDeviceOpenAttributes.Promiscuous, 1000))
                    {
                        communicator.ReceivePackets(0, packet =>
                        {
                            token.ThrowIfCancellationRequested();
                            PacketHandler(packet);
                        });
                    }
                });
                tread.Start();
                return tread;
            }).ToArray();
        }

        private void PacketHandler(Packet packet)
        {
            var ip = packet.Ethernet.IpV4;
            var udp = ip.Udp;

            if (udp == null || udp.SourcePort != 5056 && udp.DestinationPort != 5056) return;
            try
            {
                ReceivePacket(udp.Payload.ToArray());
            }
            catch (Exception e)
            {
                //skip errors
                Debug.WriteLine($"error: {e.Message}");
            }
        }
    }
}