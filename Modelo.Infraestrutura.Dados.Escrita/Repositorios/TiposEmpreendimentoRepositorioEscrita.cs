using Modelo.Dominio.Interfaces.Repositorios.Escrita;
using Modelo.Dominio.Modelos;
using Modelo.Infraestrutura.Dados.Escrita.Contextos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Infraestrutura.Dados.Escrita.Repositorios
{
    public class TiposEmpreendimentoRepositorioEscrita : BaseRepositorioEscrita<TiposEmpreendimento>, ITiposEmpreendimentoRepositorioEscrita
    {
        private readonly DaeeCobrancaContexto _contexto;
        public TiposEmpreendimentoRepositorioEscrita(DaeeCobrancaContexto contexto) : base(contexto)
        {
            _contexto = contexto;
        }
    }
}
