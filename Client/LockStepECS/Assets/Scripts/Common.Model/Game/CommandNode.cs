using System;

namespace Lockstep.Game
{
	// Token: 0x02000034 RID: 52
	public class CommandNode : object
	{
		// Token: 0x060000F4 RID: 244 RVA: 0x00005AC6 File Offset: 0x00003CC6
		public CommandNode(int tick, ICommand cmd, CommandNode pre = null, CommandNode next = null)
		{
			this.Tick = tick;
			this.cmd = cmd;
			this.pre = pre;
			this.next = next;
		}

		// Token: 0x0400009D RID: 157
		public CommandNode pre;

		// Token: 0x0400009E RID: 158
		public CommandNode next;

		// Token: 0x0400009F RID: 159
		public int Tick;

		// Token: 0x040000A0 RID: 160
		public ICommand cmd;
	}
}
