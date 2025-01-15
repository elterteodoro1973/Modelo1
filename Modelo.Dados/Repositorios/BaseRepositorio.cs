using Modelo.Dados.Contexto;
using Modelo.Dominio.Entidades;
using Modelo.Dominio.Interfaces;
using Modelo.Dominio.Interfaces.Repositorios;
using Modelo.Dominio.Interfaces.Servicos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml;

namespace Modelo.Dados.Repositorios
{
    public abstract class BaseRepositorio<T> : IBaseRepositorio<T> where T : EntidadeBase
    {
        private readonly Contexto.DbContexto _contexto;
        private readonly ILogServico _logServico;

        public BaseRepositorio(Contexto.DbContexto contexto, ILogServico logServico)
        {
            _contexto = contexto;
            _logServico = logServico;
        }

        public virtual async Task Adicionar(T entidade)
        {
            await _contexto.Set<T>().AddAsync(entidade);
            var log = new LogTransacoes
            {
                Id = Guid.NewGuid(),
                Data = DateTime.Now,
                Comando = "INSERT",
                EntidadeId = entidade.Id,
                UsuarioId = _logServico.BuscarUsuarioId(),
                Dados = JsonSerializer.Serialize(entidade, BuscarConfiguracaoJSON())
            };
            await _contexto.LogTransacoes.AddAsync(log);

        }


        public virtual async Task<T> BuscarPorId(Guid Id, bool rastreioEntidade = true, bool incluirExcluidas = false)
        {
            if (rastreioEntidade)
                return await _contexto.Set<T>().Where(c => c.Id == Id && (incluirExcluidas ? c.Excluido.Value : !c.Excluido.Value)).FirstOrDefaultAsync();
            else
                return await _contexto.Set<T>().Where(c => c.Id == Id && (incluirExcluidas ? c.Excluido.Value : !c.Excluido.Value)).AsNoTracking().FirstOrDefaultAsync();
        }

        public virtual async Task<IList<T>> BuscarTodos(bool rastreioEntidade = true, bool incluirExcluidas = false)
        {
            if (rastreioEntidade)
                return await _contexto.Set<T>().Where(c => (incluirExcluidas ? true : !c.Excluido.Value)).ToListAsync();
            else
                return await _contexto.Set<T>().Where(c => (incluirExcluidas ? true : !c.Excluido.Value)).AsNoTracking().ToListAsync();
        }

        public virtual async Task Commit()
        {
            if (_contexto.Database.CurrentTransaction != null)
                await _contexto.Database.CommitTransactionAsync();
        }

        public virtual async Task Dispose()
        {
            if (_contexto != null)
                await _contexto.DisposeAsync();
        }

        public virtual async Task IniciarTransacao()
        {
            if (_contexto.Database.CurrentTransaction == null)
                 await _contexto.Database.BeginTransactionAsync();
           
        }

        public virtual async Task Roolback()
        {
            if (_contexto.Database.CurrentTransaction != null)
                await _contexto.Database.RollbackTransactionAsync();
        }

        public virtual async Task SalvarAlteracoes()
        {
            await _contexto.SaveChangesAsync();

        }

        public virtual async Task Atualizar(T entidade)
        {
            _contexto.Set<T>().Entry(entidade).State = EntityState.Modified;
            //var log = new Log
            //{
            //    Id = Guid.NewGuid(),
            //    Data = DateTime.Now,
            //    Comando = "UPDATE",
            //    EntidadeId = entidade.Id,
            //    UsuarioId = _logServico.BuscarUsuarioId(),
            //    Dados = JsonSerializer.Serialize(entidade, BuscarConfiguracaoJSON())
            //};
            //await _contexto.LogTransacoes.AddAsync(log);


        }

        public virtual async Task Excluir(T entidadeBase)
        {
            entidadeBase.Excluido = true;
            _contexto.Set<T>().Update(entidadeBase);
            var log = new LogTransacoes
            {
                Id = Guid.NewGuid(),
                Data = DateTime.Now,
                Comando = "DELETE",
                EntidadeId = entidadeBase.Id,
                UsuarioId = _logServico.BuscarUsuarioId(),
                Dados = JsonSerializer.Serialize(entidadeBase, BuscarConfiguracaoJSON())
            };
            await _contexto.LogTransacoes.AddAsync(log);
        }

        public async Task AdicionarLista(IList<T> entidades)
        {
            await _contexto.Set<T>().AddRangeAsync(entidades);
            //IList<Log> logs = new List<Log>();
            //foreach (var item in entidades)
            //{
            //    var log = new Log
            //    {
            //        Id = Guid.NewGuid(),
            //        Data = DateTime.Now,
            //        Comando = "INSERT",
            //        EntidadeId = item.Id,
            //        UsuarioId = _logServico.BuscarUsuarioId(),
            //        Dados = JsonSerializer.Serialize(item, BuscarConfiguracaoJSON())
            //    };
            //    logs.Add(log);
            //}
            //await _contexto.LogTransacoes.AddRangeAsync(logs);

        }

