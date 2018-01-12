namespace XFramework
{
	using System;
	using System.Reflection;

	public class SingletonCreator
	{
		public static T CreateSingleton<T>() where T : class, ISingleton
		{
			T retInstance = default(T);

			ConstructorInfo[] ctors = typeof(T).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance);
			ConstructorInfo ctor = Array.Find(ctors, c => 0 == c.GetParameters().Length);

			if (null == ctor)
			{
				throw new Exception("Non-Public Constructor() not found! in " + typeof(T));
			}

			retInstance = ctor.Invoke(null) as T;
			retInstance.OnSingletonInit();
			return retInstance;
		}
	}
}