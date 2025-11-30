using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ExtensionMethods
{
	public static class CollectionExtension
	{
		public static ICollection<T> ToCollection<T>(this T item)
		{
			return new List<T> { item };
		}
	}
}
