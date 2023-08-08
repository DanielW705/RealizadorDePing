using System.Net;
using System.Net.Mail;
using System.IO;
using Razor.Templating.Core;
namespace RealizadorDePing.classes
{
    class EmailSend
    {
        //Clase privada para el que envia el correo
        private MailAddress emailSender;

        //Clase privada para el que recibe el correo
        private MailAddress emailRecever;

        //Clase para el uso de la comunicacion del correo
        private SmtpClient smtpClient;

        //Clase para construir el mensaje de gmail
        private MailMessage message;

        //Constructor con tres parametros, correo del emisor, correo del receptor y contraseña del emisor
        public EmailSend(string emailSender, string emailRecever, string passwordSender)
        {
            //Se crea el correo del emisor
            this.emailSender = new MailAddress(emailSender);

            //Se crea el correo del receptor
            this.emailRecever = new MailAddress(emailRecever);

            //Se instancia una nueva clase del servicio de correo a usar
            smtpClient = new SmtpClient()
            {
                Host = "smtp.office365.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(emailSender, passwordSender)
            };
        }
        public async void SendMessage(object source, ListPingedEventsArgs e)
        {

            //ES IMPORTANTE GENERAR LA CARPETA "VIEWS" ANTES DE HACER USO DE LAS VISTAS RAZOR, uso de la vista razor, renderiza la vista y maanda al modelo valores
            var invoceHtml = await RazorTemplateEngine.RenderAsync("/Views/message.cshtml", e.Pings);
            //Console.WriteLine(invoceHtml);
            //Se crea el mensaje electronico
            using (message = new MailMessage(emailSender, emailRecever)
            {
                IsBodyHtml = true,
                Subject = "Prueba",
                Body = invoceHtml
            })
            {
                //Se envia el correo electronico por el servicio
                smtpClient.Send(message);
            }
            Console.WriteLine("Se envio el mensaje");

        }
    }
}
