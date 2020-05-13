using System.Collections.Generic;
using EDII_PROYECTO.ArbolB;
using EDII_PROYECTO.Models;
using Microsoft.AspNetCore.Mvc;

namespace EDII_PROYECTO.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class StoreController : Controller
    {
        delegate string ToString(object obj);
        delegate object ToObject(string obj);
        delegate object Edit(object obj, string[] txt);
        public string treeFile = "TreeStore";
        /// <summary>
        /// Obtiene una tienda buscada
        /// </summary>
        /// <response code="200">Muestra de tienda y respectivos valores</response>
        /// <response code="404">No se encuentran valores con ID seleccionado</response>
        [HttpGet]
        public ActionResult<List<Comp_Store>> getStore(int id)
        {
            if (id >= 0)
            {
                BTree<Comp_Store>.Create("TreeStore", new ToObject(Comp_Store.ConvertToObject), new ToString(Comp_Store.ConvertToString));
            }
            else
            {
                return NotFound("El ID: " + id + " no cuenta con tiendas asignadas.");
            }
            return Ok(BTree<Comp_Store>.Traversal(new Comp_Store { _id = id }, 1)) ;
        }
        /// <summary>
        /// Obtención de tiendas totales
        /// </summary>
        /// <response code="200">Muestra de todas las tiendas en el sistema</response>
        /// <response code="200">No se encuentran valores disponibles</response>
        [HttpGet, Route("Display")]
        public ActionResult<List<Comp_Store>> getShops()
        {
            BTree<Comp_Store>.Create("TreeStore", new ToObject(Comp_Store.ConvertToObject), new ToString(Comp_Store.ConvertToString));
            var total = BTree<Comp_Store>.Traversal(null);
            if (total.Count == 0)
            {
                return NotFound("No se encuentran valores disponibles.");
            }
            return Ok(total);
        }
        /// <summary>
        /// Ingresar tiendas
        /// </summary>
        /// <response code="200">Tienda ingresada correctamente</response>
        /// <response code="404">Datos no compatibles</response>
        [HttpPost]
        public ActionResult<IEnumerable<string>> postStore([FromForm]Comp_Store store)
        {
            if (store._name != null && store._address != null)
            {
                BTree<Comp_Store>.Create(treeFile, new ToObject(Comp_Store.ConvertToObject), new ToString(Comp_Store.ConvertToString));
                BTree<Comp_Store>.ValidateIncert(new Comp_Store { _id = BTree<Comp_Store>.KnowId(), _name = store._name, _address = store._address });
            }
            else
            {
                return BadRequest("Solicitud erronea.");
            }
            return Ok("Datos ingresados correctamente.");
        }
        /// <summary>
        /// Modificación de datos
        /// </summary>
        [HttpPut]
        public ActionResult<IEnumerable<string>> putProduct([FromForm]Comp_Store store)
        {
            BTree<Comp_Store>.Create(treeFile, new ToObject(Comp_Store.ConvertToObject), new ToString(Comp_Store.ConvertToString));
            if (BTree<Comp_Store>.Traversal(new Comp_Store { _id = store._id }, 1).Count == 0)
            {
                return BadRequest("El ID no se encontró.");
            }

            BTree<Comp_Store>.ValidateEdit(store, new string[2] { store._name, store._address }, new Edit(Comp_Store.Modify));
            return Ok("El id: " + store._id + " fue actualizado.");
        }
    }
}