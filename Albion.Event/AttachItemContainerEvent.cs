//using Albion.Network;
//using System;
//using System.Collections.Generic;

//namespace Albion.Event
//{
//    public class AttachItemContainerEvent : BaseEvent
//    {
//        public AttachItemContainerEvent(Dictionary<byte, object> parameters) : base(parameters)
//        {
//            Id = Convert.ToInt64(parameters[0]);
//            ContainerId = new Guid((byte[])parameters[1]);
//            Guid1 = new Guid((byte[])parameters[2]);
//            //ItemIds = 
//            Size = Convert.ToInt32(parameters[4]);
//        }

//        public long Id { get; set; }
//        public Guid ContainerId { get; set; }
//        public Guid Guid1 { get; set; }
//        public long[] ItemIds { get; set; }
//        public int Size { get; set; }
//    }
//}
