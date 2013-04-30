using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pong
{
public class PongBall: XNANF.AnimatedSprite.Sprite
{
    public Vector2 Velocity;
    public float Acceleration = 20f;
    public float speed = 500f;

    public PongBall(Texture2D texture, Rectangle? srcRect, Vector2 position, float scale)
        :base(texture, srcRect, position, scale)
    {
        Velocity = new Vector2(1, 1);
    }

    public override void Update(GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        Position = new Vector2(Position.X + Velocity.X * speed * dt, Position.Y + Velocity.Y * speed * dt);
        speed += Acceleration * dt;
    }

    public override Rectangle BoundingRectangle
    {
        get
        {
            return new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);
        }
    }
}
}
