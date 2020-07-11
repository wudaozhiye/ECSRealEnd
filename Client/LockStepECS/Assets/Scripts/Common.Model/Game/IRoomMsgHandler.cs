using System;
using Lockstep.Logging;
using NetMsg.Common;

namespace Lockstep.Game
{
	// Token: 0x0200004D RID: 77
	public interface IRoomMsgHandler
	{
		// Token: 0x0600017E RID: 382
		void SetLogger(DebugInstance debug);

		// Token: 0x0600017F RID: 383
		void OnTcpHello(Msg_G2C_Hello msg);

		// Token: 0x06000180 RID: 384
		void OnUdpHello(int mapId, byte localId);

		// Token: 0x06000181 RID: 385
		void OnGameStartInfo(Msg_G2C_GameStartInfo data);

		// Token: 0x06000182 RID: 386
		void OnLoadingProgress(byte[] progresses);

		// Token: 0x06000183 RID: 387
		void OnAllFinishedLoaded(short level);

		// Token: 0x06000184 RID: 388
		void OnGameStartFailed();

		// Token: 0x06000185 RID: 389
		void OnServerFrames(Msg_ServerFrames msg);

		// Token: 0x06000186 RID: 390
		void OnMissFrames(Msg_RepMissFrame msg);

		// Token: 0x06000187 RID: 391
		void OnGameEvent(byte[] data);
	}
}
