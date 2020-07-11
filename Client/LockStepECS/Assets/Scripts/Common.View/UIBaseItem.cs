using System;
using Lockstep.Game.UI;
using Lockstep.Logging;
using UnityEngine;
using Debug = Lockstep.Logging.Debug;

namespace Lockstep.Game
{
	// Token: 0x0200001D RID: 29
	public abstract class UIBaseItem : MonoBehaviour
	{
		// Token: 0x060000AF RID: 175 RVA: 0x00005E28 File Offset: 0x00004028
		protected T GetRef<T>(string name) where T : UnityEngine.Object
		{
			return this._referenceHolder.GetRef<T>(name);
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00005E46 File Offset: 0x00004046
		protected virtual void Awake()
		{
			this._referenceHolder = base.GetComponent<IReferenceHolder>();
			Debug.Assert(this._referenceHolder != null, base.GetType() + " miss IReferenceHolder ");
			this.DoAwake();
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x0000517B File Offset: 0x0000337B
		protected virtual void DoAwake()
		{
		}

		// Token: 0x040000A5 RID: 165
		protected IReferenceHolder _referenceHolder;
	}
}
