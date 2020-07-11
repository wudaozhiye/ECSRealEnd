using System;
using System.Collections.Generic;
using System.Linq;
using Lockstep.Logging;
using Lockstep.Math;
using Lockstep.Util;
using NetMsg.Common;

namespace Lockstep.Game
{
	// Token: 0x0200002A RID: 42
	public class FrameBuffer : object, IFrameBuffer
	{

		public int PingVal { get; private set; }
		
		public int DelayVal { get; private set; }
		
		public int CurTickInServer { get; private set; }
		
		public int NextTickToCheck { get; private set; }

		public int MaxServerTickInBuffer { get; private set; } = -1;
		
		public bool IsNeedRollback { get; private set; }
		
		public int MaxContinueServerTick { get; private set; }
		
		public FrameBuffer(SimulatorService _simulatorService, INetworkService networkService, int bufferSize, int snapshotFrameInterval, int maxClientPredictFrameCount)
		{
			this._simulatorService = _simulatorService;
			this._predictHelper = new FrameBuffer.PredictCountHelper(_simulatorService, this);
			this._bufferSize = bufferSize;
			this._networkService = networkService;
			this._maxClientPredictFrameCount = maxClientPredictFrameCount;
			this._spaceRollbackNeed = snapshotFrameInterval * 2;
			this._maxServerOverFrameCount = bufferSize - this._spaceRollbackNeed;
			this._serverBuffer = new ServerFrame[bufferSize];
			this._clientBuffer = new ServerFrame[bufferSize];
		}
		
		public void SetClientTick(int tick)
		{
			this._nextClientTick = tick + 1;
		}
		
		public void PushLocalFrame(ServerFrame frame)
		{
			int num = frame.Tick % this._bufferSize;
			Debug.Assert(this._clientBuffer[num] == null || this._clientBuffer[num].Tick <= frame.Tick, "Push local frame error!");
			this._clientBuffer[num] = frame;
		}
		
		public void OnPlayerPing(Msg_G2C_PlayerPing msg)
		{
			long num = LTime.realtimeSinceStartupMS - msg.SendTimestamp;
			this._pings.Add(num);
			bool flag = num > this._maxPing;
			if (flag)
			{
				this._maxPing = num;
			}
			bool flag2 = num < this._minPing;
			if (flag2)
			{
				this._minPing = num;
				this._guessServerStartTimestamp = LTime.realtimeSinceStartupMS - msg.TimeSinceServerStart - this._minPing / 2L;
			}
		}
		
		public void PushMissServerFrames(ServerFrame[] frames, bool isNeedDebugCheck = true)
		{
			this.PushServerFrames(frames, isNeedDebugCheck);
			this._networkService.SendMissFrameRepAck(this.MaxContinueServerTick + 1);
		}
		
		public void ForcePushDebugFrame(ServerFrame data)
		{
			int num = data.Tick % this._bufferSize;
			this._serverBuffer[num] = data;
			this._clientBuffer[num] = data;
		}
		
		public void PushServerFrames(ServerFrame[] frames, bool isNeedDebugCheck = true)
		{
			int num = frames.Length;
			for (int i = 0; i < num; i++)
			{
				ServerFrame serverFrame = frames[i];
				long num2;
				bool flag = this._tick2SendTimestamp.TryGetValue(serverFrame.Tick, out num2);
				if (flag)
				{
					long item = LTime.realtimeSinceStartupMS - num2;
					this._delays.Add(item);
					this._tick2SendTimestamp.Remove(serverFrame.Tick);
				}
				bool flag2 = serverFrame.Tick < this.NextTickToCheck;
				if (!flag2)
				{
					bool flag3 = serverFrame.Tick > this.CurTickInServer;
					if (flag3)
					{
						this.CurTickInServer = serverFrame.Tick;
					}
					bool flag4 = serverFrame.Tick >= this.NextTickToCheck + this._maxServerOverFrameCount - 1;
					if (flag4)
					{
						break;
					}
					bool flag5 = serverFrame.Tick > this.MaxServerTickInBuffer;
					if (flag5)
					{
						this.MaxServerTickInBuffer = serverFrame.Tick;
					}
					int num3 = serverFrame.Tick % this._bufferSize;
					bool flag6 = this._serverBuffer[num3] == null || this._serverBuffer[num3].Tick != serverFrame.Tick;
					if (flag6)
					{
						this._serverBuffer[num3] = serverFrame;
						bool flag7 = serverFrame.Tick > this._predictHelper.nextCheckMissTick && serverFrame.Inputs[(int)this.LocalId].IsMiss && this._predictHelper.missTick == -1;
						if (flag7)
						{
							this._predictHelper.missTick = serverFrame.Tick;
						}
					}
				}
			}
		}
		
