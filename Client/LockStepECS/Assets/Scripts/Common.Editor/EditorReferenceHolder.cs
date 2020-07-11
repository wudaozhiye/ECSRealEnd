using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Lockstep.Game;
using Lockstep.Game.UI;
using Lockstep.Math;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000003 RID: 3
[CustomEditor(typeof(ReferenceHolder))]
public class EditorReferenceHolder : Editor
{
	// Token: 0x06000003 RID: 3 RVA: 0x0000205C File Offset: 0x0000025C
	protected virtual void OnEnable()
	{
		this.m_RefHolder = (ReferenceHolder)base.target;
		this.m_ReorderLst = new ReorderableList(base.serializedObject, base.serializedObject.FindProperty("Datas"));
		this.m_ReorderLst.drawElementCallback = new ReorderableList.ElementCallbackDelegate(this.DrawChild);
		this.m_ReorderLst.onAddCallback = new ReorderableList.AddCallbackDelegate(this.AddButton);
		this.m_ReorderLst.drawHeaderCallback = new ReorderableList.HeaderCallbackDelegate(this.DrawHeader);
		this.m_ReorderLst.onRemoveCallback = new ReorderableList.RemoveCallbackDelegate(this.RemoveButton);
	}

	// Token: 0x06000004 RID: 4 RVA: 0x000020F8 File Offset: 0x000002F8
	public override void OnInspectorGUI()
	{
		EditorGUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		this.ShowSearchTool();
		EditorGUILayout.EndHorizontal();
		this.m_ReorderLst.DoLayoutList();
		GUI.color = Color.white;
		this.ShowAutoImport();
		EditorGUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		this.ShowAutoRename();
		this.ShowClearAll();
		this.ShowAutoDetect();
		this.ShowAutoBinding();
		EditorGUILayout.EndHorizontal();
	}

	// Token: 0x06000005 RID: 5 RVA: 0x0000216C File Offset: 0x0000036C
	public Rect[] GetRects(Rect r)
	{
		Rect[] array = new Rect[6];
		float width = r.width;
		float num = 50f;
		float num2 = 50f;
		float num3 = 40f;
		float num4 = 4f;
		float num5 = (float)Mathf.FloorToInt((width - num - num2 - num3 - num4 * 5f) / 3f);
		int num6 = 0;
		float num7 = r.x;
		array[num6++] = new Rect(num7, r.y, num, r.height);
		num7 += num + num4;
		array[num6++] = new Rect(num7, r.y, num5, r.height);
		num7 += num5 + num4;
		array[num6++] = new Rect(num7, r.y, num2, r.height);
		num7 += num2 + num4;
		array[num6++] = new Rect(num7, r.y, num5, r.height);
		num7 += num5 + num4;
		array[num6++] = new Rect(num7, r.y, num5, r.height);
		num7 += num5 + num4;
		array[num6++] = new Rect(num7, r.y, num3, r.height);
		return array;
	}

