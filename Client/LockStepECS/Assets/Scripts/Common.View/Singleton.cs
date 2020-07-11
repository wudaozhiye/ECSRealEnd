using System;

namespace Lockstep.Game
{
	// Token: 0x02000020 RID: 32
	public class Singleton<T> : object where T : new()
	{
		// Token: 0x060000C6 RID: 198 RVA: 0x000060A5 File Offset: 0x000042A5
		protected Singleton()
		{
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060000C7 RID: 199 RVA: 0x000060AF File Offset: 0x000042AF
		public static T Instance
		{
			get
			{
				return Singleton<T>._instance;
			}
		}

		// Token: 0x040000A9 RID: 169
		private static readonly T _instance = Activator.CreateInstance<T>();
	}
}
