using Albion.Network;
using System;
using System.Collections.Generic;

namespace Albion.Operation
{
    public class JoinOperation : BaseOperation
    {
        public JoinOperation(Dictionary<byte, object> parameters) : base(parameters)
        {
            UserId = Convert.ToInt64(parameters[0]);
            Name = Convert.ToString(parameters[2]);
            LocationId = Convert.ToString(parameters[8]);
        }

        public long UserId { get; }
        public string Name { get; }
        public string LocationId { get; }
    }
}