	// Token: 0x06000006 RID: 6 RVA: 0x000022DC File Offset: 0x000004DC
	private void DrawChild(Rect r, int index, bool selected, bool focused)
	{
		bool flag = index >= this.m_ReorderLst.serializedProperty.arraySize;
		if (!flag)
		{
			RefData refData = this.m_RefHolder.Datas[index];
			bool flag2 = string.IsNullOrEmpty(refData.TypeName);
			if (!flag2)
			{
				SerializedProperty arrayElementAtIndex = this.m_ReorderLst.serializedProperty.GetArrayElementAtIndex(index);
				float y = r.y;
				r.y = y + 1f;
				r.height = 16f;
				Rect[] rects = this.GetRects(r);
				GUI.color = Color.white;
				bool hasVal = refData.hasVal;
				if (hasVal)
				{
					GUI.color = Color.red;
				}
				else
				{
					bool flag3 = this.m_SearchMatchedData.Contains(refData);
					if (flag3)
					{
						GUI.color = Color.yellow;
					}
				}
				bool flag4 = GUI.Button(rects[0], index.ToString());
				if (flag4)
				{
				}
				refData.name = EditorGUI.TextField(rects[1], refData.name);
				bool flag5 = GUI.Button(rects[2], "→");
				if (flag5)
				{
					bool flag6 = refData.GetGameObject() != null;
					if (flag6)
					{
						refData.GetGameObject().name = refData.name;
						base.serializedObject.ApplyModifiedProperties();
					}
				}
				EComponentType ecomponentType = (EComponentType)Enum.Parse(typeof(EComponentType), refData.TypeName);
				bool flag7 = ecomponentType <= EComponentType.GameObject;
				if (flag7)
				{
					UnityEngine.Object @object = EditorGUI.ObjectField(rects[3], refData.bindObj, typeof(System.Object), true);
					bool flag8 = refData.bindObj != @object;
					if (flag8)
					{
						refData.bindObj = @object;
						refData = this.GetData(refData.GetGameObject());
						base.serializedObject.ApplyModifiedProperties();
					}
				}
				else
				{
					bool flag9 = ecomponentType == EComponentType.Number;
					if (flag9)
					{
						bool flag10 = refData.bindVal.GetType() != typeof(LFloat);
						if (flag10)
						{
							refData.bindVal = LFloat.zero;
						}
						float num = EditorGUI.FloatField(rects[3], ((LFloat)refData.bindVal).ToFloat());
						refData.bindVal = new LFloat(true, (long)(num * 1000000f));
					}
					else
					{
						bool flag11 = ecomponentType == EComponentType.Color;
						if (flag11)
						{
							bool flag12 = refData.bindVal.GetType() != typeof(Color);
							if (flag12)
							{
								refData.bindVal = Color.white;
							}
							refData.bindVal = EditorGUI.ColorField(rects[3], (Color)refData.bindVal);
						}
					}
				}
				bool flag13 = !string.IsNullOrEmpty(refData.TypeName);
				if (flag13)
				{
					string text = EditorGUI.EnumPopup(rects[4], (EComponentType)Enum.Parse(typeof(EComponentType), refData.TypeName)).ToString();
					bool flag14 = refData.TypeName != text;
					if (flag14)
					{
						refData.TypeName = text;
						base.serializedObject.ApplyModifiedProperties();
					}
				}
				bool flag15 = GUI.Button(rects[5], "×");
				if (flag15)
				{
					this.m_ReorderLst.serializedProperty.DeleteArrayElementAtIndex(index);
					base.serializedObject.ApplyModifiedProperties();
				}
			}
		}
	}

	// Token: 0x06000007 RID: 7 RVA: 0x00002644 File Offset: 0x00000844
	private void DrawHeader(Rect headerRect)
	{
		headerRect.xMin += 14f;
		float y = headerRect.y;
		headerRect.y = y + 1f;
		headerRect.height = 15f;
		Rect[] rects = this.GetRects(headerRect);
		int num = 0;
		string[] array = new string[]
		{
			"order",
			"name",
			"rename",
			"content",
			"type",
			"delete"
		};
		for (int i = 0; i < rects.Length; i++)
		{
			GUI.Label(rects[num], array[i], EditorStyles.label);
			num++;
		}
	}

	// Token: 0x06000008 RID: 8 RVA: 0x000026FE File Offset: 0x000008FE
	private void RemoveButton(ReorderableList list)
	{
		this.m_ReorderLst.serializedProperty.DeleteArrayElementAtIndex(list.index);
		base.serializedObject.ApplyModifiedProperties();
	}

	// Token: 0x06000009 RID: 9 RVA: 0x00002724 File Offset: 0x00000924
	public void AddButton(ReorderableList list)
	{
		GenericMenu genericMenu = new GenericMenu();
		genericMenu.AddItem(new GUIContent("Add Component Param"), false, new GenericMenu.MenuFunction(this.AddComponentData));
		genericMenu.AddItem(new GUIContent("Add Number Param"), false, new GenericMenu.MenuFunction(this.AddNumberData));
		genericMenu.AddItem(new GUIContent("Add Color Param"), false, new GenericMenu.MenuFunction(this.AddColorData));
		genericMenu.ShowAsContext();
	}

