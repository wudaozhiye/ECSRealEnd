using System;
using System.Runtime.CompilerServices;

public class BaseTileMapSerializer
{
	public class FileContentException : Exception
	{
		public FileContentException(string message)
			: base(message)
		{
		}
	}

	protected static readonly byte[] MAGIC_BYTES = new byte[4]
	{
		102,
		109,
		74,
		80
	};

	protected static bool CheckMagicNumber(byte[] bs)
	{
		return CheckBytes(MAGIC_BYTES, bs);
	}

	protected static bool CheckBytes(byte[] ba, byte[] bb)
	{
		if (bb == null == (ba == null))
		{
			if (bb != null)
			{
				if (bb.Length == ba.Length)
				{
					for (int i = 0; i < 4; i++)
					{
						if (ba[i] != bb[i])
						{
							return false;
						}
					}
					return true;
				}
				return false;
			}
			return true;
		}
		return false;
	}
}
