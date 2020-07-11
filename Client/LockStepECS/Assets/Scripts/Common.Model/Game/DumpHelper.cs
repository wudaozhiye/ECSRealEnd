using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Lockstep.Game
{
	// Token: 0x02000028 RID: 40
	public class DumpHelper : BaseSimulatorHelper
	{
		// Token: 0x06000090 RID: 144 RVA: 0x00004852 File Offset: 0x00002A52
		public DumpHelper(IServiceContainer serviceContainer, World world, HashHelper hashHelper) : base(serviceContainer, world)
		{
			this._hashHelper = hashHelper;
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000091 RID: 145 RVA: 0x00004882 File Offset: 0x00002A82
		private string dumpPath
		{
			get
			{
				return "";
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000092 RID: 146 RVA: 0x00004889 File Offset: 0x00002A89
		private string dumpAllPath
		{
			get
			{
				return "/tmp/Tutorial/LockstepTutorial/DumpLog";
			}
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00004890 File Offset: 0x00002A90
		public void DumpFrame(bool isNewFrame)
		{
			bool flag = !this.enable;
			if (!flag)
			{
				this._curSb = this.DumpFrame();
				if (isNewFrame)
				{
					this._tick2RawFrameData[base.Tick] = this._curSb;
					this._tick2OverrideFrameData[base.Tick] = this._curSb;
				}
				else
				{
					this._tick2OverrideFrameData[base.Tick] = this._curSb;
				}
			}
		}

		// Token: 0x06000094 RID: 148 RVA: 0x0000490C File Offset: 0x00002B0C
		public void DumpToFile(bool withCurFrame = false)
		{
			bool flag = !this.enable;
			if (!flag)
			{
				string path = this.dumpPath + "/cur.txt";
				string directoryName = Path.GetDirectoryName(path);
				bool flag2 = !Directory.Exists(directoryName);
				if (flag2)
				{
					Directory.CreateDirectory(directoryName);
				}
				StringBuilder stringBuilder = new StringBuilder();
				StringBuilder stringBuilder2 = new StringBuilder();
				for (int i = 0; i <= base.Tick; i++)
				{
					stringBuilder2.AppendLine(this._tick2RawFrameData[i].ToString());
					stringBuilder.AppendLine(this._tick2OverrideFrameData[i].ToString());
				}
				File.WriteAllText(this.dumpPath + "/resume.txt", stringBuilder.ToString());
				File.WriteAllText(this.dumpPath + "/raw.txt", stringBuilder2.ToString());
				if (withCurFrame)
				{
					this._curSb = this.DumpFrame();
					int num = this._hashHelper.CalcHash(true);
					File.WriteAllText(this.dumpPath + "/cur_single.txt", this._curSb.ToString());
					File.WriteAllText(this.dumpPath + "/raw_single.txt", this._tick2RawFrameData[base.Tick].ToString());
				}
				Debug.Break();
			}
		}

		public void OnFrameEnd()
		{
			this._curSb = null;
		}

		public void Trace(string msg, bool isNewLine = false, bool isNeedLogTrace = false)
		{
			bool flag = this._curSb == null;
			if (!flag)
			{
				if (isNewLine)
				{
					this._curSb.AppendLine(msg);
				}
				else
				{
					this._curSb.Append(msg);
				}
				if (isNeedLogTrace)
				{
					StackTrace stackTrace = new StackTrace(true);
					StackFrame[] frames = stackTrace.GetFrames();
					for (int i = 2; i < frames.Length; i++)
					{
						StackFrame stackFrame = frames[i];
						this._curSb.AppendLine(stackFrame.GetMethod().DeclaringType.FullName + "::" + stackFrame.GetMethod().Name);
					}
				}
			}
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00004B24 File Offset: 0x00002D24
		public void DumpAll()
		{
			bool flag = !this.enable;
			if (!flag)
			{
				string path = this.dumpAllPath + "/cur.txt";
				string directoryName = Path.GetDirectoryName(path);
				bool flag2 = !Directory.Exists(directoryName);
				if (flag2)
				{
					Directory.CreateDirectory(directoryName);
				}
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i <= base.Tick; i++)
				{
					StringBuilder stringBuilder2;
					bool flag3 = this._tick2RawFrameData.TryGetValue(i, out stringBuilder2);
					if (flag3)
					{
						stringBuilder.AppendLine(stringBuilder2.ToString());
					}
				}
				File.WriteAllText(this.dumpAllPath + string.Format("/All_{0}.txt", this._serviceContainer.GetService<IGlobalStateService>().LocalActorId), stringBuilder.ToString());
			}
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00004BF8 File Offset: 0x00002DF8
		private StringBuilder DumpFrame()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Tick : " + base.Tick + "--------------------");
			this._DumpStr(stringBuilder, "");
			return stringBuilder;
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00004C40 File Offset: 0x00002E40
		private void _DumpStr(StringBuilder sb, string prefix)
		{
			foreach (IService service in this._serviceContainer.GetAllServices())
			{
				IDumpStr dumpStr;
				bool flag = (dumpStr = (service as IDumpStr)) != null;
				if (flag)
				{
					sb.AppendLine(service.GetType() + " --------------------");
					dumpStr.DumpStr(sb, "\t" + prefix);
				}
			}
		}
		
		public Dictionary<int, StringBuilder> _tick2RawFrameData = new Dictionary<int, StringBuilder>();
		
		public Dictionary<int, StringBuilder> _tick2OverrideFrameData = new Dictionary<int, StringBuilder>();
		
		private HashHelper _hashHelper;

		private StringBuilder _curSb;
		
		public bool enable = false;
	}
}
