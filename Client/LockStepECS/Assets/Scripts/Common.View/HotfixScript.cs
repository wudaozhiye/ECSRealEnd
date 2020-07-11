using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x0200000D RID: 13
public class HotfixScript : MonoBehaviour
{
	// Token: 0x06000053 RID: 83 RVA: 0x0000378F File Offset: 0x0000198F
	private void Awake()
	{
		base.StartCoroutine(HotfixUtil.DoHotfix(new Action<IEnumerator>(this._StartCoroutine), new Action(this.OnFinished), new Action<float>(this.OnProgress)));
	}

	// Token: 0x06000054 RID: 84 RVA: 0x000037C2 File Offset: 0x000019C2
	private void _StartCoroutine(IEnumerator routine)
	{
		base.StartCoroutine(routine);
	}

	// Token: 0x06000055 RID: 85 RVA: 0x000037CD File Offset: 0x000019CD
	private void OnFinished()
	{
		this.progressSlider.value = 1f;
		SceneManager.LoadScene("Main");
	}

	// Token: 0x06000056 RID: 86 RVA: 0x000037EC File Offset: 0x000019EC
	private void OnProgress(float percent)
	{
		this.progressSlider.value = percent;
	}

	// Token: 0x04000068 RID: 104
	public Slider progressSlider;
}
