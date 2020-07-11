using System;
using System.Collections.Generic;

namespace Lockstep.Util
{
	// Token: 0x02000013 RID: 19
	public class AttributeInfo : object
	{
		// Token: 0x0600003F RID: 63 RVA: 0x000036AD File Offset: 0x000018AD
		public AttributeInfo(object attribute, List<PublicMemberInfo> memberInfos)
		{
			this.attribute = attribute;
			this.memberInfos = memberInfos;
		}

		// Token: 0x0400002E RID: 46
		public readonly object attribute;

		// Token: 0x0400002F RID: 47
		public readonly List<PublicMemberInfo> memberInfos;
	}
}
