using System.Collections.Generic;
using EDII_PROYECTO.ArbolB;
using EDII_PROYECTO.Encrip;
using EDII_PROYECTO.Models;
using Microsoft.AspNetCore.Mvc;

namespace EDII_PROYECTO.Controllers
{
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
        /// <response code="400">Datos no compatibles</response>
        [HttpPost]
        public ActionResult<IEnumerable<string>> postStoreProduct([FromForm]Comp_Store_Product storeproduct)
        {
            EncriptarSDES.StartKey();

            if (storeproduct._idStore >= 0 && storeproduct._idProduct >= 0 && storeproduct._stock >= 0)
            {
                BTree<Comp_Store_Product>.Create(treeFile, new ToObject(Comp_Store_Product.ConvertToObject), new ToString(Comp_Store_Product.ConvertToString));
                BTree<Comp_Store_Product>.ValidateIncert(new Comp_Store_Product { _idStore = storeproduct._idStore, _idProduct = storeproduct._idProduct, _stock = storeproduct._stock });
            }
            else
            {
                return BadRequest("Solicitud erronea.");
            }

            return Ok("Datos ingresados correctamente.");
        }
        /// <summary>
        /// Obtiene un producto buscado
        /// </summary>
        /// <response code="200">Producto y respectivos valores</response>
        /// <response code="400">Solicitud erronea</response>
        [HttpGet]
        public ActionResult<List<Comp_Store_Product>> getStoreProduct(int store, int product)
        {
            EncriptarSDES.StartKey();
            if (store >= 0 && product >= 0)
            {
                BTree<Comp_Store_Product>.Create(treeFile, new ToObject(Comp_Store_Product.ConvertToObject), new ToString(Comp_Store_Product.ConvertToString));
            }
            else
            {
                return BadRequest("Solicitud Erronea");
            }
            var total = BTree<Comp_Store_Product>.Traversal(new Comp_Store_Product { _idStore = store, _idProduct = product }, 1);
            if (total.Count == 0)
            {
                return NotFound("No se encuentran valores disponibles.");
            }
            return Ok(total);
        }
        /// <summary>
        /// Muestra de stock en tienda con ID de productos
        /// </summary>
        /// <response code="200">Muestra de todos los productos dentro del sistema</response>
        /// <response code="404">No se encuentran valores disponibles</response>
        [HttpGet, Route("Display")]
        public ActionResult<List<Comp_Store_Product>> getStoreProducts()
        {
            EncriptarSDES.StartKey();
            BTree<Comp_Store_Product>.Create(treeFile, new ToObject(Comp_Store_Product.ConvertToObject), new ToString(Comp_Store_Product.ConvertToString));
            var total = BTree<Comp_Store_Product>.Traversal(null);
            if (total.Count == 0)
            {
                return NotFound("No se encuentran valores disponibles.");
            }
            return Ok(total);
        }
        /// <summary>
        /// Busqueda de una tienda
        /// </summary>
        /// <response code="200">Muestra de la tienda con ID seleccionado</response>
        /// <response code="404">No se encuentran valores disponibles</response>
        [HttpGet, Route("OneStore")]
        public ActionResult<List<Comp_Store_Product>> getStoreProductsoneStore(int store)
        {
            EncriptarSDES.StartKey();
            BTree<Comp_Store_Product>.Create(treeFile, new ToObject(Comp_Store_Product.ConvertToObject), new ToString(Comp_Store_Product.ConvertToString));
            var OneStore = new List<Comp_Store_Product>();
            var total = BTree<Comp_Store_Product>.Traversal(null);
            if (total.Count == 0)
            {
                return NotFound("No se encuentran valores disponibles.");
            }
            foreach (var item in total)
            {
                if (item._idStore == store)
                {
                    OneStore.Add(item);
                }
            }
            return Ok(OneStore);
        }
        /// <summary>
        /// Modificación de datos
        /// </summary>
        /// <response code="200">Actualización por ID</response>
        /// <response code="400">No se encuentran coincidencias</response>
        [HttpPut]
        public ActionResult<IEnumerable<string>> putStoreProduct([FromForm]Comp_Store_Product storeProduct)
        {
            EncriptarSDES.StartKey();
            BTree<Comp_Store_Product>.Create(treeFile, new ToObject(Comp_Store_Product.ConvertToObject), new ToString(Comp_Store_Product.ConvertToString));
            if (BTree<Comp_Store_Product>.Traversal(new Comp_Store_Product { _idStore = storeProduct._idStore, _idProduct = storeProduct._idProduct }, 1).Count == 0)
            {
                return BadRequest("No se encontro ninguna coincidencia");
            }

            BTree<Comp_Store_Product>.ValidateEdit(storeProduct, new string[2] { storeProduct._stock.ToString(), null }, new Edit(Comp_Store_Product.Modify));
            return Ok("El elemento fue actualizado");
        }
        /// <summary>
        /// Transferencia de productos entre tiendas
        /// </summary>
        /// <response code="200">Transferencias de Productos entre Tiendas</response>
        /// <response code="400">La solicitud no puede ser procesada</response>
        [HttpPut, Route("Transfer")]
        public ActionResult<List<List<Comp_Store_Product>>> putTransferStoreProduct(int product, int transmitter, int receiver, int stock)
        {
            if (transmitter == receiver)
            {
                return BadRequest("Los id de las tiendas son iguales, intente con ids diferentes");
            }
            EncriptarSDES.StartKey();
            BTree<Comp_Store_Product>.Create(treeFile, new ToObject(Comp_Store_Product.ConvertToObject), new ToString(Comp_Store_Product.ConvertToString));
            var nodoTransmiter = new List<Comp_Store_Product>(BTree<Comp_Store_Product>.Traversal(new Comp_Store_Product { _idProduct = product, _idStore = transmitter }, 1));
            var nodoReceiver = new List<Comp_Store_Product>(BTree<Comp_Store_Product>.Traversal(new Comp_Store_Product { _idProduct = product, _idStore = receiver }, 1));
            if (nodoTransmiter.Count != 0 && nodoTransmiter[0]._stock - stock >= 0)
            {
                if (nodoReceiver.Count == 0)
                {
                    BTree<Comp_Store_Product>.ValidateIncert(new Comp_Store_Product { _idStore = receiver, _idProduct = product, _stock = stock });
                }
                else
                {
                    nodoReceiver[0]._stock = nodoReceiver[0]._stock + stock;
                    BTree<Comp_Store_Product>.ValidateEdit(nodoReceiver[0], new string[2] { nodoReceiver[0]._stock.ToString(), null }, new Edit(Comp_Store_Product.Modify));
                }
                nodoTransmiter[0]._stock = nodoTransmiter[0]._stock - stock;
                BTree<Comp_Store_Product>.ValidateEdit(nodoTransmiter[0], new string[2] { nodoTransmiter[0]._stock.ToString(), null }, new Edit(Comp_Store_Product.Modify));
            }
            else
            {
                return BadRequest("La solicitud no puede ser procesada");
            }
            var nodoList = new List<List<Comp_Store_Product>>();
            nodoList.Add(new List<Comp_Store_Product>(BTree<Comp_Store_Product>.Traversal(new Comp_Store_Product { _idStore = transmitter, _idProduct = product }, 1)));
            nodoList.Add(new List<Comp_Store_Product>(BTree<Comp_Store_Product>.Traversal(new Comp_Store_Product { _idStore = receiver, _idProduct = product }, 1)));
            return Ok(nodoList);
        }
    }
}