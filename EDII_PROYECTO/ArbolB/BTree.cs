using EDII_PROYECTO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EDII_PROYECTO.Helpers;
using System.IO;


namespace EDII_PROYECTO.ArbolB
{
    public class BTree<T> where T : IComparable
    {
        public static void Create(string nameFile, Delegate gNode, Delegate gText)
        {
            var grade = 7;
            Data.Instance.adress = $"Datos\\{nameFile}.txt";

            if (!Directory.Exists("Datos"))
            {
                Directory.CreateDirectory("Datos");
            }
            if (!File.Exists(Data.Instance.adress))
            {
                string start = $"{grade.ToString("0000;-0000")}|0000|0001|";
                File.WriteAllText(Data.Instance.adress, start);
            }
            Data.Instance.getNode = gNode;
            Data.Instance.getText = gText;
        }

        #region Nodo
        private static BTree _instance = null;
        public static BTree Instance
        {
            get
            {
                if (_instance == null) _instance = new BTree();
                return _instance;
            }
        }
        public static int grado = 7;
        public Node raiz = null;
        public int numero = 0;
        public int ID = 1;
        public int Proxima { get; set; }
        static int valor_raiz = ((4 * (grado - 1)) / 3);
        public bool LiberarHoja = true;
        public int validar = 0;

        #endregion

        #region Arbol
        public Product Subir_Elemento(int mitad, List<Product> nodo)
        {
            int num = 0;
            var nuevo_elemento = new Product();
            foreach (var item in nodo)
            {
                if (num == mitad)
                {
                    nuevo_elemento = item;
                    break;
                }
                num++;
            }
            return nuevo_elemento;
        }
        public List<Product> Der(int mitad, List<Product> nodo)
        {
            int num = nodo.Count;
            var nuevo_elemento = new List<Product>();
            foreach (var item in nodo)
            {
                if (num < mitad + 1)
                {
                    nuevo_elemento.Add(item);
                }
                num--;
            }
            return nuevo_elemento;
        }
        public List<Product> Izq(int mitad, List<Product> nodo)
        {
            int num = 0;
            var nuevo_elemento = new List<Product>();
            foreach (var item in nodo)
            {
                if (num < mitad)
                {
                    nuevo_elemento.Add(item);
                }
                num++;
            }
            return nuevo_elemento;
        }
        public void Primeradivision(Node PrimerNodo, int num, Product elementoRaiz)
        {
            int mitad = PrimerNodo.values.Count / 2;
            PrimerNodo.hijos[num] = new Node();
            var Izquierdo = Izq(mitad, PrimerNodo.values);
            PrimerNodo.hijos[num].values = Izquierdo;
            PrimerNodo.hijos[num].id = 2;
            PrimerNodo.hijos[num].padre = PrimerNodo;
            PrimerNodo.hijos[num + 1] = new Node();
            PrimerNodo.hijos[num + 1].id = 3;
            PrimerNodo.hijos[num + 1].padre = PrimerNodo;
            var Derecho = Der(mitad, PrimerNodo.values);
            PrimerNodo.hijos[num + 1].values = Derecho;
            PrimerNodo.values.Clear();
            PrimerNodo.values.Add(elementoRaiz);
            LiberarHoja = false;
        }
        public void Insertar(int num, Product Valores)
        {
            if (raiz == null)
            {
                raiz = new Node();
                raiz.values.Add(Valores);
                raiz.id = ID;
            }
            else
            {
                if (raiz.MaximoRaiz)//full
                {
                    int mitad = raiz.values.Count / 2;
                    var NuevoElmento = Subir_Elemento(mitad, raiz.values);
                    raiz.values.Add(Valores);
                    Primeradivision(raiz, num, NuevoElmento);
                }
                if (LiberarHoja)
                {
                    raiz.values.Add(Valores);
                    raiz.values = raiz.values.OrderBy(x => x.Name).ToList();
                }
                if (validar == 1) // hijos de la raiz
                {
                    if (raiz.values[raiz.values.Count - 1].Name.CompareTo(Valores.Name) < 0) // -1 porque es menor a la raiz
                    {
                        if (raiz.hijos[num + 1].values.Count < grado - 1)
                        {
                            raiz.hijos[num + 1].values.Add(Valores);
                        }
                        else
                        {
                            if (raiz.hijos[num].values.Count < grado - 1) /// && raiz.hijos[2] == null
                            {
                                raiz.hijos[num + 1].values.Add(Valores);
                                var salida = raiz.values[0];
                                raiz.values.RemoveAt(num);
                                raiz.hijos[num].values.Add(salida);
                                raiz.values.Add(raiz.hijos[num + 1].values[0]);
                                raiz.hijos[num + 1].values.RemoveAt(0);
                            }
                            else
                            {
                                raiz.hijos[num + 1].values.Add(Valores);
                                int mitad = (raiz.hijos[num + 1].values.Count) / 2;
                                var nuevo_elemento = Subir_Elemento(mitad, raiz.hijos[num + 1].values);
                                raiz.values.Add(nuevo_elemento);
                                raiz.hijos[num + 2] = new Node();
                                raiz.hijos[num + 2].id = 4;
                                raiz.hijos[num + 2].padre = raiz;
                                var derecho = Der(mitad, raiz.hijos[num + 1].values);
                                derecho.OrderBy(x => x.Name).ToList();
                                var Izquierdo = Izq(mitad, raiz.hijos[num + 1].values);
                                Izquierdo.OrderBy(x => x.Name).ToList();
                                raiz.hijos[num + 2].values = derecho;
                                raiz.hijos[num + 1].values = Izquierdo;
                                numero++;
                            }
                        }
                    }
                }
                if (LiberarHoja == false)
                {
                    validar = 1;
                }
            }
            Recorrido(raiz);
            EscribirArchivo(num + 4);
            Arbollista.Clear();
        }
        #endregion

    }
}
