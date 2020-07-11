using System;

namespace Lockstep
{
	[AttributeUsage(AttributeTargets.Class, Inherited = true)]
	public class PureModeAttribute : Attribute
	{
		private EPureModeType _pureMode;

		public EPureModeType Type => _pureMode;

		public PureModeAttribute(EPureModeType pureMode)
		{
			_pureMode = pureMode;
		}
	}
}
