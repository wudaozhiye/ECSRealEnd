using System;
using System.Collections.Generic;
using Lockstep.Game;
using Lockstep.Math;
using UnityEngine;

// Token: 0x02000012 RID: 18
public class SpriteEffect : RollbackEffect
{
	// Token: 0x06000064 RID: 100 RVA: 0x000049CB File Offset: 0x00002BCB
	public override void DoStart(int curTick)
	{
		base.DoStart(curTick);
		this.render = base.GetComponent<SpriteRenderer>();
	}

	// Token: 0x06000065 RID: 101 RVA: 0x000049E4 File Offset: 0x00002BE4
	public override void DoUpdate(int tick)
	{
		LFloat a = (tick - base.createTick) * LFloat.one / 33;
		int index = (a * this.sprites.Count / this.interval).Floor() % this.sprites.Count;
		this.render.sprite = this.sprites[index];
	}

	// Token: 0x0400008D RID: 141
	public List<Sprite> sprites = new List<Sprite>();

	// Token: 0x0400008E RID: 142
	private SpriteRenderer render;

	// Token: 0x0400008F RID: 143
	public LFloat interval = new LFloat(1);
}
