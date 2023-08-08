using Microsoft.Extensions.Configuration;
using System.Configuration;

namespace RealizadorDePing.classes
{
    public class GetSecretClass
    {

        private IConfiguration configuration;

        private List<string[]> IpSecrets;

        private string GmailSecretSender;

        private string PasswordSecretSender;

        private string GmailSecretRecever;


        public List<string[]> _IpSecrets
        {
            get
            {
                return IpSecrets;
            }
        }


        public string _GmailSecretSender
        {
            get
            {
                return GmailSecretSender;
            }
        }

        public string _PasswordSecretSender
        {
            get
            {
                return PasswordSecretSender;
            }
        }
        public string _GmailSecretRecever
        {
            get
            {
                return GmailSecretRecever;
            }
        }

        public GetSecretClass()
        {
            IConfiguration appsettingsConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();


            configuration = new ConfigurationBuilder().AddUserSecrets(appsettingsConfig["ApiSettings:ApiKey"]).Build();

            //Opcion A, para que sea uno a un arreglo de tres
            IpSecrets =  configuration.GetSection("IpAdresses").Get<string[][]>().ToList();

            //Opcion B, para que sea uno a uno
            //configuration.GetSection("IpAdresses").Get<string[][]>().SelectMany(subArray => subArray.Select(val => val)).ToList();

            GmailSecretSender = configuration["GmailSender"];

            PasswordSecretSender = configuration["PasswordSender"];

            GmailSecretRecever = configuration["GmailReciber"];
        }


    }
}
