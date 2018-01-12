namespace BasketballRash
{
    /// <summary>
    /// 游戏入口。
    /// </summary>
    public partial class GameEntry
    {
        public static ConfigComponent Config
        {
            get;
            private set;
        }

        public static HPBarComponent HPBar
        {
            get;
            private set;
        }

        public static PlayerActionComponent PlayerAction
        {
            get;
            private set;
        }

        private static void InitCustomComponents()
        {
            Config = UnityGameFramework.Runtime.GameEntry.GetComponent<ConfigComponent>();
            HPBar = UnityGameFramework.Runtime.GameEntry.GetComponent<HPBarComponent>();

            PlayerAction = UnityGameFramework.Runtime.GameEntry.GetComponent<PlayerActionComponent>();
        }
    }
}
