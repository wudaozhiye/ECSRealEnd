using System;
using Lockstep.Serialization;

// Token: 0x02000007 RID: 7
public static class BinaryWriterExtension
{
	
	public static void Write(this Serializer writer, byte v1, byte v2, byte v3)
	{
		writer.Write(v1);
		writer.Write(v2);
		writer.Write(v3);
	}


	public static void Write(this Serializer writer, ushort v1, ushort v2, byte v3)
	{
		writer.Write(v1);
		writer.Write(v2);
		writer.Write(v3);
	}
}
