using System;
using Lockstep.Math;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Lockstep.Game
{
	// Token: 0x0200001A RID: 26
	public class UnityEffectService : UnityBaseService, IEffectService, IService
	{
		// Token: 0x06000091 RID: 145 RVA: 0x000054CC File Offset: 0x000036CC
		public void CreateEffect(int assetId, LVector2 pos)
		{
			string assetPath = this._resService.GetAssetPath((ushort)assetId);
			bool flag = string.IsNullOrEmpty(assetPath);
			if (!flag)
			{
				GameObject prefab = Resources.Load<GameObject>(assetPath);
				this.CreateEffect(prefab, pos);
			}
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00005504 File Offset: 0x00003704
		public void CreateEffect(GameObject prefab, LVector2 pos)
		{
			bool flag = prefab == null;
			if (!flag)
			{
				LFloat liveTime = prefab.GetComponent<IRollbackEffect>().LiveTime;
				EffectProxy effectProxy = new EffectProxy();
				GameObject gameObject = null;
				bool flag2 = !this._globalStateService.IsVideoLoading;
				if (flag2)
				{
					gameObject = Object.Instantiate<GameObject>(prefab, base.transform.position + pos.ToVector3(), Quaternion.identity);
				}
				bool flag3 = this.tail == null;
				if (flag3)
				{
					this.head = (this.tail = effectProxy);
				}
				else
				{
					effectProxy.pre = this.tail;
					this.tail.next = effectProxy;
					this.tail = effectProxy;
				}
				effectProxy.DoStart(base.CurTick, (gameObject != null) ? gameObject.GetComponent<IRollbackEffect>() : null, liveTime);
			}
		}

		// Token: 0x06000093 RID: 147 RVA: 0x000055D0 File Offset: 0x000037D0
		public void DestroyEffect(object obj)
		{
			EffectProxy effectProxy = obj as EffectProxy;
			bool flag = effectProxy.Effect != null;
			if (flag)
			{
				Component component = effectProxy.Effect as Component;
				Object.Destroy(component.gameObject);
			}
		}

		// Token: 0x06000094 RID: 148 RVA: 0x0000560C File Offset: 0x0000380C
		public override void Backup(int tick)
		{
			EffectProxy next = this.head;
			while (next != null)
			{
				EffectProxy effectProxy = next;
				next = next.next;
				bool flag = effectProxy.IsLive(tick);
				if (flag)
				{
					effectProxy.DoUpdate(tick);
				}
				else
				{
					bool flag2 = this.head == effectProxy;
					if (flag2)
					{
						this.head = effectProxy.next;
					}
					bool flag3 = this.tail == effectProxy;
					if (flag3)
					{
						this.tail = effectProxy.pre;
					}
					bool flag4 = effectProxy.pre != null;
					if (flag4)
					{
						effectProxy.pre.next = effectProxy.next;
					}
					bool flag5 = effectProxy.next != null;
					if (flag5)
					{
						effectProxy.next.pre = effectProxy.pre;
					}
					this.DestroyEffect(effectProxy);
				}
			}
		}

		// Token: 0x04000099 RID: 153
		private EffectProxy head;

		// Token: 0x0400009A RID: 154
		private EffectProxy tail;
	}
}
