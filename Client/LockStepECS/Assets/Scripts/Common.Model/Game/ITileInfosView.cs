using System;
using Lockstep.Math;

namespace Lockstep.Game
{
	public interface ITileInfosView
	{
		void SetTileID(int idx, LVector2Int pos, ushort id);
	}
}
