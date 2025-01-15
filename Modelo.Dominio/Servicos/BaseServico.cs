using Modelo.Dominio.Entidades;
using Modelo.Dominio.Interfaces;
using Modelo.Dominio.Interfaces.Servicos;
using Modelo.Dominio.Notificacoes;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Dominio.Servicos
{
    public abstract class BaseServico<T> where T : class
    {
        private readonly INotificador _notificador;
        private readonly ILogServico _logServico;

        public BaseServico(INotificador notificador, ILogServico logServico)
        {
           _notificador = notificador;
            _logServico = logServico;
        }

        protected void Notificar(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                Notificar(error.ErrorMessage);
            }
        }

        protected void Notificar(string mensagem)
        {
            _notificador.Adicionar(new Notificacao(mensagem));
        }

        protected bool ExecutarValidacao<TV, TE>(TV validacao, TE entidade) where TV : AbstractValidator<TE> where TE : class
        {
            var validator = validacao.Validate(entidade);

            if (validator.IsValid) return true;

            Notificar(validator);

            return false;
        }
       
    }
}
