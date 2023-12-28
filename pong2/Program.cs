
using static Raylib_cs.Raylib;
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
            const int screenWidth = 800;
            const int screenHeight = 450;

            InitWindow(screenWidth, screenHeight, "STN pong");
            int opt = InputBox.ChooseOpt();
            Console.WriteLine(opt);
            // 1 - регистрация
            LoginPassword logPass;
            string str = "default";
            logPass.login = str;
            logPass.password = str;
            switch (opt) {
                case 1:
                    logPass = InputBox.Registration();
                    break;
                case 2:
                    // после проверки если нашелся - пускаем в меню, а вот если не нашелся 
                    // я хуй знает
                    logPass = InputBox.Autorization();
                    break;
            }
            Console.WriteLine(logPass.login);
            Console.WriteLine(logPass.password);
            int menu = InputBox.Menu();

            String[] names = new String[5] { "Olya", "Tanya", "Nastya", "Sergey", "Sasha" };
            int[] scores = new int[5] { 12, 9, 8, 7, 6 };

            if (menu == 2)
            {
                //поле структуры field:
                //4 - вернуться в меню
                //2 - выход из профиля+ открыть choseOpt
                //3 - удалить профиль
                // ecли в структуре поле login\password == "not changed" - оставляем пароль и логин неизменнымим!
                // если логин не "not changed" - поменять на новый логин
                // и в профиль функцию надо отправить данные по топ скор
                LoginPassword MayBeSmNew = InputBox.Profile(logPass.login, names, scores ); 
            }
            CloseWindow();
        }
    }
}













