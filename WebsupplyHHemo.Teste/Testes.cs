using WebsupplyHHemo.InterfaceNew.Metodos;

namespace WebsupplyHHemo.Teste
{
    [TestClass]
    public class Testes
    {
        [TestMethod]
        public void InterfaceUnidadesFiliais()
        {
            UnidadesFiliaisMetodo unidadesFiliaisMetodo = new UnidadesFiliaisMetodo();

            Console.WriteLine(unidadesFiliaisMetodo.CadastraAtualiza("26398136000195").Result);
            Console.WriteLine(unidadesFiliaisMetodo.strMensagem);
        }

        [TestMethod]
        public void InterfaceNatureza()
        {
            NaturezaMetodo naturezaMetodo = new NaturezaMetodo();

            Console.WriteLine(naturezaMetodo.CadastraAtualizaExclui().Result);
            Console.WriteLine(naturezaMetodo.strMensagem);
        }

        [TestMethod]
        public void InterfaceUsuarios()
        {
            UsuarioMetodo usuarioMetodo = new UsuarioMetodo();

            Console.WriteLine(usuarioMetodo.CadastraAtualiza("26398136000195", "HHEMO", "HHEMO").Result);
            Console.WriteLine(usuarioMetodo.strMensagem);
        }

        [TestMethod]
        public void InterfaceUsuariosUnidades()
        {
            UsuarioUnidadeMetodo usuarioUnidadeMetodo = new UsuarioUnidadeMetodo();

            Console.WriteLine(usuarioUnidadeMetodo.CadastraAtualizaExclui().Result);
            Console.WriteLine(usuarioUnidadeMetodo.strMensagem);
        }

        [TestMethod]
        public void InterfaceUnidadesMedida()
        {
            UnidadeMedidaMetodo unidadeMedida = new UnidadeMedidaMetodo();

            Console.WriteLine(unidadeMedida.CadastraAtualiza().Result);
            Console.WriteLine(unidadeMedida.strMensagem);
        }
    }
}