using EDII_PROYECTO.Helpers;
using System;
using System.Collections.Generic;
using System.IO;

namespace EDII_PROYECTO.ArbolB
{
    public class Node<T>
    {
        public int index;
        public int father;
        public int numberValues;
        public List<int> children = new List<int>();
        public List<T> values = new List<T>();
        static int lenght = 300;
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

        // string
        public static Node<T> ConvertToNodo(int position)
        {
            var numberChildren = ((4 * (Data.Instance.grade - 1)) / 3) + 1;
            var cantCaracteres = 8 + (4 * numberChildren) + (lenght * (numberChildren - 1));
            var buffer = new byte[cantCaracteres];
            using (var file = new FileStream(Data.Instance.adress, FileMode.OpenOrCreate))
            {
                file.Seek((position - 1) * cantCaracteres + 15, SeekOrigin.Begin);
                file.Read(buffer, 0, cantCaracteres);
            }

            var toString = ConverToString(buffer);
            var listValue = new List<string>();
            for (int i = 0; i < numberChildren + 2; i++)
            {
                listValue.Add(toString.Substring(0, 4));
                toString = toString.Substring(4);
            }

            for (int i = 0; i < numberChildren - 1; i++)
            {
                listValue.Add(toString.Substring(0, lenght));
                toString = toString.Substring(lenght);
            }

            var finalNode = new Node<T>(Convert.ToInt32(listValue[1]));

            finalNode.index = Convert.ToInt32(listValue[0]);
            for (int i = 2; i < (2 + numberChildren); i++)
            {
                if (listValue[i].Trim() != "-")
                {
                    finalNode.children.Add(Convert.ToInt32(listValue[i]));
                }
            }

            for (int i = (2 + numberChildren); i < (1 + (2 * numberChildren)); i++)
            {
                if (listValue[i].Trim() != "-")
                {
                    finalNode.values.Add((T)Data.Instance.getNode.DynamicInvoke(listValue[i]));
                }
            }

            return finalNode;
        }
        //buffer
        static string ConverToString(byte[] texttLine)
        {
            var auxText = string.Empty;

            foreach (var item in texttLine)
            {
                auxText += (char)item;
            }
            return auxText;
        }
        //string
        static byte[] ConvertToBuffer(string texttLine)
        {
            var auxTextt = new List<byte>();
            foreach (var item in texttLine)
            {
                auxTextt.Add((byte)item);
            }
            return auxTextt.ToArray();
        }



        public void ConvertNodetoString()
        {
            string childrenString = string.Empty;
            string valuesString = string.Empty;

            var numberChildren = ((4 * (Data.Instance.grade - 1)) / 3) + 1;

            foreach (var item in children)
            {
                childrenString += item.ToString("0000;-0000");
            }
            for (int i = children.Count; i < numberChildren; i++)
            {
                childrenString += string.Format("{0,-4}", "-");
            }

            foreach (var item in values)
            {
                valuesString += Convert.ToString(Data.Instance.getText.DynamicInvoke(item));
            }

            for (int i = values.Count; i < (numberChildren - 1); i++)
            {
                valuesString += string.Format("{0,-300}", "-");
            }
            var textNode = ($"{index.ToString("0000;-0000")}{father.ToString("0000;-0000")}{childrenString}{valuesString}");
            var numberCharacter = 8 + (4 * numberChildren) + (lenght * (numberChildren - 1));
            using (var file = new FileStream(Data.Instance.adress, FileMode.OpenOrCreate))
            {
                file.Seek((index - 1) * numberCharacter + 15, SeekOrigin.Begin);
                file.Write(ConvertToBuffer(textNode), 0, numberCharacter);
            }
        }

    }
}
