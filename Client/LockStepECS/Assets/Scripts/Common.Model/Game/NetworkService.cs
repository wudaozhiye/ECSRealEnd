using System;
using System.Collections.Generic;
using Lockstep.Logging;
using Lockstep.Math;
using NetMsg.Common;

namespace Lockstep.Game
{
	// Token: 0x02000053 RID: 83
	public class NetworkService : BaseService, INetworkService, IService
	{
		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060001BB RID: 443 RVA: 0x00006075 File Offset: 0x00004275
		// (set) Token: 0x060001BC RID: 444 RVA: 0x0000607C File Offset: 0x0000427C
		public static NetworkService Instance { get; private set; }

		// Token: 0x060001BD RID: 445 RVA: 0x00006084 File Offset: 0x00004284
		public NetworkService()
		{
			NetworkService.Instance = this;
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060001BE RID: 446 RVA: 0x000060E5 File Offset: 0x000042E5
		public int Ping
		{
			get
			{
				return 50;
			}
		}

		// Token: 0x060001BF RID: 447 RVA: 0x000060EC File Offset: 0x000042EC
		public override void DoAwake(IServiceContainer services)
		{
			this._noNetwork = (this._globalStateService.IsVideoMode || this._globalStateService.IsClientMode);
			bool noNetwork = this._noNetwork;
			if (!noNetwork)
			{
				this._roomMsgMgr = new RoomMsgManager();
				this._msgHandler = new NetworkMsgHandler();
				this._roomMsgMgr.Init(this._msgHandler);
			}
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x00006150 File Offset: 0x00004350
		public override void DoStart()
		{
			bool noNetwork = this._noNetwork;
			if (!noNetwork)
			{
				this._roomMsgMgr.ConnectToGameServer(new Msg_C2G_Hello(), null, false);
			}
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x00006180 File Offset: 0x00004380
		public void DoUpdate(LFloat deltaTime)
		{
			bool noNetwork = this._noNetwork;
			if (!noNetwork)
			{
				RoomMsgManager roomMsgMgr = this._roomMsgMgr;
				if (roomMsgMgr != null)
				{
					roomMsgMgr.DoUpdate(deltaTime);
				}
			}
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x000061B0 File Offset: 0x000043B0
		public override void DoDestroy()
		{
			bool noNetwork = this._noNetwork;
			if (!noNetwork)
			{
				RoomMsgManager roomMsgMgr = this._roomMsgMgr;
				if (roomMsgMgr != null)
				{
					roomMsgMgr.DoDestroy();
				}
				this._roomMsgMgr = null;
			}
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x000061E4 File Offset: 0x000043E4
		public void OnEvent_TryLogin(object param)
		{
			bool noNetwork = this._noNetwork;
			if (!noNetwork)
			{
				Debug.Log("OnEvent_TryLogin" + param.ToJson(), Array.Empty<object>());
			}
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x0000621C File Offset: 0x0000441C
		private void OnEvent_OnConnectToGameServer(object param)
		{
			bool noNetwork = this._noNetwork;
			if (!noNetwork)
			{
				bool isReconnecting = (bool)param != null;
				this._globalStateService.IsReconnecting = isReconnecting;
			}
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x0000624C File Offset: 0x0000444C
		private void OnEvent_LevelLoadProgress(object param)
		{
			bool noNetwork = this._noNetwork;
			if (!noNetwork)
			{
				this._roomMsgMgr.OnLevelLoadProgress((float)param);
				this.CheckLoadingProgress();
			}
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x00006280 File Offset: 0x00004480
		private void OnEvent_PursueFrameProcess(object param)
		{
			bool noNetwork = this._noNetwork;
			if (!noNetwork)
			{
				this._roomMsgMgr.FramePursueRate = (float)param;
				this.CheckLoadingProgress();
			}
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x000062B4 File Offset: 0x000044B4
		private void OnEvent_PursueFrameDone(object param)
		{
			bool noNetwork = this._noNetwork;
			if (!noNetwork)
			{
				this._roomMsgMgr.FramePursueRate = 1f;
				this.CheckLoadingProgress();
			}
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x000062E8 File Offset: 0x000044E8
		private void CheckLoadingProgress()
		{
			if (_roomMsgMgr.IsReconnecting)
			{
				float num = (float)(int)_roomMsgMgr.CurProgress / 100f;
				EventHelper.Trigger(EEvent.ReconnectLoadProgress, num);
				if (_roomMsgMgr.CurProgress >= 100)
				{
					_globalStateService.IsReconnecting = false;
					_roomMsgMgr.IsReconnecting = false;
					EventHelper.Trigger(EEvent.ReconnectLoadDone, null);
				}
			}
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x00003A82 File Offset: 0x00001C82
		public void CreateRoom(int mapId, string name, int size)
		{
		}

		// Token: 0x060001CA RID: 458 RVA: 0x00003A82 File Offset: 0x00001C82
		public void StartGame()
		{
		}

		// Token: 0x060001CB RID: 459 RVA: 0x00003A82 File Offset: 0x00001C82
		public void ReadyInRoom(bool isReady)
		{
		}

		// Token: 0x060001CC RID: 460 RVA: 0x00003A82 File Offset: 0x00001C82
		public void JoinRoom(int roomId)
		{
		}

		// Token: 0x060001CD RID: 461 RVA: 0x00003A82 File Offset: 0x00001C82
		public void ReqRoomList(int startIdx)
		{
		}

		// Token: 0x060001CE RID: 462 RVA: 0x00003A82 File Offset: 0x00001C82
		public void LeaveRoom()
		{
		}

		// Token: 0x060001CF RID: 463 RVA: 0x00003A82 File Offset: 0x00001C82
		public void SendChatInfo(RoomChatInfo chatInfo)
		{
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x00006364 File Offset: 0x00004564
		public void SendGameEvent(byte[] data)
		{
			bool noNetwork = this._noNetwork;
			if (!noNetwork)
			{
				this._roomMsgMgr.SendGameEvent(data);
			}
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x0000638C File Offset: 0x0000458C
		public void SendPing(byte localId, long timestamp)
		{
			bool noNetwork = this._noNetwork;
			if (!noNetwork)
			{
				this._roomMsgMgr.SendPing(localId, timestamp);
			}
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x000063B4 File Offset: 0x000045B4
		public void SendInput(Msg_PlayerInput msg)
		{
			bool noNetwork = this._noNetwork;
			if (!noNetwork)
			{
				this._roomMsgMgr.SendInput(msg);
			}
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x000063DC File Offset: 0x000045DC
		public void SendMissFrameReq(int missFrameTick)
		{
			bool noNetwork = this._noNetwork;
			if (!noNetwork)
			{
				this._roomMsgMgr.SendMissFrameReq(missFrameTick);
			}
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x00006404 File Offset: 0x00004604
		public void SendMissFrameRepAck(int missFrameTick)
		{
			bool noNetwork = this._noNetwork;
			if (!noNetwork)
			{
				this._roomMsgMgr.SendMissFrameRepAck(missFrameTick);
			}
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x0000642C File Offset: 0x0000462C
		public void SendHashCodes(int firstHashTick, List<int> allHashCodes, int startIdx, int count)
		{
			bool noNetwork = this._noNetwork;
			if (!noNetwork)
			{
				this._roomMsgMgr.SendHashCodes(firstHashTick, allHashCodes, startIdx, count);
			}
		}

		// Token: 0x040000B1 RID: 177
		public string ServerIp = "127.0.0.1";

		// Token: 0x040000B2 RID: 178
		public int ServerPort = 10083;

		// Token: 0x040000B3 RID: 179
		private long _playerID;

		// Token: 0x040000B4 RID: 180
		private int _roomId;

		// Token: 0x040000B5 RID: 181
		public bool IsConnected = true;

		// Token: 0x040000B6 RID: 182
		private bool _noNetwork;

		// Token: 0x040000B7 RID: 183
		private bool _isReconnected = false;

		// Token: 0x040000B8 RID: 184
		private RoomMsgManager _roomMsgMgr;

		// Token: 0x040000B9 RID: 185
		public NetworkMsgHandler _msgHandler = new NetworkMsgHandler();

		// Token: 0x040000BA RID: 186
		public List<RoomPlayerInfo> PlayerInfos = new List<RoomPlayerInfo>();

		// Token: 0x040000BB RID: 187
		public List<RoomInfo> RoomInfos = new List<RoomInfo>();
	}
}
