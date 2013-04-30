using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Pong
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Pong : Microsoft.Xna.Framework.Game
    {
        public static GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        /* Our Players */
        PongPlayer Player1;
        PongPlayer Player2;
        /* The ball for the game */
        PongBall ball;
        /* A reference to a particle system */
        int particle_id = 0;
        /* A font used to display the score */
        public SpriteFont font;
        /* The state we're in */
        string GameState = "Pregame";


        public Pong()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            /* set the screen to standard 720p */
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            this.IsMouseVisible = true;
            graphics.ApplyChanges();
            XNANF.Utility.TimeKeeper.Instance.time_objects.Add("PreGameOver", new XNANF.Utility.TimeObject(5.0f, 0.0f));

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            particle_id = XNANF.ParticleSystem.ParticleEngine.Instance.LoadFromFile("explosion blue", Content);
            Player1 = new PongPlayer(PlayerIndex.One, 1, Content.Load<Texture2D>(@"paddle"));
            Player2 = new PongPlayer(PlayerIndex.One, 2, Content.Load<Texture2D>(@"paddle"));
            ball = new PongBall(Content.Load<Texture2D>(@"ball"), null, new Vector2(1280 / 2, 720 / 2), 1.0f);
            font = Content.Load<SpriteFont>(@"gameFont");
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GameState == "Game")
            {

                // Allows the game to exit
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                    this.Exit();
                Player1.Update(gameTime);
                Player2.Update(gameTime);
                ball.Update(gameTime);
                CheckBallBounce();

                CheckBallPaddleCollision(gameTime);
            }
            else
            {
                if (XNANF.Utility.TimeKeeper.Instance.time_objects["PreGameOver"].ConsumeAvailability)
                {
                    GameState = "Game";
                }
            }

            XNANF.Utility.TimeKeeper.Instance.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            XNANF.ParticleSystem.ParticleEngine.Instance.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            base.Update(gameTime);
        }

        private void CheckBallPaddleCollision(GameTime gameTime)
        {
            /* Now let's check the balls bounds against the paddles of the players */
            if (Player1.paddle.BoundingRectangle.Intersects(ball.BoundingRectangle))
            {
                /* handle collision, we'll just do x-swap for now */
                ResolveBallPaddleCollision(Player1.paddle.BoundingRectangle.Top, ball.BoundingRectangle.Top);
                ball.Update(gameTime);
            }

            if (Player2.paddle.BoundingRectangle.Intersects(ball.BoundingRectangle))
            {
                /* handle collision, we'll just do x-swap for now */
                ResolveBallPaddleCollision(Player2.paddle.BoundingRectangle.Top, ball.BoundingRectangle.Top);
                ball.Update(gameTime);
            }
        }

        private void CheckBallBounce()
        {
            int screenWidth = graphics.PreferredBackBufferWidth;
            int screenHeight = graphics.PreferredBackBufferHeight;
            /* Ball was updated, lets check its bounds against the screen */
            if (ball.BoundingRectangle.Left < 0)
            {
                // A point has been scored for Player2
                ResetBall();
                Player2.Score++;
            }
            else if (ball.BoundingRectangle.Right > screenWidth)
            {
                // A point has been scored for Player1
                ResetBall();
                Player1.Score++;
            }
            else if (ball.BoundingRectangle.Bottom > screenHeight)
            {
                BallYBounce();
            }
            else if (ball.BoundingRectangle.Top < 0)
            {
                BallYBounce();
            }
        }

        private void ResetBall()
        {
            ((XNANF.ParticleSystem.OneShotParticleEffect)XNANF.ParticleSystem.ParticleEngine.Instance.systems[particle_id].effects[0]).Emitter.Location = ball.Position;
            ((XNANF.ParticleSystem.OneShotParticleEffect)XNANF.ParticleSystem.ParticleEngine.Instance.systems[particle_id].effects[0]).Emitter.LastLocation = ball.Position;
            ((XNANF.ParticleSystem.OneShotParticleEffect)XNANF.ParticleSystem.ParticleEngine.Instance.systems[particle_id].effects[0]).Fire();
            ball.Position = new Vector2(1280 / 2, 720 / 2);
            ball.speed = 500f;

        }

        private void BallYBounce()
        {
            ball.Velocity.Y *= -1;
        }

        private void ResolveBallPaddleCollision(int paddleY, int ballY)
        {
            ball.Velocity.X *= -1;
            
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            string ScoreString = String.Format("{0}   |   {1}", Player1.Score, Player2.Score);
            // TODO: Add your drawing code here
            spriteBatch.Begin();
                Player1.Draw(spriteBatch);
                Player2.Draw(spriteBatch);
                ball.Draw(spriteBatch);
                spriteBatch.DrawString(font, ScoreString, new Vector2(1280 / 2 - 70, 0), Color.White);
            spriteBatch.End();

            XNANF.ParticleSystem.ParticleEngine.Instance.Draw(spriteBatch);

            base.Draw(gameTime);
        }
    }
}
