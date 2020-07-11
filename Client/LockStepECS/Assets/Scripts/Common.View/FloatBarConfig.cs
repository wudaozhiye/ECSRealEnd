using System;
using UnityEngine;

// Token: 0x02000002 RID: 2
[CreateAssetMenu(menuName = "HealthBarConfig")]
[Serializable]
public class FloatBarConfig : ScriptableObject
{
	// Token: 0x04000001 RID: 1
	public float defaultAlpha = 0.7f;

	// Token: 0x04000002 RID: 2
	public float defaultFadeSpeed = 0.1f;

	// Token: 0x04000003 RID: 3
	public float fullAlpha = 1f;

	// Token: 0x04000004 RID: 4
	public float fullFadeSpeed = 0.1f;

	// Token: 0x04000005 RID: 5
	public float nullAlpha = 0f;

	// Token: 0x04000006 RID: 6
	public float nullFadeSpeed = 0.1f;

	// Token: 0x04000007 RID: 7
	public OnHit onHit = new OnHit();

	// Token: 0x04000008 RID: 8
	public bool keepSize = true;

	// Token: 0x04000009 RID: 9
	public bool IsDrawOffDistance;

	// Token: 0x0400000A RID: 10
	public float drawDistance = 10f;

	// Token: 0x0400000B RID: 11
	public bool showHealthInfo;

	// Token: 0x0400000C RID: 12
	public HealthInfoAlignment healthInfoAlignment = HealthInfoAlignment.Center;

	// Token: 0x0400000D RID: 13
	public float healthInfoSize = 10f;
}
