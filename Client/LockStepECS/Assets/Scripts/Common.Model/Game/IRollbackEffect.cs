using System;
using Lockstep.Math;

namespace Lockstep.Game
{
	public interface IRollbackEffect
	{
		EffectProxy __proxy
		{
			get;
			set;
		}

		LFloat LiveTime
		{
			get;
			set;
		}

		void DoStart(int curTick);

		void DoUpdate(int tick);
	}
}
