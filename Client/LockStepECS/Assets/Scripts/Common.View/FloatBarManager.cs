using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

// Token: 0x02000005 RID: 5
public class FloatBarManager : MonoBehaviour
{
	// Token: 0x17000001 RID: 1
	// (get) Token: 0x06000003 RID: 3 RVA: 0x000020FF File Offset: 0x000002FF
	public static Canvas canvas
	{
		get
		{
			return FloatBarManager._Instance._canvas;
		}
	}

	// Token: 0x06000004 RID: 4 RVA: 0x0000210C File Offset: 0x0000030C
	private void Awake()
	{
		FloatBarManager.cam = Camera.main;
		this.cameraTransform = FloatBarManager.cam.transform;
		for (int i = 0; i < base.transform.childCount; i++)
		{
			this.healthBars.Add(base.transform.GetChild(i));
		}
		FloatBarManager._Instance = this;
		bool flag = this.prefab == null;
		if (flag)
		{
			this.prefab = (GameObject)Resources.Load("Prefabs/FloatingDamage");
		}
		bool flag2 = this._config == null;
		if (flag2)
		{
			this._config = (FloatBarConfig)Resources.Load("FloatBarConfig");
		}
		FloatBarManager.config = this._config;
		this.poolTrans = this._CreateTrans("HealthbarPool");
		this.showTrans = this._CreateTrans("HealthbarRoot");
	}

	// Token: 0x06000005 RID: 5 RVA: 0x000021E8 File Offset: 0x000003E8
	private Transform _CreateTrans(string name)
	{
		Transform transform = new GameObject(name).transform;
		transform.SetParent(FloatBarManager.canvas.transform, false);
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
		transform.localScale = Vector3.one;
		return transform;
	}

	// Token: 0x06000006 RID: 6 RVA: 0x00002240 File Offset: 0x00000440
	private void Update()
	{
		float deltaTime = Time.deltaTime;
		this.healthBars.Sort(new Comparison<Transform>(this.DistanceCompare));
		for (int i = 0; i < this.healthBars.Count; i++)
		{
			this.healthBars[i].SetSiblingIndex(this.healthBars.Count - (i + 1));
		}
		foreach (UIFloatBar uifloatBar in this.allFloatElem)
		{
			uifloatBar.DoUpdate(deltaTime);
		}
	}

	// Token: 0x06000007 RID: 7 RVA: 0x000022F4 File Offset: 0x000004F4
	private int DistanceCompare(Transform a, Transform b)
	{
		return Mathf.Abs((this.WorldPos(a.position) - this.cameraTransform.position).sqrMagnitude).CompareTo(Mathf.Abs((this.WorldPos(b.position) - this.cameraTransform.position).sqrMagnitude));
	}

	// Token: 0x06000008 RID: 8 RVA: 0x00002360 File Offset: 0x00000560
	private Vector3 WorldPos(Vector3 pos)
	{
		return FloatBarManager.cam.ScreenToWorldPoint(pos);
	}

	// Token: 0x06000009 RID: 9 RVA: 0x00002380 File Offset: 0x00000580
	private void _DestroyText(UIFloatBar text)
	{
		bool flag = this.pools.Count > FloatBarManager._Instance.maxPoolCount;
		if (flag)
		{
			Object.Destroy(text);
		}
		else
		{
			text.OnRecycle();
			text.transform.SetParent(this.poolTrans, false);
			this.pools.Enqueue(text);
		}
	}

	// Token: 0x0600000A RID: 10 RVA: 0x000023DC File Offset: 0x000005DC
	private UIFloatBar GetOrCreateText()
	{
		bool flag = this.pools.Count > 0;
		UIFloatBar result;
		if (flag)
		{
			UIFloatBar uifloatBar = this.pools.Dequeue();
			uifloatBar.transform.SetParent(this.showTrans, false);
			result = uifloatBar;
		}
		else
		{
			GameObject gameObject = Object.Instantiate<GameObject>(this.prefab, this.showTrans, false);
			result = gameObject.GetComponent<UIFloatBar>();
		}
		return result;
	}

	// Token: 0x0600000B RID: 11 RVA: 0x0000243D File Offset: 0x0000063D
	public static void DestroyText(UIFloatBar trans)
	{
		FloatBarManager instance = FloatBarManager._Instance;
		if (instance != null)
		{
			instance._DestroyText(trans);
		}
	}

	// Token: 0x0600000C RID: 12 RVA: 0x00002454 File Offset: 0x00000654
	public static UIFloatBar CreateFloatBar(Transform trans, int val, int maxVal)
	{
		FloatBarManager instance = FloatBarManager._Instance;
		return (instance != null) ? instance._CreateFloatBar(trans, val, maxVal) : null;
	}

	// Token: 0x0600000D RID: 13 RVA: 0x0000247C File Offset: 0x0000067C
	private UIFloatBar _CreateFloatBar(Transform trans, int val, int maxVal)
	{
		UIFloatBar orCreateText = this.GetOrCreateText();
		this.allFloatElem.Add(orCreateText);
		orCreateText.OnUse(trans, val, maxVal);
		return orCreateText;
	}

	// Token: 0x04000015 RID: 21
	private static FloatBarManager _Instance;

	// Token: 0x04000016 RID: 22
	private List<Transform> healthBars = new List<Transform>();

	// Token: 0x04000017 RID: 23
	private Transform cameraTransform;

	// Token: 0x04000018 RID: 24
	[Header("Pools")]
	public GameObject prefab;

	// Token: 0x04000019 RID: 25
	public int maxPoolCount = 50;

	// Token: 0x0400001A RID: 26
	public Canvas _canvas;

	// Token: 0x0400001B RID: 27
	private List<UIFloatBar> allFloatElem = new List<UIFloatBar>();

	// Token: 0x0400001C RID: 28
	private Queue<UIFloatBar> pools = new Queue<UIFloatBar>();

	// Token: 0x0400001D RID: 29
	public FloatBarConfig _config;

	// Token: 0x0400001E RID: 30
	public static FloatBarConfig config;

	// Token: 0x0400001F RID: 31
	public static Camera cam;

	// Token: 0x04000020 RID: 32
	private Transform poolTrans;

	// Token: 0x04000021 RID: 33
	private Transform showTrans;
}
