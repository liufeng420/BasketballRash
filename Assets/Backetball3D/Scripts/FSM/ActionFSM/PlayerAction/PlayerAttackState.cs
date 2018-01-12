namespace BasketballRash
{
    using UnityEngine;
    using GameFramework.Event;
    using ActionOwner = GameFramework.Fsm.IFsm<IActionManager>;
    public class PlayerAttackState : PlayerActionBase
    {
        float endTime = 0.0f;
        float startTime = 0.0f;
        public PlayerAttackState(PlayerController controller)
            : base(controller)
        {

        }

        protected override void OnEnter(ActionOwner actionOwner)
        {
            base.OnEnter(actionOwner);
            GameEntry.Event.Subscribe(PlayerActionAttackEventArgs.EventID, OnPlayerAttack);
            GameEntry.Event.Subscribe(PlayerActionMoveEventArgs.EventID, OnPlayerMove);
            GameEntry.Event.Subscribe(PlayerActionJumpEventArgs.EventID, OnPlayerJump);

            Controller.skeletonAnimation.AnimationName = Controller.attackName;
            // 攻击时停止移动
            Controller.velocity = Vector3.zero;
            startTime = Time.time;
            endTime = startTime + Controller.attackDuration;
        }

        protected override void OnLeave(ActionOwner actionOwner, bool isShutDown)
        {
            base.OnLeave(actionOwner, isShutDown);
            GameEntry.Event.Unsubscribe(PlayerActionAttackEventArgs.EventID, OnPlayerAttack);
            GameEntry.Event.Unsubscribe(PlayerActionMoveEventArgs.EventID, OnPlayerMove);
            GameEntry.Event.Unsubscribe(PlayerActionJumpEventArgs.EventID, OnPlayerJump);
        }

        protected override void OnUpdate(ActionOwner actionOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(actionOwner, elapseSeconds, realElapseSeconds);

            if (Time.time > endTime)
            {
                ChangeState<PlayerIdleState>(actionOwner);
            }
        }

        private void OnPlayerAttack(object sender, GameEventArgs e)
        {
            if (Time.time > startTime + Controller.attackDuration * 0.6f)
            {
                endTime = Time.time + Controller.attackDuration;
                Controller.skeletonAnimation.AnimationName = Controller.attackName;
            }
        }

        private void OnPlayerMove(object sender, GameEventArgs e)
        {
            // 攻击时不允许移动
            // ChangeState<PlayerWalkState>(Owner);
        }

        private void OnPlayerJump(object sender, GameEventArgs e)
        {
            // ChangeState<PlayerJumpState>(Owner);
        }
    }
}
