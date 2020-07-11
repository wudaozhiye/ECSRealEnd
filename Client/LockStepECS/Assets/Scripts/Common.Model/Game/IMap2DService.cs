﻿using System;
using Lockstep.Math;

namespace Lockstep.Game
{
	public interface IMap2DService : IService
	{
		ushort Pos2TileId(LVector2Int pos, bool isCollider);
		
		void ReplaceTile(LVector2Int pos, ushort srcId, ushort dstId);
	}
}
