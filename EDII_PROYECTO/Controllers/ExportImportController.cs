using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using System.Threading.Tasks;
using EDII_PROYECTO.Helpers;
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
        [Route("IMPORTAR")]
        [HttpPost]
        public ActionResult PostHuffmanImportar()
        {
            var nombreRuta = "TusArchivos";
            if (!Directory.Exists(nombreRuta))
            {
                Directory.CreateDirectory(nombreRuta);
            }
            string[] ruta = Directory.GetFiles(nombreRuta, "*.huff");
            if (ruta.Length == 0)
            {
                return BadRequest(new string[] { "Por favor guarde el archivo en: " + nombreRuta });
            }
            foreach (var archivo in ruta)
            {
                CompressHuffman HuffmanCompress = new CompressHuffman();
                using (FileStream thisFile = new FileStream(archivo, FileMode.OpenOrCreate))
                {
                    if (thisFile.Length == 0)
                    {
                        return BadRequest(new string[] { "Archivo no contiene elementos"  });
                    }
                    var file = archivo.Replace(".huff", string.Empty);
                    file = file.Replace("TusArchivos\\", string.Empty);
                    Data.Instance.adress = $"Database\\{file}.txt";
                    thisFile.Close();
                    HuffmanCompress.DescompresionHuffman(thisFile);
                }
            }
            return Ok(new string[] { "Datos guardados" });
        }
    }
}