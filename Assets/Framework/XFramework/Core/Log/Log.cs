namespace XFramework
{
	using System;

#if UNITY_5
	using UnityEngine;
#endif

    public enum LogLevel
	{
		None = 0,
		Exception = 1,
		Error = 2,
		Warning = 3,
		Info = 4,
		Max = 5,
	}

	public static class Log
	{
		public static void LogInfo(this object selfMsg)
		{
			I(selfMsg);
		}
		public static void LogWarning(this object selfMsg)
		{
			W(selfMsg);
		}
		public static void LogError(this object selfMsg)
		{
			E(selfMsg);
		}
		public static void LogExcption(this Exception selfExp)
		{
			E(selfExp);
		}
		private static LogLevel mLogLevel = LogLevel.Info;

		public static LogLevel Level
		{
			get { return mLogLevel; }
			set { mLogLevel = value; }
		}

		public static void I(object msg)
		{
			if (mLogLevel < LogLevel.Info)
			{
				return;
			}
			#if UNITY_5
			Debug.Log(msg);
			#else
			Console.WriteLine(msg);
			#endif
		}

		public static void I(string msg, params object[] args)
		{
			if (mLogLevel < LogLevel.Info)
			{
				return;
			}
			#if UNITY_5
			Debug.Log(string.Format(msg, args));
			#else
			Console.WriteLine(msg, args);
			#endif
		}

		public static void E(object msg)
		{
			if (mLogLevel < LogLevel.Error)
			{
				return;
			}

			#if UNITY_5
			Debug.LogError(msg);
			#else
			Console.WriteLine("[Error] {0}", msg);
			#endif
		}

		public static void E(string msg, params object[] args)
        {
            if (mLogLevel < LogLevel.Error)
            {
                return;
            }

#if UNITY_5
			Debug.LogError(string.Format(msg, args));
#else
            Console.WriteLine(string.Format("[Error] {0}", msg, args));
#endif
        }

		public static void E(Exception e)
		{
			if (mLogLevel < LogLevel.Exception)
			{
				return;
			}
			#if UNITY_5
			Debug.LogExcption(e);
			#else
			Console.WriteLine("[Exception] {0}", e);
			#endif
		}

		public static void W(object msg)
		{
			if (mLogLevel < LogLevel.Warning)
			{
				return;
			}

			#if UNITY_5
			Debug.LogWarning(msg);
			#else
			Console.WriteLine("[Warning] {0}", msg);
			#endif
		}

		public static void W(string msg, params object[] args)
		{
			if (mLogLevel < LogLevel.Warning)
			{
				return;
			}

			#if UNITY_5
			Debug.LogWarning(string.Format(msg, args));
			#else
			Console.WriteLine(string.Format("[Warning] {0}", msg), args);
			#endif
		}
	}
}