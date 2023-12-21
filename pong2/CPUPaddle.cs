namespace pong2
{
    public class CPUPaddle : Paddle
    {
        public CPUPaddle(float w, float h, float x, float y, int s) : base(w, h, x, y, s) { }

        public void Update(int ball_y)
        {
            if (y + height / 2 > ball_y)
            {
                y -= speed;
            }
            if (y + height / 2 <= ball_y)
            {
                y += speed;
            }
            LimitMovement();
        }
    }
}
