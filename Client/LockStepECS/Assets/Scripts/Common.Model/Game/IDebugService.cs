using System;

namespace Lockstep.Game
{

	public interface IDebugService : IService
	{
		void Trace(string msg, bool isNewLine = false, bool isNeedLogTrace = false);
	}
}
