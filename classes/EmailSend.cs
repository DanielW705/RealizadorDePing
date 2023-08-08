using System.Net;
using System.Net.Mail;
using System.IO;

namespace RealizadorDePing.classes
{
    class EmailSend : IDisposable
    {
        private string email;
        private string password;
        private string email_reciber;
        private readonly List<PingCommand> _commands = new List<PingCommand>();
        private MailMessage _mailMessage;
        private SmtpClient smtp;
        private string ruta = $@"{Directory.GetCurrentDirectory()}\IPs.txt";
        List<string> Ip_S = new List<string>();
        public EmailSend(string _email, string _password, string _email_reciber)
        {
            if (File.Exists(this.ruta))
            {
                using (StreamReader sr = new StreamReader(this.ruta))
                {
                    Ip_S = sr.ReadToEnd().Replace("\r", string.Empty).Split('\n').ToList();
                    Ip_S.Add("8.8.8.8");
                }
                this.email = _email;
                this.password = _password;
                this.email_reciber = _email_reciber;
                this.smtp = new SmtpClient("smtp.office365.com")
                {
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Host = "smtp.office365.com",
                    Port = 587,
                    Credentials = new NetworkCredential(this.email, this.password)
                };
            }
            else
            {
                throw new Exception($"No existe el archivo en el directorio:\n{ruta}");
            }

        }
        private string ConstructMessage()
        {
            string messageCompleto = "<style>" +
            "table, thead th, tbody td{" +
            "border: 1px solid rgba(0,0,0,0.2);" +
            "}" +
            "th,td{" +
            "padding: 10px 20px" +
            "}" +
            "</style>"
            + "<table>" +
            "<thead>" +
            "<tr>" +
            "<th>Ip</th>" +
            "<th>Activo</th>" +
            "<th>Status</th>" +
            "<th>Tiempo de respuesta</th>" +
            "<th>Hora de ping</th>" +
            "</tr>" +
            "</thead>" +
            "<tbody>";

            foreach (string ip in Ip_S)
            {
                PingCommand comando = new PingCommand(ip);
                var tupla = comando.Result();
                string message =
                    $"<tr>" +
                    $"<td>{tupla.ip}</td>" +
                    $"<td>{(tupla.enable ? "Verdadero" : "Falso")}</td>" +
                    $"<td>{tupla.status}</td>" +
                    $"<td>{tupla.time}ms</td>" +
                    $"<td>{comando.lastPing.ToString("dd/MM/yyyy HH:mm:ss")}</td>" +
                    $"</tr>";
                messageCompleto += message;
                this._commands.Add(comando);
            }
            return messageCompleto +=
            "</tbody>" +
            "</table>";
        }
        public void SendMensage()
        {
            this._mailMessage = new MailMessage(this.email, this.email_reciber, "prueba mensaje", ConstructMessage()) { IsBodyHtml = true };
            smtp.Send(this._mailMessage);
        }
        public void Dispose()
        {
            smtp.Dispose();
        }
    }
}
