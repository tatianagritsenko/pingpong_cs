﻿using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace pong2
{
    public static class InputBox
    {
        private const int MaxInputChars = 9;
        public static bool Update(Rectangle chars) {
            bool mouseOnText;
            if (CheckCollisionPointRec(GetMousePosition(), chars))
                mouseOnText = true;
            else
                mouseOnText = false;
            return mouseOnText;
        }
        public static int WriteRec(bool mouseOnText, int letterCountLog, char[] chars) {
            if (mouseOnText)
            {
                // Set the window's cursor to the I-Beam
                SetMouseCursor(MouseCursor.MOUSE_CURSOR_IBEAM);

                // Check if more characters have been pressed on the same frame
                int key = GetCharPressed();

                while (key > 0)
                {
                    // NOTE: Only allow keys in range [32..125]
                    if ((key >= 32) && (key <= 125) && (letterCountLog < MaxInputChars))
                    {
                        chars[letterCountLog] = (char)key;
                        letterCountLog++;
                    }

                    // Check next character in the queue
                    key = GetCharPressed();
                }

                if (IsKeyPressed(KeyboardKey.KEY_BACKSPACE))
                {
                    letterCountLog -= 1;
                    if (letterCountLog < 0)
                    {
                        letterCountLog = 0;
                    }
                    chars[letterCountLog] = '\0';
                }
            }
            else
            {
                SetMouseCursor(MouseCursor.MOUSE_CURSOR_DEFAULT);
            }
            return letterCountLog;
        }
        public static void DrawRec(Rectangle rec, bool mouseOnText) {
            if (mouseOnText)
            {
                DrawRectangleLines(
                    (int)rec.X,
                    (int)rec.Y,
                    (int)rec.Width,
                    (int)rec.Height,
                    Color.RED
                );
            }
            else
            {
                DrawRectangleLines(
                    (int)rec.X,
                    (int)rec.Y,
                    (int)rec.Width,
                    (int)rec.Height,
                    Color.DARKGRAY
                );
            }
        }
        public static void WriteInRec(bool mouseOnText, int letterCount, char[] chars, Rectangle rec, int framesCounter) {
            if (mouseOnText)
            {
                if (letterCount < MaxInputChars)
                {
                    // Draw blinking underscore char
                    if ((framesCounter / 20 % 2) == 0)
                    {
                        DrawText(
                            "_",
                            (int)rec.X + 8 + MeasureText(new string(chars), 40),
                            (int)rec.Y + 12,
                            40,
                            Color.MAROON
                        );
                    }
                }
                else
                {
                    DrawText("Press BACKSPACE to delete chars...", 230, 270, 20, Color.GRAY);
                }
            }

        }
        public static LoginPassword Autorization()
        {
            // Initialization
            LoginPassword lp = new LoginPassword();
            const int screenWidth = 800;
            const int screenHeight = 450;

            InitWindow(screenWidth, screenHeight, "STN pong - Autorization");

            // NOTE: One extra space required for line ending char '\0'
            char[] log = new char[MaxInputChars];
            char[] pas = new char[MaxInputChars];
            int letterCountLog= 0;
            int letterCountPas = 0;

            Rectangle login = new(screenWidth / 2 - 250, 180, 225, 50);
            Rectangle pass = new(screenWidth / 2 + 25, 180, 225, 50);
            bool mouseOnText = false;
            bool mouseOnText3 = false;

            int framesCounter = 0;
            int framesCounter2 = 0;
            SetTargetFPS(60);
         
            // Main game loop
            while (!WindowShouldClose())
            {
                mouseOnText = Update(login);
                mouseOnText3 = Update(pass);
                letterCountLog = WriteRec(mouseOnText, letterCountLog, log);
                letterCountPas = WriteRec(mouseOnText3, letterCountPas, pas);
                if (mouseOnText)
                    framesCounter += 1;
                else
                    framesCounter = 0;

                if (mouseOnText3)
                    framesCounter2 += 1;
                else
                    framesCounter2 = 0;

                // Draw
                BeginDrawing();
                ClearBackground(Color.RAYWHITE);

                DrawText("Welcome to the Pong game!", 270, 140, 20, Color.GRAY);
                DrawRectangleRec(login, Color.LIGHTGRAY);
                DrawRectangleRec(pass, Color.LIGHTGRAY);

                DrawRec(login, mouseOnText);
                DrawRec(pass, mouseOnText3);
             
                bool isButtonClicked = false;
                Vector2 mousePosition = Raylib.GetMousePosition();

                if (IsKeyPressed(KeyboardKey.KEY_ENTER))
                {
                    lp.login = new string(log);
                    lp.password = new string(pas);
                    break;
                }
                char[] secret = new char[256];
                for (int i = 0; i < letterCountPas; i++)
                    secret[i] = '*';
                DrawText(new string(log), (int)login.X + 5, (int)login.Y + 8, 40, Color.MAROON);
                DrawText(new string(secret), (int)pass.X + 5, (int)pass.Y + 8, 40, Color.MAROON);
                DrawText("Press ENTER to continue...", screenWidth / 2 - 250, 300 + 8, 40, Color.MAROON);
                if (log[0] == 0)
                    DrawText("login", (int)login.X+15, (int)login.Y+15, 20, Color.DARKGRAY);
                if (pas[0] == 0)
                    DrawText("password", (int)pass.X + 15, (int)pass.Y + 15, 20, Color.DARKGRAY);
                WriteInRec(mouseOnText, letterCountLog, log, login, framesCounter);
                WriteInRec(mouseOnText3, letterCountPas, pas, pass, framesCounter2);

                EndDrawing();
            }
            CloseWindow();
            return lp;
        }

        public static LoginPassword Registration()
        {
            // Initialization
            LoginPassword lp = new LoginPassword();
            const int screenWidth = 800;
            const int screenHeight = 450;

            InitWindow(screenWidth, screenHeight, "STN pong - Registration");

            // NOTE: One extra space required for line ending char '\0'
            char[] log = new char[MaxInputChars];
            char[] pas = new char[MaxInputChars];
            int letterCountLog = 0;
            int letterCountPas = 0;

            Rectangle login = new(screenWidth / 2 - 250, 180, 225, 50);
            Rectangle pass = new(screenWidth / 2 + 25, 180, 225, 50);
            bool mouseOnText = false;
            bool mouseOnText3 = false;

            int framesCounter = 0;
            int framesCounter2 = 0;
            SetTargetFPS(60);

            // Main game loop
            while (!WindowShouldClose())
            {
                mouseOnText = Update(login);
                mouseOnText3 = Update(pass);
                letterCountLog = WriteRec(mouseOnText, letterCountLog, log);
                letterCountPas = WriteRec(mouseOnText3, letterCountPas, pas);
                if (mouseOnText)
                    framesCounter += 1;
                else
                    framesCounter = 0;

                if (mouseOnText3)
                    framesCounter2 += 1;
                else
                    framesCounter2 = 0;

                // Draw
                BeginDrawing();
                ClearBackground(Color.RAYWHITE);

                DrawText("Welcome to the Pong game!", 270, 140, 20, Color.GRAY);
                DrawRectangleRec(login, Color.LIGHTGRAY);
                DrawRectangleRec(pass, Color.LIGHTGRAY);

                DrawRec(login, mouseOnText);
                DrawRec(pass, mouseOnText3);

                bool isButtonClicked = false;
                Vector2 mousePosition = Raylib.GetMousePosition();

                if (IsKeyPressed(KeyboardKey.KEY_ENTER))
                {
                    lp.login = new string(log);
                    lp.password = new string(pas);
                    lp.login = lp.login.Remove(letterCountLog);
                    lp.password = lp.password.Remove(letterCountPas);
                    break;
                }
                char[] secret = new char[256];
                for (int i = 0; i < letterCountPas; i++)
                    secret[i] = '*';
                DrawText(new string(log), (int)login.X + 5, (int)login.Y + 8, 40, Color.MAROON);
                DrawText(new string(secret), (int)pass.X + 5, (int)pass.Y + 8, 40, Color.MAROON);
                DrawText("Press ENTER to continue...", screenWidth / 2 - 250, 300 + 8, 40, Color.MAROON);
                if (log[0] == 0)
                    DrawText("Create a login", (int)login.X + 15, (int)login.Y + 15, 20, Color.DARKGRAY);
                if (pas[0] == 0)
                    DrawText("Create a password", (int)pass.X + 15, (int)pass.Y + 15, 20, Color.DARKGRAY);
                WriteInRec(mouseOnText, letterCountLog, log, login, framesCounter);
                WriteInRec(mouseOnText3, letterCountPas, pas, pass, framesCounter2);

                EndDrawing();
            }
            CloseWindow();
            return lp;
        }
        public static int ChooseOpt() {
            int opt = 0;
            const int screenWidth = 800;
            const int screenHeight = 450;

            InitWindow(screenWidth, screenHeight, "STN pong - Choose Option To Start");
            while (!WindowShouldClose())
            {
                BeginDrawing();
                ClearBackground(Color.RAYWHITE);

                DrawText("Welcome to the Pong game!", screenWidth/2 - 250, 140, 40, Color.MAROON);
                DrawText("Choose the right option:", screenWidth / 2 - 175, 190, 30, Color.GRAY);
                DrawText("1) Sign up", screenWidth / 2 - 75, 230, 20, Color.DARKGRAY);
                DrawText("2) Sign in", screenWidth / 2 - 75, 260, 20, Color.DARKGRAY);

                if(IsKeyPressed(KeyboardKey.KEY_ONE))
                {
                    opt = 1; 
                    break;
                }
                if (IsKeyPressed(KeyboardKey.KEY_TWO))
                {
                    opt = 2;
                    break;
                }
                EndDrawing();
            }
            CloseWindow();
            return opt;
        }
    }
}
