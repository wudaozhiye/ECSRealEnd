using System;

namespace Lockstep.Game
{
	// Token: 0x02000036 RID: 54
	public interface ICommandBuffer
	{
		// Token: 0x060000F7 RID: 247
		void Init(object param, FuncUndoCommands funcUndoCommand);

		// Token: 0x060000F8 RID: 248
		void Jump(int curTick, int dstTick);

		// Token: 0x060000F9 RID: 249
		void Clean(int maxVerifiedTick);

		// Token: 0x060000FA RID: 250
		void Execute(int tick, ICommand cmd);
	}
}
