﻿using System;

namespace Lockstep.Game
{
	public class UIDefine
	{
		public static WindowCreateInfo UIRoot = new WindowCreateInfo("UIRoot", EWindowDepth.Normal);

		public static WindowCreateInfo UILogin = new WindowCreateInfo("UILogin", EWindowDepth.Normal);

		public static WindowCreateInfo UILobby = new WindowCreateInfo("UILobby", EWindowDepth.Normal);

		public static WindowCreateInfo UICreateRoom = new WindowCreateInfo("UICreateRoom", EWindowDepth.Normal);

		public static WindowCreateInfo UIRoomList = new WindowCreateInfo("UIRoomList", EWindowDepth.Normal);

		public static WindowCreateInfo UINetStatus = new WindowCreateInfo("UINetStatus", EWindowDepth.Normal);

		public static WindowCreateInfo UIGameStatus = new WindowCreateInfo("UIGameStatus", EWindowDepth.Normal);

		public static WindowCreateInfo UIDialogBox = new WindowCreateInfo("UIDialogBox", EWindowDepth.Forward);

		public static WindowCreateInfo UILoading = new WindowCreateInfo("UILoading", EWindowDepth.Notice);

		public static WindowCreateInfo UIDebugInfo = new WindowCreateInfo("UIDebugInfo", EWindowDepth.Notice);
	}
}
