
using System;

namespace IGCConsWrapper
{
	public class Update
	{
		public string basename;
		public string lastUpdate;
		public string docCount;
		
		public Update()
		{
		}
		
		public Update (string basename, string lastUpdate, string docCount)
		{
			this.basename = basename;
			this.lastUpdate = lastUpdate;
			this.docCount = docCount;
		}
		
		public Update (params string[] par)
		{
		}
	}
}
