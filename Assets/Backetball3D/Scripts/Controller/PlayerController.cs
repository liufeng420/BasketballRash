namespace BasketballRash
{
    using UnityEngine;
    using GameFramework;
    using GameFramework.Fsm;
    using Spine.Unity;

    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Moving")]
        public float walkSpeed = 6f;
        public float runSpeed = 10f;
        public float gravityScale = 6.6f;
        public float jumpSpeedFix = 4f; //起跳后反向速度修正系数
        public Transform shadow;
        public float walkToRun = 0.6f;

        [Header("Jumping")]
        public float jumpSpeed = 25;
        public float jumpDuration = 0.95f;
        public int jumpCountLimits = 2;
        public float jumpInterruptFactor = 100;
        public float forceCrouchVelocity = 25;
        public float forceCrouchDuration = 0.5f;

        [Header("Attacking")]
        public float attackDuration = 0.5f;
        public int shootRate = 99;

        [Header("Graphics")]
        public SkeletonAnimation skeletonAnimation;

        [Header("Animation")]
        [SpineAnimation(dataField: "skeletonAnimation")]
        public string walkName = "Walk";
        [SpineAnimation(dataField: "skeletonAnimation")]
        public string runName = "Run";
        [SpineAnimation(dataField: "skeletonAnimation")]
        public string idleName = "Idle";
        [SpineAnimation(dataField: "skeletonAnimation")]
        public string jumpName = "Jump";
        [SpineAnimation(dataField: "skeletonAnimation")]
        public string fallName = "Fall";
        [SpineAnimation(dataField: "skeletonAnimation")]
        public string attackName = "Attack";
        [SpineAnimation(dataField: "skeletonAnimation")]
        public string beattackName = "Beattack";
        [Header("Effects")]
        public AudioSource jumpAudioSource;
        public AudioSource hardfallAudioSource;
        public AudioSource footstepAudioSource;
        public ParticleSystem landParticles;
        [SpineEvent]
        public string footstepEventName = "Footstep";

        public CharacterController CharController
        {
            get;
            private set;
        }

        public Vector3 velocity = default(Vector3);
        public Vector3 jumpVelocity = default(Vector3);
        public Vector3 lastVelocity = default(Vector3);

        bool doingAttack = false;   // 是否正在攻击
        float attackEndTime;	// 攻击结束时间

        public PlayerActions InputActions
        {
            get;
            private set;
        }
        private ActionManager actionManager = null;

        private void Start()
        {
            actionManager.StartAction<PlayerIdleState>();
            Physics.IgnoreLayerCollision(gameObject.layer, gameObject.layer);
        }

        private void Awake()
        {
            CharController = GetComponent<CharacterController>();
            InputActions = PlayerActions.CreateWithDefaultBindings();
            actionManager = new ActionManager();

            PlayerActionBase[] actions = new PlayerActionBase[4];
            actions[0] = new PlayerIdleState(this);
            actions[1] = new PlayerAttackState(this);
            actions[2] = new PlayerWalkState(this);
            actions[3] = new PlayerJumpState(this);

            actionManager.Initialize(GameFrameworkEntry.GetModule<IFsmManager>(), actions);

            foreach (var action in actions)
            {
                action.Owner = actionManager.GetActionFsm();
            }
        }

        private void Update()
        {
            float x = InputActions.Move.X;
            float y = InputActions.Move.Y;
            if (x != 0 || y != 0)
            {
                PlayerActionMoveEventArgs e = new PlayerActionMoveEventArgs(x, y);
                GameEntry.Event.Fire(this, e);
            }

            if (InputActions.Jump.WasPressed)
            {
                PlayerActionJumpEventArgs e = new PlayerActionJumpEventArgs();
                GameEntry.Event.Fire(this, e);
            }

            if(InputActions.Fire.WasPressed)
            {
                PlayerActionAttackEventArgs e = new PlayerActionAttackEventArgs();
                GameEntry.Event.Fire(this, e);
            }

            CharController.Move(velocity * Time.deltaTime);

            if (InputActions.Move.X != 0)
                skeletonAnimation.Skeleton.FlipX = InputActions.Move.X < 0;

            Vector3 v = shadow.position;
            v.y = 0.1f;
            shadow.position = v;
        }
    }
}
