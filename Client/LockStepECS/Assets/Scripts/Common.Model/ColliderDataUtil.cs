using System;
using System.Collections.Generic;
using System.IO;
using Lockstep.Collision2D;
using Lockstep.Serialization;

// Token: 0x02000002 RID: 2
public class ColliderDataUtil : object
{
	// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
	public static void SaveToFile(string path, List<ColliderData> allData)
	{
		Serializer serializer = new Serializer();
		serializer.Write((ushort)allData.Count);
		foreach (ColliderData colliderData in allData)
		{
		}
		bool flag = !Directory.Exists(Path.GetDirectoryName(path));
		if (flag)
		{
			Directory.CreateDirectory(Path.GetDirectoryName(path));
		}
		File.WriteAllBytes(path, serializer.CopyData());
	}

	// Token: 0x06000002 RID: 2 RVA: 0x000020E0 File Offset: 0x000002E0
	public static ColliderData[] ReadFromFile(string path)
	{
		byte[] source = File.ReadAllBytes(path);
		Deserializer deserializer = new Deserializer(source);
		ushort num = deserializer.ReadUInt16();
		ColliderData[] array = new ColliderData[(int)num];
		for (int i = 0; i < (int)num; i++)
		{
			array[i] = new ColliderData();
		}
		return array;
	}
}
