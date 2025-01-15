namespace Modelo.Dominio.Interfaces.Servicos
{
    public interface IEmailServico
    {
        Task Enviar(string destinatario, string assunto, string email, IList<string>? listaEmailCopias = null);

        String GetCredenciasAcesso(String BasePath,  String Nome, String link);

        String GetCredenciasPrimeiroAcesso(String BasePath, String Nome, String link);

        String GetTextoResetSenha(String BasePath,  String Nome, String link);
    }
}
