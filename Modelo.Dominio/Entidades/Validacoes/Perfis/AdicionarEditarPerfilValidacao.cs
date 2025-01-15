using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Dominio.Entidades.Validacoes.Perfis
{
    public class AdicionarEditarPerfilValidacao : AbstractValidator<Entidades.Perfis>
    {
        public AdicionarEditarPerfilValidacao(bool validarIdentificador = false)
        {
            if (validarIdentificador)
            {
                RuleFor(c => c.Id)
                    .NotEqual(Guid.Empty).WithMessage("O campo Identificador do Perfil não pode ser igual a {ComparisonValue}");
            }

            RuleFor(c => c.Nome)
                .NotNull().WithMessage("O campo {PropertyName} não pode ser igual a {ComparisonValue}")
                .NotEqual(string.Empty).WithMessage("O campo {PropertyName} não pode ser igual a {ComparisonValue}")
                .MaximumLength(256).WithMessage("O campo {PropertyName} não pode ser maior {MaxLength}");

            RuleFor(c => c.Descricao)
                .MaximumLength(4000).WithMessage("O campo {PropertyName} não pode ser maior {MaxLength}");

            RuleForEach(a => a.PermissoesPerfis).ChildRules(c =>
            {
                RuleFor(c => c.Id)
                    .NotEqual(Guid.Empty).WithMessage("O campo Identificador da Permissão do Perfil não pode ser igual a {ComparisonValue}");

                //c.RuleFor(c => c.Tipo).NotNull().WithMessage("O campo Tipo da Permissão do Perfil não pode ser igual a {ComparisonValue}")
                //.NotEqual(string.Empty).WithMessage("O campo Tipo da Permissão do Perfil não pode ser igual a {ComparisonValue}")
                //.MaximumLength(4000).WithMessage("O campo Tipo da Permissão do Perfil não pode ser maior {MaxLength}");

                //c.RuleFor(c => c.TipoAcessoId).NotNull().WithMessage("O campo Valor da Permissão do Perfil não pode ser igual a {ComparisonValue}")
                //.NotEqual(string.Empty).WithMessage("O campo Valor da Permissão do Perfil não pode ser igual a {ComparisonValue}")
                //.MaximumLength(4000).WithMessage("O campo Valor da Permissão do Perfil não pode ser maior {MaxLength}");

            });
        }
    }
}
