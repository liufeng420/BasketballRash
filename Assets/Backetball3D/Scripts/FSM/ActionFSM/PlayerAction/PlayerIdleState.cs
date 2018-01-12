namespace BasketballRash
{
    using GameFramework.Event;
    using ActionOwner = GameFramework.Fsm.IFsm<IActionManager>;

    using UnityEngine;

    public class PlayerIdleState : PlayerActionBase
    {
        public PlayerIdleState(PlayerController controller)
            : base(controller)
        {
        }

        protected override void OnEnter(ActionOwner actionOwner)
        {
            Debug.Log("Enter Idle");
            base.OnEnter(actionOwner);
            
            GameEntry.Event.Subscribe(PlayerActionAttackEventArgs.EventID, OnPlayerAttack);
            GameEntry.Event.Subscribe(PlayerActionMoveEventArgs.EventID, OnPlayerMove);
            GameEntry.Event.Subscribe(PlayerActionJumpEventArgs.EventID, OnPlayerJump);

            Controller.skeletonAnimation.AnimationName = Controller.idleName;
            Controller.velocity = Vector3.zero;
        }

        protected override void OnLeave(ActionOwner actionOwner, bool isShutDown)
        {
            Debug.Log("Leave Idle");
            base.OnLeave(actionOwner, isShutDown);
            GameEntry.Event.Unsubscribe(PlayerActionAttackEventArgs.EventID, OnPlayerAttack);
            GameEntry.Event.Unsubscribe(PlayerActionMoveEventArgs.EventID, OnPlayerMove);
            GameEntry.Event.Unsubscribe(PlayerActionJumpEventArgs.EventID, OnPlayerJump);
        }

        protected override void OnUpdate(ActionOwner actionOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(actionOwner, elapseSeconds, realElapseSeconds);
        }

        private void OnPlayerAttack(object sender, GameEventArgs e)
        {
            ChangeState<PlayerAttackState>(Owner);
        }

        private void OnPlayerMove(object sender, GameEventArgs e)
        {
            ChangeState<PlayerWalkState>(Owner);
        }

        private void OnPlayerJump(object sender, GameEventArgs e)
        {
            ChangeState<PlayerJumpState>(Owner);
        }
    }
}
