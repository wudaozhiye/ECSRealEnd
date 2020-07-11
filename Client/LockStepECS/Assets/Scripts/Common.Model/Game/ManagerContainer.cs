using System;
using System.Collections.Generic;

namespace Lockstep.Game
{
	public class ManagerContainer : object, IManagerContainer
	{
		private Dictionary<string, BaseService> _name2Mgr = new Dictionary<string, BaseService>();
		
		public List<BaseService> AllMgrs = new List<BaseService>();
		public void RegisterManager(BaseService service)
		{
			string name = service.GetType().Name;
			bool flag = this._name2Mgr.ContainsKey(name);
			if (!flag)
			{
				this._name2Mgr.Add(name, service);
				this.AllMgrs.Add(service);
			}
		}

		public T GetManager<T>() where T : BaseService
		{
			BaseService baseService;
			bool flag = this._name2Mgr.TryGetValue(typeof(T).Name, out baseService);
			T result;
			if (flag)
			{
				result = (baseService as T);
			}
			else
			{
				result = default(T);
			}
			return result;
		}

		public void Foreach(Action<BaseService> func)
		{
			foreach (BaseService obj in this.AllMgrs)
			{
				func(obj);
			}
		}
		
	}
}
