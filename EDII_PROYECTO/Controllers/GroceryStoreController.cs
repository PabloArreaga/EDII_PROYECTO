using EDII_PROYECTO.ArbolB;
using EDII_PROYECTO.Huffman;
using EDII_PROYECTO.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;

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
        public ActionResult<IEnumerable<string>> postProduct([FromForm]Comp_Product product)//Datos principales para el nodo
        {
            if (product._name != null && product._price >= 0)
            {
                BTree<Comp_Product>.Create("TreeProducts", new ConvertToObject(Comp_Product.ConvertToObject), new ConvertToString(Comp_Product.ConverttToString));
                BTree<Comp_Product>.ValidateIncert(new Comp_Product { _id = BTree<Comp_Product>.KnowId(), _name = product._name, _price = product._price });
            }
            else
            {
                return BadRequest("Solicitud erronea");
            }

            //Llamar nodo y crear arbol
            return Ok();
        }
        [Route("UploadInventory")]
        [HttpPost]
        public ActionResult<IEnumerable<string>> Inventory([FromForm]IFormFile file)//Datos principales para el nodo
        {
            BTree<Comp_Product>.Create("TreeProducts", new ConvertToObject(Comp_Product.ConvertToObject), new ConvertToString(Comp_Product.ConverttToString));
            Comp_Product.LoadInventory(file.OpenReadStream());
            return Ok();
        }
        [Route("Store")]
        [HttpPost]
        public ActionResult<IEnumerable<string>> postStore([FromForm]Comp_Store store)//Datos principales para el nodo
        {
            if (store._name != null && store._address != null)
            {
                BTree<Comp_Store>.Create("TreeStore", new ConvertToObject(Comp_Store.ConvertToObject), new ConvertToString(Comp_Store.ConvertToString));
                BTree<Comp_Store>.ValidateIncert(new Comp_Store { _id = BTree<Comp_Store>.KnowId(), _name = store._name, _address = store._address });
            }
            else
            {
                return BadRequest("Solicitud erronea");
            }
            //Llamar nodo y crear arbol
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
