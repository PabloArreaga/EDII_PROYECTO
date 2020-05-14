using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EDII_PROYECTO.Encrip;

namespace EDII_PROYECTO.Models
{
    public class Comp_Store : IComparable
    {
        public int _id { get; set; }
        [Required]
        public string _name { get; set; }
        [Required]
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
            return $"{string.Format("{0,-100}", currentNode._id.ToString())}{EncriptarSDES.Cifrado(string.Format("{0,-100}",  currentNode._name), true)}{ EncriptarSDES.Cifrado(string.Format("{0,-100}",currentNode._address),true)}";
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
                _name = EncriptarSDES.Cifrado(dataSeparate[1], false).Trim(),
                _address = EncriptarSDES.Cifrado(dataSeparate[2], false).Trim()
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
