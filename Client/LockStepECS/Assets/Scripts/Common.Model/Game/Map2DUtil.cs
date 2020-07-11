using System;
using System.IO;
using Lockstep.Serialization;

namespace Lockstep.Game
{
	public static class Map2DUtil 
	{
		public static string GetMapPathFull(int level)
		{
			return ProjectConfig.MapPath + level + ".bytes";
		}
		
		public static GridInfo LoadMapData(int mapId)
		{
			string mapPathFull = Map2DUtil.GetMapPathFull(mapId);
			byte[] source = File.ReadAllBytes(mapPathFull);
			Deserializer reader = new Deserializer(source);
			return TileMapDeserializer.ReadGrid(reader);
		}
	}
}
