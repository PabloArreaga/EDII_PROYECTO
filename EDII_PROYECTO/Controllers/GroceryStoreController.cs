using EDII_PROYECTO.ArbolB;
using EDII_PROYECTO.Huffman;
using EDII_PROYECTO.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace EDII_PROYECTO.Controllers
{
    delegate string ConvertToString(object obj);
    delegate object ConvertToObject(string obj);
    delegate object Modify(object obj, string[] txt);

    [ApiController]
    [Route("api/[controller]")]

    public class GroceryStoreController : ControllerBase
    {
        public string nombreTree = "TreeProduct";
        public string nombreTreeStore = "TreeStoreProduct";
        public string nombreStore = "TreeStore";
        [Route("EnterProduct")]
        [HttpPost]
        public ActionResult<IEnumerable<string>> postProduct([FromForm]Comp_Product product)
        {
            if (product._name != null && product._price >= 0)
            {
                BTree<Comp_Product>.Create(nombreTree, new ConvertToObject(Comp_Product.ConvertToObject), new ConvertToString(Comp_Product.ConverttToString));
                BTree<Comp_Product>.ValidateIncert(new Comp_Product { _id = BTree<Comp_Product>.KnowId(), _name = product._name, _price = product._price });
            }
            else
            {
                return BadRequest("Solicitud erronea");
            }
            return Ok();
        }
        [Route("EnterProducts")]
        [HttpPost]
        public ActionResult<IEnumerable<string>> Inventory([FromForm]IFormFile file)
        {
            BTree<Comp_Product>.Create(nombreTree, new ConvertToObject(Comp_Product.ConvertToObject), new ConvertToString(Comp_Product.ConverttToString));
            Comp_Product.LoadInventory(file.OpenReadStream());
            return Ok();
        }
        [Route("EnterStore")]
        [HttpPost]
        public ActionResult<IEnumerable<string>> postStore([FromForm]Comp_Store store)
        {
            if (store._name != null && store._address != null)
            {
                BTree<Comp_Store>.Create(nombreStore, new ConvertToObject(Comp_Store.ConvertToObject), new ConvertToString(Comp_Store.ConvertToString));
                BTree<Comp_Store>.ValidateIncert(new Comp_Store { _id = BTree<Comp_Store>.KnowId(), _name = store._name, _address = store._address });
            }
            else
            {
                return BadRequest("Solicitud erronea");
            }
            return Ok();
        }
        [Route("EnterProduct-Store")]
        [HttpPost]
        public ActionResult<IEnumerable<string>> postStoreProduct([FromForm]Comp_Store_Product storeproduct)
        {
            if (storeproduct._idStore >= 0 && storeproduct._idProduct >= 0 && storeproduct._stock >= 0)
            {
                BTree<Comp_Store_Product>.Create(nombreTreeStore, new ConvertToObject(Comp_Store_Product.ConvertToObject), new ConvertToString(Comp_Store_Product.ConvertToString));
                BTree<Comp_Store_Product>.ValidateIncert(new Comp_Store_Product { _idStore = storeproduct._idStore, _idProduct = storeproduct._idProduct, _stock = storeproduct._stock });
            }
            else
            {
                return BadRequest("Solicitud erronea");
            }
            return Ok();
        }
        [Route("EXPORTAR")]
        [HttpPost]
        public ActionResult ExportarHuff([FromForm]string nombreArchivo)
        {
            CompressHuffman HuffmanCompress = new CompressHuffman();
            if (nombreArchivo == nombreTree || nombreArchivo == nombreTreeStore || nombreArchivo == nombreStore)
            {
                var rutaInicial = $"Database\\{nombreArchivo}.txt";
                using (FileStream thisFile = new FileStream(rutaInicial, FileMode.OpenOrCreate))
                {
                    HuffmanCompress.CompresionHuffman(thisFile);
                }
            }
            else
            {
                return BadRequest(new string[] { "Porfavor seleccione"});
            }
            return Ok();
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

        [Route("DisplayProduct")]
        [HttpGet]
        public List<Comp_Product> getProduct([FromForm]int product)
        {
            if (product >= 0)
            {
                BTree<Comp_Product>.Create("TreeProduct", new ConvertToObject(Comp_Product.ConvertToObject), new ConvertToString(Comp_Product.ConverttToString));
            }
            else
            {
                return null;
            }
            return BTree<Comp_Product>.Traversal(new Comp_Product { _id = product }, true);
        }

        [Route("DisplayStore")]
        [HttpGet]
        public List<Comp_Store> getStore([FromForm]int store)
        {
            if (store >= 0)
            {
                BTree<Comp_Store>.Create("TreeStore", new ConvertToObject(Comp_Store.ConvertToObject), new ConvertToString(Comp_Store.ConvertToString));
            }
            else
            {
                return null;
            }
            return BTree<Comp_Store>.Traversal(new Comp_Store { _id = store }, true);
        }

        [Route("DisplayStore-Product")]
        [HttpGet]
        public List<Comp_Store_Product> getStoreProduct([FromForm]int store, [FromForm] int product)
        {
            if (store >= 0 && product >= 0)
            {
                BTree<Comp_Store_Product>.Create("TreeStoreProduct", new ConvertToObject(Comp_Store.ConvertToObject), new ConvertToString(Comp_Store.ConvertToString));
            }
            else
            {
                return null;
            }
            return BTree<Comp_Store_Product>.Traversal(new Comp_Store_Product { _idStore = store, _idProduct = product }, true);
        }

        [Route("DisplayProducts")]
        [HttpGet]
        public List<Comp_Product> getProducts()
        {
            BTree<Comp_Product>.Create("TreeStore", new ConvertToObject(Comp_Store.ConvertToObject), new ConvertToString(Comp_Store.ConvertToString));
            return BTree<Comp_Product>.Traversal(null, false);
        }

        [Route("DisplayShops")]
        [HttpGet]
        public List<Comp_Store> getShops()
        {
            BTree<Comp_Store>.Create("TreeStore", new ConvertToObject(Comp_Store.ConvertToObject), new ConvertToString(Comp_Store.ConvertToString));
            return BTree<Comp_Store>.Traversal(null, false);
        }

        [Route("DisplayStore-Products")]
        [HttpGet]
        public List<Comp_Store_Product> getStoreProducts([FromForm]int store, [FromForm] int product)
        {
            BTree<Comp_Store_Product>.Create("TreeStore", new ConvertToObject(Comp_Store.ConvertToObject), new ConvertToString(Comp_Store.ConvertToString));
            return BTree<Comp_Store_Product>.Traversal(null, false);
        }


    }
}
