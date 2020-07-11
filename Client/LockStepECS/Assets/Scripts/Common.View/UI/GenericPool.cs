using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Lockstep.Game.UI
{
	// Token: 0x0200002C RID: 44
	public class GenericPool<T> : object where T : MonoBehaviour
	{
		// Token: 0x06000104 RID: 260 RVA: 0x00006DE0 File Offset: 0x00004FE0
		public GenericPool(T prefab, bool dontUseOriginal = false)
		{
			bool flag = prefab == null;
			if (flag)
			{
				throw new NullReferenceException("Generic pool received a null as a prefab");
			}
			prefab.gameObject.SetActive(false);
			this._prefab = prefab;
			this._freeObjects = new Stack<T>();
			bool flag2 = !dontUseOriginal;
			if (flag2)
			{
				this.Store(prefab);
			}
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00006E44 File Offset: 0x00005044
		private T InstantiateNew()
		{
			return Object.Instantiate<T>(this._prefab);
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00006E64 File Offset: 0x00005064
		public T GetResource()
		{
			bool flag = this._freeObjects.Count > 0;
			T result;
			if (flag)
			{
				result = this._freeObjects.Pop();
			}
			else
			{
				result = this.InstantiateNew();
			}
			return result;
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00006E9C File Offset: 0x0000509C
		public void Store(T obj)
		{
			obj.gameObject.SetActive(false);
			this._freeObjects.Push(obj);
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00006EBE File Offset: 0x000050BE
		public void Cleanup()
		{
			this._freeObjects.Clear();
		}

		// Token: 0x040000D4 RID: 212
		private readonly Stack<T> _freeObjects;

		// Token: 0x040000D5 RID: 213
		private readonly T _prefab;
	}
}
