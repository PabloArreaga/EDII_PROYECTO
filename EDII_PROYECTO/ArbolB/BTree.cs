using System;
using System.Collections.Generic;
using System.Linq;
using EDII_PROYECTO.Helpers;
using System.IO;
using System.Text;

namespace EDII_PROYECTO.ArbolB
{
    public class BTree<T> where T : IComparable
    {
        public static void Create(string nameFile, Delegate gNode, Delegate gText)
        {
            var grade = 7;
            Data.Instance.adress = $"Database\\{nameFile}.txt";

            if (!Directory.Exists("Database"))
            {
                Directory.CreateDirectory("Database");
            }
            if (!File.Exists(Data.Instance.adress))
            {
                string start = $"0000{grade.ToString("0000;-0000")}|0000|0001|";
                File.WriteAllText(Data.Instance.adress, start);
            }
            Data.Instance.getNode = gNode;
            Data.Instance.getText = gText;
        }

        static int[] Header(int[] header = null)
        {
            var buffer = new byte[15];
            using (var fileS = new FileStream(Data.Instance.adress, FileMode.OpenOrCreate))
            {
                if (header == null)
                {
                    fileS.Seek(4, SeekOrigin.Begin);
                    fileS.Read(buffer, 0, 15);
                    var values = Encoding.UTF8.GetString(buffer).Split('|');

                    return new int[3] { Convert.ToInt32(values[0]), Convert.ToInt32(values[1]), Convert.ToInt32(values[2]) };
                }

                var line = string.Empty;
                foreach (var item in header)
                {
                    line = $"{line}{item.ToString("0000;-0000")}|";
                }
                fileS.Seek(4, SeekOrigin.Begin);
                fileS.Write(Encoding.UTF8.GetBytes(line.ToCharArray()), 0, 15);
            }
            return null;
        }

        public static void ValidateIncert(T data)
        {
            var auxHeader = Header();
            Data.Instance.grade = auxHeader[0];

            if (auxHeader[1] == 0)
            {
                auxHeader[1]++;
                auxHeader[2]++;

                var ausNode = new Node<T>(0) { index = 1, values = new List<T> { data }, children = new List<int>() };
                ausNode.ConvertNodetoString();

                Header(auxHeader);
            }
            else
            {
                var move = false;
                var moveSon = false;
                var flagFirst = true;
                var son = 0;
                var positionSon = 0;
                Insert(auxHeader[1], ref data, ref son, ref positionSon, ref move, ref moveSon, ref flagFirst);
            }
        }

