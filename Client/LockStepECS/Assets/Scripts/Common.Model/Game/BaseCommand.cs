using System;

namespace Lockstep.Game
{
	// Token: 0x0200001F RID: 31
	public class BaseCommand : object, ICommand
	{
		// Token: 0x06000069 RID: 105 RVA: 0x00003A82 File Offset: 0x00001C82
		public virtual void Do(object param)
		{
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00003A82 File Offset: 0x00001C82
		public virtual void Undo(object param)
		{
		}
	}
}
