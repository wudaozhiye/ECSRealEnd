using System;
using NetMsg.Common;

namespace Lockstep.Game
{
	public interface ISimulationEventHandler : IService
	{
		void OnBorderVideoFrame(Msg_RepMissFrame msg);
		
		void OnServerFrame(Msg_ServerFrames msg);
		
		void OnServerMissFrame(Msg_RepMissFrame msg);
		
		void OnPlayerPing(Msg_G2C_PlayerPing msg);
		
		void OnServerHello(Msg_G2C_Hello msg);
		
		void OnGameCreateHello(Msg_G2C_Hello msg);
		
		void OnGameCreateGameStartInfo(Msg_G2C_GameStartInfo msg);
		
		void OnAllPlayerFinishedLoad(object param);
		
		void OnLevelLoadDone(object param);
	}
}
