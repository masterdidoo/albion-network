using Albion.Network;
using System;
using System.Collections.Generic;

namespace Albion.Event
{
    public class MoveEvent : BaseEvent
    {
        public MoveEvent(Dictionary<byte, object> parameters) : base(parameters)
        {
            Id = Convert.ToInt64(parameters[0]);

            byte[] array = (byte[])parameters[1];
            int num = 0;
            Byte0 = array[num];
            num++;
            Ticks = BitConverter.ToInt64(array, num);
            num += 8;
            NewPosititon = new float[]
            {
                BitConverter.ToSingle(array, num),
                BitConverter.ToSingle(array, num + 4)
            };
            num += 8;
            Direction = (float)array[num] / 256f * 360f;
            num++;
            if ((Byte0 | 0x01) != 0)
            {
                Speed = BitConverter.ToSingle(array, num);
                num += 4;
            }
            else
            {
                Speed = 0f;
            }
            if ((Byte0 | 0x02) != 0)
            {
                OldPosititon = new float[]
                {
                    BitConverter.ToSingle(array, num),
                    BitConverter.ToSingle(array, num + 4)
                };
                num += 8;
            }
            else
            {
                OldPosititon = NewPosititon;
            }
            if ((Byte0 | 0x04) != 0)
            {
                int num2 = array.Length - num;
                if (num2 == 8)
                {
                    Long0 = BitConverter.ToInt64(array, num);
                }
                else if (num2 == 4)
                {
                    Long0 = BitConverter.ToInt32(array, num);
                }
                else if (num2 == 2)
                {
                    Long0 = BitConverter.ToInt16(array, num);
                }
                else
                {
                    throw new Exception("Move decode failure!!!");
                }
            }
            else
            {
                Long0 = 0L;
            }
            Bool0 = (Byte0 | 0x08) != 0;
        }

        public long Id { get; }
        public byte Byte0 { get; set; }
        public long Ticks { get; set; }
        public float[] NewPosititon { get; }
        public float Direction { get; set; }
        public float Speed { get; set; }
        public float[] OldPosititon { get; set; }
        public long Long0 { get; set; }
        public bool Bool0 { get; set; }
    }
}
