using Modelo.Dominio.Entidades.Validacoes.Utils;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Modelo.Web.Configuracoes.Validacoes
{
    public class CustomValidationCPF : ValidationAttribute, IClientValidatable
    {
        public CustomValidationCPF()
        {
            
        }
        public override bool IsValid(object value)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return true;

            bool valido = ValidadorCpf.CpfValido(value.ToString());
            return valido;
        }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            yield return new ModelClientValidationRule
            {
                ErrorMessage = this.FormatErrorMessage(null),
                ValidationType = "customvalidationcpf"
            };
        }
    }
}
