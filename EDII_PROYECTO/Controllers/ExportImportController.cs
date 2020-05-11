using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using System.Threading.Tasks;
using EDII_PROYECTO.Huffman;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EDII_PROYECTO.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExportImportController : Controller
    {
        [Route("EXPORTAR")]
        [HttpPost]
        public async Task<IActionResult> ExportarHuff([FromForm]string nombreArchivo)
        {
            CompressHuffman HuffmanCompress = new CompressHuffman();
            if (nombreArchivo == "TreeStoreProduct" || nombreArchivo == "TreeProduct" || nombreArchivo == "TreeStore")
            {
                var rutaInicial = $"Database\\{nombreArchivo}.txt";
                using (FileStream thisFile = new FileStream(rutaInicial, FileMode.OpenOrCreate))
                {
                    HuffmanCompress.CompresionHuffman(thisFile);
                }
            }
            else
            {
                return BadRequest(new string[] { "Porfavor seleccione: " });
            }
            //Compresión y Descarga
            using (var thisFile = new FileStream($"Database\\{nombreArchivo}.txt", FileMode.Open))
            {
                HuffmanCompress.CompresionHuffman(thisFile);
            }
            var ruta = $"Database/{nombreArchivo}" + ".huff";
            if (!System.IO.File.Exists(ruta))
            {
                return BadRequest();
            }
            return File(await System.IO.File.ReadAllBytesAsync(ruta), "application/octet-stream", nombreArchivo + ".huff");
        }
        //public async Task<IActionResult> FileDownload(string name)
        //{

        //}
        [Route("IMPORTAR")]
        [HttpPost]
        public ActionResult PostHuffmanImportar(string File)
        {
            try
            {
                if (!Directory.Exists("TusArchivos"))
                {
                    Directory.CreateDirectory("TusArchivos");
                }
                var directorio = "TusArchivos/" + File;
                CompressHuffman HuffmanCompress = new CompressHuffman();
                using (FileStream thisFile = new FileStream("TusArchivos/" + File, FileMode.OpenOrCreate))
                {
                    if (thisFile.Length == 0)
                    {
                        return BadRequest(new string[] { "Por favor guarde el archivo en: " + directorio });
                    }
                    HuffmanCompress.DescompresionHuffman(thisFile);
                }

                return Ok(new string[] { "Datos guardados" });
            }
            catch (Exception)
            {

                return NotFound(new string[] { "Porfavor seleccione un archivo" });
            }
        }
    }
}