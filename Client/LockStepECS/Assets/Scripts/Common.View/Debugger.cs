using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200000F RID: 15
public class Debugger : MonoBehaviour
{
	// Token: 0x0600005C RID: 92 RVA: 0x00003A04 File Offset: 0x00001C04
	private void Awake()
	{
		bool allowDebugging = this.AllowDebugging;
		if (allowDebugging)
		{
			Application.logMessageReceived += new Application.LogCallback(this.LogHandler);
		}
	}

	// Token: 0x0600005D RID: 93 RVA: 0x00003A30 File Offset: 0x00001C30
	private void Update()
	{
		bool allowDebugging = this.AllowDebugging;
		if (allowDebugging)
		{
			this._frameNumber++;
			float num = Time.realtimeSinceStartup - this._lastShowFPSTime;
			bool flag = num >= 1f;
			if (flag)
			{
				this._fps = (int)((float)this._frameNumber / num);
				this._frameNumber = 0;
				this._lastShowFPSTime = Time.realtimeSinceStartup;
			}
		}
	}

	// Token: 0x0600005E RID: 94 RVA: 0x00003A98 File Offset: 0x00001C98
	private void OnDestory()
	{
		bool allowDebugging = this.AllowDebugging;
		if (allowDebugging)
		{
			Application.logMessageReceived -= new Application.LogCallback(this.LogHandler);
		}
	}

	// Token: 0x0600005F RID: 95 RVA: 0x00003AC4 File Offset: 0x00001CC4
	private void LogHandler(string condition, string stackTrace, LogType type)
	{
		LogData item = new LogData();
		item.time = DateTime.Now.ToString("HH:mm:ss");
		item.message = condition;
		item.stackTrace = stackTrace;
		bool flag = type == LogType.Assert;
		if (flag)
		{
			item.type = "Fatal";
			this._fatalLogCount++;
		}
		else
		{
			bool flag2 = (type == LogType.Exception || type == LogType.Error);
			if (flag2)
			{
				item.type = "Error";
				this._errorLogCount++;
			}
			else
			{
				bool flag3 = type == LogType.Warning;
				if (flag3)
				{
					item.type = "Warning";
					this._warningLogCount++;
				}
				else
				{
					bool flag4 = type == LogType.Log;
					if (flag4)
					{
						item.type = "Info";
						this._infoLogCount++;
					}
				}
			}
		}
		this._logInformations.Add(item);
		bool flag5 = this._warningLogCount > 0;
		if (flag5)
		{
			this._fpsColor = Color.yellow;
		}
		bool flag6 = this._errorLogCount > 0;
		if (flag6)
		{
			this._fpsColor = Color.red;
		}
	}

	// Token: 0x06000060 RID: 96 RVA: 0x00003BEC File Offset: 0x00001DEC
	private void OnGUI()
	{
		bool allowDebugging = this.AllowDebugging;
		if (allowDebugging)
		{
			bool expansion = this._expansion;
			if (expansion)
			{
				this._windowRect = GUI.Window(0, this._windowRect, new GUI.WindowFunction(this.ExpansionGUIWindow), "DEBUGGER");
			}
			else
			{
				this._windowRect = GUI.Window(0, this._windowRect, new GUI.WindowFunction(this.ShrinkGUIWindow), "DEBUGGER");
			}
		}
	}

