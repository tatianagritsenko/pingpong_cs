

namespace pong2
{
    public class Program
    {
        public static void Main()
        {
            string ServerIP = "127.0.0.1";
            int port = 9000;
            var client = new Client(ServerIP, port);

            client.Start();
        }
    }
}













