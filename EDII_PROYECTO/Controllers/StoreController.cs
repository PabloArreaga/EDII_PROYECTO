﻿using System.Collections.Generic;
using EDII_PROYECTO.ArbolB;
using EDII_PROYECTO.Models;
using Microsoft.AspNetCore.Mvc;

namespace EDII_PROYECTO.Controllers
{
    //[Produces("text/plain")]
    [ApiController]
    [Route("[Controller]")]
    public class StoreController : Controller
    {
        delegate string ToString(object obj);
        delegate object ToObject(string obj);
        delegate object Edit(object obj, string[] txt);

        public string treeFile = "TreeStore";
        /// <summary>
        /// Ingresar productos
        /// </summary>
        /// <response code="200">Productos ingresados correctamente</response>
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
                return BadRequest("Solicitud erronea");
            }
            return Ok();
        }
        /// <summary>
        /// Obtiene un producto buscado
        /// </summary>
        /// <response code="200">Producto y respectivos valores</response>
        [HttpGet]
        public List<Comp_Store> getStore([FromForm]int store)
        {
            if (store >= 0)
            {
                BTree<Comp_Store>.Create("TreeStore", new ToObject(Comp_Store.ConvertToObject), new ToString(Comp_Store.ConvertToString));
            }
            else
            {
                return null;
            }
            return BTree<Comp_Store>.Traversal(new Comp_Store { _id = store }, 1);
        }
        /// <summary>
        /// Obtención de productos totales
        /// </summary>
        /// <response code="200">Muestra de todos los productos dentro del sistema</response>
        [Route("All")]
        [HttpGet]
        public List<Comp_Store> getShops()
        {
            BTree<Comp_Store>.Create("TreeStore", new ToObject(Comp_Store.ConvertToObject), new ToString(Comp_Store.ConvertToString));
            return BTree<Comp_Store>.Traversal(null);
        }

        [HttpPut] // modifica los datos
        public ActionResult<IEnumerable<string>> putProduct([FromForm]Comp_Store store)
        {
            BTree<Comp_Store>.Create(treeFile, new ToObject(Comp_Store.ConvertToObject), new ToString(Comp_Store.ConvertToString));
            if (BTree<Comp_Store>.Traversal(new Comp_Store { _id = store._id }, 1).Count == 0)
            {
                return BadRequest("El id no se encontro");
            }

            BTree<Comp_Store>.ValidateEdit(store, new string[2] { store._name, store._address }, new Edit(Comp_Store.Modify));
            return Ok("El id: " + store._id + "fue actualizado");
        }
    }
}