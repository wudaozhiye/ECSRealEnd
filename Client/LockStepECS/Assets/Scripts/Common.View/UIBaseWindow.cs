using System;
using Lockstep.Game.UI;
using Lockstep.Logging;
using NetMsg.Common;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Debug = Lockstep.Logging.Debug;

namespace Lockstep.Game
{
	// Token: 0x0200001E RID: 30
	public class UIBaseWindow : MonoBehaviour
	{
		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060000B3 RID: 179 RVA: 0x00005E7B File Offset: 0x0000407B
		// (set) Token: 0x060000B4 RID: 180 RVA: 0x00005E83 File Offset: 0x00004083
		public string ResPath { get; set; }

		// Token: 0x060000B5 RID: 181 RVA: 0x00005E8C File Offset: 0x0000408C
		public T GetRef<T>(string name) where T : UnityEngine.Object
		{
			return this._referenceHolder.GetRef<T>(name);
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00005EAA File Offset: 0x000040AA
		protected virtual void Awake()
		{
			this._referenceHolder = base.GetComponent<IReferenceHolder>();
			Debug.Assert(this._referenceHolder != null, base.GetType() + " miss IReferenceHolder ");
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00005ED8 File Offset: 0x000040D8
		public void Close()
		{
			this._uiService.CloseWindow(this);
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x0000517B File Offset: 0x0000337B
		public virtual void DoAwake()
		{
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x0000517B File Offset: 0x0000337B
		public virtual void DoStart()
		{
		}

		// Token: 0x060000BA RID: 186 RVA: 0x0000517B File Offset: 0x0000337B
		public virtual void OnClose()
		{
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00005EE8 File Offset: 0x000040E8
		protected Button BindEvent(string name, UnityAction func)
		{
			Button component = base.transform.Find(name).GetComponent<Button>();
			component.onClick.AddListener(func);
			return component;
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00005F1A File Offset: 0x0000411A
		protected void OpenWindow(WindowCreateInfo windowInfo)
		{
			this._uiService.OpenWindow(windowInfo.resDir, windowInfo.depth, null);
		}

		// Token: 0x060000BD RID: 189 RVA: 0x0000517B File Offset: 0x0000337B
		protected void SendMessage(EMsgSC type, object body)
		{
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00005F38 File Offset: 0x00004138
		protected T GetService<T>() where T : IService
		{
			return this._uiService.GetIService<T>();
		}

		// Token: 0x040000A6 RID: 166
		public IUIService _uiService;

		// Token: 0x040000A8 RID: 168
		protected IReferenceHolder _referenceHolder;
	}
}
