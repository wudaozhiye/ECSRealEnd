using System;
using System.IO;
using UnityEngine;

namespace Lockstep.Game
{
	// Token: 0x02000024 RID: 36
	public static class UnityPathUtil
	{
		// Token: 0x060000D9 RID: 217 RVA: 0x0000656C File Offset: 0x0000476C
		public static string GetFilePath(string dirName, string fileName)
		{
			string streamingAssetsPath = UnityPathUtil.GetStreamingAssetsPath();
			string path = Path.Combine(streamingAssetsPath, dirName.Replace("\\", "/"));
			return Path.Combine(path, fileName).Replace("\\", "/");
		}

		// Token: 0x060000DA RID: 218 RVA: 0x000065B4 File Offset: 0x000047B4
		public static string GetFilePath(string path)
		{
			string streamingAssetsPath = UnityPathUtil.GetStreamingAssetsPath();
			return Path.Combine(streamingAssetsPath, path.Replace("\\", "/"));
		}

		// Token: 0x060000DB RID: 219 RVA: 0x000065E4 File Offset: 0x000047E4
		public static string GetFileName(string path)
		{
			path = path.Replace("\\", "/");
			path = path.Remove(0, path.LastIndexOf("/", StringComparison.Ordinal) + 1);
			return path.Remove(path.LastIndexOf(".", StringComparison.Ordinal));
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00006634 File Offset: 0x00004834
		public static string GetFileNameWithPostfix(string path)
		{
			path = path.Replace("\\", "/");
			return path.Remove(0, path.LastIndexOf("/", StringComparison.Ordinal) + 1);
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00006670 File Offset: 0x00004870
		public static void DeleteFile(string deletePath, bool isRefresh = true)
		{
			string path = deletePath;
			bool flag = deletePath.IndexOf(Application.dataPath, StringComparison.Ordinal) == -1;
			if (flag)
			{
				path = Path.Combine(Application.dataPath, deletePath);
			}
			bool flag2 = File.Exists(path);
			if (flag2)
			{
				File.Delete(path);
			}
		}

		// Token: 0x060000DE RID: 222 RVA: 0x000066B4 File Offset: 0x000048B4
		public static void DeleteDir(string dir, bool isRefresh = true)
		{
			bool flag = Directory.Exists(dir);
			if (flag)
			{
				Directory.Delete(dir, true);
			}
		}

		// Token: 0x060000DF RID: 223 RVA: 0x000066D8 File Offset: 0x000048D8
		public static void SaveToFile(string SavePath, string content, bool isRefresh = true)
		{
			string path = Path.Combine(Application.dataPath, SavePath);
			string directoryName = Path.GetDirectoryName(path);
			bool flag = !Directory.Exists(directoryName);
			if (flag)
			{
				Directory.CreateDirectory(directoryName);
			}
			File.WriteAllText(path, content);
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00006718 File Offset: 0x00004918
		public static string GetStreamingAssetsPath()
		{
			return "";
		}
	}
}
