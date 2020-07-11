using System;
using Lockstep.Math;

namespace Lockstep.Game
{
	// Token: 0x0200001C RID: 28
	public class UpdatableService : BaseGameService, IUpdateable
	{
		// Token: 0x06000057 RID: 87 RVA: 0x00003A82 File Offset: 0x00001C82
		public virtual void DoUpdate(LFloat deltaTime)
		{
		}

		// Token: 0x04000048 RID: 72
		public bool enable = true;
	}
}
