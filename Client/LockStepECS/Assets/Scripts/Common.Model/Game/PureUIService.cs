using System;
using System.Reflection;

namespace Lockstep.Game
{
	// Token: 0x0200005B RID: 91
	public class PureUIService : PureBaseService, IUIService, IService
	{
		// Token: 0x0600021D RID: 541 RVA: 0x00006F70 File Offset: 0x00005170
		public T GetIService<T>() where T : IService
		{
			return base.GetService<T>();
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x0600021E RID: 542 RVA: 0x00006F88 File Offset: 0x00005188
		// (set) Token: 0x0600021F RID: 543 RVA: 0x00006F90 File Offset: 0x00005190
		public bool IsDebugMode { get; set; }

		// Token: 0x06000220 RID: 544 RVA: 0x00003A82 File Offset: 0x00001C82
		public void RegisterAssembly(Assembly uiAssembly)
		{
		}

		// Token: 0x06000221 RID: 545 RVA: 0x00003A82 File Offset: 0x00001C82
		public void OpenWindow(string dir, EWindowDepth dep, UICallback callback = null)
		{
		}

		// Token: 0x06000222 RID: 546 RVA: 0x00003A82 File Offset: 0x00001C82
		public void OpenWindow(WindowCreateInfo info, UICallback callback = null)
		{
		}

		// Token: 0x06000223 RID: 547 RVA: 0x00003A82 File Offset: 0x00001C82
		public void CloseWindow(string dir)
		{
		}

		// Token: 0x06000224 RID: 548 RVA: 0x00003A82 File Offset: 0x00001C82
		public void CloseWindow(object window)
		{
		}

		// Token: 0x06000225 RID: 549 RVA: 0x00003A82 File Offset: 0x00001C82
		public void ShowDialog(string title, string body, Action<bool> resultCallback)
		{
		}
	}
}
