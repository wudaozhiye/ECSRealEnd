using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Lockstep.Game.UI;
using Lockstep.Logging;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace Lockstep.Game
{
	// Token: 0x0200001B RID: 27
	[Serializable]
	public class UnityUIService : UnityBaseService, IUIService, IService
	{
		// Token: 0x06000096 RID: 150 RVA: 0x000056EC File Offset: 0x000038EC
		public T GetIService<T>() where T : IService
		{
			return base.GetService<T>();
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000097 RID: 151 RVA: 0x00005704 File Offset: 0x00003904
		public bool IsDebugMode
		{
			get
			{
				return this._globalStateService.IsVideoMode || this._globalStateService.IsClientMode;
			}
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00005724 File Offset: 0x00003924
		public override void DoStart()
		{
			GameObject gameObject = GameObject.Find("Canvas");
			GameObject gameObject2 = Resources.Load<GameObject>("UI/" + UIDefine.UIRoot.resDir);
			bool flag = gameObject2 == null;
			if (flag)
			{
				Debug.LogError("Can not load UIRoot !" + UIDefine.UIRoot.resDir);
			}
			GameObject gameObject3 = Object.Instantiate<GameObject>(gameObject2, gameObject.transform);
			IReferenceHolder component = gameObject3.GetComponent<IReferenceHolder>();
			this.normalParent = component.GetRef<Transform>("TransNormal");
			this.forwardParent = component.GetRef<Transform>("TransForward");
			this.importParent = component.GetRef<Transform>("TransNotice");
			Debug.Log("UIStart ", null);
			bool isDebugMode = this.IsDebugMode;
			if (isDebugMode)
			{
				this.OpenWindow(UIDefine.UILoading, null);
			}
			else
			{
				this.OpenWindow(UIDefine.UILogin, null);
			}
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00005804 File Offset: 0x00003A04
		private Transform GetParentFromDepth(EWindowDepth depth)
		{
			Transform result;
			switch (depth)
			{
			case EWindowDepth.Normal:
				result = this.normalParent;
				break;
			case EWindowDepth.Notice:
				result = this.forwardParent;
				break;
			case EWindowDepth.Forward:
				result = this.importParent;
				break;
			default:
				result = null;
				break;
			}
			return result;
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00005848 File Offset: 0x00003A48
		protected void OnEvent_OnTickPlayer(object param)
		{
			this.ShowDialog("Message", "OnTickPlayer reason" + param, delegate
			{
				foreach (UIBaseWindow uibaseWindow in this.openedWindows.ToArray<UIBaseWindow>())
				{
					uibaseWindow.Close();
				}
				this.OpenWindow(UIDefine.UILogin, null);
			});
		}

		// Token: 0x0600009B RID: 155 RVA: 0x0000586E File Offset: 0x00003A6E
		protected void OnEvent_OnLoginFailed(object param)
		{
			this.ShowDialog("Message", "Login failed " + param.ToString(), null);
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00005890 File Offset: 0x00003A90
		public void ShowDialog(string title, string body, Action<bool> resultCallback)
		{
			this.OpenWindow(UIDefine.UIDialogBox, new UICallback(delegate(object window)
			{
				UIDialogBox uidialogBox = window as UIDialogBox;
				if (uidialogBox != null)
				{
					uidialogBox.Init(title, body, resultCallback);
				}
			}));
		}

		// Token: 0x0600009D RID: 157 RVA: 0x000058D4 File Offset: 0x00003AD4
		public void ShowDialog(string title, string body, object callback, Action resultCallback = null)
		{
			this.OpenWindow(UIDefine.UIDialogBox, new UICallback(delegate(object window)
			{
				UIDialogBox uidialogBox = window as UIDialogBox;
				if (uidialogBox != null)
				{
					uidialogBox.Init(title, body, resultCallback);
				}
			}));
		}

		// Token: 0x0600009E RID: 158 RVA: 0x0000517B File Offset: 0x0000337B
		public void CloseWindow(string dir)
		{
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00005915 File Offset: 0x00003B15
		public void CloseWindow(object window)
		{
			this.CloseWindow(window as UIBaseWindow);
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00005928 File Offset: 0x00003B28
		public void CloseWindow(UIBaseWindow window)
		{
			bool flag = window != null;
			if (flag)
			{
				window.OnClose();
				this.openedWindows.Remove(window);
				this._eventRegisterService.UnRegisterEvent(window);
				bool flag2 = this._windowPool.ContainsKey(window.ResPath);
				if (flag2)
				{
					Object.Destroy(window.gameObject);
				}
				else
				{
					window.gameObject.SetActive(false);
					this._windowPool[window.ResPath] = window;
				}
			}
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x000059AB File Offset: 0x00003BAB
		public void OpenWindow(WindowCreateInfo info, UICallback callback = null)
		{
			this.OpenWindow(info.resDir, info.depth, callback);
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x000059C2 File Offset: 0x00003BC2
		public void OpenWindow(string resPath, EWindowDepth depth, UICallback callback = null)
		{
			this.OpenWindow(this.GetType(resPath), resPath, this.GetParentFromDepth(depth), callback);
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x000059DC File Offset: 0x00003BDC
		public void RegisterAssembly(Assembly uiAssembly)
		{
			this._uiAssembly = uiAssembly;
			Type[] types = this._uiAssembly.GetTypes();
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00005A00 File Offset: 0x00003C00
		private Type GetType(string resPath)
		{
			return this._uiAssembly.GetType("Lockstep.Game.UI." + resPath);
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00005A2C File Offset: 0x00003C2C
		private void OpenWindow(Type type, string resPath, Transform parent, UICallback callback)
		{
			UIBaseWindow uibaseWindow;
			bool flag = this._windowPool.TryGetValue(resPath, out uibaseWindow);
			UIBaseWindow uibaseWindow2;
			if (flag)
			{
				uibaseWindow.gameObject.SetActive(true);
				uibaseWindow2 = uibaseWindow;
				this._windowPool.Remove(resPath);
			}
			else
			{
				GameObject gameObject = Resources.Load<GameObject>("UI/" + resPath);
				bool flag2 = gameObject == null;
				if (flag2)
				{
					Debug.LogError("OpenWindow failed: can not find prefab" + resPath);
					if (callback != null)
					{
						callback(null);
					}
					return;
				}
				Debug.Log("UIresPath " + resPath);
				GameObject obj = Object.Instantiate<GameObject>(gameObject, parent);
				uibaseWindow2 = obj.GetOrAddComponent<UIBaseWindow>();
				uibaseWindow2.ResPath = resPath;
			}
			this.openedWindows.Add(uibaseWindow2);
			uibaseWindow2._uiService = this;
			uibaseWindow2.DoAwake();
			uibaseWindow2.DoStart();
			this._eventRegisterService.RegisterEvent(uibaseWindow2);
			this.RegisterUiEvent(uibaseWindow2);
			if (callback != null)
			{
				callback(uibaseWindow2);
			}
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00005B2C File Offset: 0x00003D2C
		public void RegisterUiEvent(object obj)
		{
			this.RegisterUiEvent<UnityAction>("OnClick_", "OnClick_".Length, new Action<object, string, UnityAction>(this.RegisterEventButton), obj);
			this.RegisterUiEvent<UnityAction<bool>>("OnToggle_", "OnToggle_".Length, new Action<object, string, UnityAction<bool>>(this.RegisterEventToggle), obj);
			this.RegisterUiEvent<UnityAction<int>>("OnSelect_", "OnSelect_".Length, new Action<object, string, UnityAction<int>>(this.RegisterEventDropdown), obj);
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00005BA4 File Offset: 0x00003DA4
		public void RegisterUiEvent<TDelegate>(string prefix, int ignorePrefixLen, Action<object, string, TDelegate> callBack, object obj) where TDelegate : Delegate
		{
			bool flag = callBack == null;
			if (!flag)
			{
				MethodInfo[] methods = obj.GetType().GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				foreach (MethodInfo methodInfo in methods)
				{
					string name = methodInfo.Name;
					bool flag2 = name.StartsWith(prefix);
					if (flag2)
					{
						string arg = name.Substring(ignorePrefixLen);
						try
						{
							TDelegate arg2 = Delegate.CreateDelegate(typeof(TDelegate), obj, methodInfo) as TDelegate;
							callBack(obj, arg, arg2);
						}
						catch (Exception ex)
						{
							Debug.LogError("methodName " + name);
							throw;
						}
					}
				}
			}
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00005C60 File Offset: 0x00003E60
		private void RegisterEventDropdown(object objWin, string compName, UnityAction<int> action)
		{
			UIBaseWindow uibaseWindow = (UIBaseWindow)objWin;
			Dropdown @ref = uibaseWindow.GetRef<Dropdown>(compName);
			bool flag = @ref != null;
			if (flag)
			{
				@ref.onValueChanged.RemoveListener(action);
				@ref.onValueChanged.AddListener(action);
			}
			else
			{
				Debug.Log(uibaseWindow.GetType() + " miss ref " + compName);
			}
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00005CC4 File Offset: 0x00003EC4
		private void RegisterEventToggle(object objWin, string compName, UnityAction<bool> action)
		{
			UIBaseWindow uibaseWindow = (UIBaseWindow)objWin;
			Toggle @ref = uibaseWindow.GetRef<Toggle>(compName);
			bool flag = @ref != null;
			if (flag)
			{
				@ref.onValueChanged.RemoveListener(action);
				@ref.onValueChanged.AddListener(action);
			}
			else
			{
				Debug.Log(uibaseWindow.GetType() + " miss ref " + compName);
			}
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00005D28 File Offset: 0x00003F28
		private void RegisterEventButton(object objWin, string compName, UnityAction action)
		{
			UIBaseWindow uibaseWindow = (UIBaseWindow)objWin;
			Button @ref = uibaseWindow.GetRef<Button>(compName);
			bool flag = @ref != null;
			if (flag)
			{
				@ref.onClick.RemoveListener(action);
				@ref.onClick.AddListener(action);
			}
			else
			{
				Debug.Log(uibaseWindow.GetType() + " miss ref " + compName);
			}
		}

		// Token: 0x0400009B RID: 155
		public RenderTexture rt;

		// Token: 0x0400009C RID: 156
		private const string _prefabDir = "UI/";

		// Token: 0x0400009D RID: 157
		[SerializeField]
		private Transform _uiRoot;

		// Token: 0x0400009E RID: 158
		private Dictionary<string, UIBaseWindow> _windowPool = new Dictionary<string, UIBaseWindow>();

		// Token: 0x0400009F RID: 159
		private Transform normalParent;

		// Token: 0x040000A0 RID: 160
		private Transform forwardParent;

		// Token: 0x040000A1 RID: 161
		private Transform importParent;

		// Token: 0x040000A2 RID: 162
		private HashSet<UIBaseWindow> openedWindows = new HashSet<UIBaseWindow>();

		// Token: 0x040000A3 RID: 163
		private Assembly _uiAssembly;
	}
}
