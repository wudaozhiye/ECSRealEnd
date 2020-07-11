using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000006 RID: 6
public class UIFloatBar : MonoBehaviour
{
	// Token: 0x17000002 RID: 2
	// (get) Token: 0x0600000F RID: 15 RVA: 0x000024DF File Offset: 0x000006DF
	private Camera cam
	{
		get
		{
			return FloatBarManager.cam;
		}
	}

	// Token: 0x17000003 RID: 3
	// (get) Token: 0x06000010 RID: 16 RVA: 0x000024E6 File Offset: 0x000006E6
	private Canvas canvas
	{
		get
		{
			return FloatBarManager.canvas;
		}
	}

	// Token: 0x17000004 RID: 4
	// (get) Token: 0x06000011 RID: 17 RVA: 0x000024ED File Offset: 0x000006ED
	public bool keepSize
	{
		get
		{
			return this.config.keepSize;
		}
	}

	// Token: 0x17000005 RID: 5
	// (get) Token: 0x06000012 RID: 18 RVA: 0x000024FA File Offset: 0x000006FA
	public bool IsDrawOffDistance
	{
		get
		{
			return this.config.IsDrawOffDistance;
		}
	}

	// Token: 0x17000006 RID: 6
	// (get) Token: 0x06000013 RID: 19 RVA: 0x00002507 File Offset: 0x00000707
	public float drawDistance
	{
		get
		{
			return this.config.drawDistance;
		}
	}

	// Token: 0x17000007 RID: 7
	// (get) Token: 0x06000014 RID: 20 RVA: 0x00002514 File Offset: 0x00000714
	public bool showHealthInfo
	{
		get
		{
			return this.config.showHealthInfo;
		}
	}

	// Token: 0x17000008 RID: 8
	// (get) Token: 0x06000015 RID: 21 RVA: 0x00002521 File Offset: 0x00000721
	public HealthInfoAlignment healthInfoAlignment
	{
		get
		{
			return this.config.healthInfoAlignment;
		}
	}

	// Token: 0x17000009 RID: 9
	// (get) Token: 0x06000016 RID: 22 RVA: 0x0000252E File Offset: 0x0000072E
	public float healthInfoSize
	{
		get
		{
			return this.config.healthInfoSize;
		}
	}

	// Token: 0x06000017 RID: 23 RVA: 0x0000253C File Offset: 0x0000073C
	private void Awake()
	{
		this.config = FloatBarManager.config;
		bool flag = this.healthVolume == null;
		if (flag)
		{
			this.healthVolume = base.transform.Find("Health").GetComponent<Image>();
		}
		bool flag2 = this.backGround == null;
		if (flag2)
		{
			this.backGround = base.transform.Find("Background").GetComponent<Image>();
		}
		bool flag3 = this.healthInfo == null;
		if (flag3)
		{
			this.healthInfo = base.transform.Find("HealthInfo").GetComponent<Text>();
		}
		this.rectTransform = base.GetComponent<RectTransform>();
		this.canvasGroup = base.GetComponent<CanvasGroup>();
		this.healInfoRectTrans = this.healthInfo.rectTransform;
		this.healthInfo.resizeTextForBestFit = true;
		this.healInfoRectTrans.anchoredPosition = Vector2.zero;
		this.healthInfo.resizeTextMinSize = 1;
		this.healthInfo.resizeTextMaxSize = 500;
		this.healthInfoPosition = this.healthInfo.rectTransform.anchoredPosition;
		this.rawSizeDelta = this.rectTransform.sizeDelta;
		this.canvasGroup.alpha = this.config.fullAlpha;
		this.canvasGroup.interactable = false;
		this.canvasGroup.blocksRaycasts = false;
		if (healthInfoAlignment == HealthInfoAlignment.Top)
		{
			healInfoRectTrans.anchoredPosition=healthInfoPosition;
		}
		else if (healthInfoAlignment == HealthInfoAlignment.Center)
		{
			healInfoRectTrans.anchoredPosition=Vector2.zero;
		}
		else
		{
			healInfoRectTrans.anchoredPosition = -healthInfoPosition;
		}
	}

