using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Lockstep.Game
{
	public class SingletonMono<T> : MonoBehaviour where T : Component
	{
		public static T Instance
		{
			get
			{
				bool flag = SingletonMono<T>._instance == null;
				if (flag)
				{
					SingletonMono<T>.CreateInstance();
				}
				return SingletonMono<T>._instance;
			}
		}
		
		private static T CreateInstance()
		{
			if ((object)_instance == null)
			{
				GameObject val = new GameObject(typeof(T).Name);
				Object.DontDestroyOnLoad(val);
				T val2 = _instance = val.AddComponent<T>();
			}
			return _instance;
		}
		
		protected static T _instance;
	}
}
