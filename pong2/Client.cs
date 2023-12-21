using System.Net.Sockets;
using System.Net;
using System.Text;

namespace pong2
{
    public class Client
    {
        private string ServerIP;
        private int port;

        public Client(string ip, int port)
        {
            ServerIP = ip;
            this.port = port;
        }

        public void Start()
        {
            // Устанавливаем удаленную точку для сокета
            IPAddress ipAddr = IPAddress.Parse(ServerIP);
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, port);

            Socket client = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            client.Connect(ipEndPoint);
            Console.WriteLine("Клиент соединился с {0} ", client.RemoteEndPoint.ToString());

            // Заготовка для меню

            /*Console.WriteLine("_______МЕНЮ_______");
            Console.WriteLine("1. Начать игру");
            Console.WriteLine("2. Профиль");
            Console.WriteLine("3. Выход");

            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 0:
                    Game.Play();
                    break;
                case 1:

                    break;
                case 2:
                    break;
                default:
                    // обработка ошибки
                    break;
            }*/

            Console.Write("Введите сообщение: "); // REGISTER login password
            string message = Console.ReadLine();
            Console.WriteLine();
            
            byte[] msg = Encoding.UTF8.GetBytes(message);

            // Отправляем данные через сокет
            int bytesSent = client.Send(msg);
            Console.WriteLine($"Отправлено {bytesSent} байт\n");

            // Буфер для входящих данных
            byte[] bytes = new byte[1024];

            // Получаем ответ от сервера
            int bytesRec = client.Receive(bytes);

            Console.WriteLine("Ответ от сервера: {0}", Encoding.UTF8.GetString(bytes, 0, bytesRec));
            Console.WriteLine($"Получено {bytesRec} байт\n");
        }

        // Заготовка для отправки команд REGISTER, LOGIN, CHANGE и т.д.
        private void SendCommand(Socket socket, string command)
        {
            byte[] commandBytes = System.Text.Encoding.ASCII.GetBytes(command + "\r\n");
            socket.Send(commandBytes, 0, commandBytes.Length, SocketFlags.None);
        }
    }
}
