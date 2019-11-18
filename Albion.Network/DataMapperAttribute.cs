using System;

namespace Albion.Network
{
    public class DataMapperAttribute : Attribute
    {
        public DataMapperAttribute(Type deserializerType)
        {
            DeserializerType = deserializerType;
        }

        public Type DeserializerType { get; }
    }
}
