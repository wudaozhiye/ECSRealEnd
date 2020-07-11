using System;
using System.Collections.Generic;
using Lockstep.Math;
using Lockstep.Serialization;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapSerializer : BaseTileMapSerializer
{
	public static byte[] WriteGrid(Grid grid, Func<TileBase, ushort> FuncGetTileIdx)
	{
		bool flag = grid == null;
		byte[] result;
		if (flag)
		{
			result = null;
		}
		else
		{
			Serializer serializer = new Serializer();
			LVector2Int val = new LVector2Int(2147483647, 2147483647);
			LVector2Int val2 = new LVector2Int(-2147483648, -2147483648);
			Tilemap[] componentsInChildren = grid.GetComponentsInChildren<Tilemap>();
			foreach (Tilemap tilemap in componentsInChildren)
			{
				Vector3Int min = tilemap.cellBounds.min;
				Vector3Int max = tilemap.cellBounds.max;
				bool flag2 = min.x < val.x;
				if (flag2)
				{
					val.x = min.x;
				}
				bool flag3 = min.y < val.y;
				if (flag3)
				{
					val.y = min.y;
				}
				bool flag4 = max.x > val2.x;
				if (flag4)
				{
					val2.x = max.x;
				}
				bool flag5 = max.y > val2.y;
				if (flag5)
				{
					val2.y = max.y;
				}
			}
			serializer.Write(val);
			serializer.Write(val2);
			serializer.Write(grid.cellSize.ToLVector3());
			serializer.Write(grid.cellGap.ToLVector3());
			serializer.Write((int)grid.cellLayout);
			serializer.Write((int)grid.cellSwizzle);
			Tilemap[] componentsInChildren2 = grid.GetComponentsInChildren<Tilemap>();
			serializer.Write(componentsInChildren2.Length);
			foreach (Tilemap tilemap2 in componentsInChildren2)
			{
				serializer.Write(tilemap2.name);
			}
			foreach (Tilemap tilemap3 in componentsInChildren2)
			{
				TilemapRenderer component = tilemap3.GetComponent<TilemapRenderer>();
				Debug.Log(tilemap3.name + " " + component.sortingOrder);
				serializer.Write(component.sortingOrder);
			}
			foreach (Tilemap map in componentsInChildren2)
			{
				TileMapSerializer.WriteMap(serializer, map, FuncGetTileIdx);
			}
			result = serializer.CopyData();
		}
		return result;
	}
	
	private static void WriteMap(Serializer writer, Tilemap map, Func<TileBase, ushort> FuncGetTileIdx)
	{
		Debug.Assert(map.cellBounds.size.z == 1, "map.cellBounds.size.z == 1");
		TileBase[] tilesBlock = map.GetTilesBlock(map.cellBounds);
		Dictionary<TileBase, int> dictionary = new Dictionary<TileBase, int>();
		int num = tilesBlock.Length;
		int num2 = 0;
		for (int i = 0; i < num; i++)
		{
			TileBase tileBase = tilesBlock[i];
			bool flag = tileBase != null;
			if (flag)
			{
				num2++;
				int num3;
				bool flag2 = dictionary.TryGetValue(tileBase, out num3);
				if (flag2)
				{
					dictionary[tileBase] = num3 + 1;
				}
				else
				{
					dictionary.Add(tileBase, 1);
				}
			}
		}
		bool flag3 = map.cellBounds.size.x < 255 && map.cellBounds.size.y < 255;
		float num4 = flag3 ? 0.333333343f : 0.2f;
		bool flag4 = (float)num2 * 1f / (float)num > num4;
		int count = dictionary.Count;
		writer.WriteBytes_255(BaseTileMapSerializer.MAGIC_BYTES);
		int position = writer.Position;
		writer.Write(0);
		bool value = true;
		bool value2 = false;
		ITileMapHelper component = map.GetComponent<ITileMapHelper>();
		bool flag5 = component != null;
		if (flag5)
		{
			value2 = component.IsTagMap;
			value = component.IsCollider;
		}
		writer.Write(value2);
		writer.Write(value);
		writer.Write(num);
		writer.Write(flag3);
		writer.Write(flag4);
		BoundsInt cellBounds = map.cellBounds;
		int x = cellBounds.size.x;
		int y = cellBounds.size.y;
		writer.Write(cellBounds.min.x);
		writer.Write(cellBounds.min.y);
		writer.Write(x);
		writer.Write(y);
		writer.Write(num2);
		Dictionary<TileBase, int> dictionary2 = new Dictionary<TileBase, int>(count);
		int num5 = 0;
		ushort[] array = new ushort[count];
		foreach (KeyValuePair<TileBase, int> keyValuePair in dictionary)
		{
			TileBase key = keyValuePair.Key;
			array[num5] = FuncGetTileIdx(keyValuePair.Key);
			num5 = (dictionary2[key] = num5 + 1);
		}
		Debug.Assert(num5 <= 254, string.Format("The num of tile type in single tilemap is too much {0}>254", num5));
		writer.Write(count);
		for (int j = 0; j < count; j++)
		{
			writer.Write(array[j]);
		}
		bool flag6 = flag4;
		if (flag6)
		{
			for (int k = 0; k < num; k++)
			{
				TileBase tileBase2 = tilesBlock[k];
				bool flag7 = tileBase2 == null;
				if (flag7)
				{
					writer.Write(0);
				}
				else
				{
					writer.Write((byte)dictionary2[tileBase2]);
				}
			}
		}
		else
		{
			for (int l = 0; l < x; l++)
			{
				for (int m = 0; m < y; m++)
				{
					TileBase tileBase3 = tilesBlock[m * x + l];
					bool flag8 = tileBase3 != null;
					if (flag8)
					{
						bool flag9 = flag3;
						if (flag9)
						{
							writer.Write((byte)l, (byte)m, (byte)dictionary2[tileBase3]);
						}
						else
						{
							writer.Write((ushort)l, (ushort)m, (byte)dictionary2[tileBase3]);
						}
					}
				}
			}
		}
		int position2 = writer.Position;
		int value3 = position2 - position - 4;
		writer.SetPosition(position);
		writer.Write(value3);
		writer.SetPosition(position2);
	}
}
