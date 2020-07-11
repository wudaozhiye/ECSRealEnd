using System;
using Lockstep.Serialization;

namespace Lockstep.Game
{
	// Token: 0x02000063 RID: 99
	public class Table_Assets : TableData<Table_Assets>
	{
		// Token: 0x060002C8 RID: 712 RVA: 0x00008F38 File Offset: 0x00007138
		public override string Name()
		{
			return "Assets";
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x00008F4F File Offset: 0x0000714F
		protected override void DoParseData(Deserializer reader)
		{
			this.id = reader.ReadInt32();
			this.dir = reader.ReadString();
			this.name = reader.ReadString();
			this.suffix = reader.ReadString();
		}

		// Token: 0x04000125 RID: 293
		private const string tableName = "Assets";

		// Token: 0x04000126 RID: 294
		public int id;

		// Token: 0x04000127 RID: 295
		public string dir;

		// Token: 0x04000128 RID: 296
		public string name;

		// Token: 0x04000129 RID: 297
		public string suffix;
	}
}
