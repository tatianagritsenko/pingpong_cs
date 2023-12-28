

namespace pong2
{
    public class Program
    {
        public static void Main()
        {
            //string ServerIP = "127.0.0.1";
            //int port = 9000;
            //var client = new Client(ServerIP, port);

            //client.Start();

            //Console.WriteLine("Нажмите на любую клавишу для старта игры...");
            //Console.ReadLine();

            //Game.Play();

            //Выбрать опцию - регистрация или авторизация 
            int opt = InputBox.ChooseOpt();
            Console.WriteLine(opt);
            // 1 - регистрация
            LoginPassword logPass;
            string str = "default";
            logPass.login = str.ToCharArray();
            logPass.password = str.ToCharArray();
            switch (opt) { 
                case 1:
                    logPass = InputBox.Registration();
                    break;
                case 2:
                    logPass = InputBox.Autorization();
                    break;
            }
            //LoginPassword logPass = InputBox.Autorization();
            Console.WriteLine(logPass.login);
            Console.WriteLine(logPass.password);
        }
    }
}













