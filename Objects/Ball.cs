using Breakout.Auxiliar;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Breakout.Objects
{
    class Ball
    {
        public Vector2 position;
        public Vector2 velocity;
        public float halfSize = 5;
        // hitCounter;

        public Ball()
        {
            position = new Vector2(Program.WIDTH / 2, Program.HEIGHT - 20 - halfSize);
            //velocity = new Vector2(5, -5);
        }

        public void waitNlaunch(float paddlePositionX)
        {
            velocity = Vector2.Zero;
            position.X = paddlePositionX;
            velocity.Y = -velocity.Y;
            position.Y = Program.game.paddle.Y - halfSize;
            Program.game.paddle.halfSize = 50;

            if (Keyboard.GetState().IsKeyDown(Keys.Space) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.X))
            {
                int speedo = 4;
                velocity = new Vector2(speedo, -speedo);
                Program.game.gameState = GameStates.FREE_BALL;
            }
        }

        public void Move()
        {
            position += velocity;
            if (position.X <= 0)
            {
                position.X = 0;
                velocity.X = -velocity.X;
            }

            if (position.X >= Program.WIDTH)
            {
                position.X = Program.WIDTH;
                velocity.X = -velocity.X;
            }
            
            if (position.Y <= 0)
            {
                position.Y = 0;
                velocity.Y = -velocity.Y;
            }

            if (position.Y >= Program.HEIGHT)
            {
                //position.Y = Program.HEIGHT - 240;
                //velocity.Y = -velocity.Y;
                
            }

        }

        public void ColidePaddle(float paddlePosition, float paddleSize)
        {
            if (position.Y >= Program.HEIGHT - 20 - halfSize &&
                position.Y <= Program.HEIGHT - 10 - halfSize && 
                position.X + halfSize >= paddlePosition - paddleSize && 
                position.X - halfSize <= paddlePosition + paddleSize)
            {
                Program.game.hit.Play();
                Program.game.hitCount = 0;
                Vector2 centroCurvatura = new Vector2 (paddlePosition, Program.HEIGHT + 300 + 5*paddleSize);
                Vector2 raioCurvatura = position - centroCurvatura;
                /*if (position.X + halfSize >= paddlePosition - paddleSize + 10 &&
                position.X - halfSize <= paddlePosition + paddleSize - 10)
                {
                    if (velocity.Y > 0)
                        velocity.Y = -velocity.Y;
                }
                else
                {
                    if (velocity.Y > 0)
                    {
                        velocity.Y = 0;
                    }
                }*/

                if (velocity.Y > 0)
                {
                    velocity -= 2*(raioCurvatura)/(raioCurvatura.Length())*((Vector2.Dot(raioCurvatura,velocity))/(raioCurvatura.Length()));
                }


            }
        }

        public void ColideBrick()
        {
            for(int t = Program.game.bricks.Count-1; t >= 0; t--){
                if (position.Y - halfSize < Program.game.bricks[t].position.Y + Program.game.bricks[t].halfHeight && position.Y + halfSize > Program.game.bricks[t].position.Y - Program.game.bricks[t].halfHeight)
                {
                    if (position.X - halfSize < Program.game.bricks[t].position.X + Program.game.bricks[t].halfWidth && position.X + halfSize > Program.game.bricks[t].position.X - Program.game.bricks[t].halfWidth)
                    {
                        Program.game.hit.Play();

                        Program.game.hitCount++;
                        Program.game.score++;

                        float pseudoAngle = (position.Y - Program.game.bricks[t].position.Y)/(position.X - Program.game.bricks[t].position.X);
                        float reference = Math.Abs(Program.game.bricks[t].halfHeight/Program.game.bricks[t].halfWidth);
                        if (Math.Abs(pseudoAngle) > reference)
                        {
                            velocity.Y = Math.Abs(velocity.Y) * Math.Sign(position.Y - Program.game.bricks[t].position.Y);
                            //Program.game.hitCount++;
                        }
                        else
                        {
                            velocity.X = Math.Abs(velocity.X) * Math.Sign(position.X - Program.game.bricks[t].position.X);
                            //Program.game.hitCount++;
                        }
                            Program.game.bricks.RemoveAt(t);
                        if (velocity.Length() <= 6*Math.Sqrt(2))
                        {
                            velocity *= 1.1f;
                            Console.WriteLine(velocity.X);
                        }
                        if (Program.game.hitCount > 0 && Program.game.hitCount % (Program.game.level * 3) == 0)
                        {
                            //Program.game.hitCount = 0;
                            Program.game.paddle.halfSize += 20;
                        }
                        
                        break;
                    }

                }//*/

                if (Keyboard.GetState().IsKeyDown(Keys.Q) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.Y))
                {
                    Program.game.bricks = new List<Brick>();
                    break;
                }
                //GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                //if(position.Y < Program.game.bricks[t].position.Y + Program.game.bricks[t].halfHeight) && velocity

            }

            if (Program.game.bricks.Count == 0)
            {
                Program.game.gameState = GameStates.STOPPED_BALL;
                Program.game.paddle.halfSize = 50;
                Program.game.level++;
                if (Program.game.lives < 3)
                    Program.game.lives++;

                Program.game.initeBricks(3 * Program.game.level, 5 * Program.game.level);
            }
        }
    }
}
