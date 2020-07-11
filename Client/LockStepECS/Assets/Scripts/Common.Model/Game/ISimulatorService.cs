using System;

namespace Lockstep.Game
{
	// Token: 0x02000043 RID: 67
	public interface ISimulatorService : IService
	{

		FuncCreateWorld FuncCreateWorld { get; set; }

		void RunVideo();

		void JumpTo(int tick);
	}
}
