using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Raylib_cs.Raylib;

namespace pong2
{
    public class BallClient
    {
        float x, y;
        int radius;

        public BallClient(float x, float y, int r)
        {
            this.x = x;
            this.y = y;
            radius = r;
        }

        public void Draw()
        {
            DrawCircle((int)x, (int)y, radius, Colors.Yellow);
        }

        public void Update(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
