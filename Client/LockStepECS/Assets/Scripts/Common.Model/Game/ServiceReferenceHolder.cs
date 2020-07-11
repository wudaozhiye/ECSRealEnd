using System;

namespace Lockstep.Game
{
	public class ServiceReferenceHolder : object
	{
		protected IServiceContainer _serviceContainer;
		
		protected IECSFactoryService _ecsFactoryService;
		
		protected IRandomService _randomService;
		
		protected ITimeMachineService _timeMachineService;
		
		protected IGlobalStateService _globalStateService;
		
		protected IViewService _viewService;
		
		protected IAudioService _audioService;
		
		protected IInputService _inputService;
		
		protected IMap2DService _map2DService;
		
		protected IResService _resService;
		
		protected IEffectService _effectService;
		
		protected IEventRegisterService _eventRegisterService;
		
		protected IIdService _idService;
		
		protected IDebugService _debugService;
		protected T GetService<T>() where T : IService
		{
			return this._serviceContainer.GetService<T>();
		}
		
		public virtual void InitReference(IServiceContainer serviceContainer, IManagerContainer mgrContainer = null)
		{
			this._serviceContainer = serviceContainer;
			this._ecsFactoryService = serviceContainer.GetService<IECSFactoryService>();
			this._randomService = serviceContainer.GetService<IRandomService>();
			this._timeMachineService = serviceContainer.GetService<ITimeMachineService>();
			this._globalStateService = serviceContainer.GetService<IGlobalStateService>();
			this._inputService = serviceContainer.GetService<IInputService>();
			this._viewService = serviceContainer.GetService<IViewService>();
			this._audioService = serviceContainer.GetService<IAudioService>();
			this._map2DService = serviceContainer.GetService<IMap2DService>();
			this._resService = serviceContainer.GetService<IResService>();
			this._effectService = serviceContainer.GetService<IEffectService>();
			this._eventRegisterService = serviceContainer.GetService<IEventRegisterService>();
			this._idService = serviceContainer.GetService<IIdService>();
			this._debugService = serviceContainer.GetService<IDebugService>();
		}
	}
}
