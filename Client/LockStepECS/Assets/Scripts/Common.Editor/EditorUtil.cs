using System;
using System.Collections.Generic;
using System.IO;
using Lockstep.Util;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

// Token: 0x02000004 RID: 4
public class EditorUtil
{
	// Token: 0x0600001B RID: 27 RVA: 0x0000320C File Offset: 0x0000140C
	public static void ShowMessage(string content)
	{
		EditorWindow editorWindow = null;
		bool flag = EditorWindow.focusedWindow == null;
		if (flag)
		{
			editorWindow = EditorWindow.mouseOverWindow;
		}
		if (editorWindow != null)
		{
			editorWindow.ShowNotification(new GUIContent(content));
		}
	}

	// Token: 0x0600001C RID: 28 RVA: 0x00003248 File Offset: 0x00001448
	public static void WalkAllAssets<T>(string filter, Action<T> callback, params string[] path) where T : UnityEngine.Object
	{
		try
		{
			bool flag = path == null || path.Length == 0;
			string[] array;
			if (flag)
			{
				array = AssetDatabase.FindAssets(filter);
			}
			else
			{
				array = AssetDatabase.FindAssets(filter, path);
			}
			int num = array.Length;
			int num2 = 0;
			Debug.Log("Total num " + num);
			foreach (string text in array)
			{
				string text2 = AssetDatabase.GUIDToAssetPath(text);
				T t = AssetDatabase.LoadAssetAtPath<T>(text2);
				bool flag2 = t != null;
				if (flag2)
				{
					callback(t);
				}
				else
				{
					Debug.LogErrorFormat("guid asset null path = {0} guid = {1}", new object[]
					{
						text2,
						text
					});
				}
				num2++;
				bool flag3 = EditorUtility.DisplayCancelableProgressBar("Batch Modify Prefabs", string.Format("{0}/{1} path = {2}", num2, num, text2), (float)num2 * 1f / (float)num);
				if (flag3)
				{
					Debug.LogError("Cancle Task!");
					break;
				}
			}
		}
		catch (Exception)
		{
			throw;
		}
		finally
		{
			EditorUtility.ClearProgressBar();
			AssetDatabase.Refresh();
			AssetDatabase.SaveAssets();
		}
	}

	// Token: 0x0600001D RID: 29 RVA: 0x000033A8 File Offset: 0x000015A8
	public static void WalkWithProcessBar(string path, string exts, Action<string> callback, string title = "Batch Modify Prefabs")
	{
		int count = 0;
		PathUtil.Walk(path, exts, delegate
		{
			int num2 = ++count;
		}, false, true);
		if (count != 0)
		{
			int idx = 0;
			bool isNeedCancel = false;
			try
			{
				PathUtil.Walk(path, exts, delegate
				{
					int num = ++idx;
					if (!isNeedCancel && EditorUtility.DisplayCancelableProgressBar(title, "Process " + path + "Exts:" + exts, (float)idx * 0.1f / (float)count))
					{
						isNeedCancel = true;
					}
				}, false, true);
			}
			catch (Exception)
			{
				throw;
			}
			finally
			{
				if (isNeedCancel)
				{
					Debug.Log((object)(" Task " + path + ":" + exts + " Canceled "));
				}
				EditorUtility.ClearProgressBar();
				AssetDatabase.Refresh();
				AssetDatabase.SaveAssets();
			}
		}
	}

	// Token: 0x0600001E RID: 30 RVA: 0x000034BC File Offset: 0x000016BC
	public static void WalkWithProcessBar(Func<bool> callback, Func<string> processBarInfo, Func<float> process, string title = "Batch Modify Prefabs")
	{
		try
		{
			while (callback())
			{
				bool flag = EditorUtility.DisplayCancelableProgressBar(title, processBarInfo(), process());
				if (flag)
				{
					Debug.LogError("Cancle Task!");
					break;
				}
			}
		}
		catch (Exception)
		{
			throw;
		}
		finally
		{
			EditorUtility.ClearProgressBar();
			AssetDatabase.Refresh();
			AssetDatabase.SaveAssets();
		}
	}

