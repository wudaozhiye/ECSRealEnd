using System;
using Lockstep.Logging;
using Lockstep.Math;
using Lockstep.Util;
using NetMsg.Common;

namespace Lockstep.Game
{
	public class SimulatorService : BaseGameService, ISimulatorService, ISimulationEventHandler, IDebugService
	{
		public static SimulatorService Instance { get; private set; }
		
		public FuncCreateWorld FuncCreateWorld { get; set; }
		public int __debugRockbackToTick;
		
		public const long MinMissFrameReqTickDiff = 10L;
		
		public const long MaxSimulationMsPerFrame = 20L;
		
		public const int MaxPredictFrameCount = 30;
		
		private World _world;
		
		private IFrameBuffer _cmdBuffer;
		
		private HashHelper _hashHelper;
		
		private DumpHelper _dumpHelper;
		
		private bool IsPredictMode = false;
		
		private Msg_G2C_GameStartInfo _gameStartInfo;
		
		private byte[] _allActors;
		
		public int FramePredictCount = 0;
		
		public long _gameStartTimestampMs = -1L;
		
		private int _tickSinceGameStart;
		
		public int PreSendInputCount = 2;
		
		public int inputTick = 0;
		
		private Msg_RepMissFrame _videoFrames;
		
		private bool _isInitVideo = false;
		
		private int _tickOnLastJumpTo;
		
		private long _timestampOnLastJumpToMs;
		
		private bool _isDebugRollback = false;
		
		private IManagerContainer _mgrContainer;
		
		private new IServiceContainer _serviceContainer;
		
		public int snapshotFrameInterval = 1;
		
		private bool _hasRecvInputMsg;
		public int PingVal
		{
			get
			{
				IFrameBuffer cmdBuffer = this._cmdBuffer;
				return (cmdBuffer != null) ? cmdBuffer.PingVal : 0;
			}
		}
		
		public int DelayVal
		{
			get
			{
				IFrameBuffer cmdBuffer = this._cmdBuffer;
				return (cmdBuffer != null) ? cmdBuffer.DelayVal : 0;
			}
		}
		
		public World World
		{
			get
			{
				return this._world;
			}
		}
		
		public byte LocalActorId { get; private set; }
		
		private int _actorCount
		{
			get
			{
				return this._allActors.Length;
			}
		}
		
		public bool IsRunning { get; set; }
		
		public int TargetTick
		{
			get
			{
				return this._tickSinceGameStart + this.FramePredictCount;
			}
		}
		
		public int inputTargetTick
		{
			get
			{
				return this._tickSinceGameStart + this.PreSendInputCount;
			}
		}
		
		public SimulatorService()
		{
			SimulatorService.Instance = this;
		}
		
		public override void InitReference(IServiceContainer serviceContainer, IManagerContainer mgrContainer)
		{
			base.InitReference(serviceContainer, mgrContainer);
			this._serviceContainer = serviceContainer;
			this._mgrContainer = mgrContainer;
		}
		
		public override void DoStart()
		{
			this.snapshotFrameInterval = 1;
			bool isVideoMode = this._globalStateService.IsVideoMode;
			if (isVideoMode)
			{
				this.snapshotFrameInterval = this._globalStateService.SnapshotFrameInterval;
			}
			this._cmdBuffer = new FrameBuffer(this, this._networkService, 2000, this.snapshotFrameInterval, 30);
			object contexts = this._globalStateService.Contexts;
			object logicFeatureObj = this._ecsFactoryService.CreateSystems(contexts, this._serviceContainer);
			this._world = (this.FuncCreateWorld(this._serviceContainer, contexts, logicFeatureObj) as World);
			this._hashHelper = new HashHelper(this._serviceContainer, this._world, this._networkService, this._cmdBuffer);
			this._dumpHelper = new DumpHelper(this._serviceContainer, this._world, this._hashHelper);
		}
		
		public override void DoDestroy()
		{
			this.IsRunning = false;
			this._dumpHelper.DumpAll();
		}
		
