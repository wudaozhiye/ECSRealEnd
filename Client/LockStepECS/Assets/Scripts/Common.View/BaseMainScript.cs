using System;
using Lockstep.Game;
using Lockstep.Logging;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Logger = Lockstep.Logging.Logger;

// Token: 0x0200000C RID: 12
public abstract class BaseMainScript: MonoBehaviour
{
	public T GetService<T>() where T : IService
	{
		return this._serviceContainer.GetService<T>();
	}
	
	public Launcher launcher = new Launcher();
	
	public bool IsClientMode = false;
	
	public bool IsVideoMode = false;
	
	public bool IsRunVideo;
	
	protected ServiceContainer _serviceContainer;
	public bool HasInit { get; private set; }
	
	protected virtual void Awake()
	{
		ProjectConfig.DoInit("");
		this.HasInit = true;
		Debug.Log(Application.persistentDataPath);
		TableManager.Init(ProjectConfig.ExcelPath, ".bytes");
		Debug.Log(ProjectConfig.ExcelPath);
		Logger.OnMessage += UnityLogHandler.OnLog;
		this._serviceContainer = this.CreateServiceContainer();
		this._serviceContainer.GetService<IGlobalStateService>().GameName = ProjectConfig.GameName;
		this._serviceContainer.GetService<IGlobalStateService>().IsClientMode = this.IsClientMode;
		this._serviceContainer.GetService<IGlobalStateService>().IsVideoMode = this.IsVideoMode;
		this._serviceContainer.GetService<ISimulatorService>().FuncCreateWorld = new FuncCreateWorld(this.CreateWorld);
		this.launcher.DoAwake(this._serviceContainer);
	}
	
	protected abstract ServiceContainer CreateServiceContainer();
	
	protected abstract object CreateWorld(IServiceContainer services, object contextsObj, object logicFeatureObj);
	
	protected virtual void Start()
	{
		this.launcher.DoStart();
	}
	
	private void Update()
	{
		this._serviceContainer.GetService<IGlobalStateService>().IsRunVideo = this.IsVideoMode;
		this.launcher.DoUpdate(Time.deltaTime);
	}
	
	private void OnDestroy()
	{
		this.launcher.DoDestroy();
	}
	
	private void OnApplicationQuit()
	{
		this.launcher.OnApplicationQuit();
	}

	
}