		public void DoUpdate(float deltaTime)
		{
			this._networkService.SendPing(this._simulatorService.LocalActorId, LTime.realtimeSinceStartupMS);
			this._predictHelper.DoUpdate(deltaTime);
			int tick = this._simulatorService.World.Tick;
			this.UpdatePingVal(deltaTime);
			this.IsNeedRollback = false;
			while (this.NextTickToCheck <= this.MaxServerTickInBuffer && this.NextTickToCheck < tick)
			{
				int num = this.NextTickToCheck % this._bufferSize;
				ServerFrame serverFrame = this._clientBuffer[num];
				ServerFrame serverFrame2 = this._serverBuffer[num];
				bool flag = serverFrame == null || serverFrame.Tick != this.NextTickToCheck || serverFrame2 == null || serverFrame2.Tick != this.NextTickToCheck;
				if (flag)
				{
					break;
				}
				bool flag2 = serverFrame2 == serverFrame || serverFrame2.Equals(serverFrame);
				if (!flag2)
				{
					this.IsNeedRollback = true;
					break;
				}
				int nextTickToCheck = this.NextTickToCheck;
				this.NextTickToCheck = nextTickToCheck + 1;
			}
			int i;
			for (i = this.NextTickToCheck; i <= this.MaxServerTickInBuffer; i++)
			{
				int num2 = i % this._bufferSize;
				bool flag3 = this._serverBuffer[num2] == null || this._serverBuffer[num2].Tick != i;
				if (flag3)
				{
					break;
				}
			}
			this.MaxContinueServerTick = i - 1;
			bool flag4 = this.MaxContinueServerTick <= 0;
			if (!flag4)
			{
				bool flag5 = this.MaxContinueServerTick < this.CurTickInServer || this._nextClientTick > this.MaxContinueServerTick + (this._maxClientPredictFrameCount - 3);
				if (flag5)
				{
					Debug.Log("SendMissFrameReq " + this.MaxContinueServerTick, Array.Empty<object>());
					this._networkService.SendMissFrameReq(this.MaxContinueServerTick);
				}
			}
		}
		
		private void UpdatePingVal(float deltaTime)
		{
			this._pingTimer += deltaTime;
			bool flag = this._pingTimer > 0.5f;
			if (flag)
			{
				this._pingTimer = 0f;
				this.DelayVal = (int)(this._delays.Sum() / (long)LMath.Max(this._delays.Count, 1));
				this._delays.Clear();
				this.PingVal = (int)(this._pings.Sum() / (long)LMath.Max(this._pings.Count, 1));
				this._pings.Clear();
				bool flag2 = this._minPing < this._historyMinPing && this._simulatorService._gameStartTimestampMs != -1L;
				if (flag2)
				{
					this._historyMinPing = this._minPing;
					Debug.LogWarning(string.Format("Recalc _gameStartTimestampMs {0} _guessServerStartTimestamp:{1}", this._simulatorService._gameStartTimestampMs, this._guessServerStartTimestamp), Array.Empty<object>());
					this._simulatorService._gameStartTimestampMs = LMath.Min(this._guessServerStartTimestamp, this._simulatorService._gameStartTimestampMs);
				}
				this._minPing = long.MaxValue;
				this._maxPing = long.MinValue;
			}
		}
		
		public void SendInput(Msg_PlayerInput input)
		{
			this._tick2SendTimestamp[input.Tick] = LTime.realtimeSinceStartupMS;
			this._networkService.SendInput(input);
		}
		
		public ServerFrame GetFrame(int tick)
		{
			ServerFrame serverFrame = this.GetServerFrame(tick);
			bool flag = serverFrame != null;
			ServerFrame result;
			if (flag)
			{
				result = serverFrame;
			}
			else
			{
				result = this.GetLocalFrame(tick);
			}
			return result;
		}
		
		public ServerFrame GetServerFrame(int tick)
		{
			bool flag = tick > this.MaxServerTickInBuffer;
			ServerFrame result;
			if (flag)
			{
				result = null;
			}
			else
			{
				result = this._GetFrame(this._serverBuffer, tick);
			}
			return result;
		}
		
		public ServerFrame GetLocalFrame(int tick)
		{
			bool flag = tick >= this._nextClientTick;
			ServerFrame result;
			if (flag)
			{
				result = null;
			}
			else
			{
				result = this._GetFrame(this._clientBuffer, tick);
			}
			return result;
		}
		
