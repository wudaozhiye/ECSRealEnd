using System;
using UnityEngine;
using Lockstep;
using Object = UnityEngine.Object;

namespace Lockstep.Game
{
	// Token: 0x02000019 RID: 25
	[PureMode(EPureModeType.Unity)]
	public abstract class UnityBaseService : BaseService
	{
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000089 RID: 137 RVA: 0x000053ED File Offset: 0x000035ED
		// (set) Token: 0x0600008A RID: 138 RVA: 0x000053F5 File Offset: 0x000035F5
		public Transform transform { get; protected set; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600008B RID: 139 RVA: 0x000053FE File Offset: 0x000035FE
		// (set) Token: 0x0600008C RID: 140 RVA: 0x00005406 File Offset: 0x00003606
		public GameObject gameObject { get; protected set; }

		// Token: 0x0600008D RID: 141 RVA: 0x00005410 File Offset: 0x00003610
		public override void DoInit(object objParent)
		{
			Transform transform = objParent as Transform;
			base.DoInit(transform);
			this.InitGameObject(transform);
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00005438 File Offset: 0x00003638
		private void InitGameObject(Transform parent)
		{
			GameObject gameObject = new GameObject(base.GetType().Name);
			this.gameObject = gameObject;
			this.transform = gameObject.transform;
			this.transform.SetParent(parent, false);
		}

		// Token: 0x0600008F RID: 143 RVA: 0x0000547C File Offset: 0x0000367C
		public override void DoDestroy()
		{
			bool flag = this.gameObject != null;
			if (flag)
			{
				bool isPlaying = Application.isPlaying;
				if (isPlaying)
				{
					Object.Destroy(this.gameObject);
				}
				else
				{
					Object.DestroyImmediate(this.gameObject);
				}
			}
		}
	}
}
