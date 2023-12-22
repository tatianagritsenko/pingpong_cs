using System.Net.Sockets;
using System.Net;
using System.Text;
using System.ComponentModel.Design;

namespace pong2
{
    public class Client
    {
        string ServerIP;
        int port;

        IPAddress ipAddr;
        IPEndPoint ipEndPoint;

        string login;
        string password;
        string score;

        string path = "user.txt";
        FileInfo fileInfo;

        bool Exit = false; // выход из меню

        public Client(string ip, int port)
        {
            ServerIP = ip;
            this.port = port;
            fileInfo = new FileInfo(path);

            // Устанавливаем удаленную точку для сокета
            ipAddr = IPAddress.Parse(ServerIP);
            ipEndPoint = new IPEndPoint(ipAddr, port);
        }

        public void Start()
        {
            if (fileInfo.Exists)
                AutoLogin();
            else
                LoginOrRegistration();
        }

        string SendCommand(string command, params string[] args)
        {
            Socket client = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            client.Connect(ipEndPoint);
            string message = command;
            foreach (var item in args)
                message += " " + item;

            byte[] commandBytes = Encoding.UTF8.GetBytes(message);
            int bytesSent = client.Send(commandBytes);

            byte[] bytes = new byte[1024];
            int bytesRec = client.Receive(bytes);
            string answer = Encoding.UTF8.GetString(bytes, 0, bytesRec);
            client.Shutdown(SocketShutdown.Both);
            client.Close();
            return answer;
        }

        int GetCode(string answer)
        {
            string[] splitString = answer.Split(' ');

            return int.Parse(splitString[1]);
        }

        void AutoLogin()
        {
            using (StreamReader file = new StreamReader(path, Encoding.UTF8))
            {
                login = file.ReadLine();
                password = file.ReadLine();
            }

            string answer = SendCommand("LOGIN", login, password);

            if (GetCode(answer) == 0)
                Console.WriteLine("An incorrect username or password has been entered");
            else
                Menu();
        }

        private void LoginOrRegistration()
        {
            Console.WriteLine();
            Console.WriteLine("1. Вход");
            Console.WriteLine("2. Регистрация");
            Console.WriteLine();
            Console.WriteLine("3. Выход из приложения");
            int choice = int.Parse(Console.ReadLine());

            if (choice != 1 && choice != 2 && choice != 3)
            {
                Console.WriteLine("Некорректный ввод");
                return;
            }

            if (choice == 3)
                return;

            int code = 0; // код, который возвращает сервер

            while (code == 0)
            {
                Console.Write("Введите логин: ");
                login = Console.ReadLine();

                Console.Write("Введите пароль: ");
                password = Console.ReadLine();

                string answer = "";

                switch (choice)
                {
                    case 1:
                        answer = SendCommand("LOGIN", login, password);
                        break;

                    case 2:
                        answer = SendCommand("REGISTER", login, password);
                        break;

                    case 3:
                        return;

                    default:
                        Console.WriteLine("Некорректный ввод");
                        break;
                }

                code = GetCode(answer);

                switch (code)
                {
                    case 0:
                        switch (choice)
                        {
                            case 1:
                                Console.WriteLine("An incorrect username or password has been entered");
                                break;

                            case 2:
                                Console.WriteLine("Username is already in use");
                                break;

                            default:
                                Console.WriteLine("Некорректный ввод");
                                break;
                        }
                        break;

                    case 1:
                        using (StreamWriter file = new StreamWriter(path, false, Encoding.UTF8))
                        {
                            file.WriteLine(login);
                            file.WriteLine(password);
                        }
                        Menu();
                        break;

                    default:
                        Console.WriteLine("Некорректный код");
                        break;
                }
            }
        }

