using System;
using Lockstep.Logging;
using NetMsg.Common;

namespace Lockstep.Game
{
	// Token: 0x0200004E RID: 78
	public class BaseRoomMsgHandler : BaseLogger, IRoomMsgHandler
	{
		// Token: 0x06000188 RID: 392 RVA: 0x00003A82 File Offset: 0x00001C82
		public virtual void OnTcpHello(Msg_G2C_Hello msg)
		{
		}

		// Token: 0x06000189 RID: 393 RVA: 0x00003A82 File Offset: 0x00001C82
		public virtual void OnUdpHello(int mapId, byte localId)
		{
		}

		// Token: 0x0600018A RID: 394 RVA: 0x00003A82 File Offset: 0x00001C82
		public virtual void OnGameStartInfo(Msg_G2C_GameStartInfo data)
		{
		}

		// Token: 0x0600018B RID: 395 RVA: 0x00003A82 File Offset: 0x00001C82
		public virtual void OnLoadingProgress(byte[] progresses)
		{
		}

		// Token: 0x0600018C RID: 396 RVA: 0x00003A82 File Offset: 0x00001C82
		public virtual void OnAllFinishedLoaded(short level)
		{
		}

		// Token: 0x0600018D RID: 397 RVA: 0x00003A82 File Offset: 0x00001C82
		public virtual void OnGameStartFailed()
		{
		}

		// Token: 0x0600018E RID: 398 RVA: 0x00003A82 File Offset: 0x00001C82
		public virtual void OnServerFrames(Msg_ServerFrames msg)
		{
		}

		// Token: 0x0600018F RID: 399 RVA: 0x00003A82 File Offset: 0x00001C82
		public virtual void OnMissFrames(Msg_RepMissFrame msg)
		{
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00003A82 File Offset: 0x00001C82
		public virtual void OnGameEvent(byte[] data)
		{
		}

		// Token: 0x06000192 RID: 402 RVA: 0x00005B26 File Offset: 0x00003D26
		void IRoomMsgHandler.SetLogger(DebugInstance debug)
		{
			base.SetLogger(debug);
		}
	}
}
