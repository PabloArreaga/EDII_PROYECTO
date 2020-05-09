using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EDII_PROYECTO.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static EDII_PROYECTO.Encrip.EncriptarSDES;
using EDII_PROYECTO.Helpers;
using EDII_PROYECTO.ArbolB;
using System.IO;
using EDII_PROYECTO.Huffman;
using Newtonsoft.Json.Linq;

namespace EDII_PROYECTO.Controllers
{
    delegate string ConvertToString(object obj);
    delegate object ConvertToObject(string obj);
    delegate object Modify(object obj, string[] txt);

    [ApiController]
    [Route("api/[controller]")]

    public class GroceryStoreController : ControllerBase
    {
        [Route("Product")]
        [HttpPost]
        public ActionResult postProduct(Comp_Product producto)//Datos principales para el nodo
        {
            if (producto._id > 0 && producto._name != null && producto._price >= 0)
            {
                Data.Instance.key = 15;
                BTree<Comp_Product>.Create("Product", new ConvertToObject(Comp_Product.ConvertToObject), new ConvertToString(Comp_Product.ConverttToString));
                BTree<Comp_Product>.ValidateEdit(producto, new string[2] { producto._name, producto._price.ToString() }, new Modify(Comp_Product.Modify));
            }
            else
            {
                return BadRequest(new string[] { "Solicitud erronea" });
            }

            //Llamar nodo y crear arbol
            return Ok();
        }
        [Route("Store")]
        [HttpPost]
        public ActionResult PostStore(Store tienda)
        {
            if (tienda.Name != null && tienda.Id > 0 && tienda.Address != null)
            {

            }
            else
            {
                return BadRequest(new string[] { "Solicitud erronea" });
            }
            return Ok();
        }
        [Route("Product-Store")]
        [HttpPost]
        public ActionResult postStoreProduct(Store_Product sp)
        {
            if (sp.IdProduct > 0 && sp.IdProduct > 0 && sp.Stock >= 0)
            {

            }
            else
            {
                return BadRequest(new string[] { "Solicitud erronea" });
            }
            return Ok();
        }
        [Route("EXPORTAR")]
        [HttpPost]
        public ActionResult PostHuffmanExportar(IFormFile File)
        {
            try
            {
                var extensionTipo = Path.GetExtension(File.FileName);
                var directorio = "TusArchivos/" + File.FileName;
                CompressHuffman HuffmanCompress = new CompressHuffman();
                if (extensionTipo == ".txt")//Para exportar
                {
                    using (FileStream thisFile = new FileStream(directorio, FileMode.OpenOrCreate))
                    {
                        if (thisFile.Length == 0)
                        {
                            return BadRequest(new string[] { "Por favor guarde el archivo en: " + directorio });
                        }
                        HuffmanCompress.CompresionHuffman(thisFile);
                    }
                }
                else { return BadRequest(new string[] { "La extensión debe ser .txt" }); }
                return Ok(new string[] { "Archivo comprimido exitosamente" });
            }
            catch (System.NullReferenceException)//No se envia nada
            {
                return NotFound(new string[] { "Porfavor seleccione un archivo" });
            }
        }
        [Route("IMPORTAR")]
        [HttpPost]
        public ActionResult PostHuffmanImportar(IFormFile File)
        {
            try
            {
                var extensionTipo = Path.GetExtension(File.FileName);
                var directorio = "TusArchivos/" + File.FileName;
                CompressHuffman HuffmanCompress = new CompressHuffman();
                if (extensionTipo == ".huff")//Guarda en el arbol
                {
                    using (FileStream thisFile = new FileStream("TusArchivos/" + File.FileName, FileMode.OpenOrCreate))
                    {
                        if (thisFile.Length == 0)
                        {
                            return BadRequest(new string[] { "Por favor guarde el archivo en: " + directorio });
                        }
                        HuffmanCompress.DescompresionHuffman(thisFile);
                        var nombre = Path.GetFileNameWithoutExtension(thisFile.Name);
                        nombre = nombre.Replace("IMPORTADO_", string.Empty);
                        using (StreamReader newfile = new StreamReader("TusArchivos/" + nombre + ".txt"))
                        {
                            string texto = string.Empty;
                            var linea = string.Empty;
                            var listaNodo = new List<AllData>();
                            while ((linea = newfile.ReadLine()) != null)
                            {
                                if (linea.Contains(":"))
                                {
                                    var toAdd = linea.Split(':');
                                    toAdd[1] = toAdd[1].Trim(',');
                                    listaNodo.Add(new AllData(toAdd[0].Trim(), toAdd[1].Trim()));
                                }
                            }
                            var id = string.Empty;
                            var name = string.Empty;
                            var price = string.Empty;
                            foreach (var item in listaNodo)
                            {
                                var type = item.TypeData.Trim('"');
                                if (type == "_id")
                                {
                                    id = item.StringData;
                                }
                                else if (type == "_name")
                                {
                                    name = item.StringData;
                                }
                                else if (type == "_price")
                                {
                                    price = item.StringData;
                                }
                            }
                            var nodoInterno = new Comp_Product
                            {
                                _id = Int32.Parse(id),
                                _name = name,
                                _price = Int32.Parse(price)
                            };
                            postProduct(nodoInterno);
                        }
                    }
                }
                else
                {
                    return BadRequest(new string[] { "La extensión debe ser .huff" });
                }
                return Ok(new string[] { "Datos guardados" });
            }
            catch (Exception)
            {

                return NotFound(new string[] { "Porfavor seleccione un archivo" });
            }
        }
        public struct AllData
        {
            public AllData(string strdata, string strValue)
            {
                TypeData = strdata;
                StringData = strValue;
            }
            public string TypeData { get; private set; }
            public string StringData { get; private set; }
        }
    }
}