		public void SimulationAwake(int targetFps, byte localActorId, byte actorCount, bool isNeedRender = true)
		{
			FrameBuffer.__debugMainActorID = localActorId;
			this.LocalActorId = localActorId;
			byte[] array = new byte[(int)actorCount];
			for (byte b = 0; b < actorCount; b += 1)
			{
				array[(int)b] = b;
			}
			Debug.Log("SimulateAwake " + this.LocalActorId, Array.Empty<object>());
			this._allActors = array;
			this._globalStateService.LocalActorId = this.LocalActorId;
			this._globalStateService.ActorCount = actorCount;
			this._world.SimulationAwake(this._serviceContainer, this._mgrContainer);
			EventHelper.Trigger(EEvent.SimulationAwake, null);
		}
		
		public void SimulationStart()
		{
			bool isRunning = this.IsRunning;
			if (isRunning)
			{
				Debug.Log("Already started!", Array.Empty<object>());
			}
			else
			{
				this.IsRunning = true;
				bool isClientMode = this._globalStateService.IsClientMode;
				if (isClientMode)
				{
					this._gameStartTimestampMs = LTime.realtimeSinceStartupMS;
				}
				this._world.SimulationStart(this._gameStartInfo, (int)this.LocalActorId);
				Debug.Log("SimulateStart", Array.Empty<object>());
				EventHelper.Trigger(EEvent.SimulationStart, null);
				while (this.inputTick < this.PreSendInputCount)
				{
					int num = this.inputTick;
					this.inputTick = num + 1;
					this.SendInputs(num);
				}
			}
		}
		
		public void Trace(string msg, bool isNewLine = false, bool isNeedLogTrace = false)
		{
		}
		
		public void JumpTo(int tick)
		{
			bool flag = tick + 1 == this._world.Tick || tick == this._world.Tick;
			if (!flag)
			{
				tick = LMath.Min(tick, this._videoFrames.frames.Length - 1);
				float num = (float)LTime.realtimeSinceStartupMS + 0.05f;
				bool flag2 = !this._isInitVideo;
				if (flag2)
				{
					this._globalStateService.IsVideoLoading = true;
					while (this._world.Tick < this._videoFrames.frames.Length)
					{
						ServerFrame frame = this._videoFrames.frames[this._world.Tick];
						this.Simulate(frame, true);
						bool flag3 = (float)LTime.realtimeSinceStartupMS > num;
						if (flag3)
						{
							EventHelper.Trigger(EEvent.VideoLoadProgress, (float)this._world.Tick * 1f / (float)this._videoFrames.frames.Length);
							return;
						}
					}
					this._globalStateService.IsVideoLoading = false;
					EventHelper.Trigger(EEvent.VideoLoadDone, null);
					this._isInitVideo = true;
				}
				bool flag4 = this._world.Tick > tick;
				if (flag4)
				{
					this.RollbackTo(tick, this._videoFrames.frames.Length, false);
				}
				while (this._world.Tick <= tick)
				{
					ServerFrame frame2 = this._videoFrames.frames[this._world.Tick];
					this.Simulate(frame2, false);
				}
				this._viewService.RebindAllEntities();
				this._timestampOnLastJumpToMs = LTime.realtimeSinceStartupMS;
				this._tickOnLastJumpTo = tick;
			}
		}

		public void RunVideo()
		{
			bool flag = this._tickOnLastJumpTo == this._world.Tick;
			if (flag)
			{
				this._timestampOnLastJumpToMs = LTime.realtimeSinceStartupMS;
				this._tickOnLastJumpTo = this._world.Tick;
			}
			float num = (LTime.timeSinceLevelLoad - (float)this._timestampOnLastJumpToMs) * 1000f;
			double num2 = System.Math.Ceiling((double)(num / 30f)) + (double)this._tickOnLastJumpTo;
			while ((double)this._world.Tick <= num2)
			{
				bool flag2 = this._world.Tick < this._videoFrames.frames.Length;
				if (!flag2)
				{
					break;
				}
				ServerFrame frame = this._videoFrames.frames[this._world.Tick];
				this.Simulate(frame, false);
			}
		}
		
		private long GetTimeSinceStartMS()
		{
			return LTime.realtimeSinceStartupMS;
		}
		
