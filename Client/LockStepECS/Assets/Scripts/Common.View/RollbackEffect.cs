using System;
using Lockstep.Math;
using UnityEngine;

namespace Lockstep.Game
{
	// Token: 0x02000016 RID: 22
	public class RollbackEffect : MonoBehaviour,IRollbackEffect
	{
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000072 RID: 114 RVA: 0x0000513F File Offset: 0x0000333F
		// (set) Token: 0x06000073 RID: 115 RVA: 0x00005147 File Offset: 0x00003347
		[HideInInspector]
		public EffectProxy __proxy { get; set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000074 RID: 116 RVA: 0x00005150 File Offset: 0x00003350
		[HideInInspector]
		public int createTick
		{
			get
			{
				return this.__proxy.createTick;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000075 RID: 117 RVA: 0x0000515D File Offset: 0x0000335D
		[HideInInspector]
		public int diedTick
		{
			get
			{
				return this.__proxy.diedTick;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000076 RID: 118 RVA: 0x0000516A File Offset: 0x0000336A
		// (set) Token: 0x06000077 RID: 119 RVA: 0x00005172 File Offset: 0x00003372
		public LFloat LiveTime
		{
			get
			{
				return this._liveTime;
			}
			set
			{
				this._liveTime = value;
			}
		}

		// Token: 0x06000078 RID: 120 RVA: 0x0000517B File Offset: 0x0000337B
		public virtual void DoStart(int curTick)
		{
		}

		// Token: 0x06000079 RID: 121 RVA: 0x0000517B File Offset: 0x0000337B
		public virtual void DoUpdate(int tick)
		{
		}

		// Token: 0x04000091 RID: 145
		public LFloat _liveTime;
	}
}
