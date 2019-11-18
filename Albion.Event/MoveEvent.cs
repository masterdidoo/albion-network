using Albion.Network;

namespace Albion.Event
{
    [DataMapper(typeof(MoveEventDataMapper))]
    public class MoveEvent : BaseEvent
    {
        public long Id { get; set; }
        public byte Byte0 { get; set; }
        public long Ticks { get; set; }
        public float[] NewPosititon { get; set; }
        public float Direction { get; set; }
        public float Speed { get; set; }
        public float[] OldPosititon { get; set; }
        public long Long0 { get; set; }
        public bool Bool0 { get; set; }
    }
}
