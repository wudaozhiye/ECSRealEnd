using System;
using NetMsg.Common;

namespace Lockstep.Game
{
	// Token: 0x02000061 RID: 97
	public interface IWorld
	{
		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000287 RID: 647
		// (set) Token: 0x06000288 RID: 648
		int Tick { get; set; }

		// Token: 0x06000289 RID: 649
		void SimulationAwake(IServiceContainer serviceContainer, IManagerContainer mgrContainer);

		// Token: 0x0600028A RID: 650
		void SimulationStart(Msg_G2C_GameStartInfo gameStartInfo, int localPlayerId);

		// Token: 0x0600028B RID: 651
		void OnApplicationQuit();

		// Token: 0x0600028C RID: 652
		void DoDestroy();

		// Token: 0x0600028D RID: 653
		void ProcessInputQueue(InputCmd cmd);

		// Token: 0x0600028E RID: 654
		void Step(bool isNeedGenSnap = true);

		// Token: 0x0600028F RID: 655
		void Backup(int tick);

		// Token: 0x06000290 RID: 656
		void RollbackTo(int tick, int maxContinueServerTick, bool isNeedClear = true);

		// Token: 0x06000291 RID: 657
		void CleanUselessSnapshot(int tick);
	}
}
