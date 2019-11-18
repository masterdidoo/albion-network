using System;
using Albion.Event;
using Albion.Operation;

namespace Albion.Network.Example
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (var albionParser = new AlbionParser())
            {
                albionParser.AddEventHandler<PlayerCounts>(p => { Console.WriteLine($"Players: {p.Blue} / {p.Red}"); });

                albionParser.AddOperationHandler<ConsloeCommand>(p => { Console.WriteLine($"LocId: {p.LocId}"); });

                Console.WriteLine("Start");

                try
                {
                    albionParser.Start();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error Net Init: {e.Message}");
                }

                Console.Read();
            }
        }
    }
}