using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


public class BorderLimit {
    
    public void border(ref Vector2 ballPosition, ref Texture2D ballTexture, GraphicsDeviceManager _graphics) {
        if(ballPosition.X > _graphics.PreferredBackBufferWidth - ballTexture.Width / 2) {
            ballPosition.X = _graphics.PreferredBackBufferWidth - ballTexture.Width / 2;
        } else if(ballPosition.X < ballTexture.Width / 2) {
            ballPosition.X = ballTexture.Width / 2;
        }

        if(ballPosition.Y > _graphics.PreferredBackBufferHeight - ballTexture.Height / 2) {
            ballPosition.Y = _graphics.PreferredBackBufferHeight - ballTexture.Height / 2;
        } else if(ballPosition.Y < ballTexture.Height / 2) {
            ballPosition.Y = ballTexture.Height / 2;
        }
    }
}