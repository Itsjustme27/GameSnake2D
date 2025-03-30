using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


public class BorderLimit {
    // Adding a property to track game over state
    public bool IsGameOver { get; private set ; } = false;
    public bool border(ref Vector2 ballPosition, ref Texture2D ballTexture, GraphicsDeviceManager _graphics) {
        bool collisionOccurred = false;
        if(ballPosition.X > _graphics.PreferredBackBufferWidth - ballTexture.Width / 2) {
            ballPosition.X = _graphics.PreferredBackBufferWidth - ballTexture.Width / 2;
            collisionOccurred = true;
        } else if(ballPosition.X < ballTexture.Width / 2) {
            ballPosition.X = ballTexture.Width / 2;
            collisionOccurred = true;
        }

        if(ballPosition.Y > _graphics.PreferredBackBufferHeight - ballTexture.Height / 2) {
            ballPosition.Y = _graphics.PreferredBackBufferHeight - ballTexture.Height / 2;
            collisionOccurred = true;
        } else if(ballPosition.Y < ballTexture.Height / 2) {
            ballPosition.Y = ballTexture.Height / 2;
            collisionOccurred = true;
        }

        // checking game over
        if (collisionOccurred) {
            IsGameOver = true;
            Console.WriteLine("GameOver!");
            System.Environment.Exit(0);
        }
        return collisionOccurred;
    }
    public void Reset() {
        IsGameOver = false;
    }
}