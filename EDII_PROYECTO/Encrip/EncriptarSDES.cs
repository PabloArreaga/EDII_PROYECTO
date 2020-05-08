using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using EDII_PROYECTO.Models;
using EDII_PROYECTO.Helpers;

namespace EDII_PROYECTO.Encrip
{
    public class EncriptarSDES
    {
		public void ConvertirBinario(Product LlaveBinaria)
		{
			//Convierte el numero a binario
			string binarioLlaveDiez = Convert.ToString(LlaveBinaria.BinarioDiez, 2);
			if (binarioLlaveDiez.Length < 10)
			{
				binarioLlaveDiez = binarioLlaveDiez.PadLeft(10, '0');
			}
			var ListaSerie = new List<int>();
			Data.Instance.DatosGenerados.Add(new tipo
			{
				OrdenSerie = ListaSerie
			});//Revisar lista de clase SERIE
			int cont = 0;
			foreach (var item in binarioLlaveDiez)
			{
				Data.Instance.DatosGenerados.ElementAt(0).OrdenSerie.Add(int.Parse(Convert.ToString(item)));
				cont++;
			}
			GenerarLlaves();
		}
		private void GenerarLlaves()
		{
			int cont = 0;
			while (cont < 5)
			{
				var ListaSerie = new List<int>();
				Data.Instance.DatosGenerados.Add(new tipo
				{
					OrdenSerie = ListaSerie
				});
				cont++;
			}
			//La clave del usuario es (0)
			//Generacion de P10 (1)
			foreach (var item in Data.Instance.ClavesParaLlave.ElementAt(0).OrdenSerie)
			{
				Data.Instance.DatosGenerados.ElementAt(1).OrdenSerie.Add(Data.Instance.DatosGenerados.ElementAt(0).OrdenSerie.ElementAt(int.Parse(Convert.ToString(item))));
			}
			//LS1 (2)
			for (int i = 0; i < 4; i++)
			{
				Data.Instance.DatosGenerados.ElementAt(2).OrdenSerie.Add(Data.Instance.DatosGenerados.ElementAt(1).OrdenSerie.ElementAt(i + 1));
			}
			Data.Instance.DatosGenerados.ElementAt(2).OrdenSerie.Add(Data.Instance.DatosGenerados.ElementAt(1).OrdenSerie.ElementAt(0));
			for (int i = 5; i < 9; i++)
			{
				Data.Instance.DatosGenerados.ElementAt(2).OrdenSerie.Add(Data.Instance.DatosGenerados.ElementAt(1).OrdenSerie.ElementAt(i + 1));
			}
			Data.Instance.DatosGenerados.ElementAt(2).OrdenSerie.Add(Data.Instance.DatosGenerados.ElementAt(1).OrdenSerie.ElementAt(5));
			//Generacion de P8-LS1 (3) = k1
			foreach (var item in Data.Instance.ClavesParaLlave.ElementAt(1).OrdenSerie)
			{
				Data.Instance.DatosGenerados.ElementAt(3).OrdenSerie.Add(Data.Instance.DatosGenerados.ElementAt(2).OrdenSerie.ElementAt(int.Parse(Convert.ToString(item))));
			}
			//LS2 (4)
			for (int i = 0; i < 3; i++)
			{
				Data.Instance.DatosGenerados.ElementAt(4).OrdenSerie.Add(Data.Instance.DatosGenerados.ElementAt(2).OrdenSerie.ElementAt(i + 2));
			}
			Data.Instance.DatosGenerados.ElementAt(4).OrdenSerie.Add(Data.Instance.DatosGenerados.ElementAt(2).OrdenSerie.ElementAt(0));
			Data.Instance.DatosGenerados.ElementAt(4).OrdenSerie.Add(Data.Instance.DatosGenerados.ElementAt(2).OrdenSerie.ElementAt(1));
			for (int i = 5; i < 8; i++)
			{
				Data.Instance.DatosGenerados.ElementAt(4).OrdenSerie.Add(Data.Instance.DatosGenerados.ElementAt(2).OrdenSerie.ElementAt(i + 2));
			}
			Data.Instance.DatosGenerados.ElementAt(4).OrdenSerie.Add(Data.Instance.DatosGenerados.ElementAt(2).OrdenSerie.ElementAt(5));
			Data.Instance.DatosGenerados.ElementAt(4).OrdenSerie.Add(Data.Instance.DatosGenerados.ElementAt(2).OrdenSerie.ElementAt(6));
			//Generacion de P8-LS2 (5) = k2
			foreach (var item in Data.Instance.ClavesParaLlave.ElementAt(1).OrdenSerie)
			{
				Data.Instance.DatosGenerados.ElementAt(5).OrdenSerie.Add(Data.Instance.DatosGenerados.ElementAt(4).OrdenSerie.ElementAt(int.Parse(Convert.ToString(item))));
			}
		}
	}
}
