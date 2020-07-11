using System;
using Lockstep.Math;

namespace Lockstep.Game
{
	// Token: 0x0200004A RID: 74
	public interface IViewService : IService
	{
		// Token: 0x06000159 RID: 345
		void BindView(object entity, ushort assetId, LVector2 createPos, int deg = 0);

		// Token: 0x0600015A RID: 346
		void DeleteView(uint entityId);

		// Token: 0x0600015B RID: 347
		void RebindView(object entity);

		// Token: 0x0600015C RID: 348
		void RebindAllEntities();
	}
}
