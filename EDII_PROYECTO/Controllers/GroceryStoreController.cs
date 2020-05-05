using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EDII_PROYECTO.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class GroceryStoreController : ControllerBase
    {
        private static readonly string prueba = "Hola mundo, Esto será modificado en el futuro";
    }
}