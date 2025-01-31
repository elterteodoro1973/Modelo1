using System.ComponentModel;
using System.Net;
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
            using (var smtpClient = new SmtpClient(_emailConfiguracao.EnderecoSMTP, _emailConfiguracao.Porta))
            {
                //smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.Credentials = new NetworkCredential(_emailConfiguracao.Email, _emailConfiguracao.Senha.Trim());                
                //smtpClient.UseDefaultCredentials = true;
                smtpClient.EnableSsl = true;
                
                //Configuração da Mensagem
                var mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(_emailConfiguracao.Email);
                mailMessage.To.Add(destinatario);
                //mailMessage.ReplyToList.Add(destinatario);

                if (listaEmailCopias != null) {
                    foreach (var item in listaEmailCopias) {
                        mailMessage.Bcc.Add(destinatario);
                    }                    
                }
                
                mailMessage.Subject = assunto;
                mailMessage.Body = email;
                mailMessage.IsBodyHtml = true;

                //Enviar mensagem
                smtpClient.Send(mailMessage);

                var i = 0;
            }
        }

        //Textos para envio de email:
        public String GetTextoResetSenha(string basePath, String nome, String link)
        {
            string arq = $@"{basePath}\assets\html\GetTextoResetSenha.html";
            string[] Linhas1 = System.IO.File.ReadAllLines(arq);

            string retorno = "";
            foreach (string linha in Linhas1)
            {
                retorno += linha;
            }

            retorno = retorno.Replace("$Nome", nome);
            retorno = retorno.Replace("$Link", link);

            return retorno;
        }

        public String GetCredenciasAcesso(string basePath, String nome, String link)
        {
            string arq = $@"{basePath}\assets\html\GetCredenciasAcesso.html";
            string[] Linhas1 = System.IO.File.ReadAllLines(arq);

            string retorno = "";
            foreach (string linha in Linhas1)
            {
                retorno += linha;
            }

            retorno = retorno.Replace("$Nome", nome);
            retorno = retorno.Replace("$Link", link);

            return retorno;
        }

        public String GetCredenciasPrimeiroAcesso(string basePath, String nome, String link)
        {
            string arq = $@"{basePath}\assets\html\GetCredenciasPrimeiroAcesso.html";
            string[] Linhas1 = System.IO.File.ReadAllLines(arq);

            string retorno = "";
            foreach (string linha in Linhas1)
            {
                retorno += linha;
            }

            retorno = retorno.Replace("$Nome", nome);
            retorno = retorno.Replace("$Link", link);

            return retorno;
        }



    }
}
