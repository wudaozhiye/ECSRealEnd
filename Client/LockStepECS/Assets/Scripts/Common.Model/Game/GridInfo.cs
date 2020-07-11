using System;
using Lockstep.Math;

namespace Lockstep.Game
{
	public class GridInfo : object
	{
		public TileInfos[] tileMaps;
		
		public bool[,] ColliderMasks;
		
		public LVector2Int min;
		
		public LVector2Int max;
		
		public LVector3 cellSize;
		
		public LVector3 cellGap;
		
		public int cellLayout;
		
		public int cellSwizzle;
		public TileInfos GetMapInfo(string name)
		{
			for (int i = 0; i < this.tileMaps.Length; i++)
			{
				bool flag = this.tileMaps[i].name == name;
				if (flag)
				{
					return this.tileMaps[i];
				}
			}
			return null;
		}
		
		
	}
}
