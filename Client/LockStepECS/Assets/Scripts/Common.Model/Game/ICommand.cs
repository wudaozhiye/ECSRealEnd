using System;

namespace Lockstep.Game
{
	// Token: 0x02000035 RID: 53
	public interface ICommand
	{
		// Token: 0x060000F5 RID: 245
		void Do(object param);

		// Token: 0x060000F6 RID: 246
		void Undo(object param);
	}
}
