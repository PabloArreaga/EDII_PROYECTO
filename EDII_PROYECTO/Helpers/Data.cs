using EDII_PROYECTO.Huffman;
using EDII_PROYECTO.Models;
using System;
using System.Collections.Generic;

namespace EDII_PROYECTO.Helpers
{
    public class Data
    {
        private static Data _instance = null;
        public static Data Instance
        {
            get
            {
                if (_instance == null) _instance = new Data();
                return _instance;
            }
        }
        public int value;
        public List<CaracterCodigo> ListaCod = new List<CaracterCodigo>();
        public Dictionary<string, byte> DicCarcacteres = new Dictionary<string, byte>();
        public string DirectorioHuff;
        public List<tipo> ClavesParaLlave = new List<tipo>();
        public List<tipo> DatosGenerados = new List<tipo>();
        public string[,] SBox0 = new string[4, 4] { { "01", "00", "11", "10" }, { "11", "10", "01", "00" }, { "00", "10", "01", "11" }, { "11", "01", "11", "10" } };
        public string[,] SBox1 = new string[4, 4] { { "00", "01", "10", "11" }, { "10", "00", "01", "11" }, { "11", "00", "01", "00" }, { "10", "01", "00", "11" } };
        public int grade;
        public Delegate getNode;
        public Delegate getText;
        public string adress;
    }
}
