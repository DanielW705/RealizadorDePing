
string Email = "*******************";
static void Initialize()
{
    RegistryKey rktApp = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);//Este abre para el regedit
    if (rktApp.GetValue("MedidorTemperatura") == null)
    {
        rktApp.SetValue("MedidorTemperatura", $"\"{ Application.ExecutablePath.ToString()}\"");
    }
}
Initialize();
do
{
    Thread.Sleep(10800000);
    try
    {
        using (EmailSend sender = new EmailSend(Email, "*********", Email))
        {
            sender.SendMensage();
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show(ex.Message);
    }
} while (true);
