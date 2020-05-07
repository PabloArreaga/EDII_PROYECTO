using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EDII_PROYECTO.Models
{
	public class Store_Product
	{
		int IdStore { get; set; }
		int IdProduct { get; set; }
		int Stock { get; set; }

		public int CompareTo(object obj)
		{
			if (this.IdStore.Equals(((Store_Product)obj).IdStore))
			{
				return this.IdProduct.CompareTo(((Store_Product)obj).IdProduct);
			}
			else
			{
				return 0; // no encontro coincidencia con la tienda
			}
			
		}
	}
}
