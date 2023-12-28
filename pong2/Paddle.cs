using Raylib_cs;
using static Raylib_cs.Raylib;

namespace pong2
{
    public class Paddle
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public int Speed { get; set; }
    

        public Paddle(float w, float h, float x, float y, int s)
        {
            Width = w;
            Height = h;
            X = x;
            Y = y;
            Speed = s;
        }

        public void Draw()
        {
            DrawRectangleRounded(new Rectangle(X, Y, Width, Height), 0.8f, 0, Color.WHITE);
        }

        public void Update()
        {
            if (IsKeyDown(KeyboardKey.KEY_UP))
            {
                Y -= Speed;
            }
            if (IsKeyDown(KeyboardKey.KEY_DOWN))
            {
                Y += Speed;
            }

            LimitMovement();
        }

        protected void LimitMovement()
        {
            if (Y <= 0)
            {
                Y = 0;
            }
            if (Y + Height >= GetScreenHeight())
            {
                Y = GetScreenHeight() - Height;
            }
        }
    }
}
