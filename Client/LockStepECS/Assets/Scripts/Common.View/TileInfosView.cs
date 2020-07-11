using System;
using Lockstep.Math;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Lockstep.Game
{
	public class TileInfosView : ITileInfosView
	{
		public Tilemap tilemap;
		public void SetTileID(int idx, LVector2Int pos, ushort id)
		{
			TileBase tileBase = UnityMap2DUtil.ID2Tile(id);
			this.tilemap.SetTile(new Vector3Int(pos.x, pos.y, 0), tileBase);
		}
		
	}
}
