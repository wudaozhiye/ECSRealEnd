using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lockstep.Math;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Lockstep.Game
{
	[Serializable]
	public class UnityMap2DUtil : object
	{
		public static string idMapPath = "TileIDMap";
		
		private static bool hasLoadIDMapConfig = false;
		
		private static TileBase[] id2Tiles = new TileBase[65536];
		
		private static Dictionary<TileBase, ushort> tile2ID = new Dictionary<TileBase, ushort>();

		public static void LoadLevel(Grid grid, int level)
		{
			CheckLoadTileIDMap();
			GridInfo info = Map2DUtil.LoadMapData(level);
			BindMapView(grid, info);
		}
		
		public static void SaveLevel(Grid grid, int level)
		{
			bool flag = grid == null;
			if (!flag)
			{
				CheckLoadTileIDMap();
				byte[] array = TileMapSerializer.WriteGrid(grid, new Func<TileBase, ushort>(UnityMap2DUtil.Tile2ID));
				bool flag2 = array != null;
				if (flag2)
				{
					File.WriteAllBytes(Map2DUtil.GetMapPathFull(level), array);
				}
			}
		}
		
		public static void BindMapView(Grid grid, GridInfo info)
		{
			bool flag = grid != null;
			if (flag)
			{
				foreach (Tilemap tilemap in grid.GetComponentsInChildren<Tilemap>())
				{
					TileInfos mapInfo = info.GetMapInfo(tilemap.name);
					bool flag2 = mapInfo == null;
					if (!flag2)
					{
						tilemap.ClearAllTiles();
						TileInfosView tileInfosView = new TileInfosView();
						mapInfo.view = tileInfosView;
						tileInfosView.tilemap = tilemap;
						int num = mapInfo.tileIDs.Length;
						TileBase[] array = new TileBase[num];
						for (int j = 0; j < num; j++)
						{
							array[j] = UnityMap2DUtil.ID2Tile(mapInfo.tileIDs[j]);
						}
						tilemap.SetTiles((from t in mapInfo.GetAllPositions()
						select t.ToVector3Int()).ToArray<Vector3Int>(), array);
						bool isPlaying = Application.isPlaying;
						if (isPlaying)
						{
							bool isTagMap = mapInfo.isTagMap;
							if (isTagMap)
							{
								tilemap.GetComponent<TilemapRenderer>().enabled = false;
							}
						}
					}
				}
			}
		}
		
		public static ushort Tile2ID(TileBase tile)
		{
			ushort result;
			if (tile == null)
			{
				result = 0;
			}
			else
			{
				ushort num;
				bool flag2 = UnityMap2DUtil.tile2ID.TryGetValue(tile, out num);
				if (flag2)
				{
					result = num;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}
		
		public static TileBase ID2Tile(ushort tile)
		{
			return UnityMap2DUtil.id2Tiles[(int)tile];
		}
		//加载  解析配置表
		public static void CheckLoadTileIDMap()
		{
			if (!hasLoadIDMapConfig)
			{
				UnityMap2DUtil.hasLoadIDMapConfig = true;
				TextAsset textAsset = Resources.Load<TextAsset>(UnityMap2DUtil.idMapPath);
				if (textAsset == null)
				{
					Debug.LogError("CheckLoadTileIDMap:LoadFileFailed " + UnityMap2DUtil.idMapPath);
				}
				else
				{
					string text = textAsset.text;
					string[] array = text.Replace("\r\n", "\n").Split('\n');
					int num = array.Length;
					UnityMap2DUtil.tile2ID = new Dictionary<TileBase, ushort>(num);
					int i = 0;
					try
					{
						while (i < num)
						{
							string text2 = array[i];
							bool flag3 = string.IsNullOrEmpty(text2.Trim());
							if (!flag3)
							{
								string[] array2 = text2.Split('=');
								ushort num2 = ushort.Parse(array2[0].Trim());
								string relPath = array2[1].Trim();
								TileBase tileBase = UnityMap2DUtil.LoadTile(relPath);
								id2Tiles[(int)num2] = tileBase;
								tile2ID.Add(tileBase, num2);
							}
							i++;
						}
					}
					catch (Exception ex)
					{
						Debug.LogErrorFormat("CheckLoadTileIDMap:ParseError line = {0} str = {1} path = {2} e= {3}", new object[]
						{
							i + 1,
							array[i],
							UnityMap2DUtil.idMapPath,
							ex.ToString()
						});
					}
				}
			}
		}
		private static TileBase LoadTile(string relPath)
		{
			return Resources.Load<TileBase>(relPath);
		}
		
	}
}
