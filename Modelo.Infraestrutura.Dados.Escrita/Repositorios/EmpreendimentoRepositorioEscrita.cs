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
    public class EmpreendimentoRepositorioEscrita : BaseRepositorioEscrita<Empreendimento>, IEmpreendimentoRepositorioEscrita
    {
        private readonly DaeeCobrancaContexto _contexto;
        public EmpreendimentoRepositorioEscrita(DaeeCobrancaContexto contexto) : base(contexto)
        {
            _contexto = contexto;
        }
        public override void Atualizar(Empreendimento entidade)
        {
            _contexto.Entry<Empreendimento>(entidade).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _contexto.Entry<Empreendimento>(entidade).Property(e => e.Codigo).IsModified = false;
        }
    }
}
