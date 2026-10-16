using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using Danfe.NFe.Core;
using Danfe.NFe.Core.Enumeracoes;

namespace Danfe.NFe.Core.Tests
{
    [TestClass]
    public class DanfeTest
    {

        [TestMethod]
        public void RetratoSemIcmsInterestadual()
        {
            var model = FabricaFake.DanfeViewModel_1();
            model.Orientacao = Orientacao.Retrato;
            model.ExibirIcmsInterestadual = false;
            var d = new DanfeDoc(model);
            d.Gerar();
            d.SalvarTestePdf();
        }

        [TestMethod]
        public void PaisagemSemIcmsInterestadual()
        {
            var model = FabricaFake.DanfeViewModel_1();
            model.Orientacao = Orientacao.Paisagem;
            model.ExibirIcmsInterestadual = false;
            var d = new DanfeDoc(model);
            d.Gerar();
            d.SalvarTestePdf();
        }

        [TestMethod]
        public void Paisagem_2Canhotos()
        {
            var model = FabricaFake.DanfeViewModel_1();
            model.Orientacao = Orientacao.Paisagem;
            model.QuantidadeCanhotos = 2;
            var d = new DanfeDoc(model);
            d.Gerar();
            d.SalvarTestePdf();
        }

        [TestMethod]
        public void Retrato_2Canhotos()
        {
            var model = FabricaFake.DanfeViewModel_1();
            model.Orientacao = Orientacao.Retrato;
            model.QuantidadeCanhotos = 2;
            var d = new DanfeDoc(model);
            d.Gerar();
            d.SalvarTestePdf();
        }

        [TestMethod]
        public void Paisagem_SemCanhoto()
        {
            var model = FabricaFake.DanfeViewModel_1();
            model.Orientacao = Orientacao.Paisagem;
            model.QuantidadeCanhotos = 0;
            var d = new DanfeDoc(model);
            d.Gerar();
            d.SalvarTestePdf();
        }

        [TestMethod]
        public void Retrato_SemCanhoto()
        {
            var model = FabricaFake.DanfeViewModel_1();
            model.Orientacao = Orientacao.Retrato;
            model.QuantidadeCanhotos = 0;
            var d = new DanfeDoc(model);
            d.Gerar();
            d.SalvarTestePdf();
        }

        [TestMethod]
        public void Contingencia_SVC_AN()
        {
            var model = FabricaFake.DanfeViewModel_1();
            model.TipoEmissao = Danfe.NFe.Core.Esquemas.FormaEmissao.ContingenciaSVCAN;
            model.ContingenciaDataHora = DateTime.Now;
            model.ContingenciaJustificativa = "Aqui vai o motivo da contingência";
            model.Orientacao = Orientacao.Retrato;
            var d = new DanfeDoc(model);
            d.Gerar();
            d.SalvarTestePdf();
        }

        [TestMethod]
        public void Contingencia_SVC_RS()
        {
            var model = FabricaFake.DanfeViewModel_1();
            model.TipoEmissao = Danfe.NFe.Core.Esquemas.FormaEmissao.ContingenciaSVCRS;
            model.ContingenciaDataHora = DateTime.Now;
            model.ContingenciaJustificativa = "Aqui vai o motivo da contingência";
            model.Orientacao = Orientacao.Retrato;
            var d = new DanfeDoc(model);
            d.Gerar();
            d.SalvarTestePdf();
        }

        [TestMethod]
        public void Retrato()
        {
            var model = FabricaFake.DanfeViewModel_1();
            model.Orientacao = Orientacao.Retrato;
            var d = new DanfeDoc(model);
            d.Gerar();
            d.SalvarTestePdf();
        }

        [TestMethod]
        public void OpcaoPreferirEmitenteNomeFantasia_False()
        {
            var model = FabricaFake.DanfeViewModel_1();
            model.Orientacao = Orientacao.Retrato;
            model.PreferirEmitenteNomeFantasia = false;
            var d = new DanfeDoc(model);
            d.Gerar();
            d.SalvarTestePdf();
        }

        [TestMethod]
        public void Paisagem()
        {
            var model = FabricaFake.DanfeViewModel_1();
            model.Orientacao = Orientacao.Paisagem;
            var d = new DanfeDoc(model);
            d.Gerar();
            d.SalvarTestePdf();
        }

        [TestMethod]
        public void RetratoHomologacao()
        {
            var model = FabricaFake.DanfeViewModel_1();
            model.TipoAmbiente = 2;
            model.Orientacao = Orientacao.Retrato;
            var d = new DanfeDoc(model);
            d.Gerar();
            d.SalvarTestePdf();
        }

        [TestMethod]
        public void PaisagemHomologacao()
        {
            var model = FabricaFake.DanfeViewModel_1();
            model.TipoAmbiente = 2;
            model.Orientacao = Orientacao.Paisagem;
            var d = new DanfeDoc(model);
            d.Gerar();
            d.SalvarTestePdf();
        }

        [TestMethod]
        public void ComBlocoLocalEntrega()
        {
            var model = FabricaFake.DanfeViewModel_1();
            model.LocalEntrega = FabricaFake.LocalEntregaRetiradaFake();
            var d = new DanfeDoc(model);
            d.Gerar();
            d.SalvarTestePdf();
        }

        [TestMethod]
        public void ComBlocoLocalRetirada()
        {
            var model = FabricaFake.DanfeViewModel_1();
            model.LocalRetirada = FabricaFake.LocalEntregaRetiradaFake();
            var d = new DanfeDoc(model);
            d.Gerar();
            d.SalvarTestePdf();
        }

        [TestMethod]
        public void SalvarMaisDeUmaVez()
        {
            using (var d = new DanfeDoc(FabricaFake.DanfeViewModel_1()))
            {
                d.Gerar();
                var primeiro = new MemoryStream();
                d.Salvar(primeiro);
                var segundo = d.ObterPdfBytes(new MemoryStream());

                Assert.IsTrue(primeiro.Length > 0);
                Assert.AreEqual(0, primeiro.Position);
                Assert.IsTrue(segundo.Length > 0);
                Assert.AreEqual(3, d.PdfDocument.PageCount);
            }
        }
    }
}