        static void Insert(int currentIndex, ref T data, ref int son, ref int positionSon, ref bool flagCarry, ref bool flagSon, ref bool flagFirst)
        {
            var header = Header();
            var currentNode = Node<T>.ConvertToNodo(currentIndex);

            var position = 0;
            if (currentNode.children.Count == 0)
            {
                while (position < currentNode.values.Count && currentNode.values[position].CompareTo(data) == -1)
                {
                    position++;
                }
                currentNode.values.Insert(position, data);
            }
            else
            {
                while (position < currentNode.values.Count && currentNode.values[position].CompareTo(data) == -1)
                {
                    position++;
                }
                Insert(currentNode.children[position], ref data, ref son, ref positionSon, ref flagCarry, ref flagSon, ref flagFirst);
            }

            if (flagCarry)
            {
                position = 0;
                currentNode = Node<T>.ConvertToNodo(currentNode.index);
                while (position < currentNode.values.Count && currentNode.values[position].CompareTo(data) == -1)
                {
                    position++;
                }
                currentNode.values.Insert(position, data);
                if (flagSon)
                {
                    currentNode.children.Insert(positionSon, son);
                    flagSon = false;
                }
                flagCarry = false;
            }

            if (currentNode.values.Count == currentNode.numberValues + 1)
            {
                if (currentNode.children.Count == 0 && currentNode.father != 0)
                {
                    var father = Node<T>.ConvertToNodo(currentNode.father);
                    var indexCurrent = father.children.IndexOf(currentNode.index);
                    var indexBrother = Rotate(father, indexCurrent, header);
                    T auxData = currentNode.values[0];
                    var carry = false;

                    if (indexBrother < indexCurrent)
                    {
                        var dataIndex = indexCurrent == 0 ? 0 : indexCurrent - 1;
                        for (int i = indexCurrent; i >= indexBrother; i--)
                        {
                            if (father.children[i] != currentNode.index)
                            {
                                currentNode = Node<T>.ConvertToNodo(father.children[i]);
                            }
                            if (carry)
                            {
                                father.values.Insert(dataIndex, auxData);
                                auxData = father.values[dataIndex + 1];
                                father.values.RemoveAt(dataIndex + 1);
                                father.ConvertNodetoString();
                                currentNode.values.Add(auxData);
                                auxData = currentNode.values[0];
                                if (i != indexBrother)
                                {
                                    currentNode.values.RemoveAt(0);
                                }
                                dataIndex--;
                            }
                            else
                            {
                                auxData = currentNode.values[0];
                                currentNode.values.RemoveAt(0);
                            }

                            currentNode.ConvertNodetoString();
                            carry = true;
                        }
                    }
                    else if (indexBrother > indexCurrent)
                    {
                        var indiceDato = indexCurrent == father.children.Count() - 1 ? indexCurrent - 1 : indexCurrent;
                        for (int i = indexCurrent; i <= indexBrother; i++)
                        {
                            if (father.children[i] != currentNode.index)
                            {
                                currentNode = Node<T>.ConvertToNodo(father.children[i]);
                            }
                            if (carry)
                            {
                                father.values.Insert(indiceDato, auxData);
                                auxData = father.values[indiceDato + 1];
                                father.values.RemoveAt(indiceDato + 1);
                                father.ConvertNodetoString();
                                currentNode.values.Insert(0, auxData);
                                auxData = currentNode.values[currentNode.values.Count - 1];
                                if (i != indexBrother)
                                {
                                    currentNode.values.RemoveAt(currentNode.values.Count - 1);
                                }
                                indiceDato++;
                            }
                            else
                            {
                                auxData = currentNode.values[currentNode.values.Count - 1];
                                currentNode.values.RemoveAt(currentNode.values.Count - 1);
                            }

                            currentNode.ConvertNodetoString();
                            carry = true;
                        }
                    }
                    else
                    {
                        var indexData = indexCurrent == 0 ? 0 : indexCurrent - 1;
                        indexBrother = indexCurrent - 1 >= 0 ? indexCurrent - 1 : indexCurrent + 1;
                        var brother = Node<T>.ConvertToNodo(father.children[indexBrother]);
                        var auxList = new List<T>();
                        var auxNode = new Node<T>(father.index) { index = header[2], values = new List<T>(), children = new List<int>() };

                        if (indexCurrent - 1 == indexBrother)
                        {
                            foreach (var item in brother.values)
                            {
                                auxList.Add(item);
                            }
                            auxList.Add(father.values[indexData]);
                            foreach (var item in currentNode.values)
                            {
                                auxList.Add(item);
                            }
                            var quantityDivided = (auxList.Count - 2) / 3;

                            auxNode.values.AddRange(auxList.GetRange(0, quantityDivided));
                            auxList.RemoveRange(0, quantityDivided);

                            father.values.RemoveAt(indexData);
                            father.values.Insert(indexData, auxList[0]);
                            auxList.RemoveAt(0);

                            brother.values.Clear();
                            brother.values.AddRange(auxList.GetRange(0, quantityDivided));
                            auxList.RemoveRange(0, quantityDivided);

                            father.values.Insert(indexData + 1, auxList[0]);
                            auxList.RemoveAt(0);

                            currentNode.values.Clear();
                            currentNode.values.AddRange(auxList.GetRange(0, quantityDivided));

                            indexBrother = indexCurrent - 1 >= 0 ? indexCurrent - 1 : indexCurrent + 2;
                            father.children.Insert(indexBrother, auxNode.index);
                        }
                        else
                        {
                            foreach (var item in currentNode.values)
                            {
                                auxList.Add(item);
                            }
                            auxList.Add(father.values[indexData]);
                            foreach (var item in brother.values)
                            {
                                auxList.Add(item);
                            }
                            var dividedQ = (auxList.Count - 2) / 3;

                            currentNode.values.Clear();
                            currentNode.values.AddRange(auxList.GetRange(0, dividedQ));
                            auxList.RemoveRange(0, dividedQ);

                            father.values.RemoveAt(indexData);
                            father.values.Insert(indexData, auxList[0]);
                            auxList.RemoveAt(0);

                            brother.values.Clear();
                            brother.values.AddRange(auxList.GetRange(0, dividedQ));
                            auxList.RemoveRange(0, dividedQ);

                            father.values.Insert(indexData + 1, auxList[0]);
                            auxList.RemoveAt(0);

                            auxNode.values.AddRange(auxList.GetRange(0, dividedQ));

                            father.children.Insert(indexBrother + 1, auxNode.index);
                        }

                        if (father.values.Count > father.numberValues)
                        {
                            data = father.values[0];
                            father.values.RemoveAt(0);
                            positionSon = father.children.IndexOf(auxNode.index);
                            father.children.RemoveAt(positionSon);
                            son = auxNode.index;
                            flagCarry = true;
                            flagSon = true;
                        }

                        auxNode.ConvertNodetoString();
                        father.ConvertNodetoString();
                        brother.ConvertNodetoString();
                        header[2]++;
                        Header(header);
                    }
                }
                else
                {
                    header = Header();
                    var positionMedia = currentNode.values.Count % 2 == 0 ? (currentNode.values.Count - 1) / 2 : currentNode.values.Count / 2;
                    var dadBrother = currentNode.father == 0 ? header[2] + 1 : currentNode.father;
                    var nodeBrother = new Node<T>(dadBrother) { index = header[2], values = currentNode.values.GetRange(0, positionMedia) };

                    header[2]++;

                    if (currentNode.children.Count != 0)
                    {
                        nodeBrother.children = currentNode.children.GetRange(0, positionMedia + 1);
                        currentNode.children.RemoveRange(0, positionMedia + 1);

                        foreach (var item in nodeBrother.children)
                        {
                            var nodeSon = Node<T>.ConvertToNodo(item);

                            nodeSon.father = nodeBrother.index;
                            nodeSon.ConvertNodetoString();
                        }
                    }

                    if (currentNode.father == 0)
                    {
                        var nodeFather = new Node<T>(0) { values = new List<T> { currentNode.values[positionMedia] }, children = new List<int> { nodeBrother.index, currentNode.index }, index = header[2] };
                        header[1] = header[2];
                        header[2]++;
                        nodeFather.ConvertNodetoString();
                        nodeBrother.father = nodeFather.index;
                        currentNode.father = nodeFather.index;
                    }
                    else
                    {
                        var fatherNode = Node<T>.ConvertToNodo(currentNode.father);

                        data = currentNode.values[positionMedia];
                        position = 0;
                        while (position < fatherNode.values.Count && fatherNode.values[position].CompareTo(data) == -1)
                        {
                            position++;
                        }
                        fatherNode.values.Insert(position, data);
                        fatherNode.children.Insert(fatherNode.children.IndexOf(currentNode.index), nodeBrother.index);

                        if (fatherNode.values.Count > fatherNode.numberValues)
                        {
                            data = fatherNode.values[0];
                            fatherNode.values.RemoveAt(0);
                            positionSon = fatherNode.children.IndexOf(nodeBrother.index);
                            fatherNode.children.RemoveAt(positionSon);
                            son = nodeBrother.index;
                            flagCarry = true;
                            flagSon = true;
                        }
                        fatherNode.ConvertNodetoString();
                    }
                    currentNode.values.RemoveRange(0, positionMedia + 1);
                    currentNode.ConvertNodetoString();
                    nodeBrother.ConvertNodetoString();
                    Header(header);
                }
            }
            if (flagFirst)
            {
                currentNode.ConvertNodetoString();
                flagFirst = false;
            }
        }

