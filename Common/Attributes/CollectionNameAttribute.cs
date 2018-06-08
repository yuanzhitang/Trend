using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trend.Common
{
	[AttributeUsage(AttributeTargets.All)]
	public class CollectionNameAttribute:Attribute
	{
		private string collectionName;
		public CollectionNameAttribute(string collectionName)
		{
			this.collectionName = collectionName;
		}

		public string CollectionName
		{
			get { return collectionName; }
		}
	}
}
