using System;
using System.Collections.Generic;
using Lockstep.Logging;
using Lockstep.Math;
using Lockstep.Serialization;
using Lockstep.Util;
using NetMsg.Common;

namespace Lockstep.Game
{
	public class RoomMsgManager : IRoomMsgManager
{
	private delegate void DealNetMsg(BaseMsg data);

	private delegate BaseMsg ParseNetMsg(Deserializer reader);

	public EGameState CurGameState = EGameState.Idle;

	private NetClient _netUdp;

	private NetClient _netTcp;

	private float _curLoadProgress;

	private float _framePursueRate;

	private float _nextSendLoadProgressTimer;

	private IRoomMsgHandler _handler;

	protected string _gameHash;

	protected int _curMapId;

	protected byte _localId;

	protected int _roomId;

	protected IPEndInfo _gameUdpEnd;

	protected IPEndInfo _gameTcpEnd;

	protected MessageHello helloBody;

	protected bool HasConnGameTcp;

	protected bool HasConnGameUdp;

	protected bool HasRecvGameDta;

	protected bool HasFinishedLoadLevel;

	public const float ProgressSendInterval = 0.3f;

	private short curLevel;

	private byte _maxMsgId = byte.MaxValue;

	private DealNetMsg[] _allMsgDealFuncs;

	private ParseNetMsg[] _allMsgParsers;

	public float FramePursueRate
	{
		get
		{
			return _framePursueRate;
		}
		set
		{
			_framePursueRate = System.Math.Max(System.Math.Min(1f, value), 0f);
		}
	}

	public byte CurProgress
	{
		get
		{
			if (_curLoadProgress > 1f)
			{
				_curLoadProgress = 1f;
			}
			if (_curLoadProgress < 0f)
			{
				_curLoadProgress = 0f;
			}
			if (!IsReconnecting)
			{
				float num = _curLoadProgress * 70f + (float)(HasRecvGameDta ? 10 : 0) + (float)(HasConnGameUdp ? 10 : 0) + (float)(HasConnGameTcp ? 10 : 0);
				return (byte)num;
			}
			float num2 = (float)((HasRecvGameDta ? 10 : 0) + (HasConnGameUdp ? 10 : 0) + (HasConnGameTcp ? 10 : 0)) + _curLoadProgress * 20f + FramePursueRate * 50f;
			return (byte)num2;
		}
	}

	public bool IsReconnecting
	{
		get;
		set;
	}

	public Msg_G2C_GameStartInfo GameStartInfo
	{
		get;
		private set;
	}

	public void Init(IRoomMsgHandler msgHandler)
	{
		_maxMsgId = (byte)System.Math.Min(48, 255);
		_allMsgDealFuncs = new DealNetMsg[_maxMsgId];
		_allMsgParsers = new ParseNetMsg[_maxMsgId];
		RegisterMsgHandlers();
		_handler = msgHandler;
		_netUdp = (_netTcp = new NetClient());
		_netTcp.DoStart();
		_netTcp.NetMsgHandler = OnNetMsg;
	}

	private void OnNetMsg(ushort opcode, object msg)
	{
		switch (opcode)
		{
		case 13:
			G2C_PlayerPing(msg);
			break;
		case 39:
			G2C_Hello(msg);
			break;
		case 11:
			G2C_FrameData(msg);
			break;
		case 8:
			G2C_RepMissFrame(msg);
			break;
		case 47:
			G2C_GameEvent(msg);
			break;
		case 40:
			G2C_GameStartInfo(msg);
			break;
		case 42:
			G2C_LoadingProgress(msg);
			break;
		case 43:
			G2C_AllFinishedLoaded(msg);
			break;
		}
	}

	public void DoUpdate(LFloat deltaTime)
	{
		if (CurGameState == EGameState.Loading && _nextSendLoadProgressTimer < LTime.realtimeSinceStartup)
		{
			SendLoadingProgress(CurProgress);
		}
	}

