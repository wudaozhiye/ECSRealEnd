using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.Networking;

namespace Lockstep.Game.UI
{
	// Token: 0x0200002A RID: 42
	public class BmHelper : object
	{
		// Token: 0x060000F9 RID: 249 RVA: 0x00006B9C File Offset: 0x00004D9C
		public static string CreateRandomString(int length)
		{
			bool flag = length < 0;
			if (flag)
			{
				throw new ArgumentOutOfRangeException("length", "length cannot be less than zero.");
			}
			return Guid.NewGuid().ToString().Substring(0, length);
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00006BE0 File Offset: 0x00004DE0
		public static string ColorToHex(Color32 color)
		{
			return color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2") + color.a.ToString("X2");
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00006C40 File Offset: 0x00004E40
		public static Color HexToColor(string hex)
		{
			byte b = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
			byte b2 = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
			byte b3 = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
			return new Color32(b, b2, b3, byte.MaxValue);
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00006CA0 File Offset: 0x00004EA0
		public static ConnectionConfig CreateDefaultConnectionConfig()
		{
			ConnectionConfig connectionConfig = new ConnectionConfig();
			connectionConfig.AddChannel(QosType.Reliable);
			connectionConfig.AddChannel(0);
			return connectionConfig;
		}

		// Token: 0x040000CD RID: 205
		public const int MaxUnetConnections = 500;

		// Token: 0x040000CE RID: 206
		public static Color SelectColor = BmHelper.HexToColor("599F29");
	}
}
