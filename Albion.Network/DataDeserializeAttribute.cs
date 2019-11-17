using System;

namespace Albion.Network
{
    public class DataDeserializeAttribute : Attribute
    {
        public DataDeserializeAttribute(Type deserializerType)
        {
            DeserializerType = deserializerType;
        }

        public Type DeserializerType { get; }
    }
}