	public void DoDestroy()
	{
		Debug.Log("DoDestroy", Array.Empty<object>());
		_netTcp.SendMessage(EMsgSC.C2L_LeaveRoom, new Msg_C2L_LeaveRoom().ToBytes());
		_netUdp?.DoDestroy();
		_netTcp?.DoDestroy();
		_netTcp = null;
		_netUdp = null;
	}

	private void ResetStatus()
	{
		HasConnGameTcp = false;
		HasConnGameUdp = false;
		HasRecvGameDta = false;
		HasFinishedLoadLevel = false;
	}

	public void OnLevelLoadProgress(float progress)
	{
		_curLoadProgress = progress;
		if (CurProgress >= 100)
		{
			CurGameState = EGameState.PartLoaded;
			_nextSendLoadProgressTimer = LTime.realtimeSinceStartup + 0.3f;
			SendLoadingProgress(CurProgress);
		}
	}

	public void ConnectToGameServer(Msg_C2G_Hello helloBody, IPEndInfo _gameTcpEnd, bool isReconnect)
	{
		IsReconnecting = isReconnect;
		ResetStatus();
		this.helloBody = helloBody.Hello;
		ConnectUdp();
		SendTcp(EMsgSC.C2L_JoinRoom, new Msg_C2L_JoinRoom
		{
			RoomId = 0
		});
	}

	private void ConnectUdp()
	{
		_handler.OnUdpHello(_curMapId, _localId);
	}

	protected void G2C_PlayerPing(object reader)
	{
		Msg_G2C_PlayerPing param = reader as Msg_G2C_PlayerPing;
		EventHelper.Trigger(EEvent.OnPlayerPing, param);
	}

	protected void G2C_Hello(object reader)
	{
		Msg_G2C_Hello param = reader as Msg_G2C_Hello;
		EventHelper.Trigger(EEvent.OnServerHello, param);
	}

	protected void G2C_GameEvent(object reader)
	{
		Msg_G2C_GameEvent msg_G2C_GameEvent = reader as Msg_G2C_GameEvent;
		_handler.OnGameEvent(msg_G2C_GameEvent.Data);
	}

	protected void G2C_GameStartInfo(object reader)
	{
		Msg_G2C_GameStartInfo msg_G2C_GameStartInfo = reader as Msg_G2C_GameStartInfo;
		HasRecvGameDta = true;
		GameStartInfo = msg_G2C_GameStartInfo;
		_handler.OnGameStartInfo(msg_G2C_GameStartInfo);
		HasConnGameTcp = true;
		HasConnGameUdp = true;
		CurGameState = EGameState.Loading;
		_curLoadProgress = 1f;
		EventHelper.Trigger(EEvent.OnGameCreate, msg_G2C_GameStartInfo);
		Debug.Log("G2C_GameStartInfo", Array.Empty<object>());
	}

	protected void G2C_LoadingProgress(object reader)
	{
		Msg_G2C_LoadingProgress msg_G2C_LoadingProgress = reader as Msg_G2C_LoadingProgress;
		_handler.OnLoadingProgress(msg_G2C_LoadingProgress.Progress);
	}

	protected void G2C_AllFinishedLoaded(object reader)
	{
		Msg_G2C_AllFinishedLoaded msg_G2C_AllFinishedLoaded = reader as Msg_G2C_AllFinishedLoaded;
		curLevel = msg_G2C_AllFinishedLoaded.Level;
		_handler.OnAllFinishedLoaded(msg_G2C_AllFinishedLoaded.Level);
	}

	public void SendGameEvent(byte[] msg)
	{
		SendTcp(EMsgSC.C2G_GameEvent, new Msg_C2G_GameEvent
		{
			Data = msg
		});
	}

	public void SendLoadingProgress(byte progress)
	{
		_nextSendLoadProgressTimer = LTime.realtimeSinceStartup + 0.3f;
		if (!IsReconnecting)
		{
			SendTcp(EMsgSC.C2G_LoadingProgress, new Msg_C2G_LoadingProgress
			{
				Progress = progress
			});
		}
	}

