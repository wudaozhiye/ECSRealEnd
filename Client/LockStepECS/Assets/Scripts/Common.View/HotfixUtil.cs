using System;
using System.Collections;
using System.IO;
using Lockstep.Game;

// Token: 0x02000015 RID: 21
public class HotfixUtil : object
{
	// Token: 0x0600006E RID: 110 RVA: 0x000050C1 File Offset: 0x000032C1
	public static IEnumerator DoHotfix(Action<IEnumerator> startCoroutineFunc, Action finishCallback, Action<float> progressCallback)
	{
		bool isFinishCopy = false;
		float copyPercent = 0.3f;
		string tempPath = ProjectConfig.DataPath;
		IEnumerator copyPart = HotfixUtil.CopyFromStreamingAssets(ProjectConfig.StreamDataPath, tempPath, "*.zip", startCoroutineFunc, delegate
		{
			isFinishCopy = true;
		}, delegate(float val)
		{
			progressCallback(val * copyPercent);
		});
		startCoroutineFunc(copyPart);
		while (!isFinishCopy)
		{
			yield return null;
		}
		bool isFinishUnzip = false;
		IEnumerator unzipPart = HotfixUtil.UnzipFiles(tempPath, "*.zip", delegate
		{
			isFinishUnzip = true;
		}, delegate(float val)
		{
			progressCallback(copyPercent + val * (1f - copyPercent));
		});
		startCoroutineFunc(unzipPart);
		while (!isFinishUnzip)
		{
			yield return null;
		}
		finishCallback();
		yield break;
	}

	// Token: 0x0600006F RID: 111 RVA: 0x000050DE File Offset: 0x000032DE
	private static IEnumerator UnzipFiles(string srcDir, string ext, Action finishCallback, Action<float> progressCallback)
	{
		int totalCount = 0;
		int couter = 0;
		Action onFileFinished = delegate
		{
			couter++;
			Action<float> progressCallback2 = progressCallback;
			if (progressCallback2 != null)
			{
				progressCallback2(1f * (float)couter / (float)totalCount);
			}
		};
		string[] files = Directory.GetFiles(srcDir, ext);
		totalCount = files.Length;
		foreach (string filePath in files)
		{
			string dstDir = filePath.Substring(0, filePath.Length - ext.Length + 1);
			bool flag = Directory.Exists(dstDir);
			if (flag)
			{
				Directory.Delete(dstDir, true);
			}
			ZipUtil.UnZip(filePath, dstDir);
			File.Delete(filePath);
			onFileFinished();
			
		}
		string[] array = null;
		while (totalCount > couter)
		{
			yield return null;
		}
		finishCallback();
		yield break;
	}

	// Token: 0x06000070 RID: 112 RVA: 0x00005102 File Offset: 0x00003302
	private static IEnumerator CopyFromStreamingAssets(string srcDir, string dstDir, string ext, Action<IEnumerator> startCoroutineFunc, Action finishCallback, Action<float> progressCallback)
	{
		bool flag = !Directory.Exists(dstDir);
		if (flag)
		{
			Directory.CreateDirectory(dstDir);
		}
		int totalCount = 0;
		int couter = 0;
		string[] files = Directory.GetFiles(srcDir, ext);
		totalCount = files.Length;
		foreach (string filePath in files)
		{
			string fileName = Path.GetFileName(filePath);
			string dstPath = fileName;
			ProjectConfig.CopyFileFromSAPathToPDPath(fileName, dstPath);
			int num = couter;
			couter = num + 1;
			if (progressCallback != null)
			{
				progressCallback(1f * (float)couter / (float)totalCount);
			}
		}
		string[] array = null;
		finishCallback();
		yield return null;
		yield break;
	}
}
