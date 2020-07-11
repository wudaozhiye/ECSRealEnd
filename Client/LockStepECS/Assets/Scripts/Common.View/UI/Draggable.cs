using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Lockstep.Game.UI
{
	// Token: 0x0200002B RID: 43
	public class Draggable : MonoBehaviour, IDragHandler, IEventSystemHandler, IBeginDragHandler, IEndDragHandler
	{
		// Token: 0x060000FF RID: 255 RVA: 0x00006CDC File Offset: 0x00004EDC
		public void OnBeginDrag(PointerEventData eventData)
		{
			this._offsetX = base.transform.position.x - Input.mousePosition.x;
			this._offsetY = base.transform.position.y - Input.mousePosition.y;
			bool flag = this._group != null;
			if (flag)
			{
				this._group.alpha = this.DraggedOpacity;
			}
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00006D4E File Offset: 0x00004F4E
		public void OnDrag(PointerEventData eventData)
		{
			base.transform.position = new Vector3(this._offsetX + Input.mousePosition.x, this._offsetY + Input.mousePosition.y);
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00006D84 File Offset: 0x00004F84
		public void OnEndDrag(PointerEventData eventData)
		{
			bool flag = this._group != null;
			if (flag)
			{
				this._group.alpha = 1f;
			}
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00006DB3 File Offset: 0x00004FB3
		private void Awake()
		{
			this._group = base.GetComponent<CanvasGroup>();
		}

		// Token: 0x040000CF RID: 207
		private CanvasGroup _group;

		// Token: 0x040000D0 RID: 208
		private float _offsetY;

		// Token: 0x040000D1 RID: 209
		private float _offsetX;

		// Token: 0x040000D2 RID: 210
		public bool ChangeOpacity = true;

		// Token: 0x040000D3 RID: 211
		public float DraggedOpacity = 0.7f;
	}
}
