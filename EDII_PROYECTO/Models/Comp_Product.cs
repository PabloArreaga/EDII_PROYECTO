using System;
using System.Collections.Generic;

namespace EDII_PROYECTO.Models
{
    public class Comp_Product : IComparable
    {
        public int _id { get; set; }
        public string _name { get; set; }
        public double _price { get; set; }

        public int CompareTo(object obj)
        {
            return this._id.CompareTo(((Comp_Product)obj)._id);
        }

        public static string ConverttToString(object newObj)
        {
            var currentNode = (Comp_Product)newObj;
            currentNode._name = currentNode._name == null ? string.Empty : currentNode._name;
            return $"{string.Format("{0,-100}", currentNode._id.ToString())}{string.Format("{0,-100}", currentNode._name)}{string.Format("{0,-100}", currentNode._price.ToString())}";
        }

        public static Comp_Product ConvertToObject(string data)
        {
            var dataSeparate = new List<string>();
            for (int i = 0; i < 3; i++)
            {
                dataSeparate.Add(data.Substring(0, 100));
                data = data.Substring(100);
            }

            var price = 0.00;
            double.TryParse(dataSeparate[2].Trim(), out price);

            return new Comp_Product()
            {
                _id = Convert.ToInt32(dataSeparate[0].Trim()),
                _name = dataSeparate[1].Trim(),
                _price = price
            };
        }

        public static Comp_Product Modify(object data, string[] recientData)
        {
            var startData = (Comp_Product)data;
            startData._name = recientData[0] == null ? startData._name : recientData[0];
            startData._price = recientData[1] == null ? startData._price : Convert.ToDouble(recientData[1]);
            return startData;
        }


    }
}