		public void DoUpdate(float deltaTime)
		{
			bool flag = !this.IsRunning;
			if (!flag)
			{
				bool hasRecvInputMsg = this._hasRecvInputMsg;
				if (hasRecvInputMsg)
				{
					bool flag2 = this._gameStartTimestampMs == -1L;
					if (flag2)
					{
						this._gameStartTimestampMs = this.GetTimeSinceStartMS();
					}
				}
				bool flag3 = this._gameStartTimestampMs <= 0L;
				if (!flag3)
				{
					this._tickSinceGameStart = (int)((this.GetTimeSinceStartMS() - this._gameStartTimestampMs) / 30L);
					bool isVideoMode = this._globalStateService.IsVideoMode;
					if (!isVideoMode)
					{
						bool flag4 = this.__debugRockbackToTick > 0;
						if (flag4)
						{
							base.GetService<IGlobalStateService>().IsPause = true;
							this.RollbackTo(this.__debugRockbackToTick, 0, false);
							this.__debugRockbackToTick = -1;
						}
						bool isPause = this._globalStateService.IsPause;
						if (!isPause)
						{
							this._cmdBuffer.DoUpdate(deltaTime);
							bool isClientMode = this._globalStateService.IsClientMode;
							if (isClientMode)
							{
								this.DoClientUpdate();
							}
							else
							{
								while (this.inputTick <= this.inputTargetTick)
								{
									int num = this.inputTick;
									this.inputTick = num + 1;
									this.SendInputs(num);
								}
								this.DoNormalUpdate();
							}
						}
					}
				}
			}
		}
		
		private void DoClientUpdate()
		{
			int num = 5;
			bool flag = this._isDebugRollback && this._world.Tick > num && this._world.Tick % num == 0;
			if (flag)
			{
				int tick = this._world.Tick;
				int num2 = LRandom.Range(1, num);
				for (int i = 0; i < num2; i++)
				{
					Msg_PlayerInput msg_PlayerInput = new Msg_PlayerInput(this._world.Tick, this.LocalActorId, this._inputService.GetDebugInputCmds());
					ServerFrame frame = new ServerFrame
					{
						Tick = tick - i,
						_Inputs = new Msg_PlayerInput[]
						{
							msg_PlayerInput
						}
					};
					this._cmdBuffer.ForcePushDebugFrame(frame);
				}
				this._debugService.Trace("RollbackTo " + (this._world.Tick - num2), false, false);
				bool flag2 = !this.RollbackTo(this._world.Tick - num2, this._world.Tick, true);
				if (flag2)
				{
					this._globalStateService.IsPause = true;
					return;
				}
				while (this._world.Tick < tick)
				{
					ServerFrame serverFrame = this._cmdBuffer.GetServerFrame(this._world.Tick);
					Debug.Assert(serverFrame != null && serverFrame.Tick == this._world.Tick, string.Format(" logic error: server Frame  must exist tick {0}", this._world.Tick));
					this._cmdBuffer.PushLocalFrame(serverFrame);
					this.Simulate(serverFrame, true);
					bool isPause = this._globalStateService.IsPause;
					if (isPause)
					{
						return;
					}
				}
			}
			while (this._world.Tick < this.TargetTick)
			{
				this.FramePredictCount = 0;
				Msg_PlayerInput msg_PlayerInput2 = new Msg_PlayerInput(this._world.Tick, this.LocalActorId, this._inputService.GetInputCmds());
				ServerFrame serverFrame2 = new ServerFrame
				{
					Tick = this._world.Tick,
					_Inputs = new Msg_PlayerInput[]
					{
						msg_PlayerInput2
					}
				};
				this._cmdBuffer.PushLocalFrame(serverFrame2);
				this._cmdBuffer.PushServerFrames(new ServerFrame[]
				{
					serverFrame2
				}, true);
				this.Simulate(this._cmdBuffer.GetFrame(this._world.Tick), true);
				bool isPause2 = this._globalStateService.IsPause;
				if (isPause2)
				{
					break;
				}
			}
		}
		
