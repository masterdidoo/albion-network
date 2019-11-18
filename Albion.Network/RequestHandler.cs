// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using Albion.Common;
using System;

namespace Albion.Network
{
    public class RequestHandler<TOperation> : PacketHandler<RequestPacket> where TOperation : BaseOperation
    {
        private readonly OperationCodes operationCode;
        private readonly Action<TOperation> action;

        internal RequestHandler(OperationCodes operationCode, Action<TOperation> action)
        {
            this.operationCode = operationCode;
            this.action = action;
        }

        protected internal override void OnHandle(RequestPacket packet)
        {
            if (operationCode != packet.OperationCode)
            {
                Next(packet);
            }
            else
            {
                TOperation instance;
                IDataMapper mapper;

                Type operationType = typeof(TOperation);
                DataMapperAttribute attribute = (DataMapperAttribute)Attribute.GetCustomAttribute(operationType, typeof(DataMapperAttribute));
                if (attribute != null)
                {
                    mapper = (IDataMapper)Activator.CreateInstance(attribute.DeserializerType);
                }
                else
                {
                    mapper = new DefaultDataMapper();
                }

                instance = mapper.MapFromParameters<TOperation>(packet.Parameters);

                action.Invoke(instance);
            }
        }
    }
}
