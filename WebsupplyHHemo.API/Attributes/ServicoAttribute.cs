using System;

namespace WebsupplyHHemo.API.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ServicoAttribute : Attribute
    {
        public int IDServico { get; }

        public ServicoAttribute(int idServico)
        {
            IDServico = idServico;
        }
    }
}