using System;
using Lockstep.Collision2D;
using Lockstep.Game;
using Lockstep.Math;
using UnityEngine;
using Object = UnityEngine.Object;

// Token: 0x0200000E RID: 14
[Serializable]
public class CollisionSystemDebuger : MonoBehaviour
{
	// Token: 0x06000058 RID: 88 RVA: 0x000037FC File Offset: 0x000019FC
	private void OnDrawGizmos()
	{
		bool flag = PhysicService.Instance != null;
		if (flag)
		{
			PhysicService.Instance.showTreeId = this.showTreeId;
			PhysicService.Instance.OnDrawGizmos();
		}
	}

	// Token: 0x06000059 RID: 89 RVA: 0x00003834 File Offset: 0x00001A34
	private void Update()
	{
		bool flag = !this.isShowStaticCollision;
		if (!flag)
		{
			bool flag2 = PhysicService.Instance != null;
			if (flag2)
			{
				ColliderProxy _DebugColProxy = CollisionSystem.__DebugColProxy1;
				ColliderProxy _DebugColProxy2 = CollisionSystem.__DebugColProxy2;
				Object.Destroy(this.proxyFab1);
				Object.Destroy(this.proxyFab2);
				bool flag3 = _DebugColProxy != null && _DebugColProxy2 != null;
				if (flag3)
				{
					this.DrawProxy(_DebugColProxy, ref this.proxyFab1);
					this.DrawProxy(_DebugColProxy2, ref this.proxyFab2);
				}
			}
		}
	}

	// Token: 0x0600005A RID: 90 RVA: 0x000038B4 File Offset: 0x00001AB4
	private void DrawProxy(ColliderProxy proxy, ref GameObject proxyFab1)
	{
		CBaseShape collider = proxy.Prefab.collider;
		Transform2D transform2D = proxy.Transform2D + proxy.Prefab.transform;
		EShape2D typeId = (EShape2D)collider.TypeId;
		EShape2D eshape2D = typeId;
		if (eshape2D != EShape2D.Circle)
		{
			if (eshape2D == EShape2D.OBB)
			{
				proxyFab1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
				proxyFab1.transform.SetParent(base.transform, false);
				proxyFab1.transform.localScale = ((collider as COBB).size * 2).ToVector3XZ(proxy.Height);
				proxyFab1.transform.localPosition = transform2D.Pos3.ToVector3();
				proxyFab1.transform.rotation = Quaternion.Euler(0f, transform2D.deg.ToFloat(), 0f);
			}
		}
		else
		{
			proxyFab1 = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
			proxyFab1.transform.SetParent(base.transform, false);
			float num = (collider as CCircle).radius.ToFloat() * 2f;
			proxyFab1.transform.localScale = new Vector3(num, 2f, num);
			proxyFab1.transform.localPosition = transform2D.Pos3.ToVector3();
		}
	}

	// Token: 0x04000069 RID: 105
	public int showTreeId;

	// Token: 0x0400006A RID: 106
	public GameObject proxyFab1;

	// Token: 0x0400006B RID: 107
	public GameObject proxyFab2;

	// Token: 0x0400006C RID: 108
	public bool isShowStaticCollision = false;
}
