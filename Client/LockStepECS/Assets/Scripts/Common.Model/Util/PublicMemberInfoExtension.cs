using System;
using System.Collections.Generic;
using System.Reflection;

namespace Lockstep.Util
{
	// Token: 0x02000015 RID: 21
	public static class PublicMemberInfoExtension
	{
		// Token: 0x06000046 RID: 70 RVA: 0x00003858 File Offset: 0x00001A58
		public static List<PublicMemberInfo> GetPublicMemberInfos(this Type type)
		{
			FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
			PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
			List<PublicMemberInfo> list = new List<PublicMemberInfo>(fields.Length + properties.Length);
			for (int i = 0; i < fields.Length; i++)
			{
				list.Add(new PublicMemberInfo(fields[i]));
			}
			foreach (PropertyInfo propertyInfo in properties)
			{
				bool flag = propertyInfo.CanRead && propertyInfo.CanWrite && propertyInfo.GetIndexParameters().Length == 0;
				if (flag)
				{
					list.Add(new PublicMemberInfo(propertyInfo));
				}
			}
			return list;
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00003904 File Offset: 0x00001B04
		public static object PublicMemberClone(this object obj)
		{
			object obj2 = Activator.CreateInstance(obj.GetType());
			obj.CopyPublicMemberValues(obj2);
			return obj2;
		}

		// Token: 0x06000048 RID: 72 RVA: 0x0000392C File Offset: 0x00001B2C
		public static T PublicMemberClone<T>(this object obj) where T : new()
		{
			T t = Activator.CreateInstance<T>();
			obj.CopyPublicMemberValues(t);
			return t;
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00003954 File Offset: 0x00001B54
		public static void CopyPublicMemberValues(this object source, object target)
		{
			List<PublicMemberInfo> publicMemberInfos = source.GetType().GetPublicMemberInfos();
			for (int i = 0; i < publicMemberInfos.Count; i++)
			{
				PublicMemberInfo publicMemberInfo = publicMemberInfos[i];
				publicMemberInfo.SetValue(target, publicMemberInfo.GetValue(source));
			}
		}

		// Token: 0x0600004A RID: 74 RVA: 0x0000399C File Offset: 0x00001B9C
		public static void CopyFiledsTo(this object source, object target)
		{
			bool flag = source.GetType() == target.GetType();
			if (flag)
			{
				source.CopyPublicMemberValues(target);
			}
			else
			{
				FieldInfo[] fields = source.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
				FieldInfo[] fields2 = target.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
				Dictionary<string, FieldInfo> dictionary = new Dictionary<string, FieldInfo>();
				foreach (FieldInfo fieldInfo in fields2)
				{
					dictionary[fieldInfo.Name] = fieldInfo;
				}
				foreach (FieldInfo fieldInfo2 in fields)
				{
					FieldInfo fieldInfo3;
					bool flag2 = dictionary.TryGetValue(fieldInfo2.Name, out fieldInfo3);
					if (flag2)
					{
						bool flag3 = fieldInfo3.FieldType == fieldInfo2.FieldType;
						if (flag3)
						{
							fieldInfo3.SetValue(target, fieldInfo2.GetValue(source));
						}
					}
				}
			}
		}
	}
}
