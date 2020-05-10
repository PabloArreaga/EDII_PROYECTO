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
        public ActionResult ExportarHuff([FromForm]string nombreArchivo)
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
            var archivoResultado = FileDownload(nombreArchivo);
            return Ok();
        }
        [Route("IMPORTAR/{name}")]
        [HttpPost]
        public ActionResult PostHuffmanImportar(IFormFile File)
        {
            try
            {
                var extensionTipo = Path.GetExtension(File.FileName);
                var directorio = "TusArchivos/" + File.FileName;
                CompressHuffman HuffmanCompress = new CompressHuffman();
                var listaNodo = new List<AllData>();
                using (FileStream thisFile = new FileStream("TusArchivos/" + File.FileName, FileMode.OpenOrCreate))
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
        public async Task<FileStreamResult> FileDownload(string name)
        {
            using (var thisFile = new FileStream($"Database\\{name}.txt", FileMode.Open))
            {
                CompressHuffman HuffmanCompress = new CompressHuffman();
                HuffmanCompress.CompresionHuffman(thisFile);
            }
            return await Download($"Database\\{name}.huff");
        }
        async Task<FileStreamResult> Download(string path)
        {
            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            if (!Directory.Exists("TusArchivos"))
            {
                Directory.CreateDirectory("TusArchivos");
            }
            Directory.Delete("TusArchivos", true);
            return File(memory, MediaTypeNames.Application.Octet, Path.GetFileName(path));
        }
        public struct AllData
        {
            public AllData(string strdata, string strValue)
            {
                TypeData = strdata;
                StringData = strValue;
            }
            public string TypeData { get; private set; }
            public string StringData { get; private set; }
        }
    }
}