        public async Task RemoverLista(IList<T> entidades)
        {
            foreach (var item in entidades)
            {
                item.Excluido = true;
            }
            _contexto.Set<T>().UpdateRange(entidades);
            List<LogTransacoes> logs = new List<LogTransacoes>();
            foreach (var item in entidades)
            {
                var log = new LogTransacoes
                {
                    Id = Guid.NewGuid(),
                    Data = DateTime.Now,
                    Comando = "DELETE",
                    EntidadeId = item.Id,
                    UsuarioId = _logServico.BuscarUsuarioId(),
                    Dados = JsonSerializer.Serialize(item, BuscarConfiguracaoJSON())
                };
                logs.Add(log);
            }
            if (logs.Count > 0)
                await _contexto.LogTransacoes.AddRangeAsync(logs);
        }

        public async Task<bool> VerficarSeIdEValido(Guid id)
        => await _contexto.Set<T>().Where(c => !c.Excluido.Value && c.Id == id).AnyAsync();

        private JsonSerializerOptions BuscarConfiguracaoJSON() =>
             new JsonSerializerOptions
             {
                 ReferenceHandler = ReferenceHandler.Preserve
             };

        public async Task AtualizarLista(IList<T> entidades)
        {
            _contexto.Set<T>().UpdateRange(entidades);
            //IList<Log> logs = new List<Log>();
            //foreach (var item in entidades)
            //{
            //    var log = new Log
            //    {
            //        Id = Guid.NewGuid(),
            //        Data = DateTime.Now,
            //        Comando = "UPDATE",
            //        EntidadeId = item.Id,
            //        UsuarioId = _logServico.BuscarUsuarioId(),
            //        Dados = JsonSerializer.Serialize(item, BuscarConfiguracaoJSON())
            //    };
            //    logs.Add(log);
            //}
            //await _contexto.LogTransacoes.AddRangeAsync(logs);
        }

        public virtual async Task<IList<LogTransacoes>?> BuscarLogsPorIds(IList<Guid> ids)
        => await _contexto.LogTransacoes.Where(c => ids.Contains(c.EntidadeId)).Include(c => c.Usuario).AsNoTracking().ToListAsync();



        public async Task<List<LogTransacoes>?> BuscarLogsPorIdsDatas(IList<Guid> ids, DateTime? DataInicial, DateTime? DataFinal)
        {
            if (DataInicial != null && DataFinal != null)
            {
                return await _contexto.LogTransacoes.Where(c => ids.Contains(c.EntidadeId) && (c.Data >= DataInicial.Value && c.Data <= DataFinal.Value)).Include(c => c.Usuario).AsNoTracking().ToListAsync();
            }
            else if (DataInicial != null && DataFinal == null)
            {
                return await _contexto.LogTransacoes.Where(c => ids.Contains(c.EntidadeId) && c.Data >= DataInicial.Value).Include(c => c.Usuario).AsNoTracking().ToListAsync();
            }
            else if (DataInicial == null && DataFinal != null)
            {
                return await _contexto.LogTransacoes.Where(c => ids.Contains(c.EntidadeId) && c.Data <= DataFinal.Value).Include(c => c.Usuario).AsNoTracking().ToListAsync();
            }
            else
            {
                return await _contexto.LogTransacoes.Where(c => ids.Contains(c.EntidadeId)).Include(c => c.Usuario).AsNoTracking().ToListAsync();
            }
        }



        public virtual async Task<IList<LogTransacoes>?> BuscarLogsPorId(Guid id)
        => await _contexto.LogTransacoes.Where(c => c.EntidadeId == id).Include(c => c.Usuario).AsNoTracking().ToListAsync();

        public async Task<List<LogTransacoes>?> BuscarLogsPorIdDatas(Guid id, DateTime? DataInicial, DateTime? DataFinal)
        {
            if (DataInicial != null && DataFinal != null)
            {
                return await _contexto.LogTransacoes.Where(c => c.EntidadeId == id && (c.Data >= DataInicial.Value && c.Data <= DataFinal.Value)).Include(c => c.Usuario).AsNoTracking().ToListAsync();
            }
            else if (DataInicial != null && DataFinal == null)
            {
                return await _contexto.LogTransacoes.Where(c => c.EntidadeId == id && c.Data >= DataInicial.Value).Include(c => c.Usuario).AsNoTracking().ToListAsync();
            }
            else if (DataInicial == null && DataFinal != null)
            {
                return await _contexto.LogTransacoes.Where(c => c.EntidadeId == id && c.Data <= DataFinal.Value).Include(c => c.Usuario).AsNoTracking().ToListAsync();
            }
            else
            {
                return await _contexto.LogTransacoes.Where(c => c.EntidadeId == id).Include(c => c.Usuario).AsNoTracking().ToListAsync();
            }
        }

    }
}

