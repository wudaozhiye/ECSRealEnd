using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Lockstep.Game.UI
{
	// Token: 0x0200002D RID: 45
	public class GenericUIList<T, T2> : object where T2 : class
	{
		// Token: 0x06000109 RID: 265 RVA: 0x00006ECD File Offset: 0x000050CD
		public GenericUIList(GameObject prefab, LayoutGroup list)
		{
			this._prefab = prefab;
			this._list = list;
			this._items = new List<GameObject>
			{
				prefab
			};
			prefab.SetActive(false);
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00006F00 File Offset: 0x00005100
		public void Iterate(Action<T2> expression)
		{
			foreach (GameObject gameObject in this._items)
			{
				expression(gameObject.GetComponent<T2>());
			}
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00006F5C File Offset: 0x0000515C
		public T2 FindComponent(Func<T2, bool> condition)
		{
			return (from item in this._items
			select item.GetComponent<T2>()).FirstOrDefault(condition);
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00006FA0 File Offset: 0x000051A0
		public void Generate(IEnumerable<T> items, Action<T, T2> transformer)
		{
			int num = 0;
			foreach (T arg in items)
			{
				bool flag = this._items.Count > num;
				GameObject gameObject;
				if (flag)
				{
					gameObject = this._items[num];
				}
				else
				{
					gameObject = Object.Instantiate<GameObject>(this._prefab);
					gameObject.transform.SetParent(this._list.transform, false);
					this._items.Add(gameObject);
				}
				bool flag2 = typeof(T2) == typeof(GameObject);
				if (flag2)
				{
					transformer(arg, gameObject as T2);
				}
				else
				{
					transformer(arg, gameObject.GetComponent<T2>());
				}
				gameObject.SetActive(true);
				num++;
			}
			while (this._items.Count > num)
			{
				this._items[num].gameObject.SetActive(false);
				num++;
			}
		}

		// Token: 0x0600010D RID: 269 RVA: 0x000070C8 File Offset: 0x000052C8
		public GameObject GetObjectAt(int index)
		{
			bool flag = this._items.Count <= index;
			GameObject result;
			if (flag)
			{
				result = null;
			}
			else
			{
				result = this._items.ElementAt(index);
			}
			return result;
		}

		// Token: 0x040000D6 RID: 214
		private readonly LayoutGroup _list;

		// Token: 0x040000D7 RID: 215
		private readonly GameObject _prefab;

		// Token: 0x040000D8 RID: 216
		private readonly List<GameObject> _items;
	}
}
