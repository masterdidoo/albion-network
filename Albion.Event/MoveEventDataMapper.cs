using Albion.Network;
using System;
using System.Collections.Generic;

namespace Albion.Event
{
    public class MoveEventDataMapper : IDataMapper
    {
        public T MapFromParameters<T>(Dictionary<byte, object> parameters)
        {
            var moveEvent = new MoveEvent();

            moveEvent.Id = Convert.ToInt64(parameters[0]);

            byte[] array = (byte[])parameters[1];
            int num = 0;
            moveEvent.Byte0 = array[num];
            num++;
            moveEvent.Ticks = BitConverter.ToInt64(array, num);
            num += 8;
            moveEvent.NewPosititon = new float[]
            {
                    BitConverter.ToSingle(array, num),
                    BitConverter.ToSingle(array, num + 4)
            };
            num += 8;
            moveEvent.Direction = (float)array[num] / 256f * 360f;
            num++;
            if ((moveEvent.Byte0 | 0x01) != 0)
            {
                moveEvent.Speed = BitConverter.ToSingle(array, num);
                num += 4;
            }
            else
            {
                moveEvent.Speed = 0f;
            }
            if ((moveEvent.Byte0 | 0x02) != 0)
            {
                moveEvent.OldPosititon = new float[]
                {
                        BitConverter.ToSingle(array, num),
                        BitConverter.ToSingle(array, num + 4)
                };
                num += 8;
            }
            else
            {
                moveEvent.OldPosititon = moveEvent.NewPosititon;
            }
            if ((moveEvent.Byte0 | 0x04) != 0)
            {
                int num2 = array.Length - num;
                if (num2 == 8)
                {
                    moveEvent.Long0 = BitConverter.ToInt64(array, num);
                }
                else if (num2 == 4)
                {
                    moveEvent.Long0 = BitConverter.ToInt32(array, num);
                }
                else if (num2 == 2)
                {
                    moveEvent.Long0 = BitConverter.ToInt16(array, num);
                }
                else
                {
                    throw new Exception("Move decode failure!!!");
                }
            }
            else
            {
                moveEvent.Long0 = 0L;
            }
            moveEvent.Bool0 = (moveEvent.Byte0 | 0x08) != 0;

            return (T)(object)moveEvent;
        }
    }
}