	// Token: 0x0600000A RID: 10 RVA: 0x00002799 File Offset: 0x00000999
	public void AddComponentData()
	{
		this.m_RefHolder.Datas.Add(new RefData("New GameObejct", EComponentType.GameObject, null));
		base.serializedObject.ApplyModifiedProperties();
	}

	// Token: 0x0600000B RID: 11 RVA: 0x000027C6 File Offset: 0x000009C6
	public void AddNumberData()
	{
		this.m_RefHolder.Datas.Add(new RefData("New Number", EComponentType.Number, LFloat.zero));
		base.serializedObject.ApplyModifiedProperties();
	}

	// Token: 0x0600000C RID: 12 RVA: 0x000027FC File Offset: 0x000009FC
	public void AddColorData()
	{
		this.m_RefHolder.Datas.Add(new RefData("New Color", EComponentType.Color, Color.white));
		base.serializedObject.ApplyModifiedProperties();
	}

	// Token: 0x0600000D RID: 13 RVA: 0x00002834 File Offset: 0x00000A34
	private void ShowSearchTool()
	{
		GUILayout.Label("Search", new GUILayoutOption[]
		{
			GUILayout.Width(50f)
		});
		this.searchName = EditorGUILayout.TextField(this.searchName, Array.Empty<GUILayoutOption>());
		bool flag = GUILayout.Button("×", new GUILayoutOption[]
		{
			GUILayout.Width(50f)
		});
		if (flag)
		{
			this.searchName = string.Empty;
		}
		this.SearchDatas(this.searchName);
	}

	// Token: 0x0600000E RID: 14 RVA: 0x000028B4 File Offset: 0x00000AB4
	public void SearchDatas(string searchName)
	{
		this.m_SearchMatchedData.Clear();
		bool flag = string.IsNullOrEmpty(searchName);
		if (!flag)
		{
			for (int i = 0; i < this.m_RefHolder.Datas.Count; i++)
			{
				bool flag2 = this.m_RefHolder.Datas[i].name.Contains(searchName);
				if (flag2)
				{
					this.m_SearchMatchedData.Add(this.m_RefHolder.Datas[i]);
				}
			}
		}
	}

	// Token: 0x0600000F RID: 15 RVA: 0x0000293C File Offset: 0x00000B3C
	private void ShowAutoImport()
	{
		GameObject gameObject = EditorGUILayout.ObjectField(null, typeof(GameObject), true, new GUILayoutOption[]
		{
			GUILayout.Height(50f)
		}) as GameObject;
		bool flag = gameObject != null;
		if (flag)
		{
			bool flag2 = gameObject.transform.childCount > 0;
			if (flag2)
			{
				bool flag3 = EditorUtility.DisplayDialog("Warning:", "Should import children nodes\r\n\r\ncurrent node:" + gameObject.name, "Yes", "No");
				if (flag3)
				{
					Transform[] componentsInChildren = gameObject.GetComponentsInChildren<Transform>(true);
					foreach (Transform transform in componentsInChildren)
					{
						bool flag4 = transform != gameObject.transform;
						if (flag4)
						{
							this.AddGo(transform.gameObject);
						}
					}
				}
			}
			this.AddGo(gameObject);
		}
	}

