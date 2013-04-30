using XNANF.Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pong
{
    public class PongPlayer : Player
    {
public PongPaddle paddle;
public float paddleMoveSpeed = 1000f;
public int Score = 0;

public PongPlayer(PlayerIndex playerIndex, int player, Texture2D paddleTexture) : base(playerIndex)
{
    paddle = new PongPaddle(player, paddleTexture, null, Vector2.Zero, 1.0f);
    /* we now have access to which player this class represents at run time */
}

public override void Update(GameTime gameTime)
{
    float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
    /* get PlayerInput and adjust paddles position */
    paddle.Position = new Vector2(paddle.Position.X, paddle.Position.Y + (GamePad.GetState(Player_Index).ThumbSticks.Left.Y * paddleMoveSpeed * dt * -1f));
    base.Update(gameTime);
}

public override void Draw(SpriteBatch spriteBatch)
{
    paddle.Draw(spriteBatch);
    base.Draw(spriteBatch);
}
}
}