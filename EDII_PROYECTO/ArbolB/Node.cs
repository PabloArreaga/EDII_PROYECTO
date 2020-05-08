using EDII_PROYECTO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EDII_PROYECTO.ArbolB
{
    public class Node<T>
    {
        public int index;
        public int father;
        public int numberValues;
        public List<int> children =new List<int>();
        public List<T> values = new List<T>();
        static int lenght = 500;
        public Node(int dad)
        {
            // faltan condicionales
            this.father = dad;   
        }
    }
}
