using System;
using Lockstep.Network;
using Lockstep.Serialization;
using NetMsg.Common;

namespace Lockstep.Game
{
	// Token: 0x02000050 RID: 80
	public class MessagePacker : object, IMessagePacker
	{
		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000194 RID: 404 RVA: 0x00005B2F File Offset: 0x00003D2F
		public static MessagePacker Instance { get; } = new MessagePacker();

		// Token: 0x06000195 RID: 405 RVA: 0x00005B38 File Offset: 0x00003D38
		public object DeserializeFrom(ushort opcode, byte[] bytes, int index, int count)
		{
			if (opcode <= 25)
			{
				switch (opcode)
				{
				case 6:
					return BaseFormater.FromBytes<Msg_ReqMissFrame>(bytes, index, count);
				case 7:
					return BaseFormater.FromBytes<Msg_RepMissFrameAck>(bytes, index, count);
				case 8:
					return BaseFormater.FromBytes<Msg_RepMissFrame>(bytes, index, count);
				case 9:
					return BaseFormater.FromBytes<Msg_HashCode>(bytes, index, count);
				case 10:
					return BaseFormater.FromBytes<Msg_PlayerInput>(bytes, index, count);
				case 11:
					return BaseFormater.FromBytes<Msg_ServerFrames>(bytes, index, count);
				case 12:
					return BaseFormater.FromBytes<Msg_C2G_PlayerPing>(bytes, index, count);
				case 13:
					return BaseFormater.FromBytes<Msg_G2C_PlayerPing>(bytes, index, count);
				default:
					if (opcode == 25)
					{
						return BaseFormater.FromBytes<Msg_C2L_JoinRoom>(bytes, index, count);
					}
					break;
				}
			}
			else
			{
				if (opcode == 27)
				{
					return BaseFormater.FromBytes<Msg_L2C_JoinRoomResult>(bytes, index, count);
				}
				if (opcode == 30)
				{
					return BaseFormater.FromBytes<Msg_C2L_LeaveRoom>(bytes, index, count);
				}
				switch (opcode)
				{
				case 39:
					return BaseFormater.FromBytes<Msg_G2C_Hello>(bytes, index, count);
				case 40:
					return BaseFormater.FromBytes<Msg_G2C_GameStartInfo>(bytes, index, count);
				case 41:
					return BaseFormater.FromBytes<Msg_C2G_LoadingProgress>(bytes, index, count);
				case 42:
					return BaseFormater.FromBytes<Msg_G2C_LoadingProgress>(bytes, index, count);
				case 43:
					return BaseFormater.FromBytes<Msg_G2C_AllFinishedLoaded>(bytes, index, count);
				case 47:
					return BaseFormater.FromBytes<Msg_G2C_GameEvent>(bytes, index, count);
				}
			}
			return null;
		}

		// Token: 0x06000196 RID: 406 RVA: 0x00005CA8 File Offset: 0x00003EA8
		public byte[] SerializeToByteArray(IMessage msg)
		{
			return ((BaseFormater)msg).ToBytes();
		}
	}
}
