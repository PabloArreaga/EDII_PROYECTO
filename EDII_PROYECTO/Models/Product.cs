using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace EDII_PROYECTO.Models
{
	public class Product : IComparable
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public double Price { get; set; }
	
		public int CompareTo(object obj)
		{
			return this.Id.CompareTo(((Product)obj).Id);
		}

        public static string ConvertToString(object dataNode)
        {
            var auxNode = (Product)dataNode;
            
            if (auxNode.Name == null)
            {
                auxNode.Name = "";
            }
            else
            {
                auxNode.Name = auxNode.Name;
            }
            if (auxNode.Description == null)
            {
                auxNode.Description = "";
            }
            else
            {
                auxNode.Description = auxNode.Description;
            }
            return $"{string.Format("{0,-100}", auxNode.Id.ToString())}{string.Format("{0,-100}", auxNode.Name)}{string.Format("{0,-100}", auxNode.Description)}{string.Format("{0,-100}", auxNode.Price.ToString())}";
        }

        public static Product ConvertToProducto(string dataNode)
        {
            var separateInformation = new List<string>();
            for (int i = 0; i < 4; i++)
            {
                separateInformation.Add(dataNode.Substring(0, 100));
                dataNode = dataNode.Substring(100);
            }
            return new Product() { Id = Convert.ToInt32(separateInformation[0]), Name = separateInformation[1].Trim(), Description = separateInformation[2].Trim(), Price = Convert.ToDouble(separateInformation[3])};
        }

    }

}
