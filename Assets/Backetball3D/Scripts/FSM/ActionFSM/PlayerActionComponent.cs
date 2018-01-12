namespace BasketballRash
{
    using UnityEngine;
    using System.Collections;
    using GameFramework;
    using UnityGameFramework.Runtime;
    using GameFramework.Fsm;

    public class PlayerActionComponent : GameFrameworkComponent
    {
        private IActionManager m_ActionManager = null;
        private ActionBase m_EntranceAction = null;

        public ActionBase CurrentAction
        {
            get
            {
                return m_ActionManager.CurrentAction;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            m_ActionManager = new ActionManager();
            if (null == m_ActionManager)
            {
                Log.Fatal("Action manager is invalid.");
                return;
            }
        }

        private void Start()
        {
            //ActionBase[] actions = new ActionBase[3];
            //actions[0] = new PlayerIdleState();
            //actions[1] = new PlayerAttackState();
            //actions[2] = new PlayerWalkState();

            //m_EntranceAction = actions[0];
            //m_ActionManager.Initialize(GameFrameworkEntry.GetModule<IFsmManager>(), actions);
            //m_ActionManager.StartAction(m_EntranceAction.GetType());
        }
    }
}
