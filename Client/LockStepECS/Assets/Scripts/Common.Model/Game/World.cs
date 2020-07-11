using System;
using Lockstep.Logging;
using NetMsg.Common;

namespace Lockstep.Game
{
	public class World : BaseService
	{
		public static World Instance { get; protected set; }
		
		public int Tick { get; set; }
		
		public static object MyPlayer;
		
		private bool _hasStart = false;
		public void SimulationAwake(IServiceContainer serviceContainer, IManagerContainer mgrContainer)
		{
			World.Instance = this;
			this._serviceContainer = serviceContainer;
			this.DoSimulateAwake(serviceContainer, mgrContainer);
		}
		
		protected virtual void DoSimulateAwake(IServiceContainer serviceContainer, IManagerContainer mgrContainer)
		{
		}
		
		public void SimulationStart(Msg_G2C_GameStartInfo gameStartInfo, int localPlayerId)
		{
			bool hasStart = this._hasStart;
			if (!hasStart)
			{
				this._hasStart = true;
				GameData[] userInfos = gameStartInfo.UserInfos;
				int num = userInfos.Length;
				string traceSavePath = string.Format("/tmp/LPDemo/Dump_{0}.txt", localPlayerId);
				Debug.TraceSavePath = traceSavePath;
				this._debugService.Trace("CreatePlayer " + num, false, false);
				this.DoSimulateStart();
			}
		}
		
		protected virtual void DoSimulateStart()
		{
		}
		
		public override void OnApplicationQuit()
		{
			this.DoDestroy();
		}
		
		public override void DoDestroy()
		{
			Debug.FlushTrace();
		}
		
		public void ProcessInputQueue(byte actorId, InputCmd cmd)
		{
			this.DoProcessInputQueue(actorId, cmd);
		}
		
		protected virtual void DoProcessInputQueue(byte actorId, InputCmd cmd)
		{
		}
		
		public void Step(bool isNeedGenSnap = true)
		{
			bool isPause = this._globalStateService.IsPause;
			if (!isPause)
			{
				this.DoStep(isNeedGenSnap);
				int tick = this.Tick;
				this.Tick = tick + 1;
			}
		}
		
		protected virtual void DoStep(bool isNeedGenSnap)
		{
		}
		
		public override void Backup(int tick)
		{
			this.DoBackup(tick);
		}
		
		protected virtual void DoBackup(int tick)
		{
		}
		
		public void RollbackTo(int tick, int maxContinueServerTick, bool isNeedClear = true)
		{
			bool flag = tick < 0;
			if (flag)
			{
				Debug.LogError("Target Tick invalid!" + tick, Array.Empty<object>());
			}
			else
			{
				Debug.Log(string.Format(" Rollback diff:{0} From{1}->{2}  maxContinueServerTick:{3} {4}", new object[]
				{
					this.Tick - tick,
					this.Tick,
					tick,
					maxContinueServerTick,
					isNeedClear
				}), Array.Empty<object>());
				this._timeMachineService.RollbackTo(tick);
				this.DoRollbackTo(tick, maxContinueServerTick, isNeedClear);
				this._globalStateService.SetTick(tick);
				this.Tick = tick;
			}
		}
		
		protected virtual void DoRollbackTo(int tick, int missFrameTick, bool isNeedClear = true)
		{
		}
		
		public virtual void CleanUselessSnapshot(int tick)
		{
			this.DoCleanUselessSnapshot(tick);
		}
		
		protected virtual void DoCleanUselessSnapshot(int tick)
		{
		}
		
	}
}
