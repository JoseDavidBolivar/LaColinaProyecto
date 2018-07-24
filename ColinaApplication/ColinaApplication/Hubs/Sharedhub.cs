using Microsoft.AspNet.SignalR;


namespace ColinaApplication.Hubs
{
    public class Sharedhub : Hub
    {
        public void Hello()
        {
            Clients.All.Hello("123");
        }
    }
}