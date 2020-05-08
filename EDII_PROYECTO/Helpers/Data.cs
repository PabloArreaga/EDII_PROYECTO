﻿using EDII_PROYECTO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public List<tipo> ClavesParaLlave = new List<tipo>();
        public List<tipo> DatosGenerados = new List<tipo>();//Lista clase
        public int grade;
        public Delegate getNode;
        public Delegate getText;
        public string adress;
        public int key;
    }
}