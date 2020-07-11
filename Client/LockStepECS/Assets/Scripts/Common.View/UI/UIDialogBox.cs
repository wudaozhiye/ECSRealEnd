using System;
using UnityEngine.UI;

namespace Lockstep.Game.UI
{
	// Token: 0x02000026 RID: 38
	public class UIDialogBox : UIBaseWindow
	{
		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000E2 RID: 226 RVA: 0x00006731 File Offset: 0x00004931
		private Text TextTitle
		{
			get
			{
				return base.GetRef<Text>("TextTitle");
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000E3 RID: 227 RVA: 0x0000673E File Offset: 0x0000493E
		private Text TextContent
		{
			get
			{
				return base.GetRef<Text>("TextContent");
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000E4 RID: 228 RVA: 0x0000674B File Offset: 0x0000494B
		private Button BtnYes
		{
			get
			{
				return base.GetRef<Button>("BtnYes");
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000E5 RID: 229 RVA: 0x00006758 File Offset: 0x00004958
		private Button BtnNo
		{
			get
			{
				return base.GetRef<Button>("BtnNo");
			}
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00006768 File Offset: 0x00004968
		public void Init(string title, string content, Action callback)
		{
			this.TextContent.text = content;
			this.TextTitle.text = title;
			this.BtnNo.gameObject.SetActive(false);
			this.BtnYes.gameObject.SetActive(true);
			this.callbackYes = callback;
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x000067BC File Offset: 0x000049BC
		public void Init(string title, string content, Action<bool> onBtnClick)
		{
			this.TextContent.text = content;
			this.TextTitle.text = title;
			this.callbackYesNo = onBtnClick;
			this.BtnNo.gameObject.SetActive(true);
			this.BtnYes.gameObject.SetActive(true);
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x0000680F File Offset: 0x00004A0F
		public void OnClick_BtnYes()
		{
			this.CallBack(false);
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x0000680F File Offset: 0x00004A0F
		public void OnClick_BtnNo()
		{
			this.CallBack(false);
		}

		// Token: 0x060000EA RID: 234 RVA: 0x0000681A File Offset: 0x00004A1A
		private void CallBack(bool val)
		{
			Action<bool> action = this.callbackYesNo;
			if (action != null)
			{
				action(val);
			}
			this.callbackYesNo = null;
			Action action2 = this.callbackYes;
			if (action2 != null)
			{
				action2();
			}
			this.callbackYes = null;
			base.Close();
		}

		// Token: 0x040000AF RID: 175
		public Action<bool> callbackYesNo;

		// Token: 0x040000B0 RID: 176
		public Action callbackYes;
	}
}
