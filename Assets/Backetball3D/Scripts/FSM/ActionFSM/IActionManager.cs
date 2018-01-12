namespace BasketballRash
{
    using System;
    using GameFramework.Fsm;
    /// <summary>
    /// 玩家动作管理器接口
    /// </summary>
    public interface IActionManager
    {
        /// <summary>
        /// 当前正在运行的动作
        /// </summary>
        ActionBase CurrentAction
        {
            get;
        }

        /// <summary>
        /// 获取当前动作持续的时间
        /// </summary>
        float CurrentActionTime
        {
            get;
        }

        /// <summary>
        /// 初始化动作管理器
        /// </summary>
        /// <param name="fsmManager">有限状态机管理器</param>
        /// <param name="actions">动作管理器需要管理的动作</param>
        void Initialize(IFsmManager fsmManager, params ActionBase[] actions);

        /// <summary>
        /// 开始动作
        /// </summary>
        /// <typeparam name="T">要开始的动作类型</typeparam>
        void StartAction<T>() where T : ActionBase;
        /// <summary>
        /// 开始动作
        /// </summary>
        /// <param name="actionType">要开始的动作类型</param>
        void StartAction(Type actionType);

        /// <summary>
        /// 是否存在动作
        /// </summary>
        /// <typeparam name="T">动作类型</typeparam>
        void HasAction<T>() where T : ActionBase;
        /// <summary>
        /// 是否存在动作
        /// </summary>
        /// <param name="actionType">动作类型</param>
        void HasAction(Type actionType);

        /// <summary>
        /// 获取动作
        /// </summary>
        /// <typeparam name="T">要获取的动作类型</typeparam>
        /// <returns>要获取的动作</returns>
        ActionBase GetAction<T>() where T : ActionBase;
        /// <summary>
        /// 获取动作
        /// </summary>
        /// <param name="actionType">要获取的动作类型</param>
        /// <returns>要获取的动作</returns>
        ActionBase GetAction(Type actionType);
    }
}