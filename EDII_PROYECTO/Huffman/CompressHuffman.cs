using EDII_PROYECTO.Helpers;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace EDII_PROYECTO.Huffman
{
    public class CompressHuffman
    {
        public void CompresionHuffman(FileStream Archivo)
        {
            string nombreArchivo = Path.GetFileNameWithoutExtension(Archivo.Name);
            var huffman = new Huffman();
            var PropiedadesArchivoActual = new Files();
            PropiedadesArchivoActual.TamanoArchivoDescomprimido = Archivo.Length;
            PropiedadesArchivoActual.NombreArchivoOriginal = Archivo.Name;
            Archivo.Close();
            var direccion = Path.GetFullPath(Archivo.Name);
            int cantidadCaracteres = huffman.Leer(direccion);
            huffman.CrearArbol();
            byte[] encabezado = huffman.CrearEncabezado(cantidadCaracteres);
            nombreArchivo = nombreArchivo.Replace("EXPORTADO_", string.Empty);
            using (FileStream ArchivoComprimir = new FileStream("TusArchivos/IMPORTADO_" + nombreArchivo + ".huff", FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                foreach (var item in encabezado)
                {
                    ArchivoComprimir.WriteByte(item);
                }
                int bufferLength = 80;
                var buffer = new byte[bufferLength];
                string textoCifrado = string.Empty;
                using (var file = new FileStream(Archivo.Name, FileMode.Open))
                {
                    using (var reader = new BinaryReader(file))
                    {
                        while (reader.BaseStream.Position != reader.BaseStream.Length)
                        {
                            buffer = reader.ReadBytes(bufferLength);
                            foreach (var item in buffer)
                            {
                                int posiList;
                                posiList = Data.Instance.ListaCod.FindIndex(x => x.caracter == item);
                                textoCifrado = textoCifrado + Data.Instance.ListaCod.ElementAt(posiList).codigo;
                                if ((textoCifrado.Length / 8) > 0)
                                {
                                    string escribirByte = textoCifrado.Substring(0, 8);
                                    byte byteEscribir = Convert.ToByte(escribirByte, 2);
                                    ArchivoComprimir.WriteByte(byteEscribir);
                                    textoCifrado = textoCifrado.Substring(8);
                                }
                            }
                        }
                        reader.ReadBytes(bufferLength);
                    }
                }
                if (textoCifrado.Length > 0 && (textoCifrado.Length % 8) == 0)
                {
                    byte byteEsc = Convert.ToByte(textoCifrado, 2);
                }
                else if (textoCifrado.Length > 0)
                {
                    textoCifrado = textoCifrado.PadRight(8, '0');
                    byte byteEsc = Convert.ToByte(textoCifrado, 2);
                }
                PropiedadesArchivoActual.TamanoArchivoComprimido = ArchivoComprimir.Length;
                PropiedadesArchivoActual.FactorCompresion = Convert.ToDouble(PropiedadesArchivoActual.TamanoArchivoComprimido) / Convert.ToDouble(PropiedadesArchivoActual.TamanoArchivoDescomprimido);
                PropiedadesArchivoActual.RazonCompresion = Convert.ToDouble(PropiedadesArchivoActual.TamanoArchivoDescomprimido) / Convert.ToDouble(PropiedadesArchivoActual.TamanoArchivoComprimido);
                PropiedadesArchivoActual.PorcentajeReduccion = (Convert.ToDouble(1) - PropiedadesArchivoActual.FactorCompresion).ToString();
                PropiedadesArchivoActual.FormatoCompresion = ".lzw";
                GuaradarCompresiones(PropiedadesArchivoActual, "Huffman");
            }
        }
        public void DescompresionHuffman(FileStream ArchivoImportado)
        {
            string nombreArchivo = Path.GetFileNameWithoutExtension(ArchivoImportado.Name);
            nombreArchivo = nombreArchivo.Replace("IMPORTADO_", string.Empty);
            using (FileStream archivo = new FileStream("TusArchivos/EXPORTADO_" + nombreArchivo + ".txt", FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                Data.Instance.DirectorioHuff = archivo.Name;
                int contador = 0;
                int contadorCarac = 0;
                int CantCaracteres = 0;
                int CaracteresDif = 0;
                string texto = string.Empty;
                string acumula = "";
                byte auxiliar = 0;
                int bufferLength = 80;
                var buffer = new byte[bufferLength];
                string textoCifrado = string.Empty;
                ArchivoImportado.Close();
                using (var file = new FileStream(ArchivoImportado.Name, FileMode.Open))
                {
                    using (var reader = new BinaryReader(file))
                    {
                        while (reader.BaseStream.Position != reader.BaseStream.Length)
                        {
                            buffer = reader.ReadBytes(bufferLength);
                            foreach (var item in buffer)
                            {

                                if (contador == ((CaracteresDif * 2) + 2) && contadorCarac < CantCaracteres)
                                {
                                    texto = Convert.ToString(item, 2);
                                    if (texto.Length < 8)
                                    {
                                        texto = texto.PadLeft(8, '0');
                                    }
                                    acumula = acumula + texto;
                                    int cont = 0;
                                    int canteliminar = 0;
                                    string validacion = "";
                                    foreach (var item2 in acumula)
                                    {
                                        validacion = validacion + item2;
                                        cont++;
                                        if (Data.Instance.DicCarcacteres.ContainsKey(validacion))
                                        {
                                            archivo.WriteByte(Data.Instance.DicCarcacteres[validacion]);
                                            acumula = acumula.Substring(cont);
                                            cont = 0;
                                            contadorCarac++;
                                            canteliminar = cont;
                                            validacion = "";
                                        }
                                    }
                                }
                                if (item != 44)
                                {
                                    byte[] byteCarac = { item };
                                    texto = texto + Encoding.ASCII.GetString(byteCarac);
                                }
                                if (item == 44 && contador > 1 && contador < ((CaracteresDif * 2) + 2))
                                {
                                    if (item == 44 && contador % 2 == 0)
                                    {
                                        auxiliar = Convert.ToByte(texto, 2);
                                        texto = string.Empty;
                                        contador++;
                                    }
                                    else if (contador % 2 != 0 && item == 44)
                                    {
                                        Data.Instance.DicCarcacteres.Add(texto, auxiliar);
                                        texto = string.Empty;
                                        contador++;
                                    }
                                }
                                else
                                {
                                    if (item == 44 && contador == 0)
                                    {
                                        CantCaracteres = int.Parse(texto);
                                        texto = string.Empty;
                                        contador++;
                                    }
                                    else if (item == 44 && contador == 1)
                                    {
                                        CaracteresDif = int.Parse(texto);
                                        texto = string.Empty;
                                        contador++;
                                    }
                                }
                            }
                        }
                        reader.ReadBytes(bufferLength);
                    }
                }
            };
            Data.Instance.DicCarcacteres.Clear();
        }
        public void GuaradarCompresiones(Files Archivo, string tipo)
        {
            string ArchivoMapeo = "TusArchivos/" + tipo;
            string archivoLeer = ArchivoMapeo + Path.GetFileName("ListaCompresiones");
            using (var writer = new StreamWriter(archivoLeer, true))
            {
                if (!(Archivo.TamanoArchivoComprimido <= 0 && Archivo.TamanoArchivoDescomprimido <= 0))
                {
                    writer.WriteLine(Archivo.NombreArchivoOriginal + "|" + Archivo.TamanoArchivoDescomprimido + "|" + Archivo.TamanoArchivoComprimido + "|" + Archivo.FactorCompresion + "|" + Archivo.RazonCompresion + "|" + Archivo.PorcentajeReduccion + "|" + Archivo.FormatoCompresion);
                }
            }
        }
    }
}