		private void DoNormalUpdate()
		{
			int maxContinueServerTick = this._cmdBuffer.MaxContinueServerTick;
			bool flag = this._world.Tick - maxContinueServerTick > 30;
			if (!flag)
			{
				int num = maxContinueServerTick - maxContinueServerTick % this.snapshotFrameInterval;
				long num2 = LTime.realtimeSinceStartupMS + 20L;
				while (this._world.Tick < this._cmdBuffer.CurTickInServer)
				{
					int tick = this._world.Tick;
					ServerFrame serverFrame = this._cmdBuffer.GetServerFrame(tick);
					bool flag2 = serverFrame == null;
					if (flag2)
					{
						this.OnPursuingFrame();
						return;
					}
					this._cmdBuffer.PushLocalFrame(serverFrame);
					this.Simulate(serverFrame, tick == num);
					bool flag3 = LTime.realtimeSinceStartupMS > num2;
					if (flag3)
					{
						this.OnPursuingFrame();
						return;
					}
				}
				bool isPursueFrame = this._globalStateService.IsPursueFrame;
				if (isPursueFrame)
				{
					this._globalStateService.IsPursueFrame = false;
					EventHelper.Trigger(EEvent.PursueFrameDone, null);
				}
				bool isPredictMode = this.IsPredictMode;
				if (isPredictMode)
				{
					bool isNeedRollback = this._cmdBuffer.IsNeedRollback;
					if (isNeedRollback)
					{
						this.RollbackTo(this._cmdBuffer.NextTickToCheck, maxContinueServerTick, true);
						this.CleanUselessSnapshot(System.Math.Min(this._cmdBuffer.NextTickToCheck - 1, this._world.Tick));
						num = System.Math.Max(num, this._world.Tick + 1);
						while (this._world.Tick <= maxContinueServerTick)
						{
							ServerFrame serverFrame2 = this._cmdBuffer.GetServerFrame(this._world.Tick);
							Debug.Assert(serverFrame2 != null && serverFrame2.Tick == this._world.Tick, string.Format(" logic error: server Frame  must exist tick {0}", this._world.Tick));
							this._cmdBuffer.PushLocalFrame(serverFrame2);
							this.Simulate(serverFrame2, this._world.Tick == num);
						}
					}
					while (this._world.Tick <= this.TargetTick)
					{
						int tick2 = this._world.Tick;
						ServerFrame serverFrame3 = this._cmdBuffer.GetServerFrame(tick2);
						bool flag4 = serverFrame3 != null;
						ServerFrame frame;
						if (flag4)
						{
							frame = serverFrame3;
						}
						else
						{
							ServerFrame localFrame = this._cmdBuffer.GetLocalFrame(tick2);
							this.FillInputWithLastFrame(localFrame);
							frame = localFrame;
						}
						this._cmdBuffer.PushLocalFrame(frame);
						this.Predict(frame, true);
					}
				}
				else
				{
					while (this._world.Tick <= this.TargetTick)
					{
						int tick3 = this._world.Tick;
						ServerFrame serverFrame4 = this._cmdBuffer.GetServerFrame(tick3);
						bool flag5 = serverFrame4 == null;
						if (flag5)
						{
							return;
						}
						ServerFrame frame2 = serverFrame4;
						this._cmdBuffer.PushLocalFrame(frame2);
						this.Simulate(frame2, true);
						bool flag6 = tick3 > 0;
						if (flag6)
						{
							this.CleanUselessSnapshot(tick3 - 1);
						}
					}
				}
				this._hashHelper.CheckAndSendHashCodes();
			}
		}
		
		private void SendInputs(int curTick)
		{
			Msg_PlayerInput msg_PlayerInput = new Msg_PlayerInput(curTick, this.LocalActorId, this._inputService.GetInputCmds());
			ServerFrame serverFrame = new ServerFrame();
			Msg_PlayerInput[] array = new Msg_PlayerInput[this._actorCount];
			array[(int)this.LocalActorId] = msg_PlayerInput;
			serverFrame.Inputs = array;
			serverFrame.Tick = curTick;
			this.FillInputWithLastFrame(serverFrame);
			this._cmdBuffer.PushLocalFrame(serverFrame);
			bool flag = curTick > this._cmdBuffer.MaxServerTickInBuffer;
			if (flag)
			{
				this._cmdBuffer.SendInput(msg_PlayerInput);
			}
		}

		private void Simulate(ServerFrame frame, bool isNeedGenSnap = true)
		{
			this.Step(frame, isNeedGenSnap);
		}
		
		private void Predict(ServerFrame frame, bool isNeedGenSnap = true)
		{
			this.Step(frame, isNeedGenSnap);
		}

