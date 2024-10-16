﻿using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace pong2
{
    public static class Game
    {
        public static int Player_score { get; set; } = 0;
        public static int Cpu_score { get; set; } = 0;

        const int screen_width = 1280;
        const int screen_height = 800;

        public static void Play()
        {
            Console.WriteLine("Starting the game");
            InitWindow(screen_width, screen_height, "My Pong Game!");
            SetTargetFPS(60);

            Ball ball = new Ball(screen_width / 2, screen_height / 2, 7, 7, 20);
            Paddle player = new Paddle(25, 120, screen_width - 35, screen_height / 2 - 60, 6);
            CPUPaddle cpu = new CPUPaddle(25, 120, 10, screen_height / 2 - 60, 6);

            while (!WindowShouldClose()) // ESC - выход из игры
            {
                BeginDrawing();

                // Updating
                ball.Update();
                player.Update();
                cpu.Update((int)ball.y);

                // Checking for collisions
                if (CheckCollisionCircleRec(new Vector2(ball.x, ball.y), ball.radius, new Rectangle(player.X, player.Y, player.Width, player.Height)))
                {
                    ball.speed_x *= -1;
                }
                if (CheckCollisionCircleRec(new Vector2(ball.x, ball.y), ball.radius, new Rectangle(cpu.X, cpu.Y, cpu.Width, cpu.Height)))
                {
                    ball.speed_x *= -1;
                }

                // Drawing
                ClearBackground(Colors.Dark_Green);
                DrawRectangle(screen_width / 2, 0, screen_width / 2, screen_height, Colors.Green);
                DrawCircle(screen_width / 2, screen_height / 2, 150, Colors.Light_Green);
                DrawLine(screen_width / 2, 0, screen_width / 2, screen_height, Color.WHITE);
                ball.Draw();
                cpu.Draw();
                player.Draw();
                DrawText(Cpu_score.ToString(), screen_width / 4 - 20, 20, 80, Color.WHITE);
                DrawText(Player_score.ToString(), 3 * screen_width / 4 - 20, 20, 80, Color.WHITE);

                EndDrawing();
            }

            CloseWindow();
        }
    }
}
