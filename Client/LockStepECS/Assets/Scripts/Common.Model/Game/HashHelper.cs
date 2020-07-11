using System;
using System.Collections.Generic;
using Lockstep.Math;

namespace Lockstep.Game
{
	// Token: 0x0200002B RID: 43
	public class HashHelper : BaseSimulatorHelper
	{
		// Token: 0x060000C8 RID: 200 RVA: 0x000054DA File Offset: 0x000036DA
		public HashHelper(IServiceContainer serviceContainer, World world, INetworkService networkService, IFrameBuffer cmdBuffer) : base(serviceContainer, world)
		{
			this._networkService = networkService;
			this._cmdBuffer = cmdBuffer;
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00005514 File Offset: 0x00003714
		public void CheckAndSendHashCodes()
		{
			bool flag = this._cmdBuffer.NextTickToCheck > this._firstHashTick;
			if (flag)
			{
				int num = LMath.Min(new int[]
				{
					this._allHashCodes.Count,
					this._cmdBuffer.NextTickToCheck - this._firstHashTick,
					120
				});
				bool flag2 = num > 0;
				if (flag2)
				{
					this._networkService.SendHashCodes(this._firstHashTick, this._allHashCodes, 0, num);
					this._firstHashTick += num;
					this._allHashCodes.RemoveRange(0, num);
				}
			}
		}

		// Token: 0x060000CA RID: 202 RVA: 0x000055B0 File Offset: 0x000037B0
		public bool TryGetValue(int tick, out int hash)
		{
			return this._tick2Hash.TryGetValue(tick, out hash);
		}

		// Token: 0x060000CB RID: 203 RVA: 0x000055D0 File Offset: 0x000037D0
		public int CalcHash(bool isNeedTrace = false)
		{
			int num = 0;
			return this.CalcHash(ref num, isNeedTrace);
		}

		// Token: 0x060000CC RID: 204 RVA: 0x000055F0 File Offset: 0x000037F0
		private int CalcHash(ref int idx, bool isNeedTrace)
		{
			int num = 0;
			int num2 = 0;
			IDebugService service = this._serviceContainer.GetService<IDebugService>();
			foreach (IService service2 in this._serviceContainer.GetAllServices())
			{
				IHashCode hashCode;
				bool flag = (hashCode = (service2 as IHashCode)) != null;
				if (flag)
				{
					num2 += hashCode.GetHash(ref num) * PrimerLUT.GetPrimer(num++);
				}
			}
			return num2;
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00005668 File Offset: 0x00003868
		public void SetHash(int tick, int hash)
		{
			bool flag = tick < this._firstHashTick;
			if (!flag)
			{
				int num = tick - this._firstHashTick;
				bool flag2 = this._allHashCodes.Count <= num;
				if (flag2)
				{
					for (int i = 0; i < num + 1; i++)
					{
						this._allHashCodes.Add(0);
					}
				}
				this._allHashCodes[num] = hash;
				this._tick2Hash[base.Tick] = hash;
			}
		}

		// Token: 0x04000093 RID: 147
		private INetworkService _networkService;

		// Token: 0x04000094 RID: 148
		private IFrameBuffer _cmdBuffer;

		// Token: 0x04000095 RID: 149
		private List<int> _allHashCodes = new List<int>();

		// Token: 0x04000096 RID: 150
		private int _firstHashTick = 0;

		// Token: 0x04000097 RID: 151
		private Dictionary<int, int> _tick2Hash = new Dictionary<int, int>();
	}
}
