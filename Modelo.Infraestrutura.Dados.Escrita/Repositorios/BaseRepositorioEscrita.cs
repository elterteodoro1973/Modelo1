using Modelo.Dominio.Interfaces.Repositorios.Escrita;
using Modelo.Infraestrutura.Dados.Escrita.Contextos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Infraestrutura.Dados.Escrita.Repositorios
{
    public abstract class BaseRepositorioEscrita<T> : IBaseRepositorioEscrita<T> where T : class
    {
        private readonly DaeeCobrancaContexto _contexto;

        public BaseRepositorioEscrita(DaeeCobrancaContexto contexto)
        {
            _contexto = contexto;
        }

        public virtual async Task AbrirTransacao()
        {
            if (_contexto.Database.CurrentTransaction == null)
                await _contexto.Database.BeginTransactionAsync();
        }

        public virtual async Task Adicionar(T entidade)
        {
            await _contexto.Set<T>().AddAsync(entidade);
        }

        public virtual void Atualizar(T entidade)
        {
            _contexto.Set<T>().Update(entidade);
        }

        public async Task Commit()
        {
            if (_contexto.Database.CurrentTransaction != null)
                await _contexto.Database.CommitTransactionAsync();
        }

        public void Dispose()
        {
            _contexto.Dispose();
        }

        public virtual void Excluir(T entidade)
        {
            _contexto.Set<T>().Remove(entidade);
        }

        public async Task Rollback()
        {
            if (_contexto.Database.CurrentTransaction != null)
                await _contexto.Database.RollbackTransactionAsync();
        }

        public async Task SalvarAlteracao()
        {
            await _contexto.SaveChangesAsync();
        }
    }
}