	// Token: 0x06000010 RID: 16 RVA: 0x00002A18 File Offset: 0x00000C18
	private void ShowAutoRename()
	{
		bool flag = GUILayout.Button("Auto rename", Array.Empty<GUILayoutOption>());
		if (flag)
		{
			RectTransform[] componentsInChildren = this.m_RefHolder.gameObject.GetComponentsInChildren<RectTransform>(true);
			int num = 0;
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				bool flag2 = componentsInChildren[i].GetComponent<Image>() != null;
				if (flag2)
				{
					componentsInChildren[i].gameObject.name = "Image_" + num;
					num++;
				}
			}
		}
	}

	// Token: 0x06000011 RID: 17 RVA: 0x00002AA0 File Offset: 0x00000CA0
	private void ShowClearAll()
	{
		bool flag = GUILayout.Button("ClearAll", Array.Empty<GUILayoutOption>());
		if (flag)
		{
			this.m_ReorderLst.serializedProperty.ClearArray();
			base.serializedObject.ApplyModifiedProperties();
		}
	}

	// Token: 0x06000012 RID: 18 RVA: 0x00002AE0 File Offset: 0x00000CE0
	private void ShowAutoDetect()
	{
		bool flag = GUILayout.Button("Auto Detect", Array.Empty<GUILayoutOption>());
		if (flag)
		{
			for (int i = 0; i < this.m_RefHolder.Datas.Count; i++)
			{
				for (int j = 0; j < this.m_RefHolder.Datas.Count; j++)
				{
					bool flag2 = i != j && this.m_RefHolder.Datas[i].name == this.m_RefHolder.Datas[j].name;
					if (flag2)
					{
						this.m_RefHolder.Datas[i].hasVal = true;
					}
				}
			}
			this.m_RefHolder.Datas.Sort(new Comparison<RefData>(this.ListSort));
		}
	}

	// Token: 0x06000013 RID: 19 RVA: 0x00002BC4 File Offset: 0x00000DC4
	private void ShowAutoBinding()
	{
		bool flag = GUILayout.Button("AutoBind", Array.Empty<GUILayoutOption>());
		if (flag)
		{
			Transform transform = this.m_RefHolder.transform;
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			Type[] array = (from tt in assemblies.SelectMany((Assembly assembly) => assembly.GetTypes())
			where typeof(UIBaseWindow).IsAssignableFrom(tt) || (typeof(UIBaseItem).IsAssignableFrom(tt) && !tt.IsAbstract)
			select tt).ToArray<Type>();
			Type type = null;
			foreach (Type type2 in array)
			{
				bool flag2 = type2.Name == transform.name;
				if (flag2)
				{
					type = type2;
					break;
				}
			}
			bool flag3 = type == null;
			if (flag3)
			{
				Debug.Log("No type " + transform.name);
			}
			else
			{
				this.m_RefHolder.Datas.Clear();
				HashSet<Type> targetTypes = new HashSet<Type>
				{
					typeof(Button),
					typeof(Text),
					typeof(Image),
					typeof(Toggle),
					typeof(Slider),
					typeof(InputField),
					typeof(LayoutGroup),
					typeof(Dropdown),
					typeof(GameObject),
					typeof(RawImage),
					typeof(Transform)
				};
				PropertyInfo[] array3 = (from tt in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
				where targetTypes.Contains(tt.PropertyType)
				select tt).ToArray<PropertyInfo>();
				foreach (PropertyInfo propertyInfo in array3)
				{
					string name = propertyInfo.Name;
					Transform transform2 = this.RecursiveFindChild(name, transform, propertyInfo.PropertyType);
					bool flag4 = transform2 != null;
					if (flag4)
					{
						this.AddGo(transform2.gameObject, propertyInfo.PropertyType);
					}
				}
				this.m_ReorderLst.serializedProperty.ClearArray();
				base.serializedObject.ApplyModifiedProperties();
			}
		}
	}

	// Token: 0x06000014 RID: 20 RVA: 0x00002E5C File Offset: 0x0000105C
	private Transform RecursiveFindChild(string name, Transform parent, Type targetType)
	{
		Queue<Transform> queue = new Queue<Transform>();
		queue.Enqueue(parent);
		while (queue.Count > 0)
		{
			Transform transform = queue.Dequeue();
			bool flag = transform.name == name;
			if (flag)
			{
				bool flag2 = targetType == typeof(GameObject) || targetType == typeof(Transform);
				Transform result;
				if (flag2)
				{
					result = transform;
				}
				else
				{
					bool flag3 = transform.gameObject.GetComponent(targetType) != null;
					if (!flag3)
					{
						goto IL_79;
					}
					result = transform;
				}
				return result;
			}
			IL_79:
			foreach (object obj in transform)
			{
				Transform item = (Transform)obj;
				queue.Enqueue(item);
			}
		}
		return null;
	}

	// Token: 0x06000015 RID: 21 RVA: 0x00002F54 File Offset: 0x00001154
	private int ListSort(RefData x, RefData y)
	{
		bool flag = x.hasVal && !y.hasVal;
		int result;
		if (flag)
		{
			result = 1;
		}
		else
		{
			bool flag2 = !x.hasVal && y.hasVal;
			if (flag2)
			{
				result = -1;
			}
			else
			{
				result = 0;
			}
		}
		return result;
	}

	// Token: 0x06000016 RID: 22 RVA: 0x00002FA0 File Offset: 0x000011A0
	private void Move(int i, int type)
	{
		bool flag = type == 1;
		if (flag)
		{
			bool flag2 = i != 0;
			if (flag2)
			{
				RefData value = this.m_RefHolder.Datas[i];
				this.m_RefHolder.Datas[i] = this.m_RefHolder.Datas[i - 1];
				this.m_RefHolder.Datas[i - 1] = value;
			}
		}
		else
		{
			bool flag3 = type == 2;
			if (flag3)
			{
				bool flag4 = i != this.m_RefHolder.Datas.Count - 1;
				if (flag4)
				{
					RefData value2 = this.m_RefHolder.Datas[i];
					this.m_RefHolder.Datas[i] = this.m_RefHolder.Datas[i + 1];
					this.m_RefHolder.Datas[i + 1] = value2;
				}
			}
		}
	}

	// Token: 0x06000017 RID: 23 RVA: 0x00003088 File Offset: 0x00001288
	private RefData GetData(GameObject go)
	{
		bool flag = go != null;
		RefData result;
		if (flag)
		{
			RefData refData = null;
			for (int i = 0; i < 18; i++)
			{
				EComponentType ecomponentType = (EComponentType)i;
				bool flag2 = ecomponentType != EComponentType.GameObject;
				if (flag2)
				{
					Component component = go.GetComponent(ecomponentType.ToString());
					bool flag3 = component != null;
					if (flag3)
					{
						refData = new RefData(go.name, ecomponentType, component);
						break;
					}
				}
			}
			bool flag4 = refData == null;
			if (flag4)
			{
				refData = new RefData(go.name, EComponentType.GameObject, go);
			}
			result = refData;
		}
		else
		{
			result = null;
		}
		return result;
	}

	// Token: 0x06000018 RID: 24 RVA: 0x00003128 File Offset: 0x00001328
	private void AddGo(GameObject go)
	{
		RefData data = this.GetData(go);
		bool flag = data != null;
		if (flag)
		{
			this.m_RefHolder.Datas.Add(data);
		}
	}

	// Token: 0x06000019 RID: 25 RVA: 0x0000315C File Offset: 0x0000135C
	private void AddGo(GameObject go, Type type)
	{
		RefData data = this.GetData(go);
		bool flag = data != null;
		if (flag)
		{
			bool flag2 = type == typeof(GameObject);
			if (flag2)
			{
				data.bindObj = go;
			}
			else
			{
				bool flag3 = type == typeof(Transform);
				if (flag3)
				{
					data.bindObj = go.transform;
				}
				else
				{
					data.bindObj = go.GetComponent(type);
				}
			}
			data.TypeName = type.Name;
			this.m_RefHolder.Datas.Add(data);
		}
	}

	// Token: 0x04000001 RID: 1
	private ReferenceHolder m_RefHolder;

	// Token: 0x04000002 RID: 2
	private ReorderableList m_ReorderLst;

	// Token: 0x04000003 RID: 3
	[SerializeField]
	private string searchName = "";

	// Token: 0x04000004 RID: 4
	private HashSet<RefData> m_SearchMatchedData = new HashSet<RefData>();
}
