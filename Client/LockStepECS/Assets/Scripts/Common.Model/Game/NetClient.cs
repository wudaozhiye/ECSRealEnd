using System;
using System.Net;
using Lockstep.Network;
using NetMsg.Common;

namespace Lockstep.Game
{
	public class NetClient : IMessageDispatcher
	{
		public void DoStart()
		{
			this.net.Awake(0);
			this.net.MessageDispatcher = this;
			this.net.MessagePacker = MessagePacker.Instance;
			this.Session = this.net.Create(NetClient.serverIpPoint);
			this.Session.Start();
		}
		
		public void DoDestroy()
		{
			bool flag = this.Session != null;
			if (flag)
			{
				this.net.Dispose();
				this.Session = null;
			}
		}
		
		public void Dispatch(Session session, Packet packet)
		{
			ushort num = packet.Opcode();
			object arg = session.Network.MessagePacker.DeserializeFrom(num, packet.Bytes, 3, (int)(packet.Length - 3));
			Action<ushort, object> netMsgHandler = this.NetMsgHandler;
			if (netMsgHandler != null)
			{
				netMsgHandler(num, arg);
			}
		}
		
		public void Send(IMessage msg)
		{
			this.Session.Send(msg);
		}
		
		public void SendMessage(EMsgSC type, byte[] bytes)
		{
			Session?.Send(0, (ushort)type, bytes);
		}
		
		public static IPEndPoint serverIpPoint = NetworkUtil.ToIPEndPoint("127.0.0.1", 10083);
		
		private NetOuterProxy net = new NetOuterProxy();
		
		public Session Session;
		
		public Action<ushort, object> NetMsgHandler;
		
		private int count = 0;
		
		public int id;
	}
}
