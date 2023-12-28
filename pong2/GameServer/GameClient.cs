using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Raylib_cs;
using static Raylib_cs.Raylib;
using System.IO;


namespace pong2
{
    public class GameClient
    {
        int Player_score = 0;
        int CPU_score = 0;

        const int screen_width = 1280;
        const int screen_height = 800;

        string ServerIP;
        int port;

        IPAddress ipAddr;
        IPEndPoint ipEndPoint;

        Socket client;

        public GameClient(string ip, int new_port)
        {
            ServerIP = ip;
            port = new_port;
        }
        string SendCommand(params string[] args)
        {
            string message = "";
            //string message = command;
            foreach (var item in args)
                message += item + " ";

            byte[] commandBytes = Encoding.UTF8.GetBytes(message);
            int bytesSent = client.Send(commandBytes);

            byte[] bytes = new byte[1024];
            int bytesRec = client.Receive(bytes);
            string answer = Encoding.UTF8.GetString(bytes, 0, bytesRec);
            
            return answer;
        }

        int GetCode(string answer)
        {
            string[] splitString = answer.Split(' ');

            return int.Parse(splitString[1]);
        }

        public int Play()
        {
            // Устанавливаем удаленную точку для сокета
            ipAddr = IPAddress.Parse(ServerIP);
            ipEndPoint = new IPEndPoint(ipAddr, port);

            client = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            client.Connect(ipEndPoint);

            Console.WriteLine("Starting the game");
            InitWindow(screen_width, screen_height, "My Pong Game!");
            SetTargetFPS(60);

            BallClient ball = new BallClient(screen_width / 2, screen_height / 2, 20); 
            PaddleClient player = new PaddleClient(25, 120, screen_width - 35, screen_height / 2 - 60);
            PaddleClient cpu = new PaddleClient(25, 120, 10, screen_height / 2 - 60);

            while (!WindowShouldClose()) // ESC - выход из игры
            {
                BeginDrawing();

                int KeyUpPressed = 0;
                int KeyDownPressed = 0;
                if (IsKeyDown(KeyboardKey.KEY_UP))
                    KeyUpPressed = 1;
                if (IsKeyDown(KeyboardKey.KEY_DOWN))
                    KeyDownPressed = 1;

                string answer = SendCommand(KeyUpPressed.ToString(), KeyDownPressed.ToString());
                string[] coordinates = answer.Split(", ");

                string[] score = coordinates[5].Split(' ');

                float ballX = float.Parse(coordinates[0]);
                float ballY = float.Parse(coordinates[1]);
                float playerX = float.Parse(coordinates[2]);
                float playerY = float.Parse(coordinates[3]);
                float cpuX = float.Parse(coordinates[4]);
                float cpuY = float.Parse(score[0]);
                CPU_score = int.Parse(score[1]);
                Player_score = int.Parse(score[2]);

                // Updating
                ball.Update(ballX, ballY);
                player.Update(playerX, playerY);
                cpu.Update(cpuX, cpuY);

                // Drawing
                ClearBackground(Colors.Dark_Green);
                DrawRectangle(screen_width / 2, 0, screen_width / 2, screen_height, Colors.Green);
                DrawCircle(screen_width / 2, screen_height / 2, 150, Colors.Light_Green);
                DrawLine(screen_width / 2, 0, screen_width / 2, screen_height, Color.WHITE);
                ball.Draw();
                cpu.Draw();
                player.Draw();
                DrawText(CPU_score.ToString(), screen_width / 4 - 20, 20, 80, Color.WHITE);
                DrawText(Player_score.ToString(), 3 * screen_width / 4 - 20, 20, 80, Color.WHITE);

                EndDrawing();
            }

            CloseWindow();

            client.Shutdown(SocketShutdown.Both);
            client.Close();
            return Player_score;
        }
    }
}
