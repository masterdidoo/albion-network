using Albion.Common;
using Albion.Event;
using Albion.Operation;
using PcapDotNet.Core;
using PcapDotNet.Packets;
using PcapDotNet.Packets.IpV4;
using PcapDotNet.Packets.Transport;
using System;
using System.Linq;
using System.Threading;

namespace Albion.Network.Example
{
    class Program
    {
        static AlbionParser albionParser;

        static void Main(string[] args)
        {
            albionParser = new AlbionParser();

            albionParser.AddEventHandler<AttachItemContainerEvent>(EventCodes.AttachItemContainer, (obj) =>
            {
                Console.WriteLine($"{obj.Id} {obj.ContainerId} {obj.Guid1} {obj.Size}");
            });
            //albionParser.AddEventHandler<DetachItemContainerEvent>(EventCodes.DetachItemContainer, (obj) =>
            //{
            //    Console.WriteLine("DetachItemContainer");
            //});
            //foreach(var item in Enum.GetValues(typeof(EventCodes)))
            //{
            //    if ((EventCodes)item != EventCodes.Move)
            //    {
            //        albionParser.AddEventHandler<DebugEvent>((EventCodes)item, (obj) =>
            //        {
            //            Console.WriteLine($"Event: {item}");
            //        });
            //    }
            //}

            //foreach (var item in Enum.GetValues(typeof(OperationCodes)))
            //{
            //    if ((OperationCodes)item != OperationCodes.Move)
            //    {
            //        albionParser.AddRequestHandler<DebugOperation>((OperationCodes)item, (obj) =>
            //        {
            //            Console.WriteLine($"Request: {item}");
            //        });
            //    }
            //}

            //foreach (var item in Enum.GetValues(typeof(OperationCodes)))
            //{
            //    if ((OperationCodes)item != OperationCodes.Move)
            //    {
            //        albionParser.AddResponseHandler<DebugOperation>((OperationCodes)item, (obj) =>
            //        {
            //            Console.WriteLine($"Response: {item}");
            //        });
            //    }
            //}

            //albionParser.AddEventHandler<DebugEvent>(EventCodes.AccessStatus, (obj) =>
            //{
            //    Console.WriteLine("");
            //});

            //albionParser.AddRequestHandler<DebugOperation>(OperationCodes.GetChestLogs, (obj) =>
            //{
            //    var guid = new Guid((byte[])obj.Parameters[0]);

            //    Console.WriteLine("");
            //});

            Console.WriteLine("Start");

            var devices = LivePacketDevice.AllLocalMachine;
            foreach (var device in devices)
            {
                new Thread(() =>
                {
                    Console.WriteLine($"Open... {device.Description}");

                    using (PacketCommunicator communicator = device.Open(65536, PacketDeviceOpenAttributes.Promiscuous, 1000))
                    {
                        communicator.ReceivePackets(0, PacketHandler);
                    }
                })
                .Start();
            }

            Console.Read();
        }

        static void PacketHandler(Packet packet)
        {
            IpV4Datagram ip = packet.Ethernet.IpV4;
            UdpDatagram udp = ip.Udp;

            if (udp == null || (udp.SourcePort != 5056 && udp.DestinationPort != 5056))
            {
                return;
            }

            albionParser.ReceivePacket(udp.Payload.ToArray());
        }
    }
}
