namespace BasketballRash
{
    using GameFramework.Fsm;
    using GameFramework;
    using ActionOwner = GameFramework.Fsm.IFsm<IActionManager>;

    public abstract class ActionBase : FsmState<IActionManager>
    {
        protected override void OnInit(ActionOwner actionOwner)
        {
            base.OnInit(actionOwner);
        }

        protected override void OnEnter(ActionOwner actionOwner)
        {
            base.OnEnter(actionOwner);
        }

        protected override void OnUpdate(ActionOwner actionOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(actionOwner, elapseSeconds, realElapseSeconds);
        }

        protected override void OnLeave(ActionOwner actionOwner, bool isShutDown)
        {
            base.OnLeave(actionOwner, isShutDown);
        }

        protected override void OnDestroy(ActionOwner actionOwner)
        {
            base.OnDestroy(actionOwner);
        }
    }
}