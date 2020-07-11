using System;
using Lockstep.Logging;
using NetMsg.Common;

namespace Lockstep.Game
{
	// Token: 0x0200004C RID: 76
	public class BaseLoginHandler : BaseLogger, ILoginHandler
	{
		// Token: 0x0600016D RID: 365 RVA: 0x00003A82 File Offset: 0x00001C82
		public virtual void OnTickPlayer(byte reason)
		{
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00003A82 File Offset: 0x00001C82
		public virtual void OnConnectedLoginServer()
		{
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00003A82 File Offset: 0x00001C82
		public virtual void OnConnLobby(RoomInfo[] roomInfos)
		{
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00003A82 File Offset: 0x00001C82
		public virtual void OnRoomInfo(RoomInfo[] roomInfos)
		{
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00003A82 File Offset: 0x00001C82
		public virtual void OnCreateRoom(RoomInfo info, RoomPlayerInfo[] playerInfos)
		{
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00003A82 File Offset: 0x00001C82
		public virtual void OnRoomInfoUpdate(RoomInfo[] addInfo, int[] deleteInfos, RoomChangedInfo[] changedInfos)
		{
		}

		// Token: 0x06000173 RID: 371 RVA: 0x00003A82 File Offset: 0x00001C82
		public virtual void OnStartRoomResult(int reason)
		{
		}

		// Token: 0x06000174 RID: 372 RVA: 0x00003A82 File Offset: 0x00001C82
		public virtual void OnGameStart(Msg_C2G_Hello msg, IPEndInfo tcpEnd, bool isConnect)
		{
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00005AFE File Offset: 0x00003CFE
		public virtual void OnLoginFailed(ELoginResult result)
		{
			base.Log("Login failed reason " + result, Array.Empty<object>());
		}

		// Token: 0x06000176 RID: 374 RVA: 0x00003A82 File Offset: 0x00001C82
		public virtual void OnGameStartFailed()
		{
		}

		// Token: 0x06000177 RID: 375 RVA: 0x00003A82 File Offset: 0x00001C82
		public virtual void OnPlayerJoinRoom(RoomPlayerInfo info)
		{
		}

		// Token: 0x06000178 RID: 376 RVA: 0x00003A82 File Offset: 0x00001C82
		public virtual void OnPlayerLeaveRoom(long userId)
		{
		}

		// Token: 0x06000179 RID: 377 RVA: 0x00003A82 File Offset: 0x00001C82
		public virtual void OnRoomChatInfo(RoomChatInfo info)
		{
		}

		// Token: 0x0600017A RID: 378 RVA: 0x00003A82 File Offset: 0x00001C82
		public virtual void OnPlayerReadyInRoom(long userId, byte state)
		{
		}

		// Token: 0x0600017B RID: 379 RVA: 0x00003A82 File Offset: 0x00001C82
		public virtual void OnLeaveRoom()
		{
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00005B26 File Offset: 0x00003D26
		void ILoginHandler.SetLogger(DebugInstance logger)
		{
			base.SetLogger(logger);
		}
	}
}
