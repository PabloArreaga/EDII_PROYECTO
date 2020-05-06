using EDII_PROYECTO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EDII_PROYECTO.ArbolB
{
    public class Node
    {
        public int id { get; set; }
        public Node padre { get; set; }
        public Node[] hijos { get; set; }
        public List<Product> values { get; set; }
        public static int grado = 7;//{ get; set; }
        public Node()
        {
            hijos = new Node[grado];
            valores = new List<Product>();
        }
        public int MAX()
        {
            int max = (4 * (grado - 1) / 3);
            if (max % 2 != 0)
            {
                max = max + 1;
            }
            return max;
        }
        public bool EsHoja => hijos == null;
        public bool Minimo => valores.Count == ((2 * grado) - 1) / 3;
        public bool Maximo => valores.Count == grado - 1;
        public bool MaximoRaiz => valores.Count == MAX();
    }
}
