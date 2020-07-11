using System;
using System.Collections.Generic;
using Lockstep.Game;
using Lockstep.Logging;
using Lockstep.Math;
using Lockstep.Serialization;

public class TileMapDeserializer : BaseTileMapSerializer
{
	public static GridInfo ReadGrid(Deserializer reader)
	{
		GridInfo gridInfo = new GridInfo();
		gridInfo.min = reader.ReadLVector2Int();
		gridInfo.max = reader.ReadLVector2Int();
		gridInfo.cellSize = reader.ReadLVector3();
		gridInfo.cellGap = reader.ReadLVector3();
		gridInfo.cellLayout = reader.ReadInt32();
		gridInfo.cellSwizzle = reader.ReadInt32();
		int num = reader.ReadInt32();
		gridInfo.tileMaps = new TileInfos[num];
		string[] array = new string[num];
		int[] array2 = new int[num];
		for (int i = 0; i < num; i++)
		{
			string text = reader.ReadString();
			array[i] = text;
		}
		for (int j = 0; j < num; j++)
		{
			int num2 = reader.ReadInt32();
			array2[j] = num2;
		}
		for (int k = 0; k < num; k++)
		{
			TileInfos tileInfos = TileMapDeserializer.ReadMap(reader);
			gridInfo.tileMaps[k] = tileInfos;
			tileInfos.name = array[k];
		}
		return gridInfo;
	}
	
	private static TileInfos ReadMap(Deserializer reader)
	{
		int position = reader.Position;
		int rawDataSize = reader.RawDataSize;
		bool flag = rawDataSize - position < 9;
		if (flag)
		{
			//throw new BaseTileMapSerializer.FileContentException(string.Format("FileSpace is not enough for head!! pos = {0} len = {1} ", position, rawDataSize));
		}
		byte[] bs = reader.ReadBytes_255();
		bool flag2 = !CheckMagicNumber(bs);
		if (flag2)
		{
			//throw new BaseTileMapSerializer.FileContentException("MagicNumberError");
		}
		int num = reader.ReadInt32();
		int num2 = rawDataSize - reader.Position;
		bool flag3 = num2 < num;
		if (flag3)
		{
			//throw new BaseTileMapSerializer.FileContentException(string.Format("FileSpace is not enough!! reader.Position = {0} remainSize = {1} needSize = {2}", reader.Position, num2, num));
		}
		bool isTagMap = reader.ReadBoolean();
		bool hasCollider = reader.ReadBoolean();
		int num3 = reader.ReadInt32();
		bool flag4 = reader.ReadBoolean();
		bool flag5 = reader.ReadBoolean();
		int x = reader.ReadInt32();
		int y = reader.ReadInt32();
		int num4 = reader.ReadInt32();
		int y2 = reader.ReadInt32();
		int num5 = reader.ReadInt32();
		bool flag6 = num5 > num3;
		if (flag6)
		{
			//throw new BaseTileMapSerializer.FileContentException(string.Format("notNullCount {0} > count {1}", num5, num3));
		}
		ushort[] array = new ushort[num3];
		TileInfos tileInfos = new TileInfos();
		tileInfos.isTagMap = isTagMap;
		tileInfos.hasCollider = hasCollider;
		tileInfos.tileIDs = array;
		tileInfos.min = new LVector2Int(x, y);
		tileInfos.size = new LVector2Int(num4, y2);
		int num6 = reader.ReadInt32();
		ushort[] array2 = new ushort[num6];
		for (int i = 0; i < num6; i++)
		{
			array2[i] = reader.ReadUInt16();
		}
		Dictionary<ushort, ushort> dictionary = new Dictionary<ushort, ushort>();
		ushort num7 = 0;
		while ((int)num7 < num6)
		{
			dictionary[(ushort)(num7 + 1)] = array2[(int)num7];
			num7 += 1;
		}
		bool flag7 = flag5;
		if (flag7)
		{
			for (int j = 0; j < num3; j++)
			{
				byte b = reader.ReadByte();
				bool flag8 = b == 0;
				if (flag8)
				{
					array[j] = 0;
				}
				else
				{
					ushort num8;
					bool flag9 = dictionary.TryGetValue((ushort)b, out num8);
					if (flag9)
					{
						array[j] = num8;
					}
				}
			}
		}
		else
		{
			bool flag10 = flag4;
			if (flag10)
			{
				for (int k = 0; k < num5; k++)
				{
					byte b2 = reader.ReadByte();
					byte b3 = reader.ReadByte();
					byte key = reader.ReadByte();
					ushort num9;
					bool flag11 = dictionary.TryGetValue((ushort)key, out num9);
					if (flag11)
					{
						array[(int)b3 * num4 + (int)b2] = num9;
					}
					Debug.Assert(array[(int)b3 * num4 + (int)b2] > 0, "");
				}
			}
			else
			{
				for (int l = 0; l < num5; l++)
				{
					ushort num10 = reader.ReadUInt16();
					ushort num11 = reader.ReadUInt16();
					byte key2 = reader.ReadByte();
					ushort num12;
					bool flag12 = dictionary.TryGetValue((ushort)key2, out num12);
					if (flag12)
					{
						array[(int)num11 * num4 + (int)num10] = num12;
					}
					Debug.Assert(array[(int)num11 * num4 + (int)num10] > 0, "");
				}
			}
		}

		//tileInfos.tileIDs = array;
		return tileInfos;
	}
}
