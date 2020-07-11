using System;
using System.Reflection;

namespace Lockstep.Game
{
	// Token: 0x0200001A RID: 26
	[Serializable]
	public class EntityConfig : object
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000052 RID: 82 RVA: 0x00003BA9 File Offset: 0x00001DA9
		public virtual object Entity { get; }

		// Token: 0x06000053 RID: 83 RVA: 0x00003BB4 File Offset: 0x00001DB4
		public void CopyTo(object dst)
		{
			bool flag = this.Entity.GetType() != dst.GetType();
			if (!flag)
			{
				FieldInfo[] fields = dst.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
				foreach (FieldInfo fieldInfo in fields)
				{
					Type fieldType = fieldInfo.FieldType;
					bool flag2 = typeof(INeedBackup).IsAssignableFrom(fieldType);
					if (flag2)
					{
						this.CopyTo(fieldInfo.GetValue(dst), fieldInfo.GetValue(this.Entity));
					}
					else
					{
						fieldInfo.SetValue(dst, fieldInfo.GetValue(this.Entity));
					}
				}
			}
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00003C60 File Offset: 0x00001E60
		private void CopyTo(object dst, object src)
		{
			bool flag = src.GetType() != dst.GetType();
			if (!flag)
			{
				FieldInfo[] fields = dst.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
				foreach (FieldInfo fieldInfo in fields)
				{
					Type fieldType = fieldInfo.FieldType;
					fieldInfo.SetValue(dst, fieldInfo.GetValue(src));
				}
			}
		}

		// Token: 0x04000047 RID: 71
		public string prefabPath;
	}
}
