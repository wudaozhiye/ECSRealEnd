using System;
using System.Collections.Generic;
using NetMsg.Common;

namespace Lockstep.Game
{
	// Token: 0x02000054 RID: 84
	public interface IRoomMsgManager
	{
		// Token: 0x060001D6 RID: 470
		void Init(IRoomMsgHandler msgHandler);

		// Token: 0x060001D7 RID: 471
		void SendInput(Msg_PlayerInput msg);

		// Token: 0x060001D8 RID: 472
		void SendMissFrameReq(int missFrameTick);

		// Token: 0x060001D9 RID: 473
		void SendMissFrameRepAck(int missFrameTick);

		// Token: 0x060001DA RID: 474
		void SendHashCodes(int firstHashTick, List<int> allHashCodes, int startIdx, int count);

		// Token: 0x060001DB RID: 475
		void SendGameEvent(byte[] data);

		// Token: 0x060001DC RID: 476
		void SendLoadingProgress(byte progress);

		// Token: 0x060001DD RID: 477
		void ConnectToGameServer(Msg_C2G_Hello helloBody, IPEndInfo _gameTcpEnd, bool isReconnect);

		// Token: 0x060001DE RID: 478
		void OnLevelLoadProgress(float progress);
	}
}
