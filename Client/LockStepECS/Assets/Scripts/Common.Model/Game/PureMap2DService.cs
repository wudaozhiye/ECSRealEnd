using System;

namespace Lockstep.Game
{
	// Token: 0x0200005A RID: 90
	public class PureMap2DService : BaseMap2DService
	{
		// Token: 0x0600021B RID: 539 RVA: 0x00006F5C File Offset: 0x0000515C
		private void OnEvent_SimulationAwake(object param)
		{
			base.LoadLevel(1);
		}
	}
}
