using System;
using NetMsg.Common;

namespace Lockstep.Game
{
	// Token: 0x02000029 RID: 41
	public interface IFrameBuffer
	{
		// Token: 0x0600009A RID: 154
		void ForcePushDebugFrame(ServerFrame frame);

		// Token: 0x0600009B RID: 155
		void PushLocalFrame(ServerFrame frame);

		// Token: 0x0600009C RID: 156
		void PushServerFrames(ServerFrame[] frames, bool isNeedDebugCheck = true);

		// Token: 0x0600009D RID: 157
		void PushMissServerFrames(ServerFrame[] frames, bool isNeedDebugCheck = true);

		// Token: 0x0600009E RID: 158
		void OnPlayerPing(Msg_G2C_PlayerPing msg);

		// Token: 0x0600009F RID: 159
		ServerFrame GetFrame(int tick);

		// Token: 0x060000A0 RID: 160
		ServerFrame GetServerFrame(int tick);

		// Token: 0x060000A1 RID: 161
		ServerFrame GetLocalFrame(int tick);

		// Token: 0x060000A2 RID: 162
		void SetClientTick(int tick);

		// Token: 0x060000A3 RID: 163
		void SendInput(Msg_PlayerInput input);

		// Token: 0x060000A4 RID: 164
		void DoUpdate(float deltaTime);

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x060000A5 RID: 165
		int NextTickToCheck { get; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x060000A6 RID: 166
		int MaxServerTickInBuffer { get; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x060000A7 RID: 167
		bool IsNeedRollback { get; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x060000A8 RID: 168
		int MaxContinueServerTick { get; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x060000A9 RID: 169
		int CurTickInServer { get; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x060000AA RID: 170
		int PingVal { get; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x060000AB RID: 171
		int DelayVal { get; }
	}
}
