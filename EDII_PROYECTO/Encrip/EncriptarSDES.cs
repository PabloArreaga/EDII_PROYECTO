using EDII_PROYECTO.Helpers;
using EDII_PROYECTO.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EDII_PROYECTO.Encrip
{
    public class EncriptarSDES
    {

        public List<int> LlaveUsuario = new List<int>();// llenar con clave que manda el usuario
        public List<int> P10 = new List<int>(new List<int> { 8, 5, 3, 7, 9, 2, 6, 0, 1, 4 });
        public List<int> P8 = new List<int>(new List<int> { 7, 9, 3, 5, 8, 2, 1, 6 });
        public List<int> P4 = new List<int>(new List<int> { 0, 3, 2, 1 });
        public List<int> EP = new List<int>(new List<int> { 0, 1, 3, 2, 3, 2, 1, 0 });
        public List<int> IP = new List<int>(new List<int> { 6, 3, 5, 7, 2, 0, 1, 4 });
        public List<int> Swap = new List<int>(new List<int> { 4, 5, 6, 7, 0, 1, 2, 3 });
        public List<int> IpInversa = new List<int>(new List<int> { 5, 6, 4, 1, 7, 2, 0, 3 });

        public void ConvertirBinario(int LlaveBinaria)
        {
            //Convierte el numero a binario
            string binarioLlaveDiez = Convert.ToString(LlaveBinaria, 2);
            if (binarioLlaveDiez.Length < 10)
            {
                binarioLlaveDiez = binarioLlaveDiez.PadLeft(10, '0');
            }
            var listaGenerado = new List<int>();
            Data.Instance.DatosGenerados.Add(new tipo
            {
                valorGen = listaGenerado
            });
            int cont = 0;
            foreach (var item in binarioLlaveDiez)
            {
                Data.Instance.DatosGenerados.ElementAt(0).valorGen.Add(int.Parse(Convert.ToString(item)));
                cont++;
            }
            GenerarLlaves();
        }
        private void GenerarLlaves()
        {
            Data.Instance.ClavesParaLlave.Add( new tipo { valorGen = LlaveUsuario} );
            Data.Instance.ClavesParaLlave.Add(new tipo { valorGen = P10 });
            Data.Instance.ClavesParaLlave.Add(new tipo { valorGen = P8 });
            Data.Instance.ClavesParaLlave.Add(new tipo { valorGen = P4 });
            Data.Instance.ClavesParaLlave.Add(new tipo { valorGen = EP });
            Data.Instance.ClavesParaLlave.Add(new tipo { valorGen = IP });
            Data.Instance.ClavesParaLlave.Add(new tipo { valorGen = Swap });
            Data.Instance.ClavesParaLlave.Add(new tipo { valorGen = IpInversa });

            int cont = 0;
            while (cont < 5)
            {
                var ListaSerie = new List<int>();
                Data.Instance.DatosGenerados.Add(new tipo
                {
                    valorGen = ListaSerie
                });
                cont++;
            }
            //La clave del usuario es (0)
            //Generacion de P10 (1)
            foreach (var item in Data.Instance.ClavesParaLlave.ElementAt(0).valorGen)
            {
                Data.Instance.DatosGenerados.ElementAt(1).valorGen.Add(Data.Instance.DatosGenerados.ElementAt(0).valorGen.ElementAt(int.Parse(Convert.ToString(item))));
            }
            //LS1 (2)
            for (int i = 0; i < 4; i++)
            {
                Data.Instance.DatosGenerados.ElementAt(2).valorGen.Add(Data.Instance.DatosGenerados.ElementAt(1).valorGen.ElementAt(i + 1));
            }
            Data.Instance.DatosGenerados.ElementAt(2).valorGen.Add(Data.Instance.DatosGenerados.ElementAt(1).valorGen.ElementAt(0));
            for (int i = 5; i < 9; i++)
            {
                Data.Instance.DatosGenerados.ElementAt(2).valorGen.Add(Data.Instance.DatosGenerados.ElementAt(1).valorGen.ElementAt(i + 1));
            }
            Data.Instance.DatosGenerados.ElementAt(2).valorGen.Add(Data.Instance.DatosGenerados.ElementAt(1).valorGen.ElementAt(5));
            //Generacion de P8-LS1 (3) = k1
            foreach (var item in Data.Instance.ClavesParaLlave.ElementAt(1).valorGen)
            {
                Data.Instance.DatosGenerados.ElementAt(3).valorGen.Add(Data.Instance.DatosGenerados.ElementAt(2).valorGen.ElementAt(int.Parse(Convert.ToString(item))));
            }
            //LS2 (4)
            for (int i = 0; i < 3; i++)
            {
                Data.Instance.DatosGenerados.ElementAt(4).valorGen.Add(Data.Instance.DatosGenerados.ElementAt(2).valorGen.ElementAt(i + 2));
            }
            Data.Instance.DatosGenerados.ElementAt(4).valorGen.Add(Data.Instance.DatosGenerados.ElementAt(2).valorGen.ElementAt(0));
            Data.Instance.DatosGenerados.ElementAt(4).valorGen.Add(Data.Instance.DatosGenerados.ElementAt(2).valorGen.ElementAt(1));
            for (int i = 5; i < 8; i++)
            {
                Data.Instance.DatosGenerados.ElementAt(4).valorGen.Add(Data.Instance.DatosGenerados.ElementAt(2).valorGen.ElementAt(i + 2));
            }
            Data.Instance.DatosGenerados.ElementAt(4).valorGen.Add(Data.Instance.DatosGenerados.ElementAt(2).valorGen.ElementAt(5));
            Data.Instance.DatosGenerados.ElementAt(4).valorGen.Add(Data.Instance.DatosGenerados.ElementAt(2).valorGen.ElementAt(6));
            //Generacion de P8-LS2 (5) = k2
            foreach (var item in Data.Instance.ClavesParaLlave.ElementAt(1).valorGen)
            {
                Data.Instance.DatosGenerados.ElementAt(5).valorGen.Add(Data.Instance.DatosGenerados.ElementAt(4).valorGen.ElementAt(int.Parse(Convert.ToString(item))));
            }
        }
		public byte CifrarODescifrar(int primera, int segundo, string caracter, int paso)
		{
			int llave = 0;
			// cuando es la primera vez el byte pasa al IP
			if (paso == 1)
			{
				llave = primera;
				char[] original = caracter.ToCharArray();
				caracter = "";
				foreach (var item in Data.Instance.ClavesParaLlave.ElementAt(4).valorGen)
				{
					caracter = caracter + Convert.ToString(original[item]);
				}
			}
			else
			{
				llave = segundo;
			}
			// El dato recibido se pasa a un arreglo
			string[] ip = new string[8];
			int conteo = 0;
			foreach (var item in caracter)
			{
				ip[conteo] = Convert.ToString(item);
				conteo++;
			}
			// EP_B2(Expandir y permutar bloque 2 de ip)
			string[] epb2 = new string[8];
			conteo = 0;
			foreach (var item in Data.Instance.ClavesParaLlave.ElementAt(3).valorGen)
			{
				epb2[conteo] = Convert.ToString(ip[item + 4]);
				conteo++;
			}
			// EP_B2 XOR K (k se determina la posicion al inicio del metodo)
			string[] primerXOR = new string[8];
			conteo = 0;
			foreach (var item in Data.Instance.DatosGenerados.ElementAt(llave).valorGen)
			{
				if (Convert.ToString(item) == epb2[conteo])
				{
					primerXOR[conteo] = Convert.ToString(0);
				}
				else
				{
					primerXOR[conteo] = Convert.ToString(1);
				}
				conteo++;
			}
			// Se buscan las posiciones en SBox
			string cFila0 = primerXOR[0] + primerXOR[3];
			string cColumna0 = primerXOR[1] + primerXOR[2];
			string cFila1 = primerXOR[4] + primerXOR[7];
			string cColumna1 = primerXOR[5] + primerXOR[6];
			int f0 = int.Parse(Convert.ToString(Convert.ToByte(cFila0, 2)));
			int c0 = int.Parse(Convert.ToString(Convert.ToByte(cColumna0, 2)));
			int f1 = int.Parse(Convert.ToString(Convert.ToByte(cFila1, 2)));
			int c1 = int.Parse(Convert.ToString(Convert.ToByte(cColumna1, 2)));
			string valorMatriz = Data.Instance.SBox0[f0, c0] + Data.Instance.SBox1[f1, c1];
			// Se realiza P4
			conteo = 0;
			char[] s0s1 = valorMatriz.ToCharArray();
			string[] p4 = new string[4];
			foreach (var item in Data.Instance.ClavesParaLlave.ElementAt(2).valorGen)
			{
				p4[conteo] = Convert.ToString(s0s1[item]);
				conteo++;
			}
			// P4 XOR B1 (bloque 1 de ip)
			conteo = 0;
			string[] segundoXOR = new string[4];
			string[] unionXORb2 = new string[8]; // vamos a aprovechar el ciclo para agrgar datos al arreglo; se desarrolla la union entre el 2 bloque del ip con el resultados del XOR
			foreach (var item in p4)
			{
				if (item == ip[conteo])
				{
					segundoXOR[conteo] = "0";
					unionXORb2[conteo] = "0";
				}
				else
				{
					segundoXOR[conteo] = "1";
					unionXORb2[conteo] = "1";
				}
				conteo++;
			}
			// Se termina de llenar la union
			for (int i = 4; i < 8; i++)
			{
				unionXORb2[i] = ip[i];
			}
			// Se realiza SWAP si es la primera vez
			if (paso == 1)
			{
				string swap = "";
				foreach (var item in Data.Instance.ClavesParaLlave.ElementAt(5).valorGen)
				{
					swap = swap + unionXORb2[item];
				}
				return CifrarODescifrar(primera, segundo, swap, 2);
			}
			else
			{
				string ipinverso = "";
				foreach (var item in Data.Instance.ClavesParaLlave.ElementAt(6).valorGen)
				{
					ipinverso = ipinverso + unionXORb2[item];
				}
				byte byteCifrado = Convert.ToByte(ipinverso, 2);
				return byteCifrado;
			}
		}
	}
}
