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
			
		}
	}
}
