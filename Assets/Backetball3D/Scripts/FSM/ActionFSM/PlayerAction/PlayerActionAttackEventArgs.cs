namespace BasketballRash
{
    using GameFramework.Event;
    public sealed class PlayerActionJumpEventArgs : GameEventArgs
    {
        public static readonly int EventID = typeof(PlayerActionJumpEventArgs).GetHashCode();

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
