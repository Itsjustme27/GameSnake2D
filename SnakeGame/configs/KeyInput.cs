using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public class KeyInput {
    public void KInput(float ballSpeed, ref Vector2 ballPosition, ref Vector2 ballDirection) {
         var kstate = Keyboard.GetState();
        
        // causes movement lol
        ballPosition += ballDirection * ballSpeed;
        
        if(kstate.IsKeyDown(Keys.Up) || kstate.IsKeyDown(Keys.W)) {
            // ballPosition.Y -= updatedBallSpeed;                        // key based movement
            ballDirection = new Vector2(0, -1);    // Move Up
            
        } else if(kstate.IsKeyDown(Keys.Down) || kstate.IsKeyDown(Keys.S)) {
            // ballPosition.Y += updatedBallSpeed;
            ballDirection = new Vector2(0, 1);     // Move Down
        } else if(kstate.IsKeyDown(Keys.Left) || kstate.IsKeyDown(Keys.A)) {
            // ballPosition.X -= updatedBallSpeed;
            ballDirection = new Vector2(-1, 0);
        } else if(kstate.IsKeyDown(Keys.Right) || kstate.IsKeyDown(Keys.D)) {
            // ballPosition.X += updatedBallSpeed;
            ballDirection = new Vector2(1,0);
        }

    }
}