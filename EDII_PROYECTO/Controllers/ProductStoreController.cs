using System.Collections.Generic;
using EDII_PROYECTO.ArbolB;
using EDII_PROYECTO.Encrip;
using EDII_PROYECTO.Helpers;
using EDII_PROYECTO.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language;

namespace EDII_PROYECTO.Controllers
{
    //[Produces("text/plain")]
    [ApiController]
    [Route("[Controller]")]
    public class ProductStoreController : Controller
    {
        delegate string ToString(object obj);
        delegate object ToObject(string obj);
        delegate object Edit(object obj, string[] txt);
        EncriptarSDES sDES = new EncriptarSDES();

        public int claveUsuario = 1000; 

        public string treeFile = "TreeStoreProduct";
        /// <summary>
        /// Ingresar productos
        /// </summary>
        /// <response code="200">Productos ingresados correctamente</response>
        /// <response code="404">Datos no compatibles</response>
        [HttpPost]
        public ActionResult<IEnumerable<string>> postStoreProduct([FromForm]Comp_Store_Product storeproduct)
        {
            if (Data.Instance.ClavesParaLlave.Count == 0)
            {
                sDES.ConvertirBinario(claveUsuario);
            }
            else
            {
                if (storeproduct._idStore >= 0 && storeproduct._idProduct >= 0 && storeproduct._stock >= 0)
                {
                    BTree<Comp_Store_Product>.Create(treeFile, new ToObject(Comp_Store_Product.ConvertToObject), new ToString(Comp_Store_Product.ConvertToString));
                    BTree<Comp_Store_Product>.ValidateIncert(new Comp_Store_Product { _idStore = storeproduct._idStore, _idProduct = storeproduct._idProduct, _stock = storeproduct._stock });
                }
                else
                {
                    return BadRequest("Solicitud erronea");
                }
            }
            return Ok();
        }
        /// <summary>
        /// Obtiene un producto buscado
        /// </summary>
        /// <response code="200">Producto y respectivos valores</response>
        [HttpGet]
        public ActionResult<List<Comp_Store_Product>> getStoreProduct(int store, int product)
        {
            if (store >= 0 && product >= 0)
            {
                BTree<Comp_Store_Product>.Create(treeFile, new ToObject(Comp_Store_Product.ConvertToObject), new ToString(Comp_Store_Product.ConvertToString));
            }
            else
            {
                return BadRequest(null);
            }
            return Ok(BTree<Comp_Store_Product>.Traversal(new Comp_Store_Product { _idStore = store, _idProduct = product }, 1)) ;
        }
        /// <summary>
        /// Obtención de productos totales
        /// </summary>
        /// <response code="200">Muestra de todos los productos dentro del sistema</response>
        
        [HttpGet , Route("All")]
        public ActionResult<List<Comp_Store_Product>> getStoreProducts()
        {
            BTree<Comp_Store_Product>.Create(treeFile, new ToObject(Comp_Store_Product.ConvertToObject), new ToString(Comp_Store_Product.ConvertToString));
            return Ok(BTree<Comp_Store_Product>.Traversal(null));
        }


        [Route("OneStore")]
        [HttpGet]
        public ActionResult<List<Comp_Store_Product>> getStoreProductsoneStore( int store)
        {
            BTree<Comp_Store_Product>.Create(treeFile, new ToObject(Comp_Store_Product.ConvertToObject), new ToString(Comp_Store_Product.ConvertToString));
            var OneStore = new List<Comp_Store_Product>();
            foreach (var item in BTree<Comp_Store_Product>.Traversal(null))
            {
                if (item._idStore == store)
                {
                    OneStore.Add(item);
                }
            }
            return Ok(OneStore) ;
        }

        [HttpPut] // modifica los datos
        public ActionResult<IEnumerable<string>> putStoreProduct([FromForm]Comp_Store_Product storeProduct)
        {
            BTree<Comp_Store_Product>.Create(treeFile, new ToObject(Comp_Store_Product.ConvertToObject), new ToString(Comp_Store_Product.ConvertToString));
            if (BTree<Comp_Store_Product>.Traversal(new Comp_Store_Product { _idStore = storeProduct._idStore, _idProduct = storeProduct._idProduct }, 1).Count == 0)
            {
                return BadRequest("No se encontro ninguna coincidencia");
            }

            BTree<Comp_Store_Product>.ValidateEdit(storeProduct, new string[2] { storeProduct._stock.ToString(), null }, new Edit(Comp_Store_Product.Modify));
            return Ok("El elemento fue actualizado");
        }

        [Route("Transfer")]
        [HttpPut]
        public ActionResult<List<List<Comp_Store_Product>>> putTransferStoreProduct([FromForm]int product, [FromForm] int transmitter, [FromForm]int receiver, [FromForm] int stock)
        {
            BTree<Comp_Store_Product>.Create(treeFile, new ToObject(Comp_Store.ConvertToObject), new ToString(Comp_Store.ConvertToString));
            var nodoTransmiter = BTree<Comp_Store_Product>.Traversal(new Comp_Store_Product { _idStore = transmitter, _idProduct = product }, 1);
            var nodoReceiver = BTree<Comp_Store_Product>.Traversal(new Comp_Store_Product { _idStore = receiver, _idProduct = product }, 1);

            if (nodoTransmiter.Count != 0 && nodoTransmiter[0]._stock - stock >= 0)
            {
                if (nodoReceiver.Count == 0)
                {
                    BTree<Comp_Store_Product>.ValidateIncert(new Comp_Store_Product { _idStore = receiver, _idProduct = product, _stock = stock });
                }
                else
                {
                    nodoReceiver[0]._stock = nodoReceiver[0]._stock + stock;
                    BTree<Comp_Store_Product>.ValidateEdit(nodoReceiver[0], new string[2] { nodoReceiver[0]._stock.ToString(), string.Empty }, new Edit(Comp_Store_Product.Modify));
                }
                nodoTransmiter[0]._stock = nodoTransmiter[0]._stock - stock;
                BTree<Comp_Store_Product>.ValidateEdit(nodoTransmiter[0], new string[2] { nodoTransmiter[0]._stock.ToString(), string.Empty }, new Edit(Comp_Store_Product.Modify));
            }
            else
            {
                return BadRequest(null);
            }

            var nodoList = new List<List<Comp_Store_Product>>();
            nodoList.Add(BTree<Comp_Store_Product>.Traversal(new Comp_Store_Product { _idStore = transmitter, _idProduct = product }, 1));
            nodoList.Add(BTree<Comp_Store_Product>.Traversal(new Comp_Store_Product { _idStore = receiver, _idProduct = product }, 1));
            return Ok(nodoList);

        }
    }
}