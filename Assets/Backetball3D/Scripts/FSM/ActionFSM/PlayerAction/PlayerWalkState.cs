namespace BasketballRash
{
    using UnityEngine;
    using GameFramework.Event;
    using GameFramework.Fsm;
    using ActionOwner = GameFramework.Fsm.IFsm<IActionManager>;
    public class PlayerWalkState : PlayerActionBase
    {
        public PlayerWalkState(PlayerController controller)
            : base(controller)
        {

        }

        protected override void OnEnter(ActionOwner actionOwner)
        {
            base.OnEnter(actionOwner);
            
            GameEntry.Event.Subscribe(PlayerActionAttackEventArgs.EventID, OnPlayerAttack);
            GameEntry.Event.Subscribe(PlayerActionMoveEventArgs.EventID, OnPlayerMove);
            GameEntry.Event.Subscribe(PlayerActionJumpEventArgs.EventID, OnPlayerJump);

            Controller.skeletonAnimation.AnimationName = Controller.runName;
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

            float inputX = Controller.InputActions.Move.X;
            float inputY = Controller.InputActions.Move.Y;

            bool hasMove = false;

            if (inputX != 0)
            {
                Controller.velocity.x = Mathf.Abs(inputX) > Controller.walkToRun ? Controller.runSpeed : Controller.walkSpeed;
                Controller.velocity.x *= Mathf.Sign(inputX);
                Controller.skeletonAnimation.skeleton.FlipX = inputX < 0;
                hasMove = true;
            }
            else
            {
                Controller.velocity.x = 0;
            }

            if (inputY != 0)
            {
                Controller.velocity.z = Mathf.Abs(inputY) > Controller.walkToRun ? Controller.runSpeed : Controller.walkSpeed;
                Controller.velocity.z *= Mathf.Sign(inputY);
                hasMove = true;
            }
            else
            {
                Controller.velocity.z = 0;
            }

            if (!hasMove)
            {
                ChangeState<PlayerIdleState>(actionOwner);
            }
        }

        private void OnPlayerAttack(object sender, GameEventArgs e)
        {
            ChangeState<PlayerAttackState>(Owner);
        }

        private void OnPlayerMove(object sender, GameEventArgs e)
        {

        }

        private void OnPlayerJump(object sender, GameEventArgs e)
        {
            ChangeState<PlayerJumpState>(Owner);
        }
    }
}