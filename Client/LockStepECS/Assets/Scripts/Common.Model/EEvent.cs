using System;

namespace Lockstep
{
	public enum EEvent
	{
		TryLogin,
		OnTickPlayer,
		OnConnLogin,
		OnLoginFailed,
		OnLoginResult,
		OnLeaveRoom,
		OnJoinRoomResult,
		OnPlayerJoinRoom,
		OnPlayerLeaveRoom,
		OnPlayerReadyInRoom,
		OnRoomChatInfo,
		OnRoomInfoUpdate,
		OnConnectToGameServer,
		VideoLoadProgress,
		VideoLoadDone,
		OnPlayerPing,
		OnServerHello,
		OnGameCreate,
		SimulationAwake,
		LevelLoadProgress,
		LevelLoadDone,
		OnLoadingProgress,
		OnAllPlayerFinishedLoad,
		SimulationStart,
		OnGameStartInfo,
		PursueFrameProcess,
		PursueFrameDone,
		ReconnectLoadProgress,
		ReconnectLoadDone,
		OnServerMissFrame,
		OnServerFrame,
		BorderVideoFrame,
		OnCreateRoom
	}
}
