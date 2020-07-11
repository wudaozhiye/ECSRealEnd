using System;
using System.Collections.Generic;
using Lockstep.Math;

namespace Lockstep.Game
{
	public class TileInfos : object
	{
		public string name;
		
		public int renderOrder;
		
		public LVector2Int min;
		
		public LVector2Int size;
		
		public ushort[] tileIDs;
		
		public bool isTagMap = false;
		
		public bool hasCollider = true;
		
		public ITileInfosView view;
		public ushort GetTileID(LVector2Int pos)
		{
			LVector2Int lvector2Int = pos - this.min;
			bool flag = lvector2Int.x < 0 || lvector2Int.y < 0 || lvector2Int.x >= this.size.x || lvector2Int.y >= this.size.y;
			ushort result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				ushort num = this.tileIDs[lvector2Int.y * this.size.x + lvector2Int.x];
				result = num;
			}
			return result;
		}
		
		public void SetTileID(LVector2Int pos, ushort id)
		{
			LVector2Int lvector2Int = pos - this.min;
			bool flag = lvector2Int.x < 0 || lvector2Int.y < 0 || lvector2Int.x >= this.size.x || lvector2Int.y >= this.size.y;
			if (!flag)
			{
				int num = lvector2Int.y * this.size.x + lvector2Int.x;
				this.tileIDs[num] = id;
				ITileInfosView tileInfosView = this.view;
				if (tileInfosView != null)
				{
					tileInfosView.SetTileID(num, pos, id);
				}
			}
		}
		
		public List<LVector2> GetAllTiles(int typeId)
		{
			List<LVector2> list = new List<LVector2>();
			ushort[] array = this.tileIDs;
			LVector3Int[] allPositions = this.GetAllPositions();
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				bool flag = (int)array[i] == typeId;
				if (flag)
				{
					list.Add(new LVector2(allPositions[i].x, allPositions[i].y));
				}
			}
			return list;
		}
		
		public LVector3Int[] GetAllPositions()
		{
			LVector3Int[] array = new LVector3Int[this.tileIDs.Length];
			int x = this.min.x;
			int y = this.min.y;
			int x2 = this.size.x;
			int y2 = this.size.y;
			for (int i = 0; i < y2; i++)
			{
				for (int j = 0; j < x2; j++)
				{
					array[i * x2 + j] = new LVector3Int(x + j, y + i, 0);
				}
			}
			return array;
		}
	}
}
