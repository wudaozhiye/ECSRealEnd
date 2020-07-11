using System;
using UnityEngine;

namespace Lockstep.Game.UI
{
	// Token: 0x02000025 RID: 37
	public interface IReferenceHolder
	{
		// Token: 0x060000E1 RID: 225
		T GetRef<T>(string name) where T : UnityEngine.Object;
	}
}
