using System;
using Lockstep.Math;

namespace Lockstep.Game
{
	// Token: 0x0200003B RID: 59
	public interface IEffectService : IService
	{
		// Token: 0x06000101 RID: 257
		void CreateEffect(int assetId, LVector2 pos);

		// Token: 0x06000102 RID: 258
		void DestroyEffect(object node);
	}
}
