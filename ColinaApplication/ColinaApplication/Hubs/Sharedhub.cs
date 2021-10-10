using Microsoft.AspNet.SignalR;


namespace ColinaApplication.Hubs
{
    public class Sharedhub : Hub
    {
        public void RefrescarMesas()
        {
            Clients.All.Mesas();
        }
    }
}