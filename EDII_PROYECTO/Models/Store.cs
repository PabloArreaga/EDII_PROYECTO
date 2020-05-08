using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EDII_PROYECTO.Models
{
	public class Store : IComparable
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string  Address { get; set; }

		public int CompareTo(object obj)
		{
			return this.Id.CompareTo(((Store)obj).Id);
		}
	}
}