		private bool RollbackTo(int tick, int maxContinueServerTick, bool isNeedClear = true)
		{
			this._world.RollbackTo(tick, maxContinueServerTick, isNeedClear);
			int hash = this._globalStateService.Hash;
			int num = this._hashHelper.CalcHash(false);
			bool flag = hash != num;
			bool result;
			if (flag)
			{
				Debug.LogError(string.Format("tick:{0} Rollback error: Hash isDiff oldHash ={1}  curHash{2}", tick, hash, num), Array.Empty<object>());
				this._dumpHelper.DumpToFile(true);
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}
		
		private void Step(ServerFrame frame, bool isNeedGenSnap = true)
		{
			this._globalStateService.SetTick(this._world.Tick);
			int hash = this._hashHelper.CalcHash(false);
			this._globalStateService.Hash = hash;
			this._timeMachineService.Backup(this._world.Tick);
			this._world.Backup(this._world.Tick);
			this.DumpFrame(hash);
			hash = this._hashHelper.CalcHash(true);
			this._hashHelper.SetHash(this._world.Tick, hash);
			this.ProcessInputQueue(frame);
			this._world.Step(isNeedGenSnap);
			this._dumpHelper.OnFrameEnd();
			int tick = this._world.Tick;
			this._cmdBuffer.SetClientTick(tick);
			bool flag = isNeedGenSnap && tick % this.snapshotFrameInterval == 0;
			if (flag)
			{
				this.CleanUselessSnapshot(System.Math.Min(this._cmdBuffer.NextTickToCheck - 1, this._world.Tick));
			}
		}
		
		private void CleanUselessSnapshot(int tick)
		{
			this._world.CleanUselessSnapshot(tick);
		}
		
		private void DumpFrame(int hash)
		{
			bool isClientMode = this._globalStateService.IsClientMode;
			if (isClientMode)
			{
				int num;
				this._dumpHelper.DumpFrame(!this._hashHelper.TryGetValue(this._world.Tick, out num));
			}
			else
			{
				this._dumpHelper.DumpFrame(true);
			}
		}
		
		private void FillInputWithLastFrame(ServerFrame frame)
		{
			int tick = frame.Tick;
			Msg_PlayerInput[] inputs = frame.Inputs;
			Msg_PlayerInput[] array;
			if (tick != 0)
			{
				ServerFrame frame2 = this._cmdBuffer.GetFrame(tick - 1);
				array = ((frame2 != null) ? frame2.Inputs : null);
			}
			else
			{
				array = null;
			}
			Msg_PlayerInput[] array2 = array;
			Msg_PlayerInput msg_PlayerInput = inputs[(int)this.LocalActorId];
			for (int i = 0; i < this._actorCount; i++)
			{
				Msg_PlayerInput[] array3 = inputs;
				int num = i;
				int tick2 = tick;
				byte actorID = this._allActors[i];
				InputCmd[] inputs2;
				if (array2 == null)
				{
					inputs2 = null;
				}
				else
				{
					Msg_PlayerInput msg_PlayerInput2 = array2[i];
					inputs2 = ((msg_PlayerInput2 != null) ? msg_PlayerInput2.Commands : null);
				}
				array3[num] = new Msg_PlayerInput(tick2, actorID, inputs2);
			}
			inputs[(int)this.LocalActorId] = msg_PlayerInput;
		}
		
		private void ProcessInputQueue(ServerFrame frame)
		{
			Msg_PlayerInput[] inputs = frame.Inputs;
			foreach (Msg_PlayerInput msg_PlayerInput in inputs)
			{
				bool flag = msg_PlayerInput.Commands == null;
				if (!flag)
				{
					byte actorId = msg_PlayerInput.ActorId;
					foreach (InputCmd cmd in msg_PlayerInput.Commands)
					{
						this._world.ProcessInputQueue(actorId, cmd);
					}
				}
			}
		}
		
		private void OnPursuingFrame()
		{
			this._globalStateService.IsPursueFrame = true;
			Debug.Log("PurchaseServering curTick:" + this._world.Tick, Array.Empty<object>());
			float num = (float)this._world.Tick * 1f / (float)this._cmdBuffer.CurTickInServer;
			EventHelper.Trigger(EEvent.PursueFrameProcess, num);
		}
		
		public void OnBorderVideoFrame(Msg_RepMissFrame msg)
		{
			this._videoFrames = msg;
		}
		
		public void OnServerFrame(Msg_ServerFrames msg)
		{
			this._hasRecvInputMsg = true;
			this._cmdBuffer.PushServerFrames(msg.frames, true);
		}
		
		public void OnServerMissFrame(Msg_RepMissFrame msg)
		{
			this._cmdBuffer.PushMissServerFrames(msg.frames, false);
		}
		
		public void OnPlayerPing(Msg_G2C_PlayerPing msg)
		{
			this._cmdBuffer.OnPlayerPing(msg);
		}
		
		public void OnServerHello(Msg_G2C_Hello msg)
		{
			this.LocalActorId = msg.LocalId;
			Debug.Log("OnEvent_OnServerHello " + this.LocalActorId, Array.Empty<object>());
		}
		
		public void OnGameCreateHello(Msg_G2C_Hello msg)
		{
			Debug.Log("Msg_G2C_Hello ", Array.Empty<object>());
			this.LocalActorId = msg.LocalId;
		}
		
		public void OnGameCreateGameStartInfo(Msg_G2C_GameStartInfo msg)
		{
			this._gameStartInfo = msg;
			this._globalStateService.GameStartInfo = this._gameStartInfo;
			Debug.Log("Msg_G2C_GameStartInfo ", Array.Empty<object>());
			this.SimulationAwake(60, this.LocalActorId, msg.UserCount, true);
		}
		
		public void OnAllPlayerFinishedLoad(object param)
		{
			Debug.Log("OnEvent_OnAllPlayerFinishedLoad", Array.Empty<object>());
			this.SimulationStart();
		}
		
		public void OnLevelLoadDone(object param)
		{
			Debug.Log("OnEvent_LevelLoadDone " + this._globalStateService.IsReconnecting.ToString(), Array.Empty<object>());
			bool flag = this._globalStateService.IsReconnecting || this._globalStateService.IsVideoMode || this._globalStateService.IsClientMode;
			if (flag)
			{
				EventHelper.Trigger(EEvent.OnAllPlayerFinishedLoad, null);
				this.SimulationStart();
			}
		}
		
		private void OnEvent_BorderVideoFrame(object param)
		{
			Msg_RepMissFrame msg = param as Msg_RepMissFrame;
			this.OnBorderVideoFrame(msg);
		}
		
		private void OnEvent_OnServerFrame(object param)
		{
			Msg_ServerFrames msg = param as Msg_ServerFrames;
			this.OnServerFrame(msg);
		}
		
		private void OnEvent_OnServerMissFrame(object param)
		{
			Debug.Log("OnEvent_OnServerMissFrame", Array.Empty<object>());
			Msg_RepMissFrame msg = param as Msg_RepMissFrame;
			this.OnServerMissFrame(msg);
		}
		
		private void OnEvent_OnPlayerPing(object param)
		{
			Msg_G2C_PlayerPing msg = param as Msg_G2C_PlayerPing;
			this.OnPlayerPing(msg);
		}
		
		private void OnEvent_OnServerHello(object param)
		{
			Msg_G2C_Hello msg_G2C_Hello = param as Msg_G2C_Hello;
			this.LocalActorId = msg_G2C_Hello.LocalId;
			Debug.Log("OnEvent_OnServerHello " + this.LocalActorId, Array.Empty<object>());
		}
		
		private void OnEvent_OnGameCreate(object param)
		{
			Debug.Log(" OnEvent_OnGameCreate", Array.Empty<object>());
			Msg_G2C_Hello msg;
			bool flag = (msg = (param as Msg_G2C_Hello)) != null;
			if (flag)
			{
				this.OnGameCreateHello(msg);
			}
			Msg_G2C_GameStartInfo msg2;
			bool flag2 = (msg2 = (param as Msg_G2C_GameStartInfo)) != null;
			if (flag2)
			{
				this.OnGameCreateGameStartInfo(msg2);
			}
		}
		
		private void OnEvent_OnAllPlayerFinishedLoad(object param)
		{
			this.OnAllPlayerFinishedLoad(param);
		}
		
		private void OnEvent_LevelLoadDone(object param)
		{
			this.OnLevelLoadDone(param);
		}
		
		
	}
}
