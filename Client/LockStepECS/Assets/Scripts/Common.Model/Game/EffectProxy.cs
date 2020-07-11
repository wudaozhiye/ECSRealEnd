using System;
using Lockstep.Math;
using UnityEngine;

namespace Lockstep.Game
{
	public class EffectProxy : MonoBehaviour
	{
		public IRollbackEffect Effect;

		public EffectProxy pre;

		public EffectProxy next;

		public int createTick;

		public int diedTick;

		public LFloat liveTime;

		public virtual void DoStart(int curTick, IRollbackEffect effect, LFloat liveTime)
		{
			this.liveTime = liveTime;
			createTick = curTick;
			diedTick = curTick + (liveTime * 33).ToInt();
			Effect = effect;
			if (effect != null)
			{
				effect.__proxy = this;
				effect.DoStart(curTick);
			}
		}

		public bool IsLive(int curTick)
		{
			return curTick >= createTick && curTick <= diedTick;
		}

		public virtual void DoUpdate(int tick)
		{
			Effect?.DoUpdate(tick);
		}
	}
}
