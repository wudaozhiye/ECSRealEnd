using System;

namespace Lockstep.Game
{

	public abstract class BaseGameService : BaseService, IBaseGameManager
	{
		public override void InitReference(IServiceContainer serviceContainer, IManagerContainer mgrContainer)
		{
			base.InitReference(serviceContainer, mgrContainer);
			this._networkService = serviceContainer.GetService<INetworkService>();
			this._simulatorService = serviceContainer.GetService<ISimulatorService>();
			this._uiService = serviceContainer.GetService<IUIService>();
			this.OnInitReference(serviceContainer, mgrContainer);
		}
		
		protected virtual void OnInitReference(IServiceContainer serviceContainer, IManagerContainer mgrContainer)
		{
		}
		
		protected INetworkService _networkService;
		
		protected ISimulatorService _simulatorService;
		
		protected IUIService _uiService;
	}
}
