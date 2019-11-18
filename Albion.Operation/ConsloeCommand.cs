using System;
using System.Collections.Generic;
using Albion.Common;
using Albion.Network.Messages;

namespace Albion.Operation
{
    public class ConsloeCommand : BaseOperation
    {
        public string LocId { get; private set; }

        public override OperationCodes Code => OperationCodes.ConsoleCommand;

        public override void Init(Dictionary<byte, object> parameters)
        {
            LocId = Convert.ToString(parameters[0]);
        }
    }
}