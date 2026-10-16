using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using Danfe.NFe.Core;
using Danfe.NFe.Core.Modelo;

namespace Danfe.NFe.Core.Tests
{
    [TestClass]
    public class DanfeXmlTests
    {
        public readonly string OutputDirectory = Path.Combine("Output", "DeXml");
        public readonly string InputXmlDirectoryPrefix = Path.Combine("Xml", "NFe");

        public DanfeXmlTests()
        {
            if (!Directory.Exists(OutputDirectory))
                Directory.CreateDirectory(OutputDirectory);
        }

        [Ignore("Caminho hardcoded do autor original — habilite manualmente apontando para um XML local.")]
        [TestMethod]
        public void TestXml()
        {
            var xmlPath = @"C:\Laranjeiras\Xmls\XmlNFeClientes\NFe33190611367874000106550010000002311000324326.xml";
            var outPdfFilePath = Path.Combine(OutputDirectory, Path.GetFileNameWithoutExtension(xmlPath) + ".pdf");
            var model = DanfeViewModelCreator.CriarDeArquivoXml(xmlPath);
            using (var danfe = new DanfeDoc(model))
            {
                danfe.Gerar();
                danfe.Salvar(outPdfFilePath);
            }
        }
        public int TestXml(String xmlPath)
        {
            var outPdfFilePath = Path.Combine(OutputDirectory, Path.GetFileNameWithoutExtension(xmlPath) + ".pdf");
            var model = DanfeViewModelCreator.CriarDeArquivoXml(Path.Combine(InputXmlDirectoryPrefix, xmlPath));
            using (var danfe = new DanfeDoc(model))
            {
                danfe.Gerar();
                danfe.Salvar(outPdfFilePath);
                return danfe.PdfDocument.PageCount;
            }
        }

        [TestMethod]
        public void v1() => TestXml("v1.00/v1.xml");

        [TestMethod]
        public void v2_Retrato() => TestXml("v2.00/v2_Retrato.xml");

        [TestMethod]
        public void v3_10_Retrato() => TestXml("v3.10/v3.10_Retrato.xml");

        [TestMethod]
        public void v4_ComLocalEntrega() => TestXml("v4.00/v4_ComLocalEntrega.xml");

        [TestMethod]
        public void v4_ComLocalRetirada() => TestXml("v4.00/v4_ComLocalRetirada.xml");

        [TestMethod]
        public void v4_Retrato() => Assert.AreEqual(1, TestXml("v4.00/v4_Retrato.xml"));

        [TestMethod]
        public void v4_Paisagem() => Assert.AreEqual(1, TestXml("v4.00/v4_Paisagem.xml"));

        [TestMethod]
        public void v4_MuitosItens() => Assert.IsTrue(TestXml("v4.00/v4_MuitosItens.xml") > 1);

        [TestMethod]
        public void v4_MuitosItensPaisagem() => Assert.IsTrue(TestXml("v4.00/v4_MuitosItensPaisagem.xml") > 1);

        [TestMethod]
        public void v4_ComLocalEntregaRetirada() => TestXml("v4.00/v4_ComLocalEntregaRetirada.xml");

        [TestMethod]
        public void v4_Issqn() => TestXml("v4.00/v4_Issqn.xml");

        [TestMethod]
        public void v4_InfCplLonga() => TestXml("v4.00/v4_InfCplLonga.xml");

        [TestMethod]
        public void v4_SemVTotTrib() => TestXml("v4.00/v4_SemVTotTrib.xml");

        [TestMethod]
        public void v4_Homologacao() => TestXml("v4.00/v4_Homologacao.xml");

        [TestMethod]
        public void v4_ContingenciaSvcAn() => TestXml("v4.00/v4_ContingenciaSvcAn.xml");

        [TestMethod]
        public void v4_VTotTribPresenteEAusente()
        {
            var com = DanfeViewModelCreator.CriarDeArquivoXml(Path.Combine(InputXmlDirectoryPrefix, "v4.00/v4_Retrato.xml"));
            var sem = DanfeViewModelCreator.CriarDeArquivoXml(Path.Combine(InputXmlDirectoryPrefix, "v4.00/v4_SemVTotTrib.xml"));
            Assert.IsNotNull(com.CalculoImposto.ValorAproximadoTributos);
            Assert.IsNull(sem.CalculoImposto.ValorAproximadoTributos);
        }
    }
}
