
using System.Net.NetworkInformation;
namespace RealizadorDePing.classes
{
    class PingCommand : IDisposable
    {
        private string IpAdress { get; }
        private readonly Ping pinger;
        private PingReply reply;
        bool pingable = false;
        public DateTime lastPing;
        IPStatus status;
        public PingCommand(string _ipAdress)
        {
            this.IpAdress = _ipAdress;
            pinger = new Ping();
        }
        public void sendAsync()
        {
            if (!(this.IpAdress == string.Empty) || !(this.IpAdress.Length == 0))
            {
                this.reply = pinger.Send(IpAdress, 120);
                this.pingable = reply.Status == IPStatus.Success;
                this.lastPing = DateTime.Now;
                this.status = reply.Status;
            }
            else
            {
                this.pingable = false;
                this.status = IPStatus.BadDestination;
                this.lastPing = DateTime.Now;
            }
        }
        public void Dispose()
        {
            pinger.Dispose();
        }
        public override string ToString()
        {
            return $"Status: {reply.Status}\nRound Trip Time: {(int)reply.RoundtripTime}ms";
        }
        public (string ip, bool enable, long time, IPStatus status) Result()
        {
            sendAsync();
            return (this.IpAdress, this.pingable, this.reply.RoundtripTime, this.reply.Status);
        }
    }
}
