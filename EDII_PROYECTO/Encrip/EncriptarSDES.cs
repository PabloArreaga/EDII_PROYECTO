﻿using EDII_PROYECTO.Helpers;
using EDII_PROYECTO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace EDII_PROYECTO.Encrip
{
	public class EncriptarSDES
	{
        private static string key1 = string.Empty;
        private static string key2 = string.Empty;

        public static void StartKey()
        {
            if (!Directory.Exists("Datos"))
            {
                Directory.CreateDirectory("Datos");
            }
            if (!File.Exists("Datos//llave.txt"))
            {
                var nuevo = new Random().Next(0, 1023);
                Data.Instance.value = 15;
                var text = Cifrado(nuevo.ToString(), true);
                Data.Instance.value = nuevo;
                File.WriteAllText("Datos//llave.txt", text);
            }
            else
            {
                Data.Instance.value = 15;
                Data.Instance.value = Convert.ToInt32(Cifrado(File.ReadAllText("Datos//llave.txt"), false));
            }
        }

        static string Permute(string key, int type)
        {
            switch (type)
            {
                case 1:
                    return $"{ key[9] }{ key[5] }{ key[4] }{ key[6] }{ key[1] }{ key[7] }{ key[0] }{ key[8] }{ key[3] }{ key[2] }";
                case 2:
                    return $"{ key[2] }{ key[5] }{ key[9] }{ key[7] }{ key[3] }{ key[1] }{ key[0] }{ key[8] }";
                case 3:
                    return $"{ key[3] }{ key[0] }{ key[2] }{ key[1] }";
            }
            return string.Empty;
        }

        static string IP(string key, bool inverse)
        {
            if (!inverse)
            {
                return $"{ key[5] }{ key[2] }{ key[0] }{ key[3] }{ key[7] }{ key[6] }{ key[1] }{ key[4] }";
            }

            return $"{ key[2] }{ key[6] }{ key[1] }{ key[3] }{ key[7] }{ key[0] }{ key[5] }{ key[4] }";
        }

        static string LS(string key, int type)
        {
            if (type == 1)
            {
                return $"{ key[1] }{ key[2] }{ key[3] }{ key[4] }{ key[0] }";
            }

            return $"{ key[2] }{ key[3] }{ key[4] }{ key[0] }{ key[1] }";
        }

        static string XOR(string key1, string key2)
        {
            var aux = string.Empty;

            for (int i = 0; i < key1.Length; i++)
            {
                aux += key1[i] == key2[i] ? '0' : '1';
            }

            return aux;
        }

        static string SW(string key)
        {
            return $"{key.Substring(4)}{key.Substring(0, 4)}";
        }

        static string EP(string key)
        {
            return $"{ key[0] }{ key[3] }{ key[2] }{ key[1] }{ key[2] }{ key[1] }{ key[0] }{ key[3] }";
        }

        static string S01(string key)
        {
            var S0 = new string[4, 4] { { "00", "01", "11", "10" }, { "11", "00", "01", "00" }, { "10", "01", "00", "11" }, { "01", "10", "00", "00" } };
            var S1 = new string[4, 4] { { "00", "10", "11", "00" }, { "11", "00", "01", "01" }, { "10", "10", "00", "10" }, { "10", "00", "11", "00" } };

            return $"{S0[Convert.ToInt32($"{key[0]}{key[3]}", 2), Convert.ToInt32($"{key[1]}{key[2]}", 2)]}{S1[Convert.ToInt32($"{key[4]}{key[7]}", 2), Convert.ToInt32($"{key[5]}{key[6]}", 2)]}";
        }

        static void CreateKey(string key, bool cipher)
        {
            var ResultLS = Permute(key, 1);
            var LS1 = $"{LS(ResultLS.Substring(0, 5), 1)}{LS(ResultLS.Substring(5), 1)}";
            key1 = cipher ? Permute(LS1, 2) : Permute($"{LS(LS1.Substring(0, 5), 2)}{LS(LS1.Substring(5), 2)}", 2);
            key2 = cipher ? Permute($"{LS(LS1.Substring(0, 5), 2)}{LS(LS1.Substring(5), 2)}", 2) : Permute(LS1, 2);
        }

        static string fK(string key, string k)
        {
            var ResultXOR1 = XOR(k, EP(key.Substring(4)));
            var ResultXOR2 = XOR(key.Substring(0, 4), Permute(S01(ResultXOR1), 3));

            return $"{ResultXOR2}{key.Substring(4)}";
        }

        public static string Cifrado(string info, bool cipher)
        {
            var aux = string.Empty;
            if (Data.Instance.value < 1024)
            {
                CreateKey(Convert.ToString(Data.Instance.value, 2).PadLeft(10, '0'), cipher);

                foreach (var character in info)
                {
                    var binCharacter = Convert.ToString((int)character, 2).PadLeft(8, '0');

                    var ResultIP = IP(binCharacter, true);

                    var ResultSW = SW(fK(ResultIP, key1));

                    var ResultIP2 = IP(fK(ResultSW, key2), false);

                    aux += Convert.ToChar(Convert.ToInt32(ResultIP2, 2));
                }
            }
            return aux;
        }
    }
}