using System;
using System.Collections;
using UnityEngine;

namespace Lockstep.Game
{
	// Token: 0x02000022 RID: 34
	public class UnityCoroutineUtil : SingletonMono<UnityCoroutineUtil>
	{
		// Token: 0x060000CC RID: 204 RVA: 0x00006150 File Offset: 0x00004350
		public static Coroutine Run(IEnumerator function)
		{
			bool isPlaying = Application.isPlaying;
			Coroutine result;
			if (isPlaying)
			{
				result = SingletonMono<UnityCoroutineUtil>.Instance.StartCoroutine(function);
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060000CD RID: 205 RVA: 0x0000617C File Offset: 0x0000437C
		public static void Stop(IEnumerator function)
		{
			bool isPlaying = Application.isPlaying;
			if (isPlaying)
			{
				SingletonMono<UnityCoroutineUtil>.Instance.StopCoroutine(function);
			}
		}

		// Token: 0x060000CE RID: 206 RVA: 0x000061A8 File Offset: 0x000043A8
		public static void StopAll()
		{
			bool isPlaying = Application.isPlaying;
			if (isPlaying)
			{
				SingletonMono<UnityCoroutineUtil>.Instance.StopAllCoroutines();
			}
		}
	}
}
