using System.Collections.Generic;
using EDII_PROYECTO.ArbolB;
using EDII_PROYECTO.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EDII_PROYECTO.Controllers
{
    [Produces("text/plain")]
    [ApiController]
    [Route("[Controller]")]
    public class ProductController : Controller
    {
        delegate string ToString(object obj);
        delegate object ToObject(string obj);
        delegate object Edit(object obj, string[] txt);

        public string nombreTree = "TreeProduct";
        /// <summary>
        /// Ingresar productos
        /// </summary>
        /// <response code="200">Productos ingresados correctamente</response>
        /// <response code="404">Datos no compatibles</response>
        [HttpPost]
        public ActionResult<IEnumerable<string>> postProduct([FromForm]Comp_Product product)
        {
            if (product._name != null && product._price >= 0)
            {
                BTree<Comp_Product>.Create(nombreTree, new ToObject(Comp_Product.ConvertToObject), new ToString(Comp_Product.ConverttToString));
                BTree<Comp_Product>.ValidateIncert(new Comp_Product { _id = BTree<Comp_Product>.KnowId(), _name = product._name, _price = product._price });
            }
            else
            {
                return BadRequest("Solicitud erronea");
            }
            return Ok();
        }
        //[HttpGet]//1Producto
        //public List<Comp_Product> getProduct([FromForm]int product)
        //{
        //    if (product >= 0)
        //    {
        //        BTree<Comp_Product>.Create("TreeProduct", new ToObject(Comp_Product.ConvertToObject), new ToString(Comp_Product.ConverttToString));
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //    return BTree<Comp_Product>.Traversal(new Comp_Product { _id = product }, 1);
        //}
        /// <summary>
        /// Obtiene un producto buscado
        /// </summary>
        /// <response code="200">Producto y respectivos valores</response>
        [HttpGet]
        public List<Comp_Product> getProducts()
        {
            BTree<Comp_Product>.Create("TreeProduct", new ToObject(Comp_Product.ConvertToObject), new ToString(Comp_Product.ConverttToString));
            return BTree<Comp_Product>.Traversal(null);
        }
        /// <summary>
        /// Obtención de productos totales
        /// </summary>
        /// <response code="200">Muestra de todos los productos dentro del sistema</response>
        [Route("Display")]
        [HttpGet]
        public ActionResult<IEnumerable<string>> Inventory([FromForm]IFormFile file)
        {
            BTree<Comp_Product>.Create(nombreTree, new ToObject(Comp_Product.ConvertToObject), new ToString(Comp_Product.ConverttToString));
            Comp_Product.LoadInventory(file.OpenReadStream());
            return Ok();
        }
    }
}