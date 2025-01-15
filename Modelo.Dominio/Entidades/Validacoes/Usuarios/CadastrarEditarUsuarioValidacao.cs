using Modelo.Dominio.Entidades.Validacoes.Utils;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Dominio.Entidades.Validacoes.Usuarios
{
    public class CadastrarEditarUsuarioValidacao : AbstractValidator<Usuario>
    {

        public CadastrarEditarUsuarioValidacao(bool validarIdentificador = false)
        {
            if (validarIdentificador)
            {
                RuleFor(c => c.Id)
                    .NotEqual(Guid.Empty).WithMessage("O campor identificador de usuário não pode ser vazio");
            }

            RuleFor(c => c.Email)
                .NotNull().WithMessage("O campo {PropertyName} precisa ser fornecido")
                .NotEqual(string.Empty).WithMessage("O campo {PropertyName} não pode ser igual a {ComparisonValue}")
                .MaximumLength(256).WithMessage("O campo {PropertyName} não pode ser maior que {MaxLength} caracteres")
                .EmailAddress().WithMessage("O campo {PropertyName} precisa ser um e-mail válido !");

            RuleFor(c => c.NomeCompleto)
                .NotNull().WithMessage("O campo {PropertyName} precisa ser fornecido")
                .NotEqual(string.Empty).WithMessage("O campo {PropertyName} não pode ser igual a {ComparisonValue}")
                .MaximumLength(256).WithMessage("O campo {PropertyName} não pode ser maior que {MaxLength} caracteres");

            RuleFor(c => c.CPF)
                .NotNull().WithMessage("O campo {PropertyName} precisa ser fornecido")
                .NotEqual(string.Empty).WithMessage("O campo {PropertyName} não pode ser igual a {ComparisonValue}")
                .MaximumLength(11).WithMessage("O campo {PropertyName} não pode ser maior que {MaxLength} caracteres")
                .Must(c => ValidadorCpf.CpfValido(c)).WithMessage("CPF do Usuário inválido !");

        }
    }
}
