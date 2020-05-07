using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace EDII_PROYECTO.Models
{
	public class Product
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public double Price { get; set; }
	
		public int CompareTo(object obj)
		{
			return this.Id.CompareTo(((Product)obj).Id);
		}
	
	
	
	
	}

}
