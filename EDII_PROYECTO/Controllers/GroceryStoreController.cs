using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static EDII_PROYECTO.Encrip.InfoSDES;

namespace EDII_PROYECTO.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class GroceryStoreController : ControllerBase
    {
        [Route("Store")]
        [HttpPost]
        public ActionResult PostStore(Datos mySdes)
        {

            return Ok();
        }
        [Route("Product")]
        [HttpPost]
        public ActionResult postProduct()
        {

            return Ok();
        }
        [Route("Product-Store")]
        [HttpPost]
        public ActionResult postStoreProduct()
        {

            return Ok();
        }
    }
}