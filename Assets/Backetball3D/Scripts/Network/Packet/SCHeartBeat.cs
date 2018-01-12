using ProtoBuf;
using System;

namespace BasketballRash
{
    [Serializable, ProtoContract(Name = @"SCHeartBeat")]
    public partial class SCHeartBeat : SCPacketBase
    {
        public SCHeartBeat()
        {

        }

        public override int Id
        {
            get
            {
                return 2;
            }
        }

        public override void Clear()
        {

        }
    }
}