	// Token: 0x0600001F RID: 31 RVA: 0x00003538 File Offset: 0x00001738
	public static void WalkAllSelect(Action<GameObject> callback)
	{
		try
		{
			int num = Selection.gameObjects.Length;
			int num2 = 0;
			Debug.Log("Total num " + num);
			foreach (GameObject gameObject in Selection.gameObjects)
			{
				UnityEngine.Object @object = null;
				PrefabType prefabType = PrefabUtility.GetPrefabType(gameObject);
				bool flag = prefabType == PrefabType.PrefabInstance;
				if (flag)
				{
					@object = PrefabUtility.GetPrefabParent(gameObject);
				}
				else
				{
					bool flag2 = prefabType == PrefabType.Prefab;
					if (flag2)
					{
						@object = gameObject;
					}
				}
				num2++;
				bool flag3 = @object != null;
				if (flag3)
				{
					callback(gameObject);
					bool flag4 = @object != gameObject;
					if (flag4)
					{
						PrefabUtility.ReplacePrefab(gameObject, @object);
					}
					bool flag5 = EditorUtility.DisplayCancelableProgressBar("Batch Modify Prefabs", string.Format("{0}/{1} path = {2}", num2, num, AssetDatabase.GetAssetPath(@object)), (float)num2 * 1f / (float)num);
					if (flag5)
					{
						Debug.LogError("Cancle Task!");
						break;
					}
				}
			}
		}
		catch (Exception)
		{
			throw;
		}
		finally
		{
			EditorUtility.ClearProgressBar();
			AssetDatabase.Refresh();
			AssetDatabase.SaveAssets();
		}
	}

	// Token: 0x06000020 RID: 32 RVA: 0x00003678 File Offset: 0x00001878
	public static void WalkAllChild<T>(Transform parent, Action<T> callback) where T : Component
	{
		Queue<Transform> queue = new Queue<Transform>();
		queue.Enqueue(parent);
		while (queue.Count > 0)
		{
			Transform transform = queue.Dequeue();
			T component = transform.gameObject.GetComponent<T>();
			bool flag = component != null;
			if (flag)
			{
				callback(component);
			}
			foreach (object obj in transform)
			{
				Transform item = (Transform)obj;
				queue.Enqueue(item);
			}
		}
	}


	public static void WalkAllPrefab(string pPath, Func<GameObject, bool> callback, bool includechildfloder = true)
	{
		if (!pPath.StartsWith("/") && !pPath.Contains(Application.dataPath))
		{
			pPath = Path.Combine(Application.dataPath, pPath);
		}
		try
		{
			bool isreturn = false;
			int num = 0;
			Action<string> callback2 = delegate
			{
				int num2 = ++num;
			};
			int idx = 0;
			Action<string> callback3 = delegate(string path)
			{
				//IL_0094: Unknown result type (might be due to invalid IL or missing references)
				idx++;
				if (!isreturn)
				{
					string text = path.Replace(Application.dataPath, "Assets");
					text = text.Replace("\\", "/");
					GameObject val = AssetDatabase.LoadAssetAtPath<GameObject>(text);
					if (val != null)
					{
						GameObject val2 = Object.Instantiate<GameObject>(val);
						if (callback(val2))
						{
							Debug.Log((object)("prefab 替换成功" + val.name));
							PrefabUtility.ReplacePrefab(val2, val);
						}
						Object.DestroyImmediate(val2);
						if (EditorUtility.DisplayCancelableProgressBar("Batch Modify Prefabs", $"{idx}/{num} path = {text}", (float)idx * 1f / (float)num))
						{
							isreturn = true;
						}
					}
				}
			};
			PathUtil.Walk(pPath, "*.prefab", callback2, includechildfloder, true);
			Debug.Log((object)("Total num " + num));
			PathUtil.Walk(pPath, "*.prefab", callback3, includechildfloder, true);
			if (isreturn)
			{
				Debug.LogError((object)"Cancled Task!");
			}
		}
		catch (Exception)
		{
			throw;
		}
		finally
		{
			EditorUtility.ClearProgressBar();
			AssetDatabase.Refresh();
			AssetDatabase.SaveAssets();
		}
	}
}
