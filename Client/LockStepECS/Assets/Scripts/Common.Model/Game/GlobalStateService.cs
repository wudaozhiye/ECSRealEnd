using System;
using System.Collections.Generic;
using Lockstep.Math;
using NetMsg.Common;

namespace Lockstep.Game
{
	public class GlobalStateService : BaseService, IGlobalStateService, ITimeMachine
	{

		public static GlobalStateService Instance { get; private set; }
		
		public GlobalStateService()
		{
			GlobalStateService.Instance = this;
		}
		
		public bool IsVideoLoading { get; set; }
		
		public bool IsVideoMode { get; set; }
		
		public bool IsRunVideo { get; set; }
		
		public bool IsClientMode { get; set; }
		
		public bool IsReconnecting { get; set; }
		
		public bool IsPursueFrame { get; set; }
		
		public string GameName { get; set; }
		
		public int CurLevel { get; set; }
		
		public object Contexts { get; set; }
		
		public int SnapshotFrameInterval { get; set; }
		
		public EPureModeType RunMode { get; set; }
		
		public byte LocalActorId { get; set; }
		
		public byte ActorCount { get; set; }
		
		public Msg_G2C_GameStartInfo GameStartInfo { get; set; }
		
		public CollisionConfig CollisionConfig { get; set; }
		
		public int Tick { get; set; }
		
		public LFloat DeltaTime
		{
			get
			{
				return this._DeltaTime;
			}
		}
		
		public LFloat TimeSinceGameStart { get; set; }
		
		public int Hash { get; set; }
		
		public bool IsPause { get; set; }
		
		public void SetTick(int val)
		{
			this.Tick = val;
		}
		
		public void SetDeltaTime(LFloat val)
		{
			this._DeltaTime = val;
		}
		
		public void SetTimeSinceGameStart(LFloat val)
		{
			this.TimeSinceGameStart = val;
		}
		
		public new int CurTick { get; set; }

		public override void RollbackTo(int tick)
		{
			this.Hash = this._tick2State[tick];
		}
		
		public override void Backup(int tick)
		{
			this._tick2State[tick] = this.Hash;
		}
		
		public override void Clean(int maxVerifiedTick)
		{
		}

		private LFloat _DeltaTime = new LFloat(null, 30L);
		
		private Dictionary<int, int> _tick2State = new Dictionary<int, int>();
	}
}
