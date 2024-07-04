using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebsupplyHHemo.InterfaceNew.Model
{
    public class FormaPagtoModel
    {
        public int CodFormaPagto { get; set; }
        public string Descricao { get; set; }
        public string CodTipo { get; set; }
        public int Dias { get; set; }
        public string Status { get; set; }
    }
}
