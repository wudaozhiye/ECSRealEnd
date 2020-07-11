using System;
using Lockstep.Math;
using NetMsg.Common;

namespace Lockstep.Game
{
	// Token: 0x0200003D RID: 61
	public interface IGlobalStateService : IService
	{

		bool IsVideoLoading { get; set; }
		
		bool IsVideoMode { get; set; }
		
		bool IsRunVideo { get; set; }
		
		bool IsClientMode { get; set; }
		
		bool IsReconnecting { get; set; }
		
		bool IsPursueFrame { get; set; }
		
		string GameName { get; set; }
		
		int CurLevel { get; set; }
		
		object Contexts { get; set; }
		
		int SnapshotFrameInterval { get; set; }
		
		EPureModeType RunMode { get; set; }
		
		byte LocalActorId { get; set; }
		
		byte ActorCount { get; set; }
		
		Msg_G2C_GameStartInfo GameStartInfo { get; set; }

		CollisionConfig CollisionConfig { get; set; }
		
		int Tick { get; }
		
		LFloat DeltaTime { get; }
		
		LFloat TimeSinceGameStart { get; }
		
		int Hash { get; set; }
		
		bool IsPause { get; set; }
		
		void SetTick(int val);
		
		void SetDeltaTime(LFloat val);
		
		void SetTimeSinceGameStart(LFloat val);
	}
}
