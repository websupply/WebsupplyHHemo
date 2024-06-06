using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebsupplyHHemo.Interface.Model
{
    public class PedidoCompraModel
    {
        public string UUID { get; set; }
        public string CodFilial { get; set; }
        public string Fornecedor { get; set; }
        public string FornecedorLoja { get; set; }
        public Dadospedido DadosPedido { get; set; }
        public Item[] Itens { get; set; }

        public class Dadospedido
        {
            public string Numero { get; set; }
            public string CondPagto { get; set; }
            public string Emissao { get; set; }
            public Valores Valores { get; set; }
        }

        public class Valores
        {
            public string Despesa { get; set; }
            public string Seguro { get; set; }
            public string Desconto { get; set; }
            public string Frete { get; set; }
            public string Total { get; set; }
        }

        public class Item
        {
            public string NumSeq { get; set; }
            public string CodProd { get; set; }
            public string Produto { get; set; }
            public string Qtd { get; set; }
            public string ValorUnitario { get; set; }
            public string ValorTotal { get; set; }
            public string DataEntregaPrev { get; set; }
            public Impostos Impostos { get; set; }
            public string ValorDespesa { get; set; }
            public string ValorSeguro { get; set; }
            public string ValorDesconto { get; set; }
            public string ValorFrete { get; set; }
            public string CentroCusto { get; set; }
            public string ContaContabil { get; set; }
        }

        public class Impostos
        {
            public string ValorIPI { get; set; }
            public string ValorICMS { get; set; }
        }

    }
}
