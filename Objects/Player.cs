using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Breakout.Objects
{
    public class Player
    {
        public float X {get; private set;}
        public float Y = Program.HEIGHT - 20;
           
        public float speed = 9;
        public float halfSize = 50;

        public Player()
        {
            X = Program.WIDTH / 2;
        }

        public void MoveLeft()
        {
            X -= speed;
            if (X - halfSize < 0)
                X = halfSize;
        }

        public void MoveRight()
        {
            X += speed;
            if (X + halfSize > Program.WIDTH)
                X = Program.WIDTH - halfSize;
        }

    }
}
