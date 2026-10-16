using System;
using System.IO;
using System.Runtime.CompilerServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Danfe.NFe.Core;
using Danfe.NFe.Core.Enumeracoes;

namespace Danfe.NFe.Core.Tests
{
    [TestClass]
    public class LogoTests
    {
        public static readonly string OutputDirectoryName = Path.Combine("Output", "ComLogo");

        static LogoTests()
        {
            if (!Directory.Exists(OutputDirectoryName)) Directory.CreateDirectory(OutputDirectoryName);
        }

        public void TestLogo(string logoPath, [CallerMemberName] string pdfName = null)
        {
            var model = FabricaFake.DanfeViewModel_1();
            model.Orientacao = Orientacao.Retrato;
            using (var d = new DanfeDoc(model))
            {
                if (logoPath.EndsWith(".pdf", StringComparison.InvariantCultureIgnoreCase))
                {
                    d.AdicionarLogoPdf(logoPath);
                    model.Emitente.NomeFantasia = "Logo Vetor Ltda.";
                }
                else
                {
                    d.AdicionarLogoImagem(logoPath);
                    model.Emitente.NomeFantasia = "Logo Raster Ltda.";
                }

                d.Gerar();
                d.Salvar(Path.Combine(OutputDirectoryName, pdfName + ".pdf"));
            }
        }

        [TestMethod]
        public void LogoQuadradoJPG() => TestLogo("Logos/JPG/Quadrado.jpg");

        [TestMethod]
        public void LogoHorizontalJPG() => TestLogo("Logos/JPG/Horizontal.jpg");

        [TestMethod]
        public void LogoVerticalJPG() => TestLogo("Logos/JPG/Vertical.jpg");

        [TestMethod]
        public void LogoQuadradoPDF() => TestLogo("Logos/PDF/Quadrado.pdf");

        [TestMethod]
        public void LogoHorizontalPDF() => TestLogo("Logos/PDF/Horizontal.pdf");

        [TestMethod]
        public void LogoVerticalPDF() => TestLogo("Logos/PDF/Vertical.pdf");

        [TestMethod]
        public void LogoQuadradoPNG() => TestLogo("Logos/PNG/Quadrado.png");

        [TestMethod]
        public void LogoHorizontalPNG() => TestLogo("Logos/PNG/Horizontal.png");

        [TestMethod]
        public void LogoVerticalPNG() => TestLogo("Logos/PNG/Vertical.png");

        public void TestLogoStream(string logoPath, [CallerMemberName] string pdfName = null)
        {
            var model = FabricaFake.DanfeViewModel_1();
            model.Orientacao = Orientacao.Retrato;
            using (var d = new DanfeDoc(model))
            {
                // O stream é descartado antes de Gerar(): o logo não pode depender dele.
                using (var stream = new MemoryStream(File.ReadAllBytes(logoPath)))
                {
                    if (logoPath.EndsWith(".pdf", StringComparison.InvariantCultureIgnoreCase))
                        d.AdicionarLogoPdf(stream);
                    else
                        d.AdicionarLogoImagem(stream);
                }

                d.Gerar();
                d.Salvar(Path.Combine(OutputDirectoryName, pdfName + ".pdf"));
            }
        }

        [TestMethod]
        public void LogoStreamJPG() => TestLogoStream("Logos/JPG/Quadrado.jpg");

        [TestMethod]
        public void LogoStreamPNG() => TestLogoStream("Logos/PNG/Quadrado.png");

        [TestMethod]
        public void LogoStreamPDF() => TestLogoStream("Logos/PDF/Quadrado.pdf");

        [TestMethod]
        public void LogoFormatoNaoSuportado()
        {
            // GIF 1x1: o ImageSharp aceitava, o PDFsharp não.
            var gif = Convert.FromBase64String("R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7");
            using (var d = new DanfeDoc(FabricaFake.DanfeViewModel_1()))
            {
                Assert.ThrowsException<NotSupportedException>(() => d.AdicionarLogoImagem(new MemoryStream(gif)));
            }
        }
    }
}
