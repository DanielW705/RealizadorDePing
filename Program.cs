using System.Net;

try
{
    //Se inicia una escritura en el regedit para realizar una escritura de regla para que la maquina local cuando se encienda se inicie el proyecto
    RegistryKey rktApp = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

        //Se revisa que este el valor sino se coloca el path de ejecucion
        if (rktApp.GetValue("MedidorTemperatura") == null)
        rktApp.SetValue("MedidorTemperatura", $"\"{Application.ExecutablePath.ToString()}\"");


    GetSecretClass secretClass = new GetSecretClass();

    Clock clock = new Clock(3, TypeTime.hours);
    //Clock clock = new Clock(1, TypeTime.minutes);


    //Realizado con la opcion A
    //PingHandler ping = new PingHandler(
    //    secretClass._IpSecrets.Select(x => IPAddress.Parse(x)).ToList()
    //);
    //Se crea una clase y se hace llamar al arreglo de IP's y con linq se hace el parseo a un arreglo
    PingHandler ping = new PingHandler(
                              //Se selecciona primero el arreglo de cadenas
                                             //Se selecciona despues cada uno de los valores de la cadena y se vuelve a hacer arreglo
        secretClass._IpSecrets.Select(x => x.Select(y => IPAddress.Parse(y)).ToArray()).ToList()
        );
    //Se instancia una nueva clase llamada EmailSend, que enviara el mensaje
    EmailSend emailSend = new EmailSend(secretClass._GmailSecretSender, secretClass._GmailSecretRecever, secretClass._PasswordSecretSender);

    //Se suscribe al evento tick del reloj
    clock.Tick += ping.Pinging;
    
    //Se suscribe al evento para cuando se termine de hacer el ping de todas las Ip's
    ping.ListPinged += emailSend.SendMessage;

    while (true)
        clock.Start();
}
catch (Exception e)
{
    Console.WriteLine(e.ToString());
}

