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
    public class ResponsavelEmpreendimentoRepositorioEscrita : BaseRepositorioEscrita<ResponsavelEmpreendimento>, IResponsavelEmpreendimentoRepositorioEscrita
    {
        private readonly DaeeCobrancaContexto _contexto;
        public ResponsavelEmpreendimentoRepositorioEscrita(DaeeCobrancaContexto contexto) : base(contexto)
        {
            _contexto = contexto;
        }
        public override void Excluir(ResponsavelEmpreendimento entidade)
        {
            entidade.Excluido = true;
            _contexto.ResponsavelEmpreendimentos.Update(entidade);
        }
    }
}