        static int Rotate(Node<T> father, int indexList, int[] header)
        {
            var auxNode = new Node<T>(1);

            if (indexList - 1 >= 0)
            {
                auxNode = Node<T>.ConvertToNodo(father.children[indexList - 1]);
                if (auxNode.values.Count < auxNode.numberValues)
                {
                    return indexList - 1;
                }
            }
            if (indexList + 1 < father.children.Count)
            {
                auxNode = Node<T>.ConvertToNodo(father.children[indexList + 1]);
                if (auxNode.values.Count < auxNode.numberValues)
                {
                    return indexList + 1;
                }
            }
            if (indexList - 2 >= 0)
            {
                for (int i = indexList - 2; i >= 0; i--)
                {
                    auxNode = Node<T>.ConvertToNodo(father.children[i]);
                    if (auxNode.values.Count < auxNode.numberValues)
                    {
                        return i;
                    }
                }
            }
            if (indexList + 2 < father.children.Count)
            {
                for (int i = indexList + 2; i < father.children.Count; i++)
                {
                    auxNode = Node<T>.ConvertToNodo(father.children[i]);
                    if (auxNode.values.Count < auxNode.numberValues)
                    {
                        return i;
                    }
                }
            }
            return indexList;
        }

        public static List<T> Traversal(T data, int option = 0)
        {
            var auxTraversal = new List<T>();
            var header = Header();
            Data.Instance.grade = header[0];
            if (header[1] != 0)
            {
                var root = Node<T>.ConvertToNodo(header[1]);
                var flagNest = true;

                switch (option)
                {
                    case 0:
                        TraversalsIn(root);
                        break;
                    case 1:
                        Search(root, data, ref flagNest);
                        break;
                }
            }

            return auxTraversal;
        }

