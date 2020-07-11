using System;

namespace Lockstep.ECS
{
	// Token: 0x02000017 RID: 23
	public interface ICloneable
	{
		// Token: 0x0600004C RID: 76
		void CopyTo(object comp);

		// Token: 0x0600004D RID: 77
		object Clone();
	}
}
