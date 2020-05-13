using System.IO;
using System.Threading.Tasks;
using EDII_PROYECTO.Helpers;
using EDII_PROYECTO.Huffman;
using Microsoft.AspNetCore.Mvc;

namespace EDII_PROYECTO.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExportImportController : Controller
    {
        /// <summary>
        /// Permite la descarga de una base de datos comprimida con extensión .huff
        /// </summary>
        /// <returns>Archivo con extensión .huff para poder utilizar en otra base de datos</returns>
        /// <response code="200">Archivo comprimido exitosamente</response>  
        /// <response code="400">Nombre no coincide con archivos en la base de datos</response>  
        /// <response code="404">Archivo no existente</response>
        [HttpPost, Route("EXPORTAR")]
        public async Task<IActionResult> ExportarHuff(string nombreArchivo)
        {
            if (nombreArchivo == "TreeStoreProduct" || nombreArchivo == "TreeProduct" || nombreArchivo == "TreeStore")
            {
                if (Directory.Exists("Database"))
                {
                    using (FileStream thisFile = new FileStream($"Database\\{nombreArchivo}.txt", FileMode.OpenOrCreate))
                    {
                        CompressHuffman HuffmanCompress = new CompressHuffman();
                        HuffmanCompress.CompresionHuffman(thisFile);
                    }
                }
                else
                {
                    return NotFound(new string[] { nombreArchivo + " no se encuentra en la base de datos" });
                }
            }
            else
            {
                return BadRequest(new string[] { "Porfavor seleccione: TreeStoreProduct, TreeProduct, TreeStore" });
            }
            //Descarga
            var ruta = $"TusArchivos/{nombreArchivo}" + ".huff";
            return File(await System.IO.File.ReadAllBytesAsync(ruta), "application/octet-stream", nombreArchivo + ".huff");
        }
        /// <summary>
        /// Todos los documentos con extención .huff serán importados a la base de datos
        /// </summary>
        /// <response code="200">Archivos compatibles descomprimidos y copiados a la base de datos</response>
        /// <response code="404">Carpeta **TusArchivos** no contiene archivos compatibles</response>
        [HttpPost, Route("IMPORTAR")]
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
                        return BadRequest(new string[] { "Archivo no contiene elementos" });
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