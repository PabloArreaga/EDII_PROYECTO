using EDII_PROYECTO.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EDII_PROYECTO.Huffman
{
    public class Huffman
    {
        List<Nodo> frecuencias = new List<Nodo>();
        public int Leer(string direccion)
        {
            int noCaracteres = 0;
            const int bufferLength = 80;
            var buffer = new byte[bufferLength];
            var File = direccion;
            using (var Lectura = new FileStream(direccion, FileMode.Open))
            {
                using (var reader = new BinaryReader(Lectura))
                {
                    while (reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        buffer = reader.ReadBytes(bufferLength);

                        foreach (var item in buffer)
                        {
                            noCaracteres++;
                            ConteoDeFrecuencia(item);
                        }
                    }
                    reader.ReadBytes(bufferLength);
                }
            }
            return noCaracteres;
        }
        public void ConteoDeFrecuencia(byte elemento)
        {
            int posicionLista;
            if (frecuencias.Exists(x => x.caracter == elemento))
            {
                posicionLista = frecuencias.FindIndex(x => x.caracter == elemento);

                Nodo prueba = new Nodo();
                prueba = frecuencias.Find(x => x.caracter == elemento);
                frecuencias.RemoveAt(posicionLista);
                frecuencias.Add(new Nodo()
                {
                    caracter = elemento,
                    probabilidad = prueba.probabilidad + 1
                });
            }
            else
            {
                frecuencias.Add(new Nodo()
                {
                    caracter = elemento,
                    probabilidad = 1
                });
            }
        }
        public void CrearArbol()
        {
            List<Nodo> frecuenciasORDEN = new List<Nodo>();
            frecuenciasORDEN = frecuencias.OrderBy(x => x.probabilidad).ToList();
            Tree MiArbol = new Tree();
            MiArbol.EtiquetarNodo(MiArbol.ConstruirArbol(frecuenciasORDEN));
            Data.Instance.ListaCod = MiArbol.ListaCodigos;
        }
        public byte[] CrearEncabezado(int noCaracteres)
        {
            double noElementos = Data.Instance.ListaCod.LongCount();
            string codigo = noCaracteres + "," + noElementos;
            foreach (var cosa in Data.Instance.ListaCod)
            {
                string p = Convert.ToString(cosa.caracter, 2);
                codigo = codigo + "," + p;
                codigo = codigo + "," + cosa.codigo;
            }
            byte[] encabezadoBytes = Encoding.ASCII.GetBytes(codigo + ",");
            return encabezadoBytes;
        }
    }
}
