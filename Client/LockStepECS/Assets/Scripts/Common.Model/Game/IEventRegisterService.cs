using System;

namespace Lockstep.Game
{
	// Token: 0x0200003C RID: 60
	public interface IEventRegisterService : IService
	{
		// Token: 0x06000103 RID: 259
		void RegisterEvent(object obj);

		// Token: 0x06000104 RID: 260
		void UnRegisterEvent(object obj);

		// Token: 0x06000105 RID: 261
		void RegisterEvent<TEnum, TDelegate>(string prefix, int ignorePrefixLen, Action<TEnum, TDelegate> callBack, object obj) where TEnum : struct where TDelegate : Delegate;
	}
}
