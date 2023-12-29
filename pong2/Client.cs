using System.Net.Sockets;
using System.Net;
using System.Text;
using System.ComponentModel.Design;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static System.Formats.Asn1.AsnWriter;

namespace pong2
{
    public class Client
    {
        const int screenWidth = 800;
        const int screenHeight = 450;

        string ServerIP;
        int port;

        IPAddress ipAddr;
        IPEndPoint ipEndPoint;

        LoginPassword logPass;
        string score;

        string path = "user.txt";
        FileInfo fileInfo;

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
            InitWindow(screenWidth, screenHeight, "STN pong");

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
                logPass.login = file.ReadLine();
                logPass.password = file.ReadLine();
            }

            string answer = SendCommand("LOGIN", logPass.login, logPass.password);

            if (GetCode(answer) == 0)
                Console.WriteLine("An incorrect username or password has been entered");
            else
                Menu();
        }

        private void LoginOrRegistration()
        {
            //Выбрать опцию - регистрация или авторизация 
            int opt = InputBox.ChooseOpt();
            Console.WriteLine(opt);

            string str = "default";
            logPass.login = str;
            logPass.password = str;

            int code = 0; // код, который возвращает сервер

            while (code == 0)
            {
                string answer = "";
                switch (opt)
                {
                    case 1:
                        logPass = InputBox.Registration();
                        answer = SendCommand("REGISTER", logPass.login, logPass.password);
                        break;
                    case 2:
                        logPass = InputBox.Autorization();
                        answer = SendCommand("LOGIN", logPass.login, logPass.password);
                        break;
                }

                code = GetCode(answer);

                switch (code)
                {
                    case 0:
                        switch (opt)
                        {
                            case 1:
                                Console.WriteLine("An incorrect username or password has been entered");
                                break;

                            case 2:
                                Console.WriteLine("Username is already in use");
                                break;
                        }
                        break;

                    case 1:
                        using (StreamWriter file = new StreamWriter(path, false, Encoding.UTF8))
                        {
                            file.WriteLine(logPass.login);
                            file.WriteLine(logPass.password);
                        }
                        Menu();
                        break;
                }
            }
        }


        private void Menu()
        {
            while (!WindowShouldClose())
            {
                int menu = InputBox.Menu();

                switch (menu)
                {
                    case 1:
                        RunGame();
                        break;

                    case 2:
                        Profile();
                        break;

                    case 3:
                        CloseWindow();
                        break;
                }
            }
        }

        private void RunGame()
        {
            string answer = SendCommand("RUNGAME", logPass.login, logPass.password);
            switch (GetCode(answer))
            {
                case 0:
                    Console.WriteLine("An incorrect username or password has been entered");
                    break;

                default:
                    
                    string[] splitAnswer = answer.Split(' ');
                    int newPort = int.Parse(splitAnswer[1]);

                    CloseWindow();
                    var game = new GameClient(ServerIP, newPort);
                    int score = game.Play();

                    InitWindow(screenWidth, screenHeight, "STN pong");
                    SendCommand("ENDGAME", logPass.login, score.ToString());
                    break;
            }
        }
        private void Profile()
        {
            while (!WindowShouldClose())
            {
                string answer = SendCommand("GETSCORE", logPass.login);
                switch (GetCode(answer))
                {
                    case 0:
                        Console.WriteLine("An incorrect username has been entered");
                        break;

                    case 1:
                        string[] splitString = answer.Split(' ');
                        score = splitString[splitString.Length - 1];
                        break;
                }
                Console.WriteLine($"Кол-во очков: {score}");

                answer = SendCommand("TOPSCORE");
                string[] splitAnswer = answer.Split(' ');
                int count = (splitAnswer.Length - 5) / 2;

                string[] names = new string[count];
                int[] scores = new int[count];

                for (int i = 0; i < count; i++)
                {
                    names[i] = splitAnswer[5 + i * 2];
                    scores[i] = int.Parse(splitAnswer[6 + i * 2]);
                }

                int choice = InputBox.Profile(logPass.login, names, scores);

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
                        string answerFromServer = SendCommand("DELETE", logPass.login, pass);
                        switch (GetCode(answerFromServer))
                        {
                            case 0:
                                Console.WriteLine("An incorrect username or password has been entered");
                                break;

                            case 1:
                                fileInfo.Delete();
                                LoginOrRegistration();
                                break;
                        }
                        break;

                    case 4:
                        Menu();
                        break;
                }
            }
        }

        private void ChangeName()
        {
            string newUserName = InputBox.ChangeName();
            string answer = SendCommand("CHANGE", logPass.login, logPass.password, newUserName);

            switch (GetCode(answer))
            {
                case 0:
                    Console.WriteLine("An incorrect username or password has been entered or new name already in use");
                    break;

                case 1:
                    logPass.login = newUserName;
                    using (StreamWriter file = new StreamWriter(path, false, Encoding.UTF8))
                    {
                        file.WriteLine(logPass.login);
                        file.WriteLine(logPass.password);
                    }
                    break;
            }
        }
    }
}
