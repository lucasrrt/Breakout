using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Breakout.Objects
{
    public class Brick
    {
        public Vector2 position;
        public float halfWidth;
        public float halfHeight;
        public Color color;


        public Brick(int i, int j, int m, int n)
        {

            color = new Color(Program.game.random.Next(50, 256), Program.game.random.Next(50, 256), Program.game.random.Next(50, 256));

            position = new Vector2(
                (Program.WIDTH) * (j + 1.0f) / (n + 1.0f),
                (Program.HEIGHT / 2) * (i + 1.0f) / (m + 1.0f)
            );

            halfHeight = (Program.HEIGHT / 2) / ( 2 * (m + 1.0f));
            halfWidth = Program.WIDTH / (2 * (n + 1.0f));
        }

    }
}
