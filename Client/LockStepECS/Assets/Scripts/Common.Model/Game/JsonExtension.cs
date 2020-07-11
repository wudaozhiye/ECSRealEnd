using System;
using Lockstep.Util;

namespace Lockstep.Game
{
	// Token: 0x02000067 RID: 103
	public static class JsonExtension
	{
		public static string ToJson(this object obj)
		{
			return (obj == null) ? "null" : JsonUtil.ToJson(obj);
		}
	}
}
