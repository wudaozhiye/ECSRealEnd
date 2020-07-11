using System;

// Token: 0x02000005 RID: 5
public interface IIncommingMessage
{
	// Token: 0x06000006 RID: 6
	T Parse<T>();

	// Token: 0x06000007 RID: 7
	byte[] GetRawBytes();
}
