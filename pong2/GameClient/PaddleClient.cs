using Raylib_cs;
using static Raylib_cs.Raylib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Net.Sockets;

namespace pong2
{
    public class PaddleClient
    {
        float x, y;
        float width, height;

        public PaddleClient(float w, float h, float x, float y)
        {
            width = w;
            height = h;
            this.x = x;
            this.y = y;
        }

        public void Draw()
        {
            DrawRectangleRounded(new Rectangle(x, y, width, height), 0.8f, 0, Color.WHITE);
        }

        public void Update(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
