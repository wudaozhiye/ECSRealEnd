using System;
using UnityEngine;

// Token: 0x02000008 RID: 8
[CreateAssetMenu(menuName = "FloatTextConfig")]
public class FloatTextConfig : ScriptableObject
{
	// Token: 0x0400003A RID: 58
	[Header("Curves")]
	public AnimationCurve curveAlpha = null;

	// Token: 0x0400003B RID: 59
	public AnimationCurve curveScale = null;

	// Token: 0x0400003C RID: 60
	public AnimationCurve curveX = null;

	// Token: 0x0400003D RID: 61
	public AnimationCurve curveY = null;

	// Token: 0x0400003E RID: 62
	public Color color;

	// Token: 0x0400003F RID: 63
	[Header("Times")]
	public float slideTotalTime;

	// Token: 0x04000040 RID: 64
	public float yOffset = 5f;

	// Token: 0x04000041 RID: 65
	public float xOffset = 5f;

	// Token: 0x04000042 RID: 66
	public Color atkColor;

	// Token: 0x04000043 RID: 67
	public Color healColor;
}
