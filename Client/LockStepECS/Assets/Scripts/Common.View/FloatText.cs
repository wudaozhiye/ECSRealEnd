using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000007 RID: 7
public class FloatText : MonoBehaviour
{
	// Token: 0x1700000A RID: 10
	// (set) Token: 0x0600001F RID: 31 RVA: 0x00002A15 File Offset: 0x00000C15
	public Color color
	{
		set
		{
			this.textDamage.color = value;
		}
	}

	// Token: 0x1700000B RID: 11
	// (get) Token: 0x06000020 RID: 32 RVA: 0x00002A24 File Offset: 0x00000C24
	public bool IsFinished
	{
		get
		{
			return this.timer > this.mgr.slideTotalTime;
		}
	}

	// Token: 0x1700000C RID: 12
	// (get) Token: 0x06000021 RID: 33 RVA: 0x00002A3C File Offset: 0x00000C3C
	public static Camera WorldCam
	{
		get
		{
			bool flag = FloatText._worldCam == null;
			if (flag)
			{
				FloatText._worldCam = Camera.main;
			}
			return FloatText._worldCam;
		}
	}

	// Token: 0x06000022 RID: 34 RVA: 0x00002A6E File Offset: 0x00000C6E
	private void Awake()
	{
		this.textRectTransform = this.textDamage.rectTransform;
		this.textRectTransform.anchoredPosition = base.GetComponent<RectTransform>().anchoredPosition;
	}

	// Token: 0x06000023 RID: 35 RVA: 0x00002A9C File Offset: 0x00000C9C
	public void DoUpdate(float deltaTime)
	{
		this.timer += deltaTime;
		base.transform.position = FloatText.WorldCam.WorldToScreenPoint(this.bornPos);
		Vector2 anchoredPosition = this.textRectTransform.anchoredPosition;
		float percent = Mathf.Clamp01(this.timer / this.mgr.slideTotalTime);
		this.SetAlpha(this.mgr.GetAlpha(percent));
		this.textRectTransform.anchoredPosition = new Vector2(this.mgr.GetXOffset(percent), anchoredPosition.y + this.mgr.GetYOffset(percent));
		this.textRectTransform.localScale = Vector3.one * this.mgr.GetScale(percent);
	}

	// Token: 0x06000024 RID: 36 RVA: 0x00002B60 File Offset: 0x00000D60
	private void SetAlpha(float alpha)
	{
		Color color = this.textDamage.color;
		color.a = alpha;
		this.textDamage.color = color;
	}

	// Token: 0x06000025 RID: 37 RVA: 0x00002B8F File Offset: 0x00000D8F
	public void OnRecycle()
	{
		base.gameObject.SetActive(false);
		this.textRectTransform.anchoredPosition = Vector2.zero;
		this.textDamage.CrossFadeAlpha(1f, 0f, true);
	}

	// Token: 0x06000026 RID: 38 RVA: 0x00002BC8 File Offset: 0x00000DC8
	public void OnUse(Vector3 pos, string text)
	{
		this.bornPos = pos;
		this.textDamage.transform.localScale = Vector3.one;
		base.transform.position = FloatText.WorldCam.WorldToScreenPoint(pos);
		this.rawXAnchoPos = 0f;
		this.textDamage.text = text;
		base.gameObject.SetActive(true);
		this.timer = 0f;
	}

	// Token: 0x04000033 RID: 51
	public FloatTextManager mgr;

	// Token: 0x04000034 RID: 52
	public Text textDamage;

	// Token: 0x04000035 RID: 53
	[Header("Debug")]
	[SerializeField]
	private float timer;

	// Token: 0x04000036 RID: 54
	[SerializeField]
	private RectTransform textRectTransform;

	// Token: 0x04000037 RID: 55
	public static Camera _worldCam;

	// Token: 0x04000038 RID: 56
	private float rawXAnchoPos;

	// Token: 0x04000039 RID: 57
	private Vector3 bornPos;
}
