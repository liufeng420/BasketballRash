namespace BasketballRash
{
    using GameFramework.Event;
    public sealed class PlayerActionAttackEventArgs : GameEventArgs
    {
        public static readonly int EventID = typeof(PlayerActionAttackEventArgs).GetHashCode();

        public override int Id
        {
            get
            {
                return EventID;
            }
        }

        public override void Clear()
        {

        }
    }
}
