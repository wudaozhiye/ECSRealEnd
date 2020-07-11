using System;
using System.Text;

namespace Lockstep.Game
{
	// Token: 0x0200001E RID: 30
	public abstract class BaseService : ServiceReferenceHolder, IService, IDoInit, ILifeCycle, ITimeMachine, IHashCode, IDumpStr
	{
		protected ICommandBuffer cmdBuffer;
		public virtual void DoInit(object objParent)
		{
		}
		
		public virtual void DoAwake(IServiceContainer services)
		{
		}
		
		public virtual void DoStart()
		{
		}
		
		public virtual void DoDestroy()
		{
		}
		
		public virtual void OnApplicationQuit()
		{
		}
		
		public virtual int GetHash(ref int idx)
		{
			return 0;
		}
		
		public virtual void DumpStr(StringBuilder sb, string prefix)
		{
		}
		
		protected BaseService()
		{
			this.cmdBuffer = new CommandBuffer();
			this.cmdBuffer.Init(this, this.GetRollbackFunc());
		}
		
		protected virtual FuncUndoCommands GetRollbackFunc()
		{
			return null;
		}
		
		public int CurTick
		{
			get
			{
				return this._globalStateService.Tick;
			}
		}

		public virtual void Backup(int tick)
		{
		}
		
		public virtual void RollbackTo(int tick)
		{
			ICommandBuffer commandBuffer = this.cmdBuffer;
			if (commandBuffer != null)
			{
				commandBuffer.Jump(this.CurTick, tick);
			}
		}
		
		public virtual void Clean(int maxVerifiedTick)
		{
			ICommandBuffer commandBuffer = this.cmdBuffer;
			if (commandBuffer != null)
			{
				commandBuffer.Clean(maxVerifiedTick);
			}
		}
	}
}
