using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public class KeyInput {
        KeyboardState oldState = Keyboard.GetState();
        KeyboardState newState = Keyboard.GetState(); 
    public void KInput(float ballSpeed, ref Vector2 ballPosition, ref Vector2 ballDirection) {

        oldState = newState;
        newState = Keyboard.GetState();
        // causes movement lol
        ballPosition += ballDirection * ballSpeed;
        
        if((newState.IsKeyDown(Keys.Up) || newState.IsKeyDown(Keys.W)) && (oldState.IsKeyUp(Keys.Up) || oldState.IsKeyUp(Keys.W))) {
            // ballPosition.Y -= updatedBallSpeed;                        // key based movement
            ballDirection = new Vector2(0, -1);    // Move Up
            
        } else if((newState.IsKeyDown(Keys.Down) || newState.IsKeyDown(Keys.S)) && (oldState.IsKeyUp(Keys.Down) || oldState.IsKeyUp(Keys.S))) {
            // ballPosition.Y += updatedBallSpeed;
            ballDirection = new Vector2(0, 1);     // Move Down
        } else if((newState.IsKeyDown(Keys.Left) || newState.IsKeyDown(Keys.A)) && (oldState.IsKeyUp(Keys.Left) || oldState.IsKeyUp(Keys.A))) {
            // ballPosition.X -= updatedBallSpeed;
            ballDirection = new Vector2(-1, 0);
        } else if((newState.IsKeyDown(Keys.Right) || newState.IsKeyDown(Keys.D)) && (oldState.IsKeyUp(Keys.Right) || oldState.IsKeyUp(Keys.D))) {
            // ballPosition.X += updatedBallSpeed;
            ballDirection = new Vector2(1,0);
        }

    }
}