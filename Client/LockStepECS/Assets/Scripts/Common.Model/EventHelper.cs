using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Lockstep
{
	public class EventHelper
	{
		public struct MsgInfo
		{
			public EEvent type;

			public object param;

			public MsgInfo(EEvent type, object param)
			{
				this.type = type;
				this.param = param;
			}
		}


		public struct ListenerInfo
		{
			public bool isRegister;

			public EEvent type;

			public GlobalEventHandler param;

			public ListenerInfo(bool isRegister, EEvent type, GlobalEventHandler param)
			{
				this.isRegister = isRegister;
				this.type = type;
				this.param = param;
			}
		}
		private static Dictionary<int, List<GlobalEventHandler>> allListeners = new Dictionary<int, List<GlobalEventHandler>>();
		
		private static Queue<EventHelper.MsgInfo> allPendingMsgs = new Queue<EventHelper.MsgInfo>();
		
		private static Queue<EventHelper.ListenerInfo> allPendingListeners = new Queue<EventHelper.ListenerInfo>();

		private static Queue<EEvent> allNeedRemoveTypes = new Queue<EEvent>();

		private static bool IsTriggingEvent;

		public static void RemoveAllListener(EEvent type)
		{
			bool isTriggingEvent = EventHelper.IsTriggingEvent;
			if (isTriggingEvent)
			{
				EventHelper.allNeedRemoveTypes.Enqueue(type);
			}
			else
			{
				EventHelper.allListeners.Remove((int)type);
			}
		}


		public static void AddListener(EEvent type, GlobalEventHandler listener)
		{
			bool isTriggingEvent = EventHelper.IsTriggingEvent;
			if (isTriggingEvent)
			{
				EventHelper.allPendingListeners.Enqueue(new EventHelper.ListenerInfo(true, type, listener));
			}
			else
			{
				List<GlobalEventHandler> list;
				bool flag = EventHelper.allListeners.TryGetValue((int)type, out list);
				if (flag)
				{
					list.Add(listener);
				}
				else
				{
					List<GlobalEventHandler> list2 = new List<GlobalEventHandler>();
					list2.Add(listener);
					EventHelper.allListeners.Add((int)type, list2);
				}
			}
		}
		
		public static void RemoveListener(EEvent type, GlobalEventHandler listener)
		{
			bool isTriggingEvent = EventHelper.IsTriggingEvent;
			if (isTriggingEvent)
			{
				EventHelper.allPendingListeners.Enqueue(new EventHelper.ListenerInfo(false, type, listener));
			}
			else
			{
				List<GlobalEventHandler> list;
				bool flag = EventHelper.allListeners.TryGetValue((int)type, out list);
				if (flag)
				{
					bool flag2 = list.Remove(listener);
					if (flag2)
					{
						bool flag3 = list.Count == 0;
						if (flag3)
						{
							EventHelper.allListeners.Remove((int)type);
						}
					}
				}
			}
		}
		
		public static void Trigger(EEvent type, object param = null)
		{
			bool isTriggingEvent = EventHelper.IsTriggingEvent;
			if (isTriggingEvent)
			{
				EventHelper.allPendingMsgs.Enqueue(new EventHelper.MsgInfo(type, param));
			}
			else
			{
				List<GlobalEventHandler> list;
				bool flag = EventHelper.allListeners.TryGetValue((int)type, out list);
				if (flag)
				{
					EventHelper.IsTriggingEvent = true;
					foreach (GlobalEventHandler globalEventHandler in list.ToArray())
					{
						if (globalEventHandler != null)
						{
							globalEventHandler(param);
						}
					}
				}
				EventHelper.IsTriggingEvent = false;
				while (EventHelper.allPendingListeners.Count > 0)
				{
					EventHelper.ListenerInfo listenerInfo = EventHelper.allPendingListeners.Dequeue();
					bool isRegister = listenerInfo.isRegister;
					if (isRegister)
					{
						EventHelper.AddListener(listenerInfo.type, listenerInfo.param);
					}
					else
					{
						EventHelper.RemoveListener(listenerInfo.type, listenerInfo.param);
					}
				}
				while (EventHelper.allNeedRemoveTypes.Count > 0)
				{
					EEvent type2 = EventHelper.allNeedRemoveTypes.Dequeue();
					EventHelper.RemoveAllListener(type2);
				}
				while (EventHelper.allPendingMsgs.Count > 0)
				{
					EventHelper.MsgInfo msgInfo = EventHelper.allPendingMsgs.Dequeue();
					EventHelper.Trigger(msgInfo.type, msgInfo.param);
				}
			}
		}


	}
}
