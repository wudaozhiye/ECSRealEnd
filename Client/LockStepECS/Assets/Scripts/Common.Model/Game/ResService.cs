using System;
using System.Collections.Generic;

namespace Lockstep.Game
{
	// Token: 0x0200005F RID: 95
	public class ResService : BaseService, IResService, IService
	{
		// Token: 0x06000280 RID: 640 RVA: 0x00007A60 File Offset: 0x00005C60
		public override void DoStart()
		{
			base.DoStart();
			this.CheckInit();
		}

		// Token: 0x06000281 RID: 641 RVA: 0x00007A74 File Offset: 0x00005C74
		private void CheckInit()
		{
			bool flag = this._id2Path != null;
			if (!flag)
			{
				this._id2Path = new Dictionary<ushort, string>();
				TableData<Table_Assets>.Load();
				Dictionary<ulong, Table_Assets> allData = TableData<Table_Assets>.GetAllData();
				foreach (KeyValuePair<ulong, Table_Assets> keyValuePair in allData)
				{
					Table_Assets value = keyValuePair.Value;
					this._id2Path.Add((ushort)keyValuePair.Key, value.dir + "/" + value.name);
				}
			}
		}

		// Token: 0x06000282 RID: 642 RVA: 0x00007B1C File Offset: 0x00005D1C
		public string GetAssetPath(ushort assetId)
		{
			this.CheckInit();
			string text;
			bool flag = this._id2Path.TryGetValue(assetId, out text);
			string result;
			if (flag)
			{
				result = text;
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x04000107 RID: 263
		private Dictionary<ushort, string> _id2Path;
	}
}
