using WebsupplyHHemo.Interface.Metodos;

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
    }
}