﻿using Raylib_cs;
using static Raylib_cs.Raylib;

namespace pong2
{
    public static class InputBox
    {
        private const int MaxInputChars = 9;
        public static void Example()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            InitWindow(screenWidth, screenHeight, "raylib [text] example - input box");

            // NOTE: One extra space required for line ending char '\0'
            char[] name = new char[MaxInputChars];
            int letterCount = 0;

            Rectangle textBox = new(screenWidth / 2 - 100, 180, 225, 50);
            bool mouseOnText = false;

            int framesCounter = 0;

            SetTargetFPS(60);
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())
            {
                // Update
                //----------------------------------------------------------------------------------
                if (CheckCollisionPointRec(GetMousePosition(), textBox))
                {
                    mouseOnText = true;
                }
                else
                {
                    mouseOnText = false;
                }

                if (mouseOnText)
                {
                    // Set the window's cursor to the I-Beam
                    SetMouseCursor(MouseCursor.MOUSE_CURSOR_IBEAM);

                    // Check if more characters have been pressed on the same frame
                    int key = GetCharPressed();

                    while (key > 0)
                    {
                        // NOTE: Only allow keys in range [32..125]
                        if ((key >= 32) && (key <= 125) && (letterCount < MaxInputChars))
                        {
                            name[letterCount] = (char)key;
                            letterCount++;
                        }

                        // Check next character in the queue
                        key = GetCharPressed();
                    }

                    if (IsKeyPressed(KeyboardKey.KEY_BACKSPACE))
                    {
                        letterCount -= 1;
                        if (letterCount < 0)
                        {
                            letterCount = 0;
                        }
                        name[letterCount] = '\0';
                    }
                }
                else
                {
                    SetMouseCursor(MouseCursor.MOUSE_CURSOR_DEFAULT);
                }

                if (mouseOnText)
                {
                    framesCounter += 1;
                }
                else
                {
                    framesCounter = 0;
                }
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();
                ClearBackground(Color.RAYWHITE);

                DrawText("PLACE MOUSE OVER INPUT BOX!", 240, 140, 20, Color.GRAY);
                DrawRectangleRec(textBox, Color.LIGHTGRAY);

                if (mouseOnText)
                {
                    DrawRectangleLines(
                        (int)textBox.X,
                        (int)textBox.Y,
                        (int)textBox.Width,
                        (int)textBox.Height,
                        Color.RED
                    );
                }
                else
                {
                    DrawRectangleLines(
                        (int)textBox.X,
                        (int)textBox.Y,
                        (int)textBox.Width,
                        (int)textBox.Height,
                        Color.DARKGRAY
                    );
                }

                DrawText(new string(name), (int)textBox.X + 5, (int)textBox.Y + 8, 40, Color.MAROON);
                DrawText($"INPUT CHARS: {letterCount}/{MaxInputChars}", 315, 250, 20, Color.DARKGRAY);

                if (mouseOnText)
                {
                    if (letterCount < MaxInputChars)
                    {
                        // Draw blinking underscore char
                        if ((framesCounter / 20 % 2) == 0)
                        {
                            DrawText(
                                "_",
                                (int)textBox.X + 8 + MeasureText(new string(name), 40),
                                (int)textBox.Y + 12,
                                40,
                                Color.MAROON
                            );
                        }
                    }
                    else
                    {
                        DrawText("Press BACKSPACE to delete chars...", 230, 300, 20, Color.GRAY);
                    }
                }

                EndDrawing();
                //----------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------
            CloseWindow();
            //--------------------------------------------------------------------------------------

        }
    }
}
