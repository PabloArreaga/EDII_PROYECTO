using System.Collections.Generic;
using EDII_PROYECTO.ArbolB;
using EDII_PROYECTO.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EDII_PROYECTO.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class ProductController : Controller
    {
        delegate string ToString(object obj);
        delegate object ToObject(string obj);
        delegate object Edit(object obj, string[] txt);
        public string treeFile = "TreeProduct";

        /// <summary>
        /// Busqueda de producto por ID
        /// </summary>
        [HttpGet, Route("Individual")]
        public List<Comp_Product> getProduct([FromForm]int product)
        {
            if (product >= 0)
            {
                BTree<Comp_Product>.Create("TreeProduct", new ToObject(Comp_Product.ConvertToObject), new ToString(Comp_Product.ConverttToString));
            }
            else
            {
                return null;
            }
            return BTree<Comp_Product>.Traversal(new Comp_Product { _id = product }, 1);
        }
        /// <summary>
        /// Obtiene los productos ingresados
        /// </summary>
        /// <response code="200">Producto y respectivos valores</response>
        [HttpGet, Route("Display")]
        public List<Comp_Product> getProducts()
        {
            BTree<Comp_Product>.Create(treeFile, new ToObject(Comp_Product.ConvertToObject), new ToString(Comp_Product.ConverttToString));
            return BTree<Comp_Product>.Traversal(null);
        }
        /// <summary>
        /// Ingresar productos
        /// </summary>
        /// <response code="200">Productos ingresados correctamente</response>
        /// <response code="404">Datos no compatibles</response>
        [HttpPost, Route("Ingresar")]
        public ActionResult<IEnumerable<string>> postProduct([FromForm]Comp_Product product)
        {
            if (product._name != null && product._price >= 0)
            {
                BTree<Comp_Product>.Create(treeFile, new ToObject(Comp_Product.ConvertToObject), new ToString(Comp_Product.ConverttToString));
                BTree<Comp_Product>.ValidateIncert(new Comp_Product { _id = BTree<Comp_Product>.KnowId(), _name = product._name, _price = product._price });
            }
            else
            {
                return BadRequest("Solicitud erronea");
            }
            return Ok("Dato ingresado correctamente");
        }
        /// <summary>
        /// Obtención de productos totales
        /// </summary>
        /// <response code="200">Muestra de todos los productos dentro del sistema</response>
        [HttpPost, Route("Inventory")]
        public ActionResult<IEnumerable<string>> Inventory([FromForm]IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("El archivo esta vacio o no selecciono ningun archivo");
            }
            BTree<Comp_Product>.Create(treeFile, new ToObject(Comp_Product.ConvertToObject), new ToString(Comp_Product.ConverttToString));
            Comp_Product.LoadInventory(file.OpenReadStream());
            return Ok("Se han ingresado los valores con excito");
        }
        /// <summary>
        /// Modificación de datos
        /// </summary>
        [HttpPut]
        public ActionResult<IEnumerable<string>> putProduct([FromForm]Comp_Product product)
        {
            BTree<Comp_Product>.Create(treeFile, new ToObject(Comp_Product.ConvertToObject), new ToString(Comp_Product.ConverttToString));
            if (BTree<Comp_Product>.Traversal(new Comp_Product { _id = product._id }, 1).Count == 0)
            {
                return BadRequest("El id no se encontro");
            }

            BTree<Comp_Product>.ValidateEdit(product, new string[2] { product._name, product._price.ToString() }, new Edit(Comp_Product.Modify));
            return Ok("El id: " + product._id + "fue actualizado");
        }
    }
}