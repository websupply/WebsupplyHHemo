using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WebsupplyHHemo.Interface.Metodos
{
    public class UnidadesFiliais
    {
        public bool CadastraAtualiza()
        {
            string strMensagem = string.Empty;
            bool retorno = false;

            try
            {
                return true;
            }
            catch (Exception ex)
            {
                strMensagem = ex.Message;
                return false;
            }
        }
    }
}
