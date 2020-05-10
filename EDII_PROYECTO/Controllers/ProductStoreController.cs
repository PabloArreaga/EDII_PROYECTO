using System.Collections.Generic;
using EDII_PROYECTO.ArbolB;
using EDII_PROYECTO.Models;
using Microsoft.AspNetCore.Mvc;

namespace EDII_PROYECTO.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductStoreController : Controller
    {
        delegate string ToString(object obj);
        delegate object ToObject(string obj);
        delegate object Edit(object obj, string[] txt);

        public string nombreTreeStore = "TreeStoreProduct";
        [HttpPost]
        public ActionResult<IEnumerable<string>> postStoreProduct([FromForm]Comp_Store_Product storeproduct)
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
            return Ok();
        }
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
            return BTree<Comp_Store_Product>.Traversal(new Comp_Store_Product { _idStore = store, _idProduct = product }, true);
        }
        [Route("DisplayStore-Products")]//Buscar mejor obtención
        [HttpGet]
        public List<Comp_Store_Product> getStoreProducts([FromForm]int store, [FromForm] int product)
        {
            BTree<Comp_Store_Product>.Create("TreeStoreProduct", new ToObject(Comp_Store.ConvertToObject), new ToString(Comp_Store.ConvertToString));
            return BTree<Comp_Store_Product>.Traversal(null, false);
        }
    }
}