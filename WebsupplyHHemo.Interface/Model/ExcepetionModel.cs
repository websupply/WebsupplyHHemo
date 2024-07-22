using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebsupplyHHemo.Interface.Model
{
    public class ExcepetionModel
    {
        public StackTrace Rastreamento { get; set; }
        public int Linha { get; set; }
        public int Coluna { get; set; }
        public string Descricao { get; set; }
        public string Arquivo { get; set; }
        public string Funcao { get; set; }
        public string Mensagem { get; set; }

        public ExcepetionModel(Exception excecao, bool dadosArquivo)
        {
            // Define o StackTrace
            Rastreamento = new StackTrace(excecao, dadosArquivo);

            // Pega a Linha da Excepetion
            Linha = Rastreamento.GetFrame(0).GetFileLineNumber();

            // Pega a Coluna da Excepetion
            Coluna = Rastreamento.GetFrame(0).GetFileColumnNumber();

            // Pega o Nome do Arquivo da Excepetion
            Arquivo = Rastreamento.GetFrame(0).GetFileName();

            // Pega a Função/Metodo que ocorreu a Excepetion
            Funcao = Rastreamento.GetFrame(0).GetMethod().ToString();

            // Pega a Mensagem interna da Excepetion
            Descricao = excecao.Message;

            // Define a mensagem padrão da Excepetiond
            Mensagem = $"Ocorreu a seguinte excepetion: {Descricao} em {Funcao} no arquivo {Arquivo} ({Linha}, {Coluna})";
        }
    }
}
