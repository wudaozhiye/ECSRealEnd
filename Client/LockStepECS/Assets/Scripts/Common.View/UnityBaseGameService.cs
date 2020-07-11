using System;
using UnityEngine;

namespace Lockstep.Game
{
	// Token: 0x02000018 RID: 24
	[PureMode(EPureModeType.Unity)]
	public abstract class UnityBaseGameService : BaseGameService
	{
		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000081 RID: 129 RVA: 0x00005317 File Offset: 0x00003517
		// (set) Token: 0x06000082 RID: 130 RVA: 0x0000531F File Offset: 0x0000351F
		public Transform transform { get; protected set; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000083 RID: 131 RVA: 0x00005328 File Offset: 0x00003528
		// (set) Token: 0x06000084 RID: 132 RVA: 0x00005330 File Offset: 0x00003530
		public GameObject gameObject { get; protected set; }

		// Token: 0x06000085 RID: 133 RVA: 0x0000533C File Offset: 0x0000353C
		public override void DoInit(object objParent)
		{
			bool flag = this.gameObject != null;
			if (!flag)
			{
				Transform transform = objParent as Transform;
				base.DoInit(transform);
				this.InitGameObject(transform);
			}
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00005374 File Offset: 0x00003574
		private void InitGameObject(Transform parent)
		{
			GameObject gameObject = new GameObject(base.GetType().Name);
			this.gameObject = gameObject;
			this.transform = gameObject.transform;
			this.transform.SetParent(parent, false);
		}

		// Token: 0x06000087 RID: 135 RVA: 0x000053B8 File Offset: 0x000035B8
		public override void DoDestroy()
		{
			bool flag = this.gameObject != null;
			if (flag)
			{
				this.gameObject.DestroyExt();
			}
		}
	}
}
