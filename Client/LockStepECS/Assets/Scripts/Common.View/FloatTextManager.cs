using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

// Token: 0x02000009 RID: 9
public class FloatTextManager : MonoBehaviour
{
	// Token: 0x06000029 RID: 41 RVA: 0x00002C80 File Offset: 0x00000E80
	public float GetAlpha(float percent)
	{
		return this.curveAlpha.Evaluate(percent);
	}

	// Token: 0x0600002A RID: 42 RVA: 0x00002CA0 File Offset: 0x00000EA0
	public float GetXOffset(float percent)
	{
		return this.curveX.Evaluate(percent) * this.xOffset;
	}

	// Token: 0x0600002B RID: 43 RVA: 0x00002CC8 File Offset: 0x00000EC8
	public float GetYOffset(float percent)
	{
		return this.curveY.Evaluate(percent) * this.yOffset;
	}

	// Token: 0x0600002C RID: 44 RVA: 0x00002CF0 File Offset: 0x00000EF0
	public float GetScale(float percent)
	{
		return this.curveScale.Evaluate(percent);
	}

	// Token: 0x0600002D RID: 45 RVA: 0x00002D10 File Offset: 0x00000F10
	private void Update()
	{
		float deltaTime = Time.deltaTime;
		foreach (FloatText floatText in this.runingTexts)
		{
			floatText.DoUpdate(deltaTime);
		}
		for (int i = this.runingTexts.Count - 1; i >= 0; i--)
		{
			FloatText floatText2 = this.runingTexts[i];
			bool isFinished = floatText2.IsFinished;
			if (isFinished)
			{
				this.runingTexts.RemoveAt(i);
				this.DestroyText(floatText2);
			}
		}
	}

	// Token: 0x0600002E RID: 46 RVA: 0x00002DC8 File Offset: 0x00000FC8
	private void Start()
	{
		FloatTextManager._Instance = this;
		bool flag = this.prefab == null;
		if (flag)
		{
			this.prefab = (GameObject)Resources.Load("Prefabs/FloatingDamage");
		}
		bool flag2 = this.config == null;
		if (flag2)
		{
			this.config = Resources.Load<FloatTextConfig>("Config/FloatDamageConfig");
		}
		this.curveAlpha = this.config.curveAlpha;
		this.curveScale = this.config.curveScale;
		this.curveX = this.config.curveX;
		this.curveY = this.config.curveY;
		this.color = this.config.color;
		this.slideTotalTime = this.config.slideTotalTime;
		this.yOffset = this.config.yOffset;
		this.xOffset = this.config.xOffset;
	}

	// Token: 0x0600002F RID: 47 RVA: 0x00002EB0 File Offset: 0x000010B0
	private void DestroyText(FloatText text)
	{
		bool flag = this.pools.Count > FloatTextManager._Instance.maxPoolCount;
		if (flag)
		{
			Object.Destroy(text);
		}
		else
		{
			text.OnRecycle();
			this.pools.Enqueue(text);
		}
	}

	// Token: 0x06000030 RID: 48 RVA: 0x00002EF8 File Offset: 0x000010F8
	private FloatText GetOrCreateText()
	{
		bool flag = this.pools.Count > 0;
		FloatText result;
		if (flag)
		{
			result = this.pools.Dequeue();
		}
		else
		{
			GameObject gameObject = Object.Instantiate<GameObject>(this.prefab, this.canvas.transform, false);
			FloatText component = gameObject.GetComponent<FloatText>();
			result = component;
		}
		return result;
	}

	// Token: 0x06000031 RID: 49 RVA: 0x00002F4C File Offset: 0x0000114C
	public static void CreateFloatText(Vector3 pos, int damage)
	{
		FloatTextManager._Instance._CreateFloatText(pos, (damage > 0) ? ("+" + damage.ToString()) : damage.ToString(), (damage > 0) ? FloatTextManager._Instance.config.healColor : FloatTextManager._Instance.config.atkColor);
	}

	// Token: 0x06000032 RID: 50 RVA: 0x00002FA8 File Offset: 0x000011A8
	public static void CreateFloatText(Vector3 pos, string text, Color color)
	{
		FloatTextManager._Instance._CreateFloatText(pos, text, color);
	}

	// Token: 0x06000033 RID: 51 RVA: 0x00002FBC File Offset: 0x000011BC
	private void _CreateFloatText(Vector3 pos, string text, Color color)
	{
		FloatText orCreateText = this.GetOrCreateText();
		orCreateText.mgr = this;
		orCreateText.color = color;
		this.runingTexts.Add(orCreateText);
		orCreateText.OnUse(pos, text);
	}

	// Token: 0x04000044 RID: 68
	[Header("Curves")]
	public AnimationCurve curveAlpha = null;

	// Token: 0x04000045 RID: 69
	[HideInInspector]
	public AnimationCurve curveScale = null;

	// Token: 0x04000046 RID: 70
	[HideInInspector]
	public AnimationCurve curveX = null;

	// Token: 0x04000047 RID: 71
	[HideInInspector]
	public AnimationCurve curveY = null;

	// Token: 0x04000048 RID: 72
	[HideInInspector]
	public Color color;

	// Token: 0x04000049 RID: 73
	[Header("Times")]
	public float slideTotalTime;

	// Token: 0x0400004A RID: 74
	public float yOffset = 5f;

	// Token: 0x0400004B RID: 75
	public float xOffset = 5f;

	// Token: 0x0400004C RID: 76
	[Header("Pools")]
	public GameObject prefab;

	// Token: 0x0400004D RID: 77
	public int maxPoolCount = 50;

	// Token: 0x0400004E RID: 78
	private static FloatTextManager _Instance;

	// Token: 0x0400004F RID: 79
	[SerializeField]
	private Canvas canvas;

	// Token: 0x04000050 RID: 80
	private List<FloatText> runingTexts = new List<FloatText>();

	// Token: 0x04000051 RID: 81
	private Queue<FloatText> pools = new Queue<FloatText>();

	// Token: 0x04000052 RID: 82
	public FloatTextConfig config;
}
