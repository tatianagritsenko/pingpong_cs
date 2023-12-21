using Raylib_cs;
using static Raylib_cs.Raylib;

namespace pong2
{
    public class Paddle
    {
        public float x, y;
        public float width, height;
        public int speed;

        public Paddle(float w, float h, float x, float y, int s)
        {
            this.x = x;
            this.y = y;
            width = w;
            height = h;
            speed = s;
        }

        public void Draw()
        {
            DrawRectangleRounded(new Rectangle(x, y, width, height), 0.8f, 0, Color.WHITE);
        }

        public void Update()
        {
            if (IsKeyDown(KeyboardKey.KEY_UP))
            {
                y -= speed;
            }
            if (IsKeyDown(KeyboardKey.KEY_DOWN))
            {
                y += speed;
            }

            LimitMovement();
        }

        protected void LimitMovement()
        {
            if (y <= 0)
            {
                y = 0;
            }
            if (y + height >= GetScreenHeight())
            {
                y = GetScreenHeight() - height;
            }
        }
    }
}
