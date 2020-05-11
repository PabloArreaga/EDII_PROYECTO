using System.Collections.Generic;
using EDII_PROYECTO.ArbolB;
using EDII_PROYECTO.Encrip;
using EDII_PROYECTO.Helpers;
using EDII_PROYECTO.Models;
using Microsoft.AspNetCore.Mvc;

namespace EDII_PROYECTO.Controllers
{
    [Produces("text/plain")]
    [ApiController]
    [Route("[Controller]")]
    public class ProductStoreController : Controller
    {
        delegate string ToString(object obj);
        delegate object ToObject(string obj);
        delegate object Edit(object obj, string[] txt);
        EncriptarSDES sDES = new EncriptarSDES();

        public int claveUsuario = 1000; 

        public string nombreTreeStore = "TreeStoreProduct";
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
                    BTree<Comp_Store_Product>.Create(nombreTreeStore, new ToObject(Comp_Store_Product.ConvertToObject), new ToString(Comp_Store_Product.ConvertToString));
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
        public List<Comp_Store_Product> getStoreProduct([FromForm]int store, [FromForm] int product)
        {
            if (store >= 0 && product >= 0)
            {
                BTree<Comp_Store_Product>.Create("TreeStoreProduct", new ToObject(Comp_Store.ConvertToObject), new ToString(Comp_Store.ConvertToString));
            }
            else
            {
                return null;
            }
            return BTree<Comp_Store_Product>.Traversal(new Comp_Store_Product { _idStore = store, _idProduct = product }, 1);
        }
        /// <summary>
        /// Obtención de productos totales
        /// </summary>
        /// <response code="200">Muestra de todos los productos dentro del sistema</response>
        [Route("Display")]
        [HttpGet]
        public List<Comp_Store_Product> getStoreProducts([FromForm]int store, [FromForm] int product)
        {
            BTree<Comp_Store_Product>.Create("TreeStoreProduct", new ToObject(Comp_Store.ConvertToObject), new ToString(Comp_Store.ConvertToString));
            return BTree<Comp_Store_Product>.Traversal(null);
        }
    }
}