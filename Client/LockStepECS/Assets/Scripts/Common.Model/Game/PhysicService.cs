using System;
using System.Collections.Generic;
using System.IO;
using Lockstep.Collision2D;
using Lockstep.Logging;
using Lockstep.Math;
using Lockstep.Util;

namespace Lockstep.Game
{
	// Token: 0x0200005E RID: 94
	public class PhysicService : UpdatableService
	{
		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000264 RID: 612 RVA: 0x000075C7 File Offset: 0x000057C7
		public static PhysicService Instance
		{
			get
			{
				return PhysicService._instance;
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000265 RID: 613 RVA: 0x000075CE File Offset: 0x000057CE
		public bool[] collisionMatrix
		{
			get
			{
				return this.config.collisionMatrix;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000266 RID: 614 RVA: 0x000075DB File Offset: 0x000057DB
		public LVector3 pos
		{
			get
			{
				return this.config.pos;
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000267 RID: 615 RVA: 0x000075E8 File Offset: 0x000057E8
		public LFloat worldSize
		{
			get
			{
				return this.config.worldSize;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000268 RID: 616 RVA: 0x000075F5 File Offset: 0x000057F5
		public LFloat minNodeSize
		{
			get
			{
				return this.config.minNodeSize;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000269 RID: 617 RVA: 0x00007602 File Offset: 0x00005802
		public LFloat loosenessval
		{
			get
			{
				return this.config.loosenessval;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x0600026A RID: 618 RVA: 0x0000760F File Offset: 0x0000580F
		private LFloat halfworldSize
		{
			get
			{
				return this.worldSize / 2 - 5;
			}
		}

		// Token: 0x0600026B RID: 619 RVA: 0x00007623 File Offset: 0x00005823
		public override void DoAwake(IServiceContainer services)
		{
			PhysicService._instance = this;
			this._DoStart();
		}

		// Token: 0x0600026C RID: 620 RVA: 0x00007634 File Offset: 0x00005834
		public void _DoStart()
		{
			bool flag = PhysicService._instance != this;
			if (flag)
			{
				Debug.LogError("Duplicate CollisionSystemAdapt!", Array.Empty<object>());
			}
			else
			{
				CollisionSystem collisionSystem = new CollisionSystem
				{
					worldSize = this.worldSize,
					pos = this.pos,
					minNodeSize = this.minNodeSize,
					loosenessval = this.loosenessval
				};
				this._debugService.Trace(string.Format("worldSize:{0} pos:{1} minNodeSize:{2} loosenessval:{3}", new object[]
				{
					this.worldSize,
					this.pos,
					this.minNodeSize,
					this.loosenessval
				}), false, false);
				this.collisionSystem = collisionSystem;
				collisionSystem.DoStart(this.collisionMatrix, this.allTypes);
				CollisionSystem collisionSystem2 = collisionSystem;
				collisionSystem2.funcGlobalOnTriggerEvent = (FuncGlobalOnTriggerEvent)Delegate.Combine(collisionSystem2.funcGlobalOnTriggerEvent, new FuncGlobalOnTriggerEvent(PhysicService.GlobalOnTriggerEvent));
				string colliderDataRelFilePath = this.config.ColliderDataRelFilePath;
				ColliderData[] datas = ColliderDataUtil.ReadFromFile(Path.Combine(ProjectConfig.DataPath, colliderDataRelFilePath));
				this.InitStaticColliders(datas);
			}
		}

		// Token: 0x0600026D RID: 621 RVA: 0x00007754 File Offset: 0x00005954
		public override void DoUpdate(LFloat deltaTime)
		{
			this.collisionSystem.ShowTreeId = this.showTreeId;
			this.collisionSystem.DoUpdate(deltaTime);
		}

		// Token: 0x0600026E RID: 622 RVA: 0x00007778 File Offset: 0x00005978
		public static void GlobalOnTriggerEvent(ColliderProxy a, ColliderProxy b, ECollisionEvent type)
		{
			ILPTriggerEventHandler a2;
			bool flag = PhysicService._colProxy2Mono.TryGetValue(a, out a2);
			if (flag)
			{
				CollisionSystem.TriggerEvent(a2, b, type);
			}
			ILPTriggerEventHandler a3;
			bool flag2 = PhysicService._colProxy2Mono.TryGetValue(b, out a3);
			if (flag2)
			{
				CollisionSystem.TriggerEvent(a3, a, type);
			}
		}

		// Token: 0x0600026F RID: 623 RVA: 0x000077C0 File Offset: 0x000059C0
		public static ColliderProxy GetCollider(int id)
		{
			return PhysicService._instance.collisionSystem.GetCollider(id);
		}

		// Token: 0x06000270 RID: 624 RVA: 0x000077E4 File Offset: 0x000059E4
		public static bool Raycast(int layerMask, Ray2D ray, out LRaycastHit2D ret)
		{
			return PhysicService.Raycast(layerMask, ray, out ret, LFloat.MaxValue);
		}

		// Token: 0x06000271 RID: 625 RVA: 0x00007804 File Offset: 0x00005A04
		public static bool Raycast(int layerMask, Ray2D ray, out LRaycastHit2D ret, LFloat maxDistance)
		{
			ret = default(LRaycastHit2D);
			LFloat t = LFloat.one;
			if (!_instance.DoRaycast(layerMask, ray, out t, out int id, maxDistance))
			{
				return false;
			}
			ret.point = ray.origin + ray.direction * t;
			ret.distance = t * ray.direction.magnitude;
			ret.colliderId = id;
			return true;
		}

		// Token: 0x06000272 RID: 626 RVA: 0x0000787A File Offset: 0x00005A7A
		public static void QueryRegion(int layerType, LVector2 pos, LVector2 size, LVector2 forward, FuncCollision callback)
		{
			PhysicService._instance._QueryRegion(layerType, pos, size, forward, callback);
		}

		// Token: 0x06000273 RID: 627 RVA: 0x0000788E File Offset: 0x00005A8E
		public static void QueryRegion(int layerType, LVector2 pos, LFloat radius, FuncCollision callback)
		{
			PhysicService._instance._QueryRegion(layerType, pos, radius, callback);
		}

		// Token: 0x06000274 RID: 628 RVA: 0x000078A0 File Offset: 0x00005AA0
		private void _QueryRegion(int layerType, LVector2 pos, LVector2 size, LVector2 forward, FuncCollision callback)
		{
			this.collisionSystem.QueryRegion(layerType, pos, size, forward, callback);
		}

		// Token: 0x06000275 RID: 629 RVA: 0x000078B6 File Offset: 0x00005AB6
		private void _QueryRegion(int layerType, LVector2 pos, LFloat radius, FuncCollision callback)
		{
			this.collisionSystem.QueryRegion(layerType, pos, radius, callback);
		}

		// Token: 0x06000276 RID: 630 RVA: 0x000078CC File Offset: 0x00005ACC
		public bool DoRaycast(int layerMask, Ray2D ray, out LFloat t, out int id, LFloat maxDistance)
		{
			UnityEngine.Profiling.Profiler.BeginSample("DoRaycast ");
			bool result = this.collisionSystem.Raycast(layerMask, ray, out t, out id, maxDistance);
			UnityEngine.Profiling.Profiler.EndSample();
			return result;
		}

		// Token: 0x06000277 RID: 631 RVA: 0x00007903 File Offset: 0x00005B03
		public void RigisterPrefab(int prefabId, int val)
		{
			PhysicService._fabId2Layer[prefabId] = val;
		}

		// Token: 0x06000278 RID: 632 RVA: 0x00007914 File Offset: 0x00005B14
		public void RegisterEntity(int prefabId, object entity)
		{
		}

		// Token: 0x06000279 RID: 633 RVA: 0x00007924 File Offset: 0x00005B24
		public void InitStaticColliders(ColliderData[] datas)
		{
			foreach (ColliderData colliderData in datas)
			{
				ColliderProxy colliderProxy = new ColliderProxy();
				colliderProxy.EntityObject = null;
				colliderProxy.IsStatic = true;
				colliderProxy.LayerType = 0;
				this.collisionSystem.AddCollider(colliderProxy);
			}
		}

		// Token: 0x0600027A RID: 634 RVA: 0x00007974 File Offset: 0x00005B74
		public void AttachToColSystem(int layer, ColliderPrefab prefab, object entity)
		{
			ColliderProxy colliderProxy = new ColliderProxy();
			colliderProxy.EntityObject = entity;
			colliderProxy.IsStatic = false;
			colliderProxy.LayerType = layer;
			this.collisionSystem.AddCollider(colliderProxy);
		}

		// Token: 0x0600027B RID: 635 RVA: 0x000079AC File Offset: 0x00005BAC
		public void RemoveCollider(ILPTriggerEventHandler handler)
		{
			ColliderProxy colliderProxy;
			bool flag = PhysicService._mono2ColProxy.TryGetValue(handler, out colliderProxy);
			if (flag)
			{
				this.RemoveCollider(colliderProxy);
				PhysicService._mono2ColProxy.Remove(handler);
				PhysicService._colProxy2Mono.Remove(colliderProxy);
			}
		}

		// Token: 0x0600027C RID: 636 RVA: 0x000079ED File Offset: 0x00005BED
		public void RemoveCollider(ColliderProxy collider)
		{
			this.collisionSystem.RemoveCollider(collider);
		}

		// Token: 0x0600027D RID: 637 RVA: 0x000079FD File Offset: 0x00005BFD
		public void OnDrawGizmos()
		{
			ICollisionSystem collisionSystem = this.collisionSystem;
			if (collisionSystem != null)
			{
				collisionSystem.DrawGizmos();
			}
		}

		// Token: 0x040000FE RID: 254
		private static PhysicService _instance;

		// Token: 0x040000FF RID: 255
		private ICollisionSystem collisionSystem;

		// Token: 0x04000100 RID: 256
		public CollisionConfig config;

		// Token: 0x04000101 RID: 257
		private static Dictionary<int, ColliderPrefab> _fabId2ColPrefab = new Dictionary<int, ColliderPrefab>();

		// Token: 0x04000102 RID: 258
		private static Dictionary<int, int> _fabId2Layer = new Dictionary<int, int>();

		// Token: 0x04000103 RID: 259
		private static Dictionary<ILPTriggerEventHandler, ColliderProxy> _mono2ColProxy = new Dictionary<ILPTriggerEventHandler, ColliderProxy>();

		// Token: 0x04000104 RID: 260
		private static Dictionary<ColliderProxy, ILPTriggerEventHandler> _colProxy2Mono = new Dictionary<ColliderProxy, ILPTriggerEventHandler>();

		// Token: 0x04000105 RID: 261
		private int[] allTypes = new int[]
		{
			default(int),
			1,
			2
		};

		// Token: 0x04000106 RID: 262
		public int showTreeId = 0;
	}
}
