using System;
using System.Collections.Generic;
using NetMsg.Common;

namespace Lockstep.Game
{
	// Token: 0x02000059 RID: 89
	public class PureInputService : PureBaseService, IInputService, IService
	{
		// Token: 0x06000217 RID: 535 RVA: 0x00003A82 File Offset: 0x00001C82
		public void Execute(InputCmd cmd, object entity)
		{
		}

		// Token: 0x06000218 RID: 536 RVA: 0x00006F18 File Offset: 0x00005118
		public List<InputCmd> GetInputCmds()
		{
			return this.cmds;
		}

		// Token: 0x06000219 RID: 537 RVA: 0x00006F30 File Offset: 0x00005130
		public List<InputCmd> GetDebugInputCmds()
		{
			return this.cmds;
		}

		// Token: 0x040000E0 RID: 224
		private List<InputCmd> cmds = new List<InputCmd>();
	}
}
