using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Lockstep.Game
{
	// Token: 0x0200001F RID: 31
	public static class ExtensionGameObject
	{
		// Token: 0x060000C0 RID: 192 RVA: 0x00005F58 File Offset: 0x00004158
		public static T GetOrAddComponent<T>(this GameObject obj) where T : Component
		{
			T component = obj.GetComponent<T>();
			bool flag = component != null;
			T result;
			if (flag)
			{
				result = component;
			}
			else
			{
				result = obj.AddComponent<T>();
			}
			return result;
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00005F8C File Offset: 0x0000418C
		public static T GetOrAddComponent<T>(this GameObject obj, Type type) where T : Component
		{
			T t = obj.GetComponent(type) as T;
			bool flag = t != null;
			T result;
			if (flag)
			{
				result = t;
			}
			else
			{
				result = (obj.AddComponent(type) as T);
			}
			return result;
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00005FD5 File Offset: 0x000041D5
		public static void DestroyExt(this Object gameObject)
		{
			Object.Destroy(gameObject);
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00005FE0 File Offset: 0x000041E0
		public static string HierarchyName(this Component comp)
		{
			return comp.transform.HierarchyName();
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00006000 File Offset: 0x00004200
		public static string HierarchyName(this GameObject go)
		{
			return go.transform.HierarchyName();
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00006020 File Offset: 0x00004220
		public static string HierarchyName(this Transform trans)
		{
			Stack<string> stack = new Stack<string>();
			StringBuilder stringBuilder = new StringBuilder();
			while (trans != null)
			{
				stack.Push(trans.name);
				trans = trans.parent;
			}
			while (stack.Count > 0)
			{
				stringBuilder.Append(stack.Pop());
				bool flag = stack.Count > 0;
				if (flag)
				{
					stringBuilder.Append("/");
				}
			}
			return stringBuilder.ToString();
		}
	}
}
