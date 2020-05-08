using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EDII_PROYECTO.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static EDII_PROYECTO.Encrip.InfoSDES;

namespace EDII_PROYECTO.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class GroceryStoreController : ControllerBase
    {
        [Route("Product")]
        [HttpPost]
        public ActionResult postProduct(Product producto)//Datos principales para el nodo
        {
            if (producto.Description != null && producto.Id > 0 && producto.Name != null && producto.Price >= 0)
            {

            }
            else
            {
                return BadRequest(new string[] { "Solicitud erronea" });
            }

            //Llamar nodo y crear arbol
            return Ok();
        }
        [Route("Store")]
        [HttpPost]
        public ActionResult PostStore(Store tienda)
        {
            if (tienda.Name != null && tienda.Id > 0 && tienda.Address != null)
            {

            }
            else
            {
                return BadRequest(new string[] { "Solicitud erronea" });
            }
            return Ok();
        }
        [Route("Product-Store")]
        [HttpPost]
        public ActionResult postStoreProduct(Store_Product sp)
        {
            if (sp.IdProduct > 0 && sp.IdProduct > 0 && sp.Stock >= 0)
            {

            }
            else
            {
                return BadRequest(new string[] { "Solicitud erronea" });
            }
            return Ok();
        }
    }
}