using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebsupplyHHemo.Interface.Model
{
    public class UsuarioModel
    {
        public string CodUsuario { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Comprador { get; set; }
        public string Solicitante { get; set; }
        public string Status { get; set; }
    }
}
