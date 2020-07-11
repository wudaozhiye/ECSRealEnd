using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Lockstep.Game.UI
{
	// Token: 0x02000027 RID: 39
	[Serializable]
	public class RefData
	{
		// Token: 0x060000EC RID: 236 RVA: 0x00006860 File Offset: 0x00004A60
		public RefData(string name, EComponentType eComponent, object bindVal = null)
		{
			this.name = name;
			this.bindObj = null;
			this.bindVal = bindVal;
			this.TypeName = eComponent.ToString();
		}

		// Token: 0x060000ED RID: 237 RVA: 0x000068B0 File Offset: 0x00004AB0
		public RefData(string name, EComponentType eComponent, Object bindObj)
		{
			this.name = name;
			this.bindObj = bindObj;
			this.bindVal = null;
			this.TypeName = eComponent.ToString();
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000EE RID: 238 RVA: 0x00006900 File Offset: 0x00004B00
		// (set) Token: 0x060000EF RID: 239 RVA: 0x00006908 File Offset: 0x00004B08
		public string TypeName
		{
			get
			{
				return this.typeName;
			}
			set
			{
				this.typeName = value;
				this.SetParams(value);
			}
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x0000691C File Offset: 0x00004B1C
		public Object GetBindObj()
		{
			return this.bindObj;
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00006934 File Offset: 0x00004B34
		private void SetParams(string typeName)
		{
			EComponentType ecomponentType = (EComponentType)Enum.Parse(typeof(EComponentType), typeName);
			bool flag = ecomponentType < EComponentType.GameObject;
			if (flag)
			{
				this.bindObj = this.GetGameObject().GetComponent(typeName);
			}
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00006978 File Offset: 0x00004B78
		public GameObject GetGameObject()
		{
			GameObject result = null;
			Component component;
			bool flag = (component = (this.bindObj as Component)) != null;
			if (flag)
			{
				result = component.gameObject;
			}
			else
			{
				GameObject gameObject;
				bool flag2 = (gameObject = (this.bindObj as GameObject)) != null;
				if (flag2)
				{
					result = gameObject;
				}
			}
			return result;
		}

		// Token: 0x040000B1 RID: 177
		public string name;

		// Token: 0x040000B2 RID: 178
		public Object bindObj;

		// Token: 0x040000B3 RID: 179
		public object bindVal;

		// Token: 0x040000B4 RID: 180
		public string typeName = "";

		// Token: 0x040000B5 RID: 181
		public bool hasVal = false;
	}
}