	private void RegisterMsgHandlers()
	{
		RegisterNetMsgHandler(EMsgSC.G2C_RepMissFrame, G2C_RepMissFrame, this.ParseData<Msg_RepMissFrame>);
		RegisterNetMsgHandler(EMsgSC.G2C_FrameData, G2C_FrameData, this.ParseData<Msg_ServerFrames>);
	}

	private void RegisterNetMsgHandler(EMsgSC type, DealNetMsg func, ParseNetMsg parseFunc)
	{
		_allMsgDealFuncs[(uint)type] = func;
		_allMsgParsers[(uint)type] = parseFunc;
	}

	private T ParseData<T>(Deserializer reader) where T : BaseMsg, new()
	{
		return reader.Parse<T>();
	}

	public void SendPing(byte localId, long timestamp)
	{
		SendUdp(EMsgSC.C2G_PlayerPing, new Msg_C2G_PlayerPing
		{
			LocalId = localId,
			SendTimestamp = timestamp
		});
	}

	public void SendInput(Msg_PlayerInput msg)
	{
		SendUdp(EMsgSC.C2G_PlayerInput, msg);
	}

	public void SendMissFrameReq(int missFrameTick)
	{
		SendUdp(EMsgSC.C2G_ReqMissFrame, new Msg_ReqMissFrame
		{
			StartTick = missFrameTick
		});
	}

	public void SendMissFrameRepAck(int missFrameTick)
	{
		SendUdp(EMsgSC.C2G_RepMissFrameAck, new Msg_RepMissFrameAck
		{
			MissFrameTick = missFrameTick
		});
	}

	public void SendHashCodes(int firstHashTick, List<int> allHashCodes, int startIdx, int count)
	{
		Msg_HashCode msg_HashCode = new Msg_HashCode();
		msg_HashCode.StartTick = firstHashTick;
		msg_HashCode.HashCodes = new int[count];
		for (int i = startIdx; i < count; i++)
		{
			msg_HashCode.HashCodes[i] = allHashCodes[i];
		}
		SendUdp(EMsgSC.C2G_HashCode, msg_HashCode);
	}

	public void SendUdp(EMsgSC msgId, ISerializable body)
	{
		Serializer serializer = new Serializer();
		body.Serialize(serializer);
		_netUdp?.SendMessage(msgId, serializer.CopyData());
	}

	public void SendTcp(EMsgSC msgId, BaseMsg body)
	{
		Serializer serializer = new Serializer();
		body.Serialize(serializer);
		_netTcp?.SendMessage(msgId, serializer.CopyData());
	}

	protected void G2C_UdpMessage(IIncommingMessage reader)
	{
		byte[] rawBytes = reader.GetRawBytes();
		Deserializer reader2 = new Deserializer(Compressor.Decompress(rawBytes));
		OnRecvMsg(reader2);
	}

	protected void OnRecvMsg(Deserializer reader)
	{
		short num = reader.ReadInt16();
		if (num >= _maxMsgId)
		{
			Debug.LogError($" send a Error msgType out of range {num}", Array.Empty<object>());
		}
		else
		{
			try
			{
				DealNetMsg dealNetMsg = _allMsgDealFuncs[num];
				ParseNetMsg parseNetMsg = _allMsgParsers[num];
				if (dealNetMsg != null && parseNetMsg != null)
				{
					dealNetMsg(parseNetMsg(reader));
				}
				else
				{
					Debug.LogError($" ErrorMsg type :no msg handler or parser {num}", Array.Empty<object>());
				}
			}
			catch (Exception arg)
			{
				Debug.LogError($" Deal Msg Error :{(EMsgSC)num}  " + arg, Array.Empty<object>());
			}
		}
	}

	protected void G2C_FrameData(object reader)
	{
		Msg_ServerFrames msg = reader as Msg_ServerFrames;
		_handler.OnServerFrames(msg);
	}

	protected void G2C_RepMissFrame(object reader)
	{
		Msg_RepMissFrame msg = reader as Msg_RepMissFrame;
		_handler.OnMissFrames(msg);
	}
}
}
