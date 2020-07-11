using System;
using System.Diagnostics;
using System.IO;
using Lockstep.Game;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using Debug = UnityEngine.Debug;

// Token: 0x02000005 RID: 5
public static class BuildTools
{
	// Token: 0x06000023 RID: 35 RVA: 0x0000384D File Offset: 0x00001A4D
	[MenuItem("LockstepEngine/BuildAll")]
	public static void BuildAll()
	{
		ProjectConfig.IsEditor = true;
		ProjectConfig.DoInit("");
		BuildTools.BuildAssetBundles();
		BuildTools.PackData();
		BuildTools.BuildPlayer();
		Debug.Log("Done!");
	}

	// Token: 0x06000024 RID: 36 RVA: 0x00002050 File Offset: 0x00000250
	public static void BuildAssetBundles()
	{
	}

	// Token: 0x06000025 RID: 37 RVA: 0x00003880 File Offset: 0x00001A80
	private static void PackData()
	{
		bool flag = !Directory.Exists(ProjectConfig.DataPath);
		if (flag)
		{
			Debug.LogError("UnExist path " + ProjectConfig.DataPath);
		}
		else
		{
			string directoryName = Path.GetDirectoryName(ProjectConfig.StreamDataPath);
			bool flag2 = !Directory.Exists(directoryName);
			if (flag2)
			{
				Directory.CreateDirectory(directoryName);
			}
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			ZipUtil.ZipDir(ProjectConfig.ExcelPath, ProjectConfig.ExcelZipPath, "*.*", 4);
			ZipUtil.ZipDir(ProjectConfig.MapPath, ProjectConfig.MapZipPath, "*.*", 4);
			stopwatch.Stop();
			Debug.LogWarningFormat("Zip Data.zip, Cost Time:{0}", new object[]
			{
				stopwatch.Elapsed.ToString()
			});
		}
	}

	// Token: 0x06000026 RID: 38 RVA: 0x00003944 File Offset: 0x00001B44
	public static string GetTargetPlayerOutputPath(BuildTarget _target)
	{
		string fullPath = Path.GetFullPath(Application.dataPath + "/../");
		return fullPath + "Builds" + BuildTools.GetTargetPlayerName(_target);
	}

	// Token: 0x06000027 RID: 39 RVA: 0x00003980 File Offset: 0x00001B80
	private static string GetTargetPlayerName(BuildTarget _target)
	{
		if (_target <= BuildTarget.StandaloneWindows)
		{
			if (_target == BuildTarget.StandaloneOSX)
			{
				return "/" + Application.productName + ".app";
			}
			if (_target != BuildTarget.StandaloneWindows)
			{
				goto IL_66;
			}
		}
		else
		{
			if (_target == BuildTarget.Android)
			{
				return "/" + Application.productName + ".apk";
			}
			if (_target != BuildTarget.StandaloneWindows64)
			{
				goto IL_66;
			}
		}
		return "/" + Application.productName + ".exe";
		IL_66:
		return "";
	}

	// Token: 0x06000028 RID: 40 RVA: 0x000039FC File Offset: 0x00001BFC
	public static bool BuildPlayer()
	{
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();
		string targetPlayerOutputPath = BuildTools.GetTargetPlayerOutputPath(EditorUserBuildSettings.activeBuildTarget);
		string directoryName = Path.GetDirectoryName(targetPlayerOutputPath);
		bool flag = !Directory.Exists(directoryName);
		if (flag)
		{
			Directory.CreateDirectory(directoryName);
		}
		BuildOptions buildOptions = BuildOptions.ShowBuiltPlayer;
		bool development = EditorUserBuildSettings.development;
		if (development)
		{
			buildOptions |= BuildOptions.Development;
		}
		bool connectProfiler = EditorUserBuildSettings.connectProfiler;
		if (connectProfiler)
		{
			buildOptions |= BuildOptions.ConnectWithProfiler;
		}
		bool allowDebugging = EditorUserBuildSettings.allowDebugging;
		if (allowDebugging)
		{
			buildOptions |= BuildOptions.AllowDebugging;
		}
		BuildReport buildReport = BuildPipeline.BuildPlayer(EditorBuildSettingsScene.GetActiveSceneList(EditorBuildSettings.scenes), targetPlayerOutputPath, EditorUserBuildSettings.activeBuildTarget, buildOptions);
		bool flag2 = buildReport.summary.totalErrors > 0;
		if (flag2)
		{
			Debug.LogError("Error MSG : " + buildReport.summary.ToString());
		}
		stopwatch.Stop();
		Debug.LogWarningFormat("BuildPlayer {0}, Cost Time:{1}", new object[]
		{
			EditorUserBuildSettings.activeBuildTarget,
			stopwatch.Elapsed.ToString()
		});
		return true;
	}
}