        static List<T> auxTraversal;
        static void TraversalsIn(Node<T> currentNode)
        {
            if (currentNode.children.Count == 0)
            {
                foreach (var item in currentNode.values)
                {
                    auxTraversal.Add(item);
                }
            }
            else
            {
                var positionData = 1;
                foreach (var item in currentNode.children)
                {
                    TraversalsIn(Node<T>.ConvertToNodo(item));
                    if (positionData < currentNode.children.Count)
                    {
                        auxTraversal.Add(currentNode.values[positionData - 1]);
                        positionData++;
                    }
                }
            }
        }

        static void Search(Node<T> currentNode, T data, ref bool flagNext)
        {
            var position = 0;
            while (flagNext && position < currentNode.values.Count && (currentNode.values[position].CompareTo(data) == -1 || currentNode.values[position].CompareTo(data) == 0))
            {
                if (currentNode.values[position].CompareTo(data) == 0)
                {
                    flagNext = false;
                    auxTraversal.Add(currentNode.values[position]);
                }
                position++;
            }
            if (flagNext && currentNode.children.Count != 0)
            {
                Search(Node<T>.ConvertToNodo(currentNode.children[position]), data, ref flagNext);
            }
        }

        public static void ValidateEdit(T data, string[] newText, Delegate edit)
        {
            var header = Header();
            Data.Instance.grade = header[0];
            if (header[1] != 0)
            {
                var root = Node<T>.ConvertToNodo(header[1]);
                var flagNext = true;
                ValidateEdit(root, data, newText, edit, ref flagNext);
            }
        }

        static void ValidateEdit(Node<T> currentNode, T data, string[] newtext, Delegate edit, ref bool flagNext)
        {
            var position = 0;
            while (flagNext && position < currentNode.values.Count && (currentNode.values[position].CompareTo(data) == -1 || currentNode.values[position].CompareTo(data) == 0))
            {
                if (currentNode.values[position].CompareTo(data) == 0)
                {
                    flagNext = false;
                    edit.DynamicInvoke(currentNode.values[position], newtext);
                    currentNode.ConvertNodetoString();
                }
                position++;
            }
            if (flagNext && currentNode.children.Count != 0)
            {
                ValidateEdit(Node<T>.ConvertToNodo(currentNode.children[position]), data, newtext, edit, ref flagNext);
            }
        }

        public static void Edit(int valueNew)
        {
            var header = Header();
            Data.Instance.grade = header[0];
            if (header[1] != 0)
            {
                var rootNode = Node<T>.ConvertToNodo(header[1]);
                TraversalsEdit(rootNode, valueNew);
            }
        }

        static void TraversalsEdit(Node<T> currentNode, int valueNew)
        {
            if (currentNode.children.Count == 0)
            {
                var nodeAux = new Node<T>(currentNode.father);
                nodeAux.index = currentNode.index;
                nodeAux.children = currentNode.children;
                foreach (var item in currentNode.values)
                {
                    nodeAux.values.Add(item);
                }
                var lastValue = Data.Instance.key;
                Data.Instance.key = valueNew;
                nodeAux.ConvertNodetoString();
                Data.Instance.key = lastValue;
            }
            else
            {
                var dataPosition = 1;
                var nodeAux = new Node<T>(currentNode.father);
                nodeAux.index = currentNode.index;
                nodeAux.children = currentNode.children;
                foreach (var item in currentNode.children)
                {
                    TraversalsEdit(Node<T>.ConvertToNodo(item), valueNew);
                    if (dataPosition < currentNode.children.Count)
                    {
                        nodeAux.values.Add(currentNode.values[dataPosition - 1]);
                        dataPosition++;
                    }
                }
                var lastValue = Data.Instance.key;
                Data.Instance.key = valueNew;
                nodeAux.ConvertNodetoString();
                Data.Instance.key = lastValue;
            }
        }

        public static int KnowId()
        {
            var buffer = new byte[15];
            var currentId = 0;
            using (var fs = new FileStream(Data.Instance.adress, FileMode.OpenOrCreate))
            {
                fs.Read(buffer, 0, 4);
                currentId = Convert.ToInt32(Encoding.UTF8.GetString(buffer)) + 1;
                fs.Position = 0;
                fs.Write(Encoding.UTF8.GetBytes(currentId.ToString("0000;-0000").ToCharArray()), 0, 4);
            }
            return currentId;
        }


    }
}
