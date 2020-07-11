using System;
using Lockstep.Logging;
using NetMsg.Common;

namespace Lockstep.Game
{
	// Token: 0x0200004B RID: 75
	public interface ILoginHandler
	{
		// Token: 0x0600015D RID: 349
		void SetLogger(DebugInstance logger);

		// Token: 0x0600015E RID: 350
		void OnTickPlayer(byte reason);

		// Token: 0x0600015F RID: 351
		void OnConnectedLoginServer();

		// Token: 0x06000160 RID: 352
		void OnConnLobby(RoomInfo[] roomInfos);

		// Token: 0x06000161 RID: 353
		void OnRoomInfo(RoomInfo[] roomInfos);

		// Token: 0x06000162 RID: 354
		void OnCreateRoom(RoomInfo info, RoomPlayerInfo[] playerInfos);

		// Token: 0x06000163 RID: 355
		void OnRoomInfoUpdate(RoomInfo[] addInfo, int[] deleteInfos, RoomChangedInfo[] changedInfos);

		// Token: 0x06000164 RID: 356
		void OnStartRoomResult(int reason);

		// Token: 0x06000165 RID: 357
		void OnGameStart(Msg_C2G_Hello msg, IPEndInfo tcpEnd, bool isConnect);

		// Token: 0x06000166 RID: 358
		void OnLoginFailed(ELoginResult result);

		// Token: 0x06000167 RID: 359
		void OnGameStartFailed();

		// Token: 0x06000168 RID: 360
		void OnPlayerJoinRoom(RoomPlayerInfo info);

		// Token: 0x06000169 RID: 361
		void OnPlayerLeaveRoom(long userId);

		// Token: 0x0600016A RID: 362
		void OnRoomChatInfo(RoomChatInfo info);

		// Token: 0x0600016B RID: 363
		void OnPlayerReadyInRoom(long userId, byte state);

		// Token: 0x0600016C RID: 364
		void OnLeaveRoom();
	}
}
