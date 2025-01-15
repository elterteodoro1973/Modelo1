using System.Security.Claims;

namespace Modelo.Web.Configuracoes.Claims
{
    public static class ClaimsUtils
    {
        private static IList<Tuple<string, string, string, Claim>> RetornarModuloClaim()
        {

            //// 1- Menu, 2 - Controller, 3 - Tipo Claim + - + Valor Claim, 4 Claim
            var lista = new List<Tuple<string, string, string, Claim>>();

            

            #region Perfis

            lista.Add(new Tuple<string, string, string, Claim>("Perfis", "Perfis", "Perfis - Visualizar", new Claim("Perfis", "Visualizar")));
            lista.Add(new Tuple<string, string, string, Claim>("Perfis", "Perfis", "Perfis - Adicionar", new Claim("Perfis", "Adicionar")));
            lista.Add(new Tuple<string, string, string, Claim>("Perfis", "Perfis", "Perfis - Editar", new Claim("Perfis", "Editar")));
            lista.Add(new Tuple<string, string, string, Claim>("Perfis", "Perfis", "Perfis - Excluir", new Claim("Perfis", "Excluir")));
            lista.Add(new Tuple<string, string, string, Claim>("Perfis", "Perfis", "Perfis - Historico", new Claim("Perfis", "Historico")));


            #endregion


            #region Usuarios

            lista.Add(new Tuple<string, string, string, Claim>("Usuarios", "Usuarios", "Usuarios - Visualizar", new Claim("Usuarios", "Visualizar")));
            lista.Add(new Tuple<string, string, string, Claim>("Usuarios", "Usuarios", "Usuarios - Adicionar", new Claim("Usuarios", "Adicionar")));
            lista.Add(new Tuple<string, string, string, Claim>("Usuarios", "Usuarios", "Usuarios - Editar", new Claim("Usuarios", "Editar")));
            lista.Add(new Tuple<string, string, string, Claim>("Usuarios", "Usuarios", "Usuarios - Excluir", new Claim("Usuarios", "Excluir")));
            lista.Add(new Tuple<string, string, string, Claim>("Usuarios", "Usuarios", "Usuarios - Historico", new Claim("Usuarios", "Historico")));
            lista.Add(new Tuple<string, string, string, Claim>("Usuarios", "Usuarios", "Usuarios - Permissoes", new Claim("Usuarios", "Permissoes")));


            #endregion

            return lista;
        }

        public static IList<Tuple<string, string, string, Claim>> RecuperarListaTuplasModulosClaims()
            => RetornarModuloClaim();

    }
}
