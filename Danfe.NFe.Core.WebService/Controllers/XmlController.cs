using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Danfe.NFe.Core;
using Danfe.NFe.Core.Modelo;

namespace DanfeNFeCore.WebService.Controllers
{
    public class XmlController : Controller
    {
        [HttpPost("/api/xml/pdf/gerar")]
        public async Task<IActionResult> Index()
        {
            string xml = null;

            using (var reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                xml = await reader.ReadToEndAsync();
            }

            var modelo = DanfeViewModelCreator.CriarDeStringXml(xml);

            using (var pdfStream = new MemoryStream())
            {
                using (var danfe = new DanfeDoc(modelo))
                {
                    danfe.Gerar();
                    var bytesPdf = danfe.ObterPdfBytes(pdfStream);
                    return File(bytesPdf, "Application/pdf", $"{modelo.ChaveAcesso}.pdf");
                }
            }
        }
    }
}
