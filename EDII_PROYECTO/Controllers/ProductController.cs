using System.Collections.Generic;
using EDII_PROYECTO.ArbolB;
using EDII_PROYECTO.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EDII_PROYECTO.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        delegate string ToString(object obj);
        delegate object ToObject(string obj);
        delegate object Edit(object obj, string[] txt);

        public string nombreTree = "TreeProduct";
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
        [HttpGet]//1Producto
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
            return BTree<Comp_Product>.Traversal(new Comp_Product { _id = product }, true);
        }
        [HttpGet]//Varios productos
        public List<Comp_Product> getProducts()
        {
            BTree<Comp_Product>.Create("TreeProduct", new ToObject(Comp_Store.ConvertToObject), new ToString(Comp_Store.ConvertToString));
            return BTree<Comp_Product>.Traversal(null, false);
        }
        [Route("All")]//Buscar mejor obtención
        [HttpPost]
        public ActionResult<IEnumerable<string>> Inventory([FromForm]IFormFile file)
        {
            BTree<Comp_Product>.Create(nombreTree, new ToObject(Comp_Product.ConvertToObject), new ToString(Comp_Product.ConverttToString));
            Comp_Product.LoadInventory(file.OpenReadStream());
            return Ok();
        }
    }
}