using System;
using System.Reflection;

namespace Lockstep.Util
{
	// Token: 0x02000014 RID: 20
	public class PublicMemberInfo : object
	{
		// Token: 0x06000040 RID: 64 RVA: 0x000036C8 File Offset: 0x000018C8
		public PublicMemberInfo(FieldInfo info)
		{
			this._fieldInfo = info;
			this.type = this._fieldInfo.FieldType;
			this.name = this._fieldInfo.Name;
			this.attributes = PublicMemberInfo.getAttributes(this._fieldInfo.GetCustomAttributes(false));
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00003720 File Offset: 0x00001920
		public PublicMemberInfo(PropertyInfo info)
		{
			this._propertyInfo = info;
			this.type = this._propertyInfo.PropertyType;
			this.name = this._propertyInfo.Name;
			this.attributes = PublicMemberInfo.getAttributes(this._propertyInfo.GetCustomAttributes(false));
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00003775 File Offset: 0x00001975
		public PublicMemberInfo(Type type, string name, AttributeInfo[] attributes = null)
		{
			this.type = type;
			this.name = name;
			this.attributes = attributes;
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00003794 File Offset: 0x00001994
		public object GetValue(object obj)
		{
			bool flag = this._fieldInfo == null;
			object value;
			if (flag)
			{
				value = this._propertyInfo.GetValue(obj, null);
			}
			else
			{
				value = this._fieldInfo.GetValue(obj);
			}
			return value;
		}

		// Token: 0x06000044 RID: 68 RVA: 0x000037D0 File Offset: 0x000019D0
		public void SetValue(object obj, object value)
		{
			bool flag = this._fieldInfo != null;
			if (flag)
			{
				this._fieldInfo.SetValue(obj, value);
			}
			else
			{
				this._propertyInfo.SetValue(obj, value, null);
			}
		}

		// Token: 0x06000045 RID: 69 RVA: 0x0000380C File Offset: 0x00001A0C
		private static AttributeInfo[] getAttributes(object[] attributes)
		{
			AttributeInfo[] array = new AttributeInfo[attributes.Length];
			for (int i = 0; i < attributes.Length; i++)
			{
				object obj = attributes[i];
				array[i] = new AttributeInfo(obj, obj.GetType().GetPublicMemberInfos());
			}
			return array;
		}

		// Token: 0x04000030 RID: 48
		public readonly Type type;

		// Token: 0x04000031 RID: 49
		public readonly string name;

		// Token: 0x04000032 RID: 50
		public readonly AttributeInfo[] attributes;

		// Token: 0x04000033 RID: 51
		private readonly FieldInfo _fieldInfo;

		// Token: 0x04000034 RID: 52
		private readonly PropertyInfo _propertyInfo;
	}
}