	// Token: 0x06000061 RID: 97 RVA: 0x00003C5C File Offset: 0x00001E5C
	private void ExpansionGUIWindow(int windowId)
	{
		GUI.DragWindow(new Rect(0f, 0f, 10000f, 20f));
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUI.contentColor = this._fpsColor;
		bool flag = GUILayout.Button("FPS:" + this._fps, new GUILayoutOption[]
		{
			GUILayout.Height(30f)
		});
		if (flag)
		{
			this._expansion = false;
			this._windowRect.width = 100f;
			this._windowRect.height = 60f;
		}
		GUI.contentColor = ((this._debugType == DebugType.Console) ? Color.white : Color.gray);
		bool flag2 = GUILayout.Button("Console", new GUILayoutOption[]
		{
			GUILayout.Height(30f)
		});
		if (flag2)
		{
			this._debugType = DebugType.Console;
		}
		GUI.contentColor = ((this._debugType == DebugType.Memory) ? Color.white : Color.gray);
		bool flag3 = GUILayout.Button("Memory", new GUILayoutOption[]
		{
			GUILayout.Height(30f)
		});
		if (flag3)
		{
			this._debugType = DebugType.Memory;
		}
		GUI.contentColor = ((this._debugType == DebugType.System) ? Color.white : Color.gray);
		bool flag4 = GUILayout.Button("System", new GUILayoutOption[]
		{
			GUILayout.Height(30f)
		});
		if (flag4)
		{
			this._debugType = DebugType.System;
		}
		GUI.contentColor = ((this._debugType == DebugType.Screen) ? Color.white : Color.gray);
		bool flag5 = GUILayout.Button("Screen", new GUILayoutOption[]
		{
			GUILayout.Height(30f)
		});
		if (flag5)
		{
			this._debugType = DebugType.Screen;
		}
		GUI.contentColor = ((this._debugType == DebugType.Quality) ? Color.white : Color.gray);
		bool flag6 = GUILayout.Button("Quality", new GUILayoutOption[]
		{
			GUILayout.Height(30f)
		});
		if (flag6)
		{
			this._debugType = DebugType.Quality;
		}
		GUI.contentColor = ((this._debugType == DebugType.Environment) ? Color.white : Color.gray);
		bool flag7 = GUILayout.Button("Environment", new GUILayoutOption[]
		{
			GUILayout.Height(30f)
		});
		if (flag7)
		{
			this._debugType = DebugType.Environment;
		}
		GUI.contentColor = Color.white;
		GUILayout.EndHorizontal();
		bool flag8 = this._debugType == 0;
		if (flag8)
		{
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			bool flag9 = GUILayout.Button("Clear", Array.Empty<GUILayoutOption>());
			if (flag9)
			{
				this._logInformations.Clear();
				this._fatalLogCount = 0;
				this._warningLogCount = 0;
				this._errorLogCount = 0;
				this._infoLogCount = 0;
				this._currentLogIndex = -1;
				this._fpsColor = Color.white;
			}
			GUI.contentColor = (this._showInfoLog ? Color.white : Color.gray);
			this._showInfoLog = GUILayout.Toggle(this._showInfoLog, "Info [" + this._infoLogCount + "]", Array.Empty<GUILayoutOption>());
			GUI.contentColor = (this._showWarningLog ? Color.white : Color.gray);
			this._showWarningLog = GUILayout.Toggle(this._showWarningLog, "Warning [" + this._warningLogCount + "]", Array.Empty<GUILayoutOption>());
			GUI.contentColor = (this._showErrorLog ? Color.white : Color.gray);
			this._showErrorLog = GUILayout.Toggle(this._showErrorLog, "Error [" + this._errorLogCount + "]", Array.Empty<GUILayoutOption>());
			GUI.contentColor = (this._showFatalLog ? Color.white : Color.gray);
			this._showFatalLog = GUILayout.Toggle(this._showFatalLog, "Fatal [" + this._fatalLogCount + "]", Array.Empty<GUILayoutOption>());
			GUI.contentColor = Color.white;
			GUILayout.EndHorizontal();
			this._scrollLogView = GUILayout.BeginScrollView(this._scrollLogView, "Box", new GUILayoutOption[]
			{
				GUILayout.Height(165f)
			});
			for (int i = 0; i < this._logInformations.Count; i++)
			{
				bool flag10 = false;
				Color contentColor = Color.white;
				string type = this._logInformations[i].type;
				if (!(type == "Fatal"))
				{
					if (!(type == "Error"))
					{
						if (!(type == "Info"))
						{
							if (type == "Warning")
							{
								flag10 = this._showWarningLog;
								contentColor = Color.yellow;
							}
						}
						else
						{
							flag10 = this._showInfoLog;
							contentColor = Color.white;
						}
					}
					else
					{
						flag10 = this._showErrorLog;
						contentColor = Color.red;
					}
				}
				else
				{
					flag10 = this._showFatalLog;
					contentColor = Color.red;
				}
				bool flag11 = flag10;
				if (flag11)
				{
					GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
					bool flag12 = GUILayout.Toggle(this._currentLogIndex == i, "", Array.Empty<GUILayoutOption>());
					if (flag12)
					{
						this._currentLogIndex = i;
					}
					GUI.contentColor = contentColor;
					GUILayout.Label("[" + this._logInformations[i].type + "] ", Array.Empty<GUILayoutOption>());
					GUILayout.Label("[" + this._logInformations[i].time + "] ", Array.Empty<GUILayoutOption>());
					GUILayout.Label(this._logInformations[i].message, Array.Empty<GUILayoutOption>());
					GUILayout.FlexibleSpace();
					GUI.contentColor = Color.white;
					GUILayout.EndHorizontal();
				}
			}
			GUILayout.EndScrollView();
			this._scrollCurrentLogView = GUILayout.BeginScrollView(this._scrollCurrentLogView, "Box", new GUILayoutOption[]
			{
				GUILayout.Height(100f)
			});
			bool flag13 = this._currentLogIndex != -1;
			if (flag13)
			{
				GUILayout.Label(this._logInformations[this._currentLogIndex].message + "\r\n\r\n" + this._logInformations[this._currentLogIndex].stackTrace, Array.Empty<GUILayoutOption>());
			}
			GUILayout.EndScrollView();
		}
		else
		{
			bool flag14 = this._debugType == DebugType.Memory;
			if (flag14)
			{
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				GUILayout.Label("Memory Information", Array.Empty<GUILayoutOption>());
				GUILayout.EndHorizontal();
				GUILayout.BeginVertical("Box", Array.Empty<GUILayoutOption>());
				GUILayout.EndVertical();
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				bool flag15 = GUILayout.Button("卸载未使用的资源", Array.Empty<GUILayoutOption>());
				if (flag15)
				{
					Resources.UnloadUnusedAssets();
				}
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				bool flag16 = GUILayout.Button("使用GC垃圾回收", Array.Empty<GUILayoutOption>());
				if (flag16)
				{
					GC.Collect();
				}
				GUILayout.EndHorizontal();
			}
			else
			{
				bool flag17 = this._debugType == DebugType.System;
				if (flag17)
				{
					GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
					GUILayout.Label("System Information", Array.Empty<GUILayoutOption>());
					GUILayout.EndHorizontal();
					this._scrollSystemView = GUILayout.BeginScrollView(this._scrollSystemView, "Box");
					GUILayout.Label("操作系统：" + SystemInfo.operatingSystem, Array.Empty<GUILayoutOption>());
					GUILayout.Label("系统内存：" + SystemInfo.systemMemorySize + "MB", Array.Empty<GUILayoutOption>());
					GUILayout.Label("处理器：" + SystemInfo.processorType, Array.Empty<GUILayoutOption>());
					GUILayout.Label("处理器数量：" + SystemInfo.processorCount, Array.Empty<GUILayoutOption>());
					GUILayout.Label("显卡：" + SystemInfo.graphicsDeviceName, Array.Empty<GUILayoutOption>());
					GUILayout.Label("显卡类型：" + SystemInfo.graphicsDeviceType, Array.Empty<GUILayoutOption>());
					GUILayout.Label("显存：" + SystemInfo.graphicsMemorySize + "MB", Array.Empty<GUILayoutOption>());
					GUILayout.Label("显卡标识：" + SystemInfo.graphicsDeviceID, Array.Empty<GUILayoutOption>());
					GUILayout.Label("显卡供应商：" + SystemInfo.graphicsDeviceVendor, Array.Empty<GUILayoutOption>());
					GUILayout.Label("显卡供应商标识码：" + SystemInfo.graphicsDeviceVendorID, Array.Empty<GUILayoutOption>());
					GUILayout.Label("设备模式：" + SystemInfo.deviceModel, Array.Empty<GUILayoutOption>());
					GUILayout.Label("设备名称：" + SystemInfo.deviceName, Array.Empty<GUILayoutOption>());
					GUILayout.Label("设备类型：" + SystemInfo.deviceType, Array.Empty<GUILayoutOption>());
					GUILayout.Label("设备标识：" + SystemInfo.deviceUniqueIdentifier, Array.Empty<GUILayoutOption>());
					GUILayout.EndScrollView();
				}
				else
				{
					bool flag18 = this._debugType == DebugType.Screen;
					if (flag18)
					{
						GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
						GUILayout.Label("Screen Information", Array.Empty<GUILayoutOption>());
						GUILayout.EndHorizontal();
						GUILayout.BeginVertical("Box", Array.Empty<GUILayoutOption>());
						GUILayout.Label("DPI：" + Screen.dpi, Array.Empty<GUILayoutOption>());
						GUILayout.Label("分辨率：" + Screen.currentResolution.ToString(), Array.Empty<GUILayoutOption>());
						GUILayout.EndVertical();
						GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
						bool flag19 = GUILayout.Button("全屏", Array.Empty<GUILayoutOption>());
						if (flag19)
						{
							Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, !Screen.fullScreen);
						}
						GUILayout.EndHorizontal();
					}
					else
					{
						bool flag20 = this._debugType == DebugType.Quality;
						if (flag20)
						{
							GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
							GUILayout.Label("Quality Information", Array.Empty<GUILayoutOption>());
							GUILayout.EndHorizontal();
							GUILayout.BeginVertical("Box", Array.Empty<GUILayoutOption>());
							string str = "";
							bool flag21 = QualitySettings.GetQualityLevel() == 0;
							if (flag21)
							{
								str = " [最低]";
							}
							else
							{
								bool flag22 = QualitySettings.GetQualityLevel() == QualitySettings.names.Length - 1;
								if (flag22)
								{
									str = " [最高]";
								}
							}
							GUILayout.Label("图形质量：" + QualitySettings.names[QualitySettings.GetQualityLevel()] + str, Array.Empty<GUILayoutOption>());
							GUILayout.EndVertical();
							GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
							bool flag23 = GUILayout.Button("降低一级图形质量", Array.Empty<GUILayoutOption>());
							if (flag23)
							{
								QualitySettings.DecreaseLevel();
							}
							GUILayout.EndHorizontal();
							GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
							bool flag24 = GUILayout.Button("提升一级图形质量", Array.Empty<GUILayoutOption>());
							if (flag24)
							{
								QualitySettings.IncreaseLevel();
							}
							GUILayout.EndHorizontal();
						}
						else
						{
							bool flag25 = this._debugType == DebugType.Environment;
							if (flag25)
							{
								GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
								GUILayout.Label("Environment Information", Array.Empty<GUILayoutOption>());
								GUILayout.EndHorizontal();
								GUILayout.BeginVertical("Box", Array.Empty<GUILayoutOption>());
								GUILayout.Label("项目名称：" + Application.productName, Array.Empty<GUILayoutOption>());
								GUILayout.Label("项目版本：" + Application.version, Array.Empty<GUILayoutOption>());
								GUILayout.Label("Unity版本：" + Application.unityVersion, Array.Empty<GUILayoutOption>());
								GUILayout.Label("公司名称：" + Application.companyName, Array.Empty<GUILayoutOption>());
								GUILayout.EndVertical();
								GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
								bool flag26 = GUILayout.Button("退出程序", Array.Empty<GUILayoutOption>());
								if (flag26)
								{
									Application.Quit();
								}
								GUILayout.EndHorizontal();
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x06000062 RID: 98 RVA: 0x00004844 File Offset: 0x00002A44
	private void ShrinkGUIWindow(int windowId)
	{
		GUI.DragWindow(new Rect(0f, 0f, 10000f, 20f));
		GUI.contentColor = this._fpsColor;
		bool flag = GUILayout.Button("FPS:" + this._fps, new GUILayoutOption[]
		{
			GUILayout.Width(80f),
			GUILayout.Height(30f)
		});
		if (flag)
		{
			this._expansion = true;
			this._windowRect.width = 600f;
			this._windowRect.height = 360f;
		}
		GUI.contentColor = Color.white;
	}

	// Token: 0x0400006D RID: 109
	public bool AllowDebugging = true;

	// Token: 0x0400006E RID: 110
	private DebugType _debugType = 0;

	// Token: 0x0400006F RID: 111
	private List<LogData> _logInformations = new List<LogData>();

	// Token: 0x04000070 RID: 112
	private int _currentLogIndex = -1;

	// Token: 0x04000071 RID: 113
	private int _infoLogCount = 0;

	// Token: 0x04000072 RID: 114
	private int _warningLogCount = 0;

	// Token: 0x04000073 RID: 115
	private int _errorLogCount = 0;

	// Token: 0x04000074 RID: 116
	private int _fatalLogCount = 0;

	// Token: 0x04000075 RID: 117
	private bool _showInfoLog = true;

	// Token: 0x04000076 RID: 118
	private bool _showWarningLog = true;

	// Token: 0x04000077 RID: 119
	private bool _showErrorLog = true;

	// Token: 0x04000078 RID: 120
	private bool _showFatalLog = true;

	// Token: 0x04000079 RID: 121
	private Vector2 _scrollLogView = Vector2.zero;

	// Token: 0x0400007A RID: 122
	private Vector2 _scrollCurrentLogView = Vector2.zero;

	// Token: 0x0400007B RID: 123
	private Vector2 _scrollSystemView = Vector2.zero;

	// Token: 0x0400007C RID: 124
	private bool _expansion = false;

	// Token: 0x0400007D RID: 125
	private Rect _windowRect = new Rect(0f, 0f, 100f, 60f);

	// Token: 0x0400007E RID: 126
	private int _fps = 0;

	// Token: 0x0400007F RID: 127
	private Color _fpsColor = Color.white;

	// Token: 0x04000080 RID: 128
	private int _frameNumber = 0;

	// Token: 0x04000081 RID: 129
	private float _lastShowFPSTime = 0f;
}
