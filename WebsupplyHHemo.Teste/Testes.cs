using WebsupplyHHemo.Interface.Metodos;

namespace WebsupplyHHemo.Teste
{
    [TestClass]
    public class Testes
    {
        [TestMethod]
        public void InterfaceCentroCusto()
        {
            CentroCustoMetodo centroCusto = new CentroCustoMetodo();

            Console.WriteLine(centroCusto.ConsomeWS());
            Console.WriteLine(centroCusto.strMensagem);
        }

        [TestMethod]
        public void InterfaceCondicaoPagto()
        {
            CondicaoPagtoMetodo condicaoPagto = new CondicaoPagtoMetodo();

            Console.WriteLine(condicaoPagto.ConsomeWS());
            Console.WriteLine(condicaoPagto.strMensagem);
        }

        [TestMethod]
        public void InterfaceFormaPagto()
        {
            FormaPagtoMetodo formaPagto = new FormaPagtoMetodo();

            Console.WriteLine(formaPagto.ConsomeWS());
            Console.WriteLine(formaPagto.strMensagem);
        }

        [TestMethod]
        public void InterfaceFornecedores()
        {
            FornecedorMetodo fornecedorMetodo = new FornecedorMetodo();

            fornecedorMetodo.intCodForWebsupply = 323747;

            Console.WriteLine($"Retorno: {fornecedorMetodo.ConsomeWS()}");
            Console.WriteLine($"strMensagem: {fornecedorMetodo.strMensagem}");
            Console.WriteLine($"intCodForWebsupply: {fornecedorMetodo.intCodForWebsupply}");
            Console.WriteLine($"strCodForProtheus: {fornecedorMetodo.strCodForProtheus}");
            Console.WriteLine($"strCodLojaProtheus: {fornecedorMetodo.strCodLojaProtheus}");
        }

        [TestMethod]
        public void InterfaceGruposItens()
        {
            GrupoItensMetodo grupoItensMetodo = new GrupoItensMetodo();

            grupoItensMetodo.strCodGrpItmWebsupply = "AB0000";

            Console.WriteLine($"Retorno: {grupoItensMetodo.ConsomeWS()}");
            Console.WriteLine($"strMensagem: {grupoItensMetodo.strMensagem}");
            Console.WriteLine($"strCodGrpItmWebsupply: {grupoItensMetodo.strCodGrpItmWebsupply}");
            Console.WriteLine($"strCodGrpItmProtheus: {grupoItensMetodo.strCodGrpItmProtheus}");
        }

        [TestMethod]
        public void InterfaceNatureza()
        {
            NaturezaMetodo naturezaMetodo = new NaturezaMetodo();

            Console.WriteLine(naturezaMetodo.ConsomeWS());
            Console.WriteLine(naturezaMetodo.strMensagem);
        }

        [TestMethod]
        public void InterfacePedidoCompra()
        {
            PedidoCompraMetodo pedidoCompraMetodo = new PedidoCompraMetodo();

            pedidoCompraMetodo.intCodPedComWebsupply = 2359956;

            Console.WriteLine($"Retorno: {pedidoCompraMetodo.ConsomeWS()}");
            Console.WriteLine($"strMensagem: {pedidoCompraMetodo.strMensagem}");
            Console.WriteLine($"intCodPedComWebsupply: {pedidoCompraMetodo.intCodPedComWebsupply}");
            Console.WriteLine($"strCodPedComProtheus: {pedidoCompraMetodo.strCodPedComProtheus}");
        }

        [TestMethod]
        public void InterfacePlanoConta()
        {
            PlanoContaMetodo planoConta = new PlanoContaMetodo();

            Console.WriteLine(planoConta.ConsomeWS());
            Console.WriteLine(planoConta.strMensagem);
        }

        [TestMethod]
        public void InterfaceRecebimento()
        {
            RecebimentoMetodo recebimentoMetodo = new RecebimentoMetodo();

            recebimentoMetodo.intCodRecComWebsupply = 2362135;
            recebimentoMetodo.intCodNFWebsupply = 12345;
            recebimentoMetodo.strFuncao = "Enviar";

            Console.WriteLine($"Retorno: {recebimentoMetodo.ConsomeWS()}");
            Console.WriteLine($"strMensagem: {recebimentoMetodo.strMensagem}");
            Console.WriteLine($"intCodRecComWebsupply: {recebimentoMetodo.intCodRecComWebsupply}");
            Console.WriteLine($"intCodNFWebsupply: {recebimentoMetodo.intCodNFWebsupply}");
        }

        [TestMethod]
        public void InterfaceTipoOperacao()
        {
            TipoOperacaoMetodo TipoOperacao = new TipoOperacaoMetodo();

            Console.WriteLine(TipoOperacao.ConsomeWS());
            Console.WriteLine(TipoOperacao.strMensagem);
        }

        [TestMethod]
        public void InterfaceUnidadesMedida()
        {
            UnidadeMedidaMetodo unidadeMedida = new UnidadeMedidaMetodo();

            Console.WriteLine(unidadeMedida.ConsomeWS());
            Console.WriteLine(unidadeMedida.strMensagem);
        }

        [TestMethod]
        public void InterfaceUnidadesFiliais()
        {
            UnidadesFiliaisMetodo unidadesFiliaisMetodo = new UnidadesFiliaisMetodo();

            Console.WriteLine(unidadesFiliaisMetodo.ConsomeWS());
            Console.WriteLine(unidadesFiliaisMetodo.strMensagem);
        }

        [TestMethod]
        public void InterfaceUsuarios()
        {
            UsuarioMetodo usuarioMetodo = new UsuarioMetodo();

            Console.WriteLine(usuarioMetodo.ConsomeWS());
            Console.WriteLine(usuarioMetodo.strMensagem);
        }

        [TestMethod]
        public void InterfaceUsuariosUnidades()
        {
            UsuarioUnidadeMetodo usuarioUnidadeMetodo = new UsuarioUnidadeMetodo();

            Console.WriteLine(usuarioUnidadeMetodo.ConsomeWS());
            Console.WriteLine(usuarioUnidadeMetodo.strMensagem);
        }

        [TestMethod]
        public void InterfaceConversaUnidadeMedida()
        {
            ConversaoUnidadeMedidaMetodo conversaoUnidadeMedidaMetodo = new ConversaoUnidadeMedidaMetodo();

            conversaoUnidadeMedidaMetodo.strCodFilial = "01010001";

            Console.WriteLine(conversaoUnidadeMedidaMetodo.ConsomeWS());
            Console.WriteLine(conversaoUnidadeMedidaMetodo.strMensagem);
        }
    }
}