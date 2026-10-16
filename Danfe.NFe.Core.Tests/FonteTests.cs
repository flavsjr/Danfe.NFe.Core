using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PdfSharp.Fonts;
using Danfe.NFe.Core.Graphics;

namespace Danfe.NFe.Core.Tests
{
    [TestClass]
    public class FonteTests
    {
        [DataTestMethod]
        [DataRow(false, false)]
        [DataRow(true, false)]
        [DataRow(false, true)]
        [DataRow(true, true)]
        public void FonteEmbutida_ResolveTodosOsEstilos(bool negrito, bool italico)
        {
            var resolver = new DanfeFontResolver(usarFontesDoSistema: false);

            var info = resolver.ResolveTypeface("Times New Roman", negrito, italico);

            Assert.IsNotNull(info);
            StringAssert.Contains(info.FaceName, "LiberationSerif");
            Assert.AreEqual(negrito && italico, info.MustSimulateItalic);
            var bytes = resolver.GetFont(info.FaceName);
            Assert.IsNotNull(bytes);
            Assert.IsTrue(bytes.Length > 100000);
        }

        [TestMethod]
        public void FamiliaDesconhecida_NaoResolve()
        {
            Assert.IsNull(new DanfeFontResolver().ResolveTypeface("Fonte Que Nao Existe", false, false));
        }

        [TestMethod]
        public void DanfeDoc_RegistraResolverComoFallback()
        {
            using (new DanfeDoc(FabricaFake.DanfeViewModel_1()))
            {
                Assert.IsInstanceOfType(GlobalFontSettings.FallbackFontResolver, typeof(DanfeFontResolver));
            }
        }

        [TestMethod]
        public void Linux_SemFontesNoSistema_UsaFonteEmbutida()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                Assert.Inconclusive("Só roda no Linux.");

            var diretorios = new[] { "/usr/share/fonts", "/usr/local/share/fonts", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".fonts") };
            var temFonte = diretorios.Where(Directory.Exists)
                .SelectMany(d => Directory.EnumerateFiles(d, "*.ttf", SearchOption.AllDirectories))
                .Any(f => Path.GetFileName(f).StartsWith("times", StringComparison.OrdinalIgnoreCase)
                       || Path.GetFileName(f).StartsWith("Times_New_Roman", StringComparison.OrdinalIgnoreCase)
                       || Path.GetFileName(f).StartsWith("LiberationSerif", StringComparison.OrdinalIgnoreCase));
            if (temFonte)
                Assert.Inconclusive("Há Times New Roman ou Liberation Serif instalada; rode num ambiente sem fontes.");

            byte[] pdf;
            using (var danfe = new DanfeDoc(FabricaFake.DanfeViewModel_1()))
            using (var ms = new MemoryStream())
            {
                danfe.Gerar();
                pdf = danfe.ObterPdfBytes(ms);
            }

            StringAssert.Contains(Encoding.ASCII.GetString(pdf), "/FontName/");
            StringAssert.Contains(Encoding.ASCII.GetString(pdf), "Liberation");
        }
    }
}