        private void Menu()
        {
            while (!Exit)
            {
                Console.WriteLine();
                Console.WriteLine("_______МЕНЮ_______");
                Console.WriteLine("1. Начать игру");
                Console.WriteLine("2. Профиль");
                Console.WriteLine("3. Выход");

                int menu = int.Parse(Console.ReadLine());

                switch (menu)
                {
                    case 1:
                        Game.Play();
                        //int endGameScore = Game.player_score - Game.cpu_score;
                        SendCommand("ENDGAME", login, Game.player_score.ToString());

                        break;

                    case 2:
                        Profile();
                        break;

                    case 3:
                        Exit = true;
                        break;

                    default:
                        Console.WriteLine("Некорректный ввод");
                        break;
                }
            }
        }

        private void Profile()
        {
            Console.WriteLine();
            Console.WriteLine("ПРОФИЛЬ");
            Console.WriteLine($"Имя пользователя: {login}");

            string answer = SendCommand("GETSCORE", login);
            switch (GetCode(answer))
            {
                case 0:
                    Console.WriteLine("An incorrect username has been entered");
                    break;

                case 1:
                    string[] splitString = answer.Split(' ');
                    score = splitString[splitString.Length - 1];
                    break;

                default:
                    Console.WriteLine("Некорректный код");
                    break;
            }
            Console.WriteLine($"Кол-во очков: {score}");

            Console.WriteLine();
            Console.WriteLine("TOPSCORE");
            answer = SendCommand("TOPSCORE");
            string[] splitAnswer = answer.Split(' ');
            int place = 1;
            for (int i = 5; i < splitAnswer.Length-1; i += 2, place++)
            {
                Console.WriteLine($"{place}. {splitAnswer[i]} {splitAnswer[i + 1]}");
            }

            Console.WriteLine();
            Console.WriteLine("1. Изменить имя пользователя");
            Console.WriteLine("2. Выход из профиля");
            Console.WriteLine("3. Удалить профиль");
            Console.WriteLine();
            Console.WriteLine("4. Меню");

            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    ChangeName();
                    break;

                case 2:
                    fileInfo.Delete();
                    LoginOrRegistration();
                    break;

                case 3:
                    Console.Write("Введите пароль: ");
                    string pass = Console.ReadLine();
                    string answerFromServer = SendCommand("DELETE", login, pass);
                    switch (GetCode(answerFromServer))
                    {
                        case 0:
                            Console.WriteLine("An incorrect username or password has been entered");
                            break;

                        case 1:
                            fileInfo.Delete();
                            LoginOrRegistration();
                            break;

                        default:
                            break;
                    }
                    break;

                case 4:
                    Menu();
                    break;

                default:
                    Console.WriteLine("Некорректный ввод");
                    break;
            }
        }

        private void ChangeName()
        {
            Console.WriteLine();
            Console.Write("Введите новое имя пользователя: ");
            string newUserName = Console.ReadLine();
            string answer = SendCommand("CHANGE", login, password, newUserName);

            switch (GetCode(answer))
            {
                case 0:
                    Console.WriteLine("An incorrect username or password has been entered or new name already in use");
                    break;

                case 1:
                    login = newUserName;
                    using (StreamWriter file = new StreamWriter(path, false, Encoding.UTF8))
                    {
                        file.WriteLine(login);
                        file.WriteLine(password);
                    }
                    break;

                default:
                    Console.WriteLine("Некорректный код");
                    break;
            }
        }

        /*private string GeneratePass()
        {
            string iPass = "";
            string[] arr = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "B", "C", "D", "F", "G", "H", "J", "K", "L", "M", "N", "P", "Q", "R", "S", "T", "V", "W", "X", "Z", "b", "c", "d", "f", "g", "h", "j", "k", "m", "n", "p", "q", "r", "s", "t", "v", "w", "x", "z", "A", "E", "U", "Y", "a", "e", "i", "o", "u", "y" };
            Random rnd = new Random();
            for (int i = 0; i < 10; i++)
            {
                iPass = iPass + arr[rnd.Next(0, 57)];
            }
            return iPass;
        }*/
    }
}
