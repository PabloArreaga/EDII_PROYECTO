using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EDII_PROYECTO.Models
{
	public class Comp_Store_Product : IComparable
    {
        //_id = _idStore-_idProduct 
        public int _idStore { get; set; }
        public int _idProduct { get; set; }
        public int _stock { get; set; }

        public int CompareTo(object obj)
        {
            return $"{this._idStore}-{this._idProduct}".CompareTo($"{((Comp_Store_Product)obj)._idStore}-{((Comp_Store_Product)obj)._idProduct}");
        }

        public static string ConvertToString(object newObj)
        {
            var currentNode = (Comp_Store_Product)newObj;
            return $"{string.Format("{0,-100}", currentNode._idStore.ToString())}{string.Format("{0,-100}", currentNode._idProduct.ToString())}{string.Format("{0,-100}", currentNode._stock.ToString())}";
        }

        public static Comp_Store_Product ConvertToObject(string data)
        {
            var separateData = new List<string>();
            for (int i = 0; i < 3; i++)
            {
                separateData.Add(data.Substring(0, 100));
                data = data.Substring(100);
            }

            var price = 0;
            int.TryParse(separateData[2], out price);

            return new Comp_Store_Product()
            {
                _idStore = Convert.ToInt32(separateData[0].Trim()),
                _idProduct = Convert.ToInt32(separateData[1].Trim()),
                _stock = price
            };
        }

        public static Comp_Store_Product Modify(object data, string[] freshInfo)
        {
            var startDta = (Comp_Store_Product)data;
            startDta._stock = Convert.ToInt32(freshInfo[0]);
            return startDta;
        }
    }
}