		private ServerFrame _GetFrame(ServerFrame[] buffer, int tick)
		{
			int num = tick % this._bufferSize;
			ServerFrame serverFrame = buffer[num];
			bool flag = serverFrame == null;
			ServerFrame result;
			if (flag)
			{
				result = null;
			}
			else
			{
				bool flag2 = serverFrame.Tick != tick;
				if (flag2)
				{
					result = null;
				}
				else
				{
					result = serverFrame;
				}
			}
			return result;
		}


		public static byte __debugMainActorID;
		
		private int _maxClientPredictFrameCount;
		
		private int _bufferSize;
		
		private int _spaceRollbackNeed;
		
		private int _maxServerOverFrameCount;
		
		private ServerFrame[] _serverBuffer;
		
		private ServerFrame[] _clientBuffer;
		
		private List<long> _pings = new List<long>();
		
		private long _guessServerStartTimestamp = long.MaxValue;
		
		private long _historyMinPing = long.MaxValue;
		
		private long _minPing = long.MaxValue;

		private long _maxPing = long.MinValue;
		
		private float _pingTimer;
		
		private List<long> _delays = new List<long>();
		
		private Dictionary<int, long> _tick2SendTimestamp = new Dictionary<int, long>();
		
		private int _nextClientTick;
		
		public byte LocalId;
		
		public INetworkService _networkService;
		
		private FrameBuffer.PredictCountHelper _predictHelper;
		
		private SimulatorService _simulatorService;
		
		public class PredictCountHelper : object
		{
			public PredictCountHelper(SimulatorService simulatorService, FrameBuffer cmdBuffer)
			{
				this._cmdBuffer = cmdBuffer;
				this._simulatorService = simulatorService;
			}
			
			public void DoUpdate(float deltaTime)
			{
				this._timer += deltaTime;
				bool flag = this._timer > this._checkInterval;
				if (flag)
				{
					this._timer = 0f;
					bool flag2 = !this.hasMissTick;
					if (flag2)
					{
						float num = (float)this._cmdBuffer._maxPing * 1f / 30f;
						this._targetPreSendTick = this._targetPreSendTick * this._oldPercent + num * (1f - this._oldPercent);
						int num2 = LMath.Clamp((int)System.Math.Ceiling((double)this._targetPreSendTick), 1, 60);
						bool flag3 = num2 != this._simulatorService.PreSendInputCount;
						if (flag3)
						{
							Debug.LogWarning(string.Format("Shrink preSend buffer old:{0} new:{1} ", this._simulatorService.PreSendInputCount, this._targetPreSendTick) + string.Format("PING: min:{0} max:{1} avg:{2}", this._cmdBuffer._minPing, this._cmdBuffer._maxPing, this._cmdBuffer.PingVal), Array.Empty<object>());
						}
						this._simulatorService.PreSendInputCount = num2;
					}
					this.hasMissTick = false;
				}
				bool flag4 = this.missTick != -1;
				if (flag4)
				{
					int num3 = this._simulatorService.TargetTick - this.missTick;
					int num4 = this._simulatorService.PreSendInputCount + (int)System.Math.Ceiling((double)((float)num3 * this._incPercent));
					num4 = LMath.Clamp(num4, 1, 60);
					Debug.LogWarning(string.Format("Expend preSend buffer old:{0} new:{1}", this._simulatorService.PreSendInputCount, num4), Array.Empty<object>());
					this._simulatorService.PreSendInputCount = num4;
					this.nextCheckMissTick = this._simulatorService.TargetTick;
					this.missTick = -1;
					this.hasMissTick = true;
				}
			}

			// Token: 0x04000156 RID: 342
			public int missTick = -1;

			// Token: 0x04000157 RID: 343
			public int nextCheckMissTick = 0;

			// Token: 0x04000158 RID: 344
			public bool hasMissTick;

			// Token: 0x04000159 RID: 345
			private SimulatorService _simulatorService;

			// Token: 0x0400015A RID: 346
			private FrameBuffer _cmdBuffer;

			// Token: 0x0400015B RID: 347
			private float _timer;

			// Token: 0x0400015C RID: 348
			private float _checkInterval = 0.5f;

			// Token: 0x0400015D RID: 349
			private float _incPercent = 0.3f;

			// Token: 0x0400015E RID: 350
			private float _targetPreSendTick;

			// Token: 0x0400015F RID: 351
			private float _oldPercent = 0.6f;
		}
	}
}
