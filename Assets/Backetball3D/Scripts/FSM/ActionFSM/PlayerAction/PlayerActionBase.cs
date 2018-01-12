namespace BasketballRash
{
    using ActionOwner = GameFramework.Fsm.IFsm<IActionManager>;

    public abstract class PlayerActionBase : ActionBase
    {
        public PlayerActionBase(PlayerController controller)
        {
            this.Controller = controller;
            this.Owner = null;
        }

        public PlayerController Controller
        {
            get;
            set;
        }

        public ActionOwner Owner
        {
            get;
            set;
        }
    }
}