using System;

namespace Lockstep.Game
{
	// Token: 0x02000027 RID: 39
	public class BaseSimulatorHelper : object
	{
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600008E RID: 142 RVA: 0x0000482D File Offset: 0x00002A2D
		public int Tick
		{
			get
			{
				return this._world.Tick;
			}
		}

		// Token: 0x0600008F RID: 143 RVA: 0x0000483A File Offset: 0x00002A3A
		public BaseSimulatorHelper(IServiceContainer serviceContainer, World world)
		{
			this._world = world;
			this._serviceContainer = serviceContainer;
		}

		// Token: 0x04000071 RID: 113
		protected World _world;

		// Token: 0x04000072 RID: 114
		protected IServiceContainer _serviceContainer;
	}
}
