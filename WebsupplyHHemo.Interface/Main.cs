using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebsupplyHHemo.Interface.Metodos;

namespace WebsupplyHHemo.Interface
{
    public class Main
    {
        public string CGC { get; set; }
        public string CC { get; set; }
        public string REQ { get; set; }
        public string strMensagem { get; set; }

        public bool CadastraUnidade()
        {
            dynamic retorno = new { };

            UnidadesFiliaisMetodo unidadesFiliais = new UnidadesFiliaisMetodo();

            retorno = unidadesFiliais.CadastraAtualiza("26398136000195");

            strMensagem = retorno.strMensagem;

            return retorno.status;
        }
    }
}
