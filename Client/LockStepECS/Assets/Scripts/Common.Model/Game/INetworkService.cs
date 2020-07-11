using System;
using System.Collections.Generic;
using NetMsg.Common;

namespace Lockstep.Game
{
	// Token: 0x02000040 RID: 64
	public interface INetworkService : IService
	{
		// Token: 0x06000133 RID: 307
		void SendGameEvent(byte[] data);

		// Token: 0x06000134 RID: 308
		void SendPing(byte localId, long timestamp);

		// Token: 0x06000135 RID: 309
		void SendInput(Msg_PlayerInput msg);

		// Token: 0x06000136 RID: 310
		void SendHashCodes(int startTick, List<int> hashCodes, int startIdx, int count);

		// Token: 0x06000137 RID: 311
		void SendMissFrameReq(int missFrameTick);

		// Token: 0x06000138 RID: 312
		void SendMissFrameRepAck(int missFrameTick);
	}
}
