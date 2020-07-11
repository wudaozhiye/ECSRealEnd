using System;
using Lockstep.Logging;
using Lockstep.Math;

namespace Lockstep.Game
{
	public abstract class BaseMap2DService : BaseService, IMap2DService, IService
	{
		public GridInfo gridInfo { get; private set; }
		
		public TileInfos GetMapInfo(string name)
		{
			return this.gridInfo.GetMapInfo(name);
		}
		
		public ushort Pos2TileId(LVector2Int pos, bool isCollider)
		{
			bool flag = pos.x > this.mapDataMax.x || pos.x < this.mapDataMin.x || pos.y > this.mapDataMax.y || pos.y < this.mapDataMin.y;
			ushort result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				LVector2Int lvector2Int = pos - this.mapDataMin;
				result = this.mapDataIds[lvector2Int.x, lvector2Int.y];
			}
			return result;
		}
		
		public void ReplaceTile(LVector2Int pos, ushort srcId, ushort dstId)
		{
			bool flag = pos.x > this.mapDataMax.x || pos.x < this.mapDataMin.x || pos.y > this.mapDataMax.y || pos.y < this.mapDataMin.y;
			if (!flag)
			{
				LVector2Int lvector2Int = pos - this.mapDataMin;
				this.mapDataIds[lvector2Int.x, lvector2Int.y] = dstId;
				this.OnReplaceTile(pos, srcId, dstId);
			}
		}

		public void DoAfterInit()
		{
			for (int i = 0; i < this.gridInfo.tileMaps.Length; i++)
			{
				TileInfos tileInfos = this.gridInfo.tileMaps[i];
				bool isTagMap = tileInfos.isTagMap;
				if (!isTagMap)
				{
					LVector2Int min = tileInfos.min;
					LVector2Int size = tileInfos.size;
					this.mapDataMin.x = LMath.Min(this.mapDataMin.x, min.x);
					this.mapDataMin.y = LMath.Min(this.mapDataMin.y, min.y);
					this.mapDataMax.x = LMath.Max(this.mapDataMax.y, min.x + size.x);
					this.mapDataMax.y = LMath.Max(this.mapDataMax.y, min.y + size.y);
				}
			}
			this.mapDataSize = this.mapDataMax - this.mapDataMin + LVector2Int.one;
			this.mapDataIds = new ushort[this.mapDataSize.x, this.mapDataSize.y];
			for (int j = 0; j < this.mapDataSize.x; j++)
			{
				for (int k = 0; k < this.mapDataSize.y; k++)
				{
					LVector2Int pos = new LVector2Int(mapDataMin.x + j, mapDataMin.y + k);
					this.mapDataIds[j, k] = this.RawPos2TileId(pos, false);
				}
			}
		}
		
		public ushort RawPos2TileId(LVector2Int pos, bool isCollider)
		{
			for (int i = 0; i < this.gridInfo.tileMaps.Length; i++)
			{
				TileInfos tileInfos = this.gridInfo.tileMaps[i];
				bool isTagMap = tileInfos.isTagMap;
				if (!isTagMap)
				{
					bool flag = isCollider && !tileInfos.hasCollider;
					if (!flag)
					{
						ushort tileID = tileInfos.GetTileID(pos);
						bool flag2 = tileID > 0;
						if (flag2)
						{
							return tileID;
						}
					}
				}
			}
			return 0;
		}
		
		private void OnReplaceTile(LVector2Int pos, ushort srcId, ushort dstId)
		{
			for (int i = 0; i < this.gridInfo.tileMaps.Length; i++)
			{
				TileInfos tileInfos = this.gridInfo.tileMaps[i];
				ushort tileID = tileInfos.GetTileID(pos);
				bool flag = tileID == srcId;
				if (flag)
				{
					this.cmdBuffer.Execute(base.CurTick, new BaseMap2DService.CmdSetTile(tileInfos, pos, srcId, dstId));
				}
			}
		}
		
		public void LoadLevel(int level)
		{
			Debug.Log("Load Level " + level, Array.Empty<object>());
			this.gridInfo = Map2DUtil.LoadMapData(level);
			this.DoAfterInit();
			this.OnLoadLevel(level, this.gridInfo);
		}
		
		protected virtual void OnLoadLevel(int level, GridInfo gridInfo)
		{
		}


		public LVector2Int mapDataMin;


		public LVector2Int mapDataMax;


		public LVector2Int mapDataSize;


		public ushort[,] mapDataIds;
		
		public class CmdSetTile : BaseCommand
		{

			public CmdSetTile(TileInfos tilemap, LVector2Int pos, ushort srcId, ushort dstId)
			{
				this.tilemap = tilemap;
				this.pos = pos;
				this.srcId = srcId;
				this.dstId = dstId;
			}
			
			public override void Do(object param)
			{
				this.tilemap.SetTileID(this.pos, this.dstId);
			}
			
			public override void Undo(object param)
			{
				this.tilemap.SetTileID(this.pos, this.srcId);
			}
			
			public TileInfos tilemap;
			
			public LVector2Int pos;
			
			public ushort srcId;
			
			public ushort dstId;
		}
	}
}
