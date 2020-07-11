using System;
using System.Threading;
using Lockstep.Logging;
using Lockstep.Math;
using Lockstep.Network;
using Lockstep.Util;
using NetMsg.Common;

namespace Lockstep.Game
{
	// Token: 0x02000022 RID: 34
	[Serializable]
	public class Launcher : ILifeCycle
	{
		public int CurTick
		{
			get
			{
				return this._serviceContainer.GetService<IGlobalStateService>().Tick;
			}
		}
		
		public static Launcher Instance { get; private set; }
		
		public bool IsRunVideo
		{
			get
			{
				return this._globalStateService.IsRunVideo;
			}
		}
		
		public bool IsVideoMode
		{
			get
			{
				return this._globalStateService.IsVideoMode;
			}
		}
		
		public bool IsClientMode
		{
			get
			{
				return this._globalStateService.IsClientMode;
			}
		}
		
		public void DoAwake(IServiceContainer services)
		{
			this._syncContext = new OneThreadSynchronizationContext();
			SynchronizationContext.SetSynchronizationContext(this._syncContext);
			Utils.StartServices();
			bool flag = Launcher.Instance != null;
			if (flag)
			{
				Debug.LogError("LifeCycle Error: Awake more than once!!", Array.Empty<object>());
			}
			else
			{
				Launcher.Instance = this;
				this._serviceContainer = (services as ServiceContainer);
				this._registerService = new EventRegisterService();
				this._mgrContainer = new ManagerContainer();
				this._timeMachineContainer = new TimeMachineContainer();
				IService[] allServices = this._serviceContainer.GetAllServices();
				foreach (IService service in allServices)
				{
					this._timeMachineContainer.RegisterTimeMachine(service as ITimeMachine);
					BaseService service2;
					bool flag2 = (service2 = (service as BaseService)) != null;
					if (flag2)
					{
						this._mgrContainer.RegisterManager(service2);
					}
				}
				this._serviceContainer.RegisterService(this._timeMachineContainer, true);
				this._serviceContainer.RegisterService(this._registerService, true);
			}
		}
		
		public void DoStart()
		{
			foreach (BaseService baseService in this._mgrContainer.AllMgrs)
			{
				baseService.InitReference(this._serviceContainer, this._mgrContainer);
			}
			foreach (BaseService obj in this._mgrContainer.AllMgrs)
			{
				this._registerService.RegisterEvent<EEvent, GlobalEventHandler>("OnEvent_", "OnEvent_".Length, new Action<EEvent, GlobalEventHandler>(EventHelper.AddListener), obj);
			}
			this._DoAwake(this._serviceContainer);
			foreach (BaseService baseService2 in this._mgrContainer.AllMgrs)
			{
				baseService2.DoAwake(this._serviceContainer);
			}
			foreach (BaseService baseService3 in this._mgrContainer.AllMgrs)
			{
				baseService3.DoStart();
			}
			this._DoStart();
		}
		
		public void _DoAwake(IServiceContainer serviceContainer)
		{
			this._globalStateService = serviceContainer.GetService<IGlobalStateService>();
			object contexts = this._serviceContainer.GetService<IECSFactoryService>().CreateContexts();
			this._globalStateService.Contexts = contexts;
			this._simulatorService = (serviceContainer.GetService<ISimulatorService>() as SimulatorService);
			this._networkService = (serviceContainer.GetService<INetworkService>() as NetworkService);
			bool isVideoMode = this.IsVideoMode;
			if (isVideoMode)
			{
				this._globalStateService.SnapshotFrameInterval = 20;
			}
		}
		
		public void _DoStart()
		{
			bool isVideoMode = this.IsVideoMode;
			if (isVideoMode)
			{
				EventHelper.Trigger(EEvent.BorderVideoFrame, this.FramesInfo);
				EventHelper.Trigger(EEvent.OnGameCreate, this.GameStartInfo);
			}
			else
			{
				bool isClientMode = this.IsClientMode;
				if (isClientMode)
				{
					this.GameStartInfo = this._serviceContainer.GetService<IGlobalStateService>().GameStartInfo;
					EventHelper.Trigger(EEvent.OnGameCreate, this.GameStartInfo);
				}
			}
		}
		
		public void DoUpdate(float fDeltaTime)
		{
			this._syncContext.Update();
			Utils.UpdateServices();
			LFloat deltaTime = fDeltaTime.ToLFloat();
			this._networkService.DoUpdate(deltaTime);
			bool flag = this.IsVideoMode && this.IsRunVideo && this.CurTick < this.MaxRunTick;
			if (flag)
			{
				this._simulatorService.RunVideo();
			}
			else
			{
				bool flag2 = this.IsVideoMode && !this.IsRunVideo;
				if (flag2)
				{
					this._simulatorService.JumpTo(this.JumpToTick);
				}
				this._simulatorService.DoUpdate(fDeltaTime);
			}
		}
		
		public void DoDestroy()
		{
			bool flag = Launcher.Instance == null;
			if (!flag)
			{
				foreach (BaseService baseService in this._mgrContainer.AllMgrs)
				{
					baseService.DoDestroy();
				}
				Launcher.Instance = null;
			}
		}
		
		public void OnApplicationQuit()
		{
			this.DoDestroy();
		}
		//IService 集合
		private ServiceContainer _serviceContainer;
		//BaseService集合
		private ManagerContainer _mgrContainer;
		//ITimeMachine集合
		private TimeMachineContainer _timeMachineContainer;
		
		private IEventRegisterService _registerService;
		
		public string RecordPath;
		
		public int MaxRunTick = int.MaxValue;
		
		public Msg_G2C_GameStartInfo GameStartInfo;
		
		public Msg_RepMissFrame FramesInfo;
		
		public int JumpToTick = 10;
		
		private SimulatorService _simulatorService = new SimulatorService();
		
		private NetworkService _networkService = new NetworkService();
		
		private IGlobalStateService _globalStateService;
		
		public object transform;
		
		private OneThreadSynchronizationContext _syncContext;
	}
}
