using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Breakout.Objects;
using Breakout.Auxiliar;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Breakout
{
    public class Breakout : Game
    {
        public GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Random random = new Random();

        Texture2D blank;
        Texture2D gameOver;
        Texture2D heart;
        Texture2D paddleSprite;
        Texture2D pokeball;
        Texture2D backgroundImage;

        private SpriteFont font;
        public int hitCount;
        public int score;

        public SoundEffect hit;
        public SoundEffect backgorundMusic;

        public KeyboardState keyState, previousKeyState;
        public GamePadState previousJoystick, joystick;            

        public Player paddle;

        Ball ball;

        public List<Brick> bricks;

        public GameStates gameState;
        public GameStates previousGameState; 

        public int level;
        public int lives;

        public void initeBricks(int m, int n)
        {
            bricks = new List<Brick>();

            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Brick brick = new Brick(i, j, m, n);
                    bricks.Add (brick);
                }
            }
        }

        public Breakout()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = Program.HEIGHT;
            graphics.PreferredBackBufferWidth = Program.WIDTH;

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            paddle = new Player();
            ball = new Ball();

            gameState = previousGameState = GameStates.STOPPED_BALL;

            keyState = previousKeyState = Keyboard.GetState();
            joystick = previousJoystick = GamePad.GetState(PlayerIndex.One);

            initeBricks(3, 5);

            score = 0;
            hitCount = 0;

            level = 1;
            lives = 3;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            blank = Content.Load<Texture2D>("blank");
            gameOver = Content.Load<Texture2D>("GameOver");
            heart = Content.Load<Texture2D>("heart");
            paddleSprite = Content.Load<Texture2D>("paddle_sprite");
            pokeball = Content.Load<Texture2D>("PokeBall");
            backgroundImage = Content.Load<Texture2D>("background");

           // font = Content.Load<SpriteFont>("Miramo");

            hit = Content.Load<SoundEffect>("hit");
            
            backgorundMusic = Content.Load<SoundEffect>("music");
            //backgorundMusic.Play();
        }

        protected override void UnloadContent()
        {
        }

        void GetInputs()
        {
            previousKeyState = keyState;
            keyState = Keyboard.GetState();
            previousJoystick = joystick;
            joystick = GamePad.GetState(PlayerIndex.One);   

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
        }
        
        void moveInputs()
        {
            if (keyState.IsKeyDown(Keys.Left) || joystick.IsButtonDown(Buttons.LeftShoulder))
            {
                paddle.MoveLeft();
            }

            if (keyState.IsKeyDown(Keys.Right) || joystick.IsButtonDown(Buttons.RightShoulder))
            {
                paddle.MoveRight();
            }
        }

        void CheckPause()
        {
            if ((keyState.IsKeyDown(Keys.P) && previousKeyState.IsKeyUp(Keys.P))||(joystick.IsButtonDown(Buttons.Start) && previousJoystick.IsButtonUp(Buttons.Start)))
            {
                if (gameState != GameStates.PAUSE_GAME)
                {
                    previousGameState = gameState;
                    gameState = GameStates.PAUSE_GAME;
                }

                else
                {
                    gameState = previousGameState;
                }
            }            
        }

        protected override void Update(GameTime gameTime)
        {
            GetInputs();
            CheckPause();

            //ballPosition += ballVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (gameState == GameStates.FREE_BALL)
            {
                moveInputs();
                ball.Move();
                ball.ColidePaddle(paddle.X, paddle.halfSize);
                ball.ColideBrick();
                if (ball.position.Y >= Program.HEIGHT - 5)
                {
                    gameState = GameStates.STOPPED_BALL;
                    lives--;
                    if (lives <= 0)
                    {
                        Console.WriteLine("Game Over");
                        bricks = new List<Brick>();
                        paddle.halfSize = 0;
                        ball.halfSize = 0;
                        gameState = GameStates.GAME_OVER;
                        
                    }
                    Console.WriteLine(lives);

                }
                
                
            }

            if (gameState == GameStates.STOPPED_BALL)
            {
                moveInputs();
               
                ball.waitNlaunch(paddle.X);
            
            }

            if (gameState == GameStates.PAUSE_GAME)
            {
                Console.WriteLine("Pause");
            }


            if (gameState == GameStates.GAME_OVER)
            {
                if (keyState.IsKeyDown(Keys.R))
                {
                    Initialize();
                    gameState = GameStates.STOPPED_BALL;
                }
            }
            

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            if (gameState != GameStates.GAME_OVER)
            {
                    spriteBatch.Draw(
                        backgroundImage,
                        new Rectangle(0, 0, 800, 480),
                        Color.White
                        );
            }
            //Paddle
            spriteBatch.Draw(
                paddleSprite, 
                new Rectangle (
                    (int)(paddle.X - paddle.halfSize),
                    (int)(paddle.Y),
                    (int)paddle.halfSize*2, 10
                ),
                Color.White
            );

            //Lives
            for (int t = 0; t < lives; t++)
                spriteBatch.Draw(
                    heart,
                    new Rectangle(
                        270 + t*100, 300, 70, 50),

                    Color.White
                );

            //Ball
            spriteBatch.Draw(
                pokeball, 
                new Rectangle(
                    (int)(ball.position.X - ball.halfSize),
                    (int)(ball.position.Y - ball.halfSize), 
                    (int)ball.halfSize * 2, 
                    (int)ball.halfSize * 2
                ),
                Color.White
            );

            //Bricks
            foreach (Brick brick in bricks)
            {
                spriteBatch.Draw(
                    blank,
                    new Rectangle(
                        (int)(brick.position.X - brick.halfWidth),
                        (int)(brick.position.Y - brick.halfHeight),
                        (int)brick.halfWidth * 2 - 2,
                        (int)brick.halfHeight * 2 - 2
                    ),
                    brick.color
                );
            }

            if(gameState != GameStates.GAME_OVER)
            {
                //spriteBatch.DrawString(font, "Score: " + score + "  Hit Count: " + hitCount, new Vector2 (0,0) ,Color.Yellow);
            }

            //Game Over
            if (gameState == GameStates.GAME_OVER)
            {
                spriteBatch.Draw(gameOver, new Rectangle (175,100,450,200), Color.White);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
