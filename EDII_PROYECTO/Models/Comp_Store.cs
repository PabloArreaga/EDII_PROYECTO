using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EDII_PROYECTO.Models
{
    public class Comp_Store : IComparable
    {
        public int _id { get; set; }
        public string _name { get; set; }
        public string _address { get; set; }

        public int CompareTo(object obj)
        {
            return this._id.CompareTo(((Comp_Store)obj)._id);
        }

        public static string ConvertToString(object newObj)
        {
            var currentNode = (Comp_Store)newObj;
            currentNode._name = currentNode._name == null ? string.Empty : currentNode._name;
            currentNode._address = currentNode._address == null ? string.Empty : currentNode._address;
            return $"{string.Format("{0,-100}", currentNode._id.ToString())}{string.Format("{0,-100}", currentNode._name)}{string.Format("{0,-100}", currentNode._address)}";
        }

        public static Comp_Store ConvertToObject(string data)
        {
            var dataSeparate = new List<string>();
            for (int i = 0; i < 3; i++)
            {
                dataSeparate.Add(data.Substring(0, 100));
                data = data.Substring(100);
            }

            return new Comp_Store()
            {
                _id = Convert.ToInt32(dataSeparate[0].Trim()),
                _name = dataSeparate[1].Trim(),
                _address = dataSeparate[2].Trim()
            };
        }

        public static Comp_Store Modify(object data, string[] recientData)
        {
            var startData = (Comp_Store)data;
            startData._name = recientData[0] == null ? startData._name : recientData[0];
            startData._address = recientData[1] == null ? startData._address : recientData[1];
            return startData;
        }
    }
}
