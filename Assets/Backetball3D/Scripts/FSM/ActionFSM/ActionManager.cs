namespace BasketballRash
{
    using System;
    using GameFramework.Fsm;
    using GameFramework;
    using UnityGameFramework.Runtime;

    public class ActionManager : GameFrameworkComponent,IActionManager
    {
        private IFsmManager m_fsmManager;
        private IFsm<IActionManager> m_ActionFsm;

        public ActionManager()
        {
            m_fsmManager = null;
            m_ActionFsm = null;
        }

        public IFsm<IActionManager> GetActionFsm()
        {
            return m_ActionFsm;
        }

        public ActionBase CurrentAction
        {
            get
            {
                if (null == m_ActionFsm)
                {
                    throw new GameFrameworkException("You must initialize action first.");
                }

                return (ActionBase)m_ActionFsm.CurrentState;
            }
        }

        public float CurrentActionTime
        {
            get
            {
                if (null == m_ActionFsm)
                {
                    throw new GameFrameworkException("You must initialize action first.");
                }

                return m_ActionFsm.CurrentStateTime;
            }
        }

        public ActionBase GetAction<T>() where T : ActionBase
        {
            throw new NotImplementedException();
        }

        public ActionBase GetAction(Type actionType)
        {
            throw new NotImplementedException();
        }

        public void HasAction<T>() where T : ActionBase
        {
            throw new NotImplementedException();
        }

        public void HasAction(Type actionType)
        {
            throw new NotImplementedException();
        }

        public void StartAction<T>() where T : ActionBase
        {
            if (null == m_ActionFsm)
            {
                throw new GameFrameworkException("You must initialize action first.");
            }
            m_ActionFsm.Start<T>();
        }

        public void StartAction(Type actionType)
        {
            if (null == m_ActionFsm)
            {
                throw new GameFrameworkException("You must initialize action first.");
            }
            m_ActionFsm.Start(actionType);
        }

        public void Initialize(IFsmManager fsmManager, params ActionBase[] actions)
        {
            if (null == fsmManager)
            {
                throw new GameFrameworkException("FSM manager is invalid.");
            }

            m_fsmManager = fsmManager;
            m_ActionFsm = m_fsmManager.CreateFsm(this, actions);
        }
    }
}
