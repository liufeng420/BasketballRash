namespace BasketballRash
{
    using UnityEngine;
    using GameFramework.Event;
    using GameFramework.Fsm;
    using ActionOwner = GameFramework.Fsm.IFsm<IActionManager>;

    public class PlayerJumpState : PlayerActionBase
    {
        private int jumpCounts = 0;  // 跳的次数
        private Vector3 jumpVelocity = Vector3.zero;
        private bool bFirstUpdate = false;
        public PlayerJumpState(PlayerController controller)
            : base(controller)
        {
        }

        protected override void OnEnter(ActionOwner actionOwner)
        {
            Debug.Log("Enter Jump: Jump Counts:" + jumpCounts);
            base.OnEnter(actionOwner);
            GameEntry.Event.Subscribe(PlayerActionAttackEventArgs.EventID, OnPlayerAttack);
            GameEntry.Event.Subscribe(PlayerActionMoveEventArgs.EventID, OnPlayerMove);
            GameEntry.Event.Subscribe(PlayerActionJumpEventArgs.EventID, OnPlayerJump);

            DoJump();
        }

        protected override void OnLeave(ActionOwner actionOwner, bool isShutDown)
        {
            Debug.Log("Leave Jump");
            base.OnLeave(actionOwner, isShutDown);

            GameEntry.Event.Unsubscribe(PlayerActionAttackEventArgs.EventID, OnPlayerAttack);
            GameEntry.Event.Unsubscribe(PlayerActionMoveEventArgs.EventID, OnPlayerMove);
            GameEntry.Event.Unsubscribe(PlayerActionJumpEventArgs.EventID, OnPlayerJump);

            jumpCounts = 0;
        }

        protected override void OnUpdate(ActionOwner actionOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(actionOwner, elapseSeconds, realElapseSeconds);

            if (Controller.CharController.isGrounded && !bFirstUpdate)
            {
                ChangeState<PlayerIdleState>(actionOwner);
                return;
            }
            bFirstUpdate = false;

            float inputX = Controller.InputActions.Move.X;
            float inputY = Controller.InputActions.Move.Y;
            Vector3 velocity = Controller.velocity;

            if (inputX != 0)
            {
                if (Mathf.Abs(jumpVelocity.x) < 0.001f)
                {
                    velocity.x = Controller.jumpSpeedFix * Mathf.Sign(inputX);
                }
                else
                {
                    if (Mathf.Sign(inputX) == Mathf.Sign(jumpVelocity.x))
                    {
                        velocity.x = jumpVelocity.x;
                    }
                    else
                    {
                        velocity.x = (Mathf.Abs(jumpVelocity.x) - Controller.jumpSpeedFix) * Mathf.Sign(jumpVelocity.x);
                    }
                }
            }

            if (inputY != 0)
            {
                if (Mathf.Abs(jumpVelocity.z) < 0.001f)
                {
                    // 原先没有输入
                    velocity.z = Controller.jumpSpeedFix * Mathf.Sign(inputY);
                }
                else
                {
                    if (Mathf.Sign(inputY) == Mathf.Sign(jumpVelocity.z))
                    {
                        velocity.z = jumpVelocity.z;
                    }
                    else
                    {
                        velocity.z = (Mathf.Abs(jumpVelocity.z) - Controller.jumpSpeedFix) * Mathf.Sign(jumpVelocity.z);
                    }
                }
            }

            Controller.velocity = velocity;

            if (!Controller.CharController.isGrounded)
            {
                var gravityDeltaVelocity = Physics.gravity * Controller.gravityScale * Time.deltaTime;
                Controller.velocity += gravityDeltaVelocity;
            }

        }

        private void OnPlayerAttack(object sender, GameEventArgs e)
        {
            //ChangeState<PlayerAttackState>(Owner);
        }

        private void OnPlayerMove(object sender, GameEventArgs e)
        {
            //ChangeState<PlayerWalkState>(Owner);
        }

        private void OnPlayerJump(object sender, GameEventArgs e)
        {
            Debug.Log(jumpCounts);
            if (jumpCounts < Controller.jumpCountLimits)
            {
                DoJump();
            }
        }

        private void DoJump()
        {
            // 进入跳跃状态
            bFirstUpdate = true;
            Controller.jumpAudioSource.Stop();
            Controller.jumpAudioSource.Play();
            Controller.velocity.y = Controller.jumpSpeed;
            jumpCounts++;
            Controller.skeletonAnimation.AnimationName = Controller.jumpName;

            float inputX = Controller.InputActions.Move.X;
            float inputY = Controller.InputActions.Move.Y;
            if (inputX != 0)
            {
                jumpVelocity.x = Mathf.Abs(inputX) > Controller.walkToRun ? Controller.runSpeed : Controller.walkSpeed;
                jumpVelocity.x *= Mathf.Sign(inputX);
            }
            else
            {
                jumpVelocity.x = 0;
                if (jumpCounts > 1)
                {
                    Controller.velocity.x = 0;
                }
            }
            if (inputY != 0)
            {
                jumpVelocity.z = Mathf.Abs(inputY) > Controller.walkToRun ? Controller.runSpeed : Controller.walkSpeed;
                jumpVelocity.z *= Mathf.Sign(inputY);
            }
            else
            {
                jumpVelocity.z = 0;
                if (jumpCounts > 1)
                {
                    Controller.velocity.z = 0;
                }
            }
        }
    }
}
