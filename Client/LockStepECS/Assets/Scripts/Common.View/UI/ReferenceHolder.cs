using System;
using System.Collections.Generic;
using Lockstep.Math;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Lockstep.Game.UI
{
	// Token: 0x02000029 RID: 41
	public class ReferenceHolder : MonoBehaviour, IReferenceHolder
	{
		// Token: 0x060000F3 RID: 243 RVA: 0x000069C8 File Offset: 0x00004BC8
		public T GetRef<T>(string name) where T : UnityEngine.Object
		{
			UnityEngine.Object @object = null;
			bool flag = this._name2Objs != null && this._name2Objs.TryGetValue(name, out @object);
			T result;
			if (flag)
			{
				result = (@object as T);
			}
			else
			{
				Debug.Log("Miss Ref " + name);
				result = default(T);
			}
			return result;
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00006A20 File Offset: 0x00004C20
		public LFloat GetVal(string name)
		{
			object obj;
			bool flag = this._name2Vals.TryGetValue(name, out obj);
			LFloat result;
			if (flag)
			{
				result = (LFloat)obj;
			}
			else
			{
				result = LFloat.zero;
			}
			return result;
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00006A54 File Offset: 0x00004C54
		public Color GetColor(string name)
		{
			object obj;
			bool flag = this._name2Vals.TryGetValue(name, out obj);
			Color result;
			if (flag)
			{
				result = (Color)obj;
			}
			else
			{
				result = Color.white;
			}
			return result;
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00006A88 File Offset: 0x00004C88
		private void Awake()
		{
			foreach (RefData refData in this.Datas)
			{
				bool flag = refData.bindObj != null;
				if (flag)
				{
					bool flag2 = this._name2Objs == null;
					if (flag2)
					{
						this._name2Objs = new Dictionary<string, Object>();
					}
					this._name2Objs.Add(refData.name, refData.bindObj);
				}
				else
				{
					bool flag3 = this._name2Vals == null;
					if (flag3)
					{
						this._name2Vals = new Dictionary<string, object>();
					}
					this._name2Vals.Add(refData.name, refData.bindVal);
				}
			}
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00006B58 File Offset: 0x00004D58
		public void OnDestroy()
		{
			bool isPlaying = Application.isPlaying;
			if (isPlaying)
			{
				this.Datas = null;
			}
			this._name2Objs = null;
			this._name2Vals = null;
		}

		// Token: 0x040000CA RID: 202
		[SerializeField]
		private Dictionary<string, Object> _name2Objs;

		// Token: 0x040000CB RID: 203
		[SerializeField]
		private Dictionary<string, object> _name2Vals;

		// Token: 0x040000CC RID: 204
		public List<RefData> Datas = new List<RefData>();
	}
}
