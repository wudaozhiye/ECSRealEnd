using System;

namespace Lockstep.Util
{
	// Token: 0x02000012 RID: 18
	public static class BitUtil
	{
		// Token: 0x0600003A RID: 58 RVA: 0x00003614 File Offset: 0x00001814
		public static bool HasBit(int val, int idx)
		{
			return (val & 1 << idx) != 0;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00003634 File Offset: 0x00001834
		public static void SetBit(ref int val, int idx, bool isSet)
		{
			if (isSet)
			{
				val |= 1 << idx;
			}
			else
			{
				val &= ~(1 << idx);
			}
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00003664 File Offset: 0x00001864
		public static bool HasBit(byte val, byte idx)
		{
			return ((int)val & 1 << (int)idx) != 0;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00003681 File Offset: 0x00001881
		public static void SetBit(ref byte val, byte idx)
		{
			val |= (byte)(1 << (int)idx);
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00003694 File Offset: 0x00001894
		public static byte ToByte(byte idx)
		{
			return (byte)(1 << (int)idx);
		}
	}
}
