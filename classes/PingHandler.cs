
using System.Net;
using System.Net.NetworkInformation;
namespace RealizadorDePing.classes
{
    //Clase que hereda de EventArgs, para que se convierta en un nuevo tipo e argumentos para los delegados
    class ListPingedEventsArgs : EventArgs
    {
        public List<Tuple<PingReply[], DateTime>> Pings;
    }
    class PingHandler
    {

        //private List<IPAddress> IpList = new List<IPAddress>();
        
        //Atributo privado para almacenar los pings que hay en los secretos
        private List<IPAddress[]> IpList = new List<IPAddress[]>();

        //Atributo privado para guardar, el resultado de los pings y el tiempo en que se hizo
        private List<Tuple<PingReply[], DateTime>> Pings;

        //Objeto de solo lectura para realizar el ping entre computadoras
        private readonly Ping pinger;

        //funcion delegada para el evento
        public delegate void ListPingedHandled(object source, ListPingedEventsArgs e);

        //Evento con la funcion delegada
        public event ListPingedHandled ListPinged;

        //Constructor como parametros una lista de arreglos
        public PingHandler(List<IPAddress[]> _IpList)
        {
            //Se encapsula la lista
            IpList = _IpList;
            //Se crea una nueva instancia de Ping
            pinger = new Ping();
        }

        //Se crea la funcion para la suscripcion del evento
        protected virtual void onListPinged(List<Tuple<PingReply[], DateTime>> Results)
        {
            if (ListPinged != null)
                ListPinged(this, new ListPingedEventsArgs { Pings = Results });
            else
                throw new Exception();
        }
        //Se pingea cada uno de las ips para despues guardarse en la lista
        public void Pinging(object source, EventArgs e)
        {
            Pings = new List<Tuple<PingReply[], DateTime>>();
            IpList.ForEach(subarray =>
            {
                Pings.Add(new Tuple<PingReply[], DateTime>(
                subarray.Select(ip =>
                pinger.Send(ip, 120)).ToArray(),
                DateTime.Now
                ));
            });
            //Utilizando la opcion B
            //IpList.ForEach(ip =>
            //{
            //    Pings.Add(
            //        new Tuple<PingReply, DateTime>(
            //            pinger.Send(ip),
            //            DateTime.Now
            //            )
            //        );
            //});
            onListPinged(Pings);
        }
    }
}
