namespace pong2
{
    public class CPUPaddle : Paddle
    {
        public CPUPaddle(float w, float h, float x, float y, int s) : base(w, h, x, y, s) { }

        public void Update(int ball_y)
        {
            if (Y + Height / 2 > ball_y)
            {
                Y -= Speed;
            }
            if (Y + Height / 2 <= ball_y)
            {
                Y += Speed;
            }
            LimitMovement();
        }
    }
}
