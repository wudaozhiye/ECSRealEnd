using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Lockstep.Game.UI
{
	// Token: 0x0200002E RID: 46
	public class GenericUIList<T> : object
	{
		// Token: 0x0600010E RID: 270 RVA: 0x000070FF File Offset: 0x000052FF
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

		// Token: 0x0600010F RID: 271 RVA: 0x00007134 File Offset: 0x00005334
		public void Iterate<T2>(Action<T2> expression) where T2 : class
		{
			foreach (GameObject gameObject in this._items)
			{
				expression(gameObject.GetComponent<T2>());
			}
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00007190 File Offset: 0x00005390
		public T2 FindObject<T2>(Func<T2, bool> condition)
		{
			foreach (GameObject gameObject in this._items)
			{
				T2 component = gameObject.GetComponent<T2>();
				bool flag = condition(component);
				if (flag)
				{
					return component;
				}
			}
			return default(T2);
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00007208 File Offset: 0x00005408
		public void Generate<T2>(IEnumerable<T> items, Action<T, T2> transformer) where T2 : Component
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
				gameObject.SetActive(true);
				bool flag2 = typeof(T2) == typeof(GameObject);
				if (flag2)
				{
					transformer(arg, gameObject as T2);
				}
				else
				{
					transformer(arg, gameObject.GetOrAddComponent<T2>());
				}
				num++;
			}
			while (this._items.Count > num)
			{
				this._items[num].gameObject.SetActive(false);
				num++;
			}
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00007330 File Offset: 0x00005530
		public GameObject GetObjectAt(int index)
		{
			return this._items.ElementAt(index);
		}

		// Token: 0x040000D9 RID: 217
		private readonly LayoutGroup _list;

		// Token: 0x040000DA RID: 218
		private readonly GameObject _prefab;

		// Token: 0x040000DB RID: 219
		private readonly List<GameObject> _items;
	}
}
