using System;
using System.Collections.Generic;
using Lockstep.Math;

namespace Lockstep.Game
{
	public class SimpleWorld : World
	{
		// Token: 0x060000CE RID: 206 RVA: 0x000056EA File Offset: 0x000038EA
		public SimpleWorld(IServiceContainer services, object contextsObj, object logicFeatureObj)
		{
			this._systems = (logicFeatureObj as List<UpdatableService>);
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00005700 File Offset: 0x00003900
		protected override void DoSimulateAwake(IServiceContainer serviceContainer, IManagerContainer mgrContainer)
		{
			this.InitReference(serviceContainer, mgrContainer);
			foreach (UpdatableService updatableService in this._systems)
			{
				updatableService.InitReference(serviceContainer, mgrContainer);
			}
			foreach (UpdatableService updatableService2 in this._systems)
			{
				updatableService2.DoAwake(serviceContainer);
			}
			this.DoAwake(serviceContainer);
			foreach (UpdatableService updatableService3 in this._systems)
			{
				updatableService3.DoStart();
			}
			this.DoStart();
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00005804 File Offset: 0x00003A04
		public override void DoDestroy()
		{
			base.DoDestroy();
			foreach (UpdatableService updatableService in this._systems)
			{
				updatableService.DoDestroy();
			}
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00005864 File Offset: 0x00003A64
		public void RegisterSystem(UpdatableService mgr)
		{
			this._systems.Add(mgr);
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00003A82 File Offset: 0x00001C82
		protected override void DoBackup(int tick)
		{
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00005874 File Offset: 0x00003A74
		protected override void DoStep(bool isNeedGenSnap)
		{
			LFloat deltaTime = new LFloat(null, 30L);
			foreach (UpdatableService updatableService in this._systems)
			{
				bool enable = updatableService.enable;
				if (enable)
				{
					updatableService.DoUpdate(deltaTime);
				}
			}
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00003A82 File Offset: 0x00001C82
		protected override void DoRollbackTo(int tick, int missFrameTick, bool isNeedClear = true)
		{
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00003A82 File Offset: 0x00001C82
		protected override void DoCleanUselessSnapshot(int tick)
		{
		}

		// Token: 0x04000098 RID: 152
		private List<UpdatableService> _systems;
	}
}
