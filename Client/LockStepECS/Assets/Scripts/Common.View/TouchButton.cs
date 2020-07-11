using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x0200000B RID: 11
public class TouchButton : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
{
	// Token: 0x06000042 RID: 66 RVA: 0x00003533 File Offset: 0x00001733
	private void Awake()
	{
		this.pointerID = -999;
	}

	// Token: 0x06000043 RID: 67 RVA: 0x00003544 File Offset: 0x00001744
	public void OnPointerDown(PointerEventData data)
	{
		bool flag = this.pointerID != -999;
		if (!flag)
		{
			this.pointerID = data.pointerId;
			this.buttonHeld = true;
			this.buttonPressed = true;
		}
	}

	// Token: 0x06000044 RID: 68 RVA: 0x00003584 File Offset: 0x00001784
	public void OnPointerUp(PointerEventData data)
	{
		bool flag = data.pointerId != this.pointerID;
		if (!flag)
		{
			this.pointerID = -999;
			this.buttonHeld = false;
			this.buttonPressed = false;
		}
	}

	// Token: 0x06000045 RID: 69 RVA: 0x000035C4 File Offset: 0x000017C4
	public bool GetButtonDown()
	{
		bool flag = this.buttonPressed;
		bool result;
		if (flag)
		{
			this.buttonPressed = false;
			result = true;
		}
		else
		{
			result = false;
		}
		return result;
	}

	// Token: 0x06000046 RID: 70 RVA: 0x000035F0 File Offset: 0x000017F0
	public bool GetButton()
	{
		return this.buttonHeld;
	}

	// Token: 0x0400005F RID: 95
	private int pointerID;

	// Token: 0x04000060 RID: 96
	private bool buttonHeld;

	// Token: 0x04000061 RID: 97
	private bool buttonPressed;
}
