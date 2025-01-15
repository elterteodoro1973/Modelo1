using System.Net.Mail;
using Microsoft.Extensions.Options;
using Modelo.Dominio.DTO;
using Modelo.Dominio.Interfaces.Servicos;

namespace Modelo.Dados.Servicos
{
    public class EmailServico : IEmailServico
    {
        private readonly EmailConfiguracao _emailConfiguracao;
        public EmailServico(IOptions<EmailConfiguracao> emailConfiguracao)
        {
            _emailConfiguracao = emailConfiguracao.Value;
        }

        public async Task Enviar(string destinatario, string assunto, string email, IList<string>? listaEmailCopias = null)
        {

            //if (email.Contains("://localhost:"))
            //{
            //    _emailConfiguracao.EnderecoSMTP = "smtps.bol.com.br";
            //    _emailConfiguracao.Senha = "*****";
            //    _emailConfiguracao.Email = "elter.teodoro@bol.com.br";
            //    _emailConfiguracao.UsarSSL = true;
            //    _emailConfiguracao.Porta = 587;
            //}

            using (var smtpClient = new SmtpClient(_emailConfiguracao.EnderecoSMTP, _emailConfiguracao.Porta))
            {
                //smtpClient.UseDefaultCredentials = false;
                //smtpClient.Credentials = new NetworkCredential(_emailConfiguracao.Email, _emailConfiguracao.Senha.Trim());
                //smtpClient.EnableSsl = true;
                //smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;


                //Configuração da Mensagem
                var mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(_emailConfiguracao.Email);
                mailMessage.To.Add(destinatario);
                mailMessage.Subject = assunto;
                mailMessage.Body = email;
                mailMessage.IsBodyHtml = true;

                //Enviar mensagem
                smtpClient.Send(mailMessage);
            }
        }



        //Textos para envio de email:
        public String GetTextoResetSenha(string BasePath, String Nome, String link)
        {
            string arq = $@"{BasePath}\assets\html\GetTextoResetSenha.html";
            string[] Linhas1 = System.IO.File.ReadAllLines(arq);

            string retorno = "";
            foreach (string linha in Linhas1)
            {
                retorno += linha;
            }

            retorno = retorno.Replace("$Nome", Nome);
            retorno = retorno.Replace("$Link", link);

            return retorno;
        }

        public String GetCredenciasAcesso(string BasePath, String Nome, String link)
        {
            string arq = $@"{BasePath}\assets\html\GetCredenciasAcesso.html";
            string[] Linhas1 = System.IO.File.ReadAllLines(arq);

            string retorno = "";
            foreach (string linha in Linhas1)
            {
                retorno += linha;
            }

            retorno = retorno.Replace("$Nome", Nome);
            retorno = retorno.Replace("$Link", link);

            return retorno;
        }

        public String GetCredenciasPrimeiroAcesso(string BasePath, String Nome, String link)
        {
            string arq = $@"{BasePath}\assets\html\GetCredenciasPrimeiroAcesso.html";
            string[] Linhas1 = System.IO.File.ReadAllLines(arq);

            string retorno = "";
            foreach (string linha in Linhas1)
            {
                retorno += linha;
            }

            retorno = retorno.Replace("$Nome", Nome);
            retorno = retorno.Replace("$Link", link);

            return retorno;
        }



    }
}
