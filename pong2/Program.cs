

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

            Console.WriteLine("Нажмите на любую клавишу для старта игры...");
            Console.ReadLine();

            Game.Play();

            // пример красивой формы для ввода данных пользователем
            //InputBox.Example(); 
        }
    }
}













