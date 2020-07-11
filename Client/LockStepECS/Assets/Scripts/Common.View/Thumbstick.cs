using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x0200000A RID: 10
public class Thumbstick : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IDragHandler, IPointerUpHandler
{
	// Token: 0x06000035 RID: 53 RVA: 0x0000305C File Offset: 0x0000125C
	private void Awake()
	{
		this.direction = Vector2.zero;
		this.pointerID = -999;
		this.center = base.transform.position;
		RectTransform component = base.GetComponent<RectTransform>();
		this.minX = this.center.x - component.rect.width / 2f;
		this.maxX = this.center.x + component.rect.width / 2f;
		this.minY = this.center.y - component.rect.height / 2f;
		this.maxY = this.center.y + component.rect.height / 2f;
	}

	// Token: 0x06000036 RID: 54 RVA: 0x00003138 File Offset: 0x00001338
	private void Update()
	{
		bool flag = this.pointerID != -999 || !this.thumbImageMoving;
		if (!flag)
		{
			this.thumbImage.position = Vector3.Lerp(this.thumbImage.position, this.center, Time.deltaTime * this.smoothing);
			bool flag2 = Vector3.Distance(this.center, this.thumbImage.position) < 0.1f;
			if (flag2)
			{
				this.thumbImage.position = this.center;
				this.thumbImageMoving = false;
			}
		}
	}

	// Token: 0x06000037 RID: 55 RVA: 0x000031E0 File Offset: 0x000013E0
	public void OnPointerDown(PointerEventData data)
	{
		bool flag = this.pointerID != -999;
		if (!flag)
		{
			this.pointerID = data.pointerId;
			this.CalculateInput(data);
		}
	}

	// Token: 0x06000038 RID: 56 RVA: 0x00003218 File Offset: 0x00001418
	public void OnDrag(PointerEventData data)
	{
		bool flag = data.pointerId != this.pointerID;
		if (!flag)
		{
			this.CalculateInput(data);
		}
	}

	// Token: 0x06000039 RID: 57 RVA: 0x00003248 File Offset: 0x00001448
	public void OnPointerUp(PointerEventData data)
	{
		bool flag = data.pointerId != this.pointerID;
		if (!flag)
		{
			this.direction = Vector3.zero;
			this.pointerID = -999;
		}
	}

	// Token: 0x0600003A RID: 58 RVA: 0x00003288 File Offset: 0x00001488
	public Vector2 GetDirection()
	{
		this.smoothDirection = Vector2.MoveTowards(this.smoothDirection, this.direction, this.smoothing);
		return this.smoothDirection;
	}

	// Token: 0x0600003B RID: 59 RVA: 0x000032C0 File Offset: 0x000014C0
	private void CalculateInput(PointerEventData data)
	{
		Vector2 data2 = this.ClampDataAndMoveImage(data.position);
		data2 = this.NormalizeToRange(data2, -1f, 1f);
		this.direction = this.ApplyAxialDeadZone(data2);
	}

	// Token: 0x0600003C RID: 60 RVA: 0x000032FC File Offset: 0x000014FC
	private Vector2 ClampDataAndMoveImage(Vector2 data)
	{
		data.x = Mathf.Clamp(data.x, this.minX, this.maxX);
		data.y = Mathf.Clamp(data.y, this.minY, this.maxY);
		this.thumbImage.position = data;
		this.thumbImageMoving = true;
		return data;
	}

	// Token: 0x0600003D RID: 61 RVA: 0x00003364 File Offset: 0x00001564
	private Vector2 NormalizeToRange(Vector2 data, float newMin, float newMax)
	{
		data.x = (data.x - this.minX) / (this.maxX - this.minX) * (newMax - newMin) + newMin;
		data.y = (data.y - this.minY) / (this.maxY - this.minY) * (newMax - newMin) + newMin;
		return data;
	}

	// Token: 0x0600003E RID: 62 RVA: 0x000033C8 File Offset: 0x000015C8
	private Vector2 ApplyAxialDeadZone(Vector2 data)
	{
		float num = Mathf.Abs(data.x);
		float num2 = Mathf.Abs(data.y);
		bool flag = num < this.deadZone;
		if (flag)
		{
			data.x = 0f;
		}
		else
		{
			data.x *= (num - this.deadZone) / (1f - this.deadZone);
		}
		bool flag2 = num2 < this.deadZone;
		if (flag2)
		{
			data.y = 0f;
		}
		else
		{
			data.y *= (num2 - this.deadZone) / (1f - this.deadZone);
		}
		return data;
	}

	// Token: 0x0600003F RID: 63 RVA: 0x00003474 File Offset: 0x00001674
	private Vector2 ApplyRadialDeadZone(Vector2 data)
	{
		float magnitude = data.magnitude;
		bool flag = magnitude < this.deadZone;
		if (flag)
		{
			data = Vector2.zero;
		}
		else
		{
			data = data.normalized * ((magnitude - this.deadZone) / (1f - this.deadZone));
		}
		return data;
	}

	// Token: 0x06000040 RID: 64 RVA: 0x000034C8 File Offset: 0x000016C8
	private void MoveThumbImage(Vector2 position)
	{
		position.x += this.center.x;
		position.y += this.center.y;
		this.thumbImage.position = position;
	}

	// Token: 0x04000053 RID: 83
	public float smoothing = 5f;

	// Token: 0x04000054 RID: 84
	public RectTransform thumbImage;

	// Token: 0x04000055 RID: 85
	public float deadZone = 0.25f;

	// Token: 0x04000056 RID: 86
	private int pointerID;

	// Token: 0x04000057 RID: 87
	private Vector2 center;

	// Token: 0x04000058 RID: 88
	private Vector2 direction;

	// Token: 0x04000059 RID: 89
	private Vector2 smoothDirection;

	// Token: 0x0400005A RID: 90
	private float minX;

	// Token: 0x0400005B RID: 91
	private float minY;

	// Token: 0x0400005C RID: 92
	private float maxX;

	// Token: 0x0400005D RID: 93
	private float maxY;

	// Token: 0x0400005E RID: 94
	private bool thumbImageMoving;
}
