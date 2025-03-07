using Microsoft.Xna.Framework;

public class Animate() {
    

    public void InfiniteMotion(ref Vector2 ballPosition, float updatedBallSpeed, GameTime gameTime) {
        var frame = 10f;
        while(frame <= 0) {
            ballPosition.X -= 100f;
            frame++;
        }
    }
}