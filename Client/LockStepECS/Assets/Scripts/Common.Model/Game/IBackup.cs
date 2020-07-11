using System;
using Lockstep.Serialization;

namespace Lockstep.Game
{
	// Token: 0x02000030 RID: 48
	public interface IBackup : IDumpStr
	{
		// Token: 0x060000ED RID: 237
		void WriteBackup(Serializer writer);

		// Token: 0x060000EE RID: 238
		void ReadBackup(Deserializer reader);
	}
}
