using System;
using System.Reflection;
using Lockstep.Logging;

namespace Lockstep.Game
{
	// Token: 0x02000021 RID: 33
	public class EventRegisterService : object, IEventRegisterService, IService
	{
		public static T CreateDelegateFromMethodInfo<T>(object instance, MethodInfo method) where T : Delegate
		{
			return Delegate.CreateDelegate(typeof(T), instance, method) as T;
		}
		
		public void UnRegisterEvent(object obj)
		{
			this.RegisterEvent<EEvent, GlobalEventHandler>("OnEvent_", "OnEvent_".Length, new Action<EEvent, GlobalEventHandler>(EventHelper.RemoveListener), obj);
		}
		
		public void RegisterEvent(object obj)
		{
			this.RegisterEvent<EEvent, GlobalEventHandler>("OnEvent_", "OnEvent_".Length, new Action<EEvent, GlobalEventHandler>(EventHelper.AddListener), obj);
		}
		
		public void RegisterEvent<TEnum, TDelegate>(string prefix, int ignorePrefixLen, Action<TEnum, TDelegate> callBack, object obj) where TEnum : struct where TDelegate : Delegate
		{
			bool flag = callBack == null;
			if (!flag)
			{
				MethodInfo[] methods = obj.GetType().GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				foreach (MethodInfo methodInfo in methods)
				{
					string name = methodInfo.Name;
					bool flag2 = name.StartsWith(prefix);
					if (flag2)
					{
						string value = name.Substring(ignorePrefixLen);
						TEnum arg;
						bool flag3 = Enum.TryParse<TEnum>(value, out arg);
						if (flag3)
						{
							try
							{
								TDelegate arg2 = EventRegisterService.CreateDelegateFromMethodInfo<TDelegate>(obj, methodInfo);
								callBack(arg, arg2);
							}
							catch (Exception ex)
							{
								Debug.LogError("methodName " + name, Array.Empty<object>());
								throw;
							}
						}
					}
				}
			}
		}
	}
}