	// Token: 0x06000018 RID: 24 RVA: 0x000026F3 File Offset: 0x000008F3
	public void OnRecycle()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x06000019 RID: 25 RVA: 0x00002704 File Offset: 0x00000904
	public void OnUse(Transform tran, int val, int maxVal)
	{
		this.taragetTrans = tran;
		Vector3 position = tran.position;
		base.transform.position = FloatBarManager.cam.WorldToScreenPoint(new Vector3(position.x, position.y + this.yOffset, position.z));
		base.gameObject.SetActive(true);
		this.UpdateHp(val, maxVal);
	}

	// Token: 0x0600001A RID: 26 RVA: 0x0000276C File Offset: 0x0000096C
	public void UpdateHp(int curVal, int maxVal)
	{
		this.healthVolume.fillAmount = (float)curVal / ((float)maxVal * 1f);
		this.delayTimestamp = Time.time + this.config.onHit.duration;
		bool showHealthInfo = this.showHealthInfo;
		if (showHealthInfo)
		{
			this.healthInfo.text = curVal + " / " + maxVal;
		}
		else
		{
			this.healthInfo.text = "";
		}
		this._curVal = curVal;
		this._maxVal = maxVal;
		bool flag = this._curVal <= 0;
		if (flag)
		{
			this._curVal = 0;
		}
		bool flag2 = this._curVal > this._maxVal;
		if (flag2)
		{
			this._maxVal = this._curVal;
		}
	}

	// Token: 0x0600001B RID: 27 RVA: 0x00002830 File Offset: 0x00000A30
	public void DoUpdate(float deltaTime)
	{
		bool flag = !this.rectTransform;
		if (!flag)
		{
			bool flag2 = this.taragetTrans == null;
			if (!flag2)
			{
				Vector3 position = this.taragetTrans.position;
				Vector3 position2 = FloatBarManager.cam.WorldToScreenPoint(new Vector3(position.x, position.y + this.yOffset, position.z));
				base.transform.position = position2;
				float num = Vector3.Dot(this.taragetTrans.position - this.cam.transform.position, this.cam.transform.forward);
				float num2 = this.keepSize ? (num / this.scale) : this.scale;
				float num3 = 0.1f;
				bool flag3 = this.backGround.fillAmount > this.healthVolume.fillAmount + num3;
				if (flag3)
				{
					this.backGround.fillAmount = this.healthVolume.fillAmount + num3;
				}
				bool flag4 = this.backGround.fillAmount > this.healthVolume.fillAmount;
				if (flag4)
				{
					this.backGround.fillAmount -= 1f / ((float)this._maxVal / 100f) * deltaTime;
				}
				else
				{
					this.backGround.fillAmount = this.healthVolume.fillAmount;
				}
			}
		}
	}

	// Token: 0x0600001C RID: 28 RVA: 0x000029A0 File Offset: 0x00000BA0
	private bool IsVisible()
	{
		return this.canvas.pixelRect.Contains(this.rectTransform.position);
	}

	// Token: 0x0600001D RID: 29 RVA: 0x000029D0 File Offset: 0x00000BD0
	private bool OutDistance(float camDistance)
	{
		return this.IsDrawOffDistance && camDistance > this.drawDistance;
	}

	// Token: 0x04000022 RID: 34
	[Header("UI ref")]
	public Image healthVolume;

	// Token: 0x04000023 RID: 35
	public Image backGround;

	// Token: 0x04000024 RID: 36
	public Text healthInfo;

	// Token: 0x04000025 RID: 37
	private RectTransform healInfoRectTrans;

	// Token: 0x04000026 RID: 38
	private RectTransform rectTransform;

	// Token: 0x04000027 RID: 39
	[Header("Prefab define")]
	public FloatBarConfig config;

	// Token: 0x04000028 RID: 40
	public float yOffset = 2.55f;

	// Token: 0x04000029 RID: 41
	public float scale = 1f;

	// Token: 0x0400002A RID: 42
	public Vector2 sizeOffsets;

	// Token: 0x0400002B RID: 43
	private Vector2 rawSizeDelta;

	// Token: 0x0400002C RID: 44
	private Vector2 healthInfoPosition;

	// Token: 0x0400002D RID: 45
	private Transform taragetTrans;

	// Token: 0x0400002E RID: 46
	private int _maxVal;

	// Token: 0x0400002F RID: 47
	private int _curVal;

	// Token: 0x04000030 RID: 48
	private float camDistance;

	// Token: 0x04000031 RID: 49
	private float delayTimestamp;

	// Token: 0x04000032 RID: 50
	private CanvasGroup canvasGroup;
}
