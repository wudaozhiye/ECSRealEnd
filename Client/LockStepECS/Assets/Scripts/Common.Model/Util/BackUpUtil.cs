using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Lockstep.Game;
using Lockstep.Serialization;

namespace Lockstep.Util
{
	// Token: 0x02000011 RID: 17
	public static class BackUpUtil
	{
		// Token: 0x06000036 RID: 54 RVA: 0x00003404 File Offset: 0x00001604
		public static void Write<T>(this Serializer writer, IList<T> lst) where T : class, IBackup, new()
		{
			writer.Write((lst != null) ? lst.Count : 0);
			foreach (T t in lst)
			{
				writer.Write(t == null);
				T t2 = t;
				if (t2 != null)
				{
					t2.WriteBackup(writer);
				}
			}
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00003480 File Offset: 0x00001680
		public static T[] ReadArray<T>(this Deserializer reader, T[] _) where T : class, IBackup, new()
		{
			int num = reader.ReadInt32();
			T[] array = new T[num];
			for (int i = 0; i < num; i++)
			{
				bool flag = reader.ReadBoolean();
				T t = default(T);
				bool flag2 = !flag;
				if (flag2)
				{
					t = Activator.CreateInstance<T>();
					t.ReadBackup(reader);
				}
				array[i] = t;
			}
			return array;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x000034F0 File Offset: 0x000016F0
		public static List<T> ReadList<T>(this Deserializer reader, IList<T> _) where T : class, IBackup, new()
		{
			int num = reader.ReadInt32();
			List<T> list = new List<T>();
			for (int i = 0; i < num; i++)
			{
				bool flag = reader.ReadBoolean();
				T t = default(T);
				bool flag2 = !flag;
				if (flag2)
				{
					t = Activator.CreateInstance<T>();
					t.ReadBackup(reader);
				}
				list.Add(t);
			}
			return list;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00003560 File Offset: 0x00001760
		public static void DumpList(string name, IList lst, StringBuilder sb, string prefix)
		{
			sb.AppendLine(prefix + name + " Count:" + lst.Count.ToString());
			sb.Append("[");
			for (int i = 0; i < lst.Count; i++)
			{
				object obj = lst[i];
				IDumpStr dumpStr;
				bool flag = (dumpStr = (obj as IDumpStr)) != null;
				if (flag)
				{
					if (dumpStr != null)
					{
						dumpStr.DumpStr(sb, "\t" + prefix);
					}
				}
				else
				{
					sb.Append(i + ":" + obj.ToString());
				}
			}
			sb.Append("]");
		}
	}
}
