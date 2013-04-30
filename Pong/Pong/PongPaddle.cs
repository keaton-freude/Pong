using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pong
{
    public class PongPaddle: XNANF.AnimatedSprite.Sprite
    {
        public PongPaddle()
        {
        }

//If 'player' is 1, then the paddle belongs on the left-side of the screen
//If 'player' is 2, then the paddle belongs on the right-side of the screen
//This only needs to be done once, because we'll set the Position of the paddle's
//x-coordinate, then only modify y coordinates from now on
//It's a bit better to do it this way rather than tracking who's paddle is who's
public PongPaddle(int player, Texture2D texture, Rectangle? srcRect, Vector2 position, float scale)
    :base(texture, srcRect, position, scale)
{
    if (player == 1)
    {
        Position = new Vector2(0f, 0f);
    }
    else
    {
        Position = new Vector2(Pong.graphics.PreferredBackBufferWidth - (this._texture.Width * _scale), 0f);
    }

    //Let's center the paddle at the start
    //if we know our screenHeight, and our paddles height, we can center it off

    //firstly, if we simply do screenHeight / 2, the TOP of the paddle is at the center
    //rather, we want the center of the paddle to be at the center, so we need
    //position.y + paddle.height / 2 to equal screenHeight / 2
    Position = new Vector2(Position.X, Pong.graphics.PreferredBackBufferHeight / 2 - ((_texture.Height * _scale) / 2));
}

        //public override void Draw(SpriteBatch spriteBatch)
        //{
        //    /* Draw the current paddle out the screen */
        //    /* Because we want to use scale, we need to use a heavily overloaded function
        //     * most of the parameters we do not need
        //     */

        //    //spriteBatch.Draw(paddleTexture, position, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 1.0f); 
        //    base.Draw(spriteBatch);
        //}

public override Rectangle BoundingRectangle
{
    get
    {
        return new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);
    }
}
    }
}
