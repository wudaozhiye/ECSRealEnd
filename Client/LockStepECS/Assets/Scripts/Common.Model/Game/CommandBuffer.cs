using System;
using System.Collections.Generic;
using Lockstep.Logging;

namespace Lockstep.Game
{
	// Token: 0x02000020 RID: 32
	public class CommandBuffer : object, ICommandBuffer
	{
		// Token: 0x0600006C RID: 108 RVA: 0x00003DA8 File Offset: 0x00001FA8
		public void Init(object param, FuncUndoCommands funcUndoCommand)
		{
			this._param = param;
			this._funcUndoCommand = (funcUndoCommand ?? new FuncUndoCommands(this.UndoCommands));
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00003DD8 File Offset: 0x00001FD8
		public void Jump(int curTick, int dstTick)
		{
			bool flag = this._tail == null || this._tail.Tick <= dstTick;
			if (!flag)
			{
				CommandNode commandNode = this._tail;
				while (commandNode.pre != null && commandNode.pre.Tick >= dstTick)
				{
					commandNode = commandNode.pre;
				}
				Debug.Assert(commandNode.Tick >= dstTick, string.Format("newTail must be the first cmd executed after that tick : tick:{0}  newTail.Tick:{1}", dstTick, commandNode.Tick));
				bool val = commandNode.pre == null || commandNode.pre.Tick < dstTick;
				string str = string.Format("newTail must be the first cmd executed in that tick : tick:{0}  ", dstTick);
				string format = "newTail.pre.Tick:{0}";
				CommandNode pre = commandNode.pre;
				Debug.Assert(val, str + string.Format(format, (pre != null) ? pre.Tick : dstTick));
				CommandNode minTickNode = commandNode;
				CommandNode tail = this._tail;
				bool flag2 = commandNode.pre == null;
				if (flag2)
				{
					this._head = null;
					this._tail = null;
				}
				else
				{
					this._tail = commandNode.pre;
					this._tail.next = null;
					commandNode.pre = null;
				}
				this._funcUndoCommand(minTickNode, tail, this._param);
			}
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00003F24 File Offset: 0x00002124
		public void Clean(int maxVerifiedTick)
		{
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00003F34 File Offset: 0x00002134
		public void Execute(int tick, ICommand cmd)
		{
			bool flag = cmd == null;
			if (!flag)
			{
				cmd.Do(this._param);
				CommandNode commandNode = new CommandNode(tick, cmd, this._tail, null);
				bool flag2 = this._head == null;
				if (flag2)
				{
					this._head = commandNode;
					this._tail = commandNode;
				}
				else
				{
					this._tail.next = commandNode;
					this._tail = commandNode;
				}
			}
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00003F9C File Offset: 0x0000219C
		protected void UndoCommands(CommandNode minTickNode, CommandNode maxTickNode, object param)
		{
			bool flag = maxTickNode == null;
			if (!flag)
			{
				while (maxTickNode != minTickNode)
				{
					maxTickNode.cmd.Undo(this._param);
					maxTickNode = maxTickNode.pre;
				}
				maxTickNode.cmd.Undo(this._param);
			}
		}

		// Token: 0x0400004D RID: 77
		protected CommandNode _head;

		// Token: 0x0400004E RID: 78
		protected CommandNode _tail;

		// Token: 0x0400004F RID: 79
		protected object _param;

		// Token: 0x04000050 RID: 80
		protected FuncUndoCommands _funcUndoCommand;

		// Token: 0x04000051 RID: 81
		public List<CommandNode> heads = new List<CommandNode>();
	}
}
