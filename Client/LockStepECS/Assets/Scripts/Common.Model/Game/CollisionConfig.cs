using System;
using System.Collections.Generic;
using Lockstep.Math;

namespace Lockstep.Game
{

	[Serializable]
	public class CollisionConfig
	{
		public string[] ColliderLayerNames
		{
			get
			{
				bool flag = this._colliderLayerNames == null || this._colliderLayerNames.Length == 0;
				if (flag)
				{
					List<string> list = new List<string>();
					for (int i = 0; i < 3; i++)
					{
						List<string> list2 = list;
						EColliderLayer ecolliderLayer =(EColliderLayer)i;
						list2.Add(ecolliderLayer.ToString());
					}
					this._colliderLayerNames = list.ToArray();
				}
				return this._colliderLayerNames;
			}
		}
		
		public void SetColliderPair(int a, int b, bool val)
		{
			this.collisionMatrix[a * 3 + b] = val;
			this.collisionMatrix[b * 3 + a] = val;
		}
		
		public bool GetColliderPair(int a, int b)
		{
			return this.collisionMatrix[a * 3 + b];
		}
		
		public LVector2 scrollPos;
		
		public bool isShow = true;
		
		public bool[] collisionMatrix = new bool[9];
		
		private string[] _colliderLayerNames;
		
		public LVector3 pos;
		
		public LFloat worldSize = new LFloat(60);
		
		public LFloat minNodeSize = new LFloat(1);
		
		public LFloat loosenessval = new LFloat(null, 1250L);
		
		public string ColliderDataRelFilePath;
		
		public LFloat percent = new LFloat(null, 100L);
		
		public int count = 100;

		public int showTreeId = 0;
	}
}
