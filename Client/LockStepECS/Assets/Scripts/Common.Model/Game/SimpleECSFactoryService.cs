using System;
using System.Collections.Generic;

namespace Lockstep.Game
{
	// Token: 0x02000060 RID: 96
	public class SimpleECSFactoryService : object, IECSFactoryService, IService
	{
		// Token: 0x06000284 RID: 644 RVA: 0x00007B50 File Offset: 0x00005D50
		public object CreateContexts()
		{
			return null;
		}

		// Token: 0x06000285 RID: 645 RVA: 0x00007B64 File Offset: 0x00005D64
		public object CreateSystems(object contexts, IServiceContainer services)
		{
			return new List<UpdatableService>();
		}
	}
}
