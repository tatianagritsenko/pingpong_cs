using static Raylib_cs.Raylib;

namespace pong2
{
    public class Ball
    {
        public float x, y;
        public int speed_x, speed_y;
        public int radius;

        public Ball(float x, float y, int sx, int sy, int r)
        {
            this.x = x;
            this.y = y;
            radius = r;
            speed_x = sx;
            speed_y = sy;
        }

        public void Draw()
        {
            DrawCircle((int)x, (int)y, radius, Colors.Yellow);
        }

        public void Update()
        {
            x += speed_x;
            y += speed_y;

            // шарик коснулся верхней или нижней грани
            if (y + radius >= GetScreenHeight() || y - radius <= 0)
            {
                speed_y *= -1;
            }

            // шарик коснулся левой или правой грани
            if (x + radius >= GetScreenWidth())
            {
                Game.Cpu_score++;
                ResetBall();
            }
            if (x - radius <= 0)
            {
                Game.Player_score++;
                ResetBall();
            }
        }

        public void ResetBall()
        {
            x = GetScreenWidth() / 2;
            y = GetScreenHeight() / 2;
            int[] speedChoices = { -1, 1 };
            speed_x *= speedChoices[GetRandomValue(0, 1)];
            speed_y *= speedChoices[GetRandomValue(0, 1)];
        }
    }
}
