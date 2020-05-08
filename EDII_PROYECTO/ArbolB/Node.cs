using EDII_PROYECTO.Helpers;
using EDII_PROYECTO.Models;
using System;
using System.Collections.Generic;
using System.IO;
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
            if (dad == 0)
            {
                numberValues = (4 * (Data.Instance.grade - 1)) / 3;
            }
            else
            {
                numberValues = Data.Instance.grade - 1;
            }
            this.father = dad;
        }
        static string bufferToString(byte[] linea)
        {
            var aux = string.Empty;

            foreach (var letra in linea)
            {
                aux += (char)letra;
            }
            return aux;
        }
        static byte[] stringToBuffer(string linea)
        {
            var aux = new List<byte>();
            foreach (var letra in linea)
            {
                aux.Add((byte)letra);
            }
            return aux.ToArray();
        }
        public static Node<T> StringToNodo(int posicion)
        {
            var cantHijos = ((4 * (Data.Instance.grade- 1)) / 3) + 1;
            var cantCaracteres = 8 + (4 * cantHijos) + (lenght * (cantHijos - 1));
            //Lee la linea de archivo de texto que contiene el nodo
            var buffer = new byte[cantCaracteres];
            using (var fs = new FileStream(Data.Instance.adress, FileMode.OpenOrCreate))
            {
                fs.Seek((posicion - 1) * cantCaracteres + 15, SeekOrigin.Begin);
                fs.Read(buffer, 0, cantCaracteres);
            }
            var nodeString = bufferToString(buffer);//Divide los valores para llenar el nodo que se va a utilizar
            var values = new List<string>();
            for (int i = 0; i < cantHijos + 2; i++)
            {
                values.Add(nodeString.Substring(0, 4));
                nodeString = nodeString.Substring(4);
            }
            for (int i = 0; i < cantHijos - 1; i++)
            {
                values.Add(nodeString.Substring(0, lenght));
                nodeString = nodeString.Substring(lenght);
            }
            var NodoSalida = new Node<T>(Convert.ToInt32(values[1]));
            NodoSalida.index = Convert.ToInt32(values[0]);
            for (int i = 2; i < (2 + cantHijos); i++)
            {
                if (values[i].Trim() != "-")
                {
                    NodoSalida.children.Add(Convert.ToInt32(values[i]));
                }
            }
            for (int i = (2 + cantHijos); i < (1 + (2 * cantHijos)); i++)
            {
                if (values[i].Trim() != "-")
                {
                    NodoSalida.values.Add((T)Data.Instance.getNode.DynamicInvoke(values[i]));
                }
            }
            return NodoSalida;
        }
    }
}
