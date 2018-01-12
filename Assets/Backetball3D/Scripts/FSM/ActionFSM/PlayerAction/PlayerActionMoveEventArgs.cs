namespace BasketballRash
{
    using GameFramework.Event;
    public sealed class PlayerActionMoveEventArgs : GameEventArgs
    {
        public static readonly int EventID = typeof(PlayerActionMoveEventArgs).GetHashCode();

        public override int Id
        {
            get
            {
                return EventID;
            }
        }

        public PlayerActionMoveEventArgs(float inputX, float inputY)
        {
            this.InputX = inputX;
            this.InputY = inputY;
        }

        public float InputX
        {
            get;
            private set;
        }

        public float InputY
        {
            get;
            private set;
        }

        public override void Clear()
        {
            this.InputX = default(float);
            this.InputY = default(float);
        }
    }
}
