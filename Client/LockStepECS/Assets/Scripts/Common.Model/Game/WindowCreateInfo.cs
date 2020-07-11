using System;
using System.Runtime.InteropServices;

namespace Lockstep.Game
{
	public struct WindowCreateInfo
	{
		public string resDir;

		public EWindowDepth depth;

		public WindowCreateInfo(string dir, EWindowDepth dep)
		{
			resDir = dir;
			depth = dep;
		}
	}
}
