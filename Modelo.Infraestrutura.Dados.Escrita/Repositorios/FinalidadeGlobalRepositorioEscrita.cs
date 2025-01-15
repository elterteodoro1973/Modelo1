using Modelo.Dominio.Interfaces.Repositorios.Escrita;
using Modelo.Dominio.Modelos;
using Modelo.Infraestrutura.Dados.Escrita.Contextos;

namespace Modelo.Infraestrutura.Dados.Escrita.Repositorios
{    
    public class FinalidadeGlobalRepositorioEscrita : BaseRepositorioEscrita<FinalidadeGlobal>, IFinalidadeGlobalRepositorioEscrita
    {
        private readonly DaeeCobrancaContexto _contexto;
        public FinalidadeGlobalRepositorioEscrita(DaeeCobrancaContexto contexto) : base(contexto)
        {
            _contexto = contexto;
        }
    }
}
