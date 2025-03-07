
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace SnakeGame;

public class Game1 : Game
{
    Texture2D ballTexture;
    Texture2D foodTexture;
    Texture2D squareTexture;
    Vector2 ballPosition;
    Vector2 ballDirection;
    Vector2 foodPosition;
    float ballSpeed;
    float inputSpeed;
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private SpriteFont font;
    private int score = 0;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        ballPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);   // Positioning my ball
        ballSpeed = 5f;   // speed
        inputSpeed = 100f;
        ballDirection = new Vector2(1, 0);

        foodPosition = new Vector2(100, 100);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        ballTexture = Content.Load<Texture2D>("ball");
        foodTexture = Content.Load<Texture2D>("apple");
        font = Content.Load<SpriteFont>("GameFont");
        squareTexture = new Texture2D(GraphicsDevice, 1,1);
        squareTexture.SetData(new[] {Color.LightGreen});
    }

    private bool CheckCollision(Vector2 ballPosition, Texture2D ballTexture, Vector2 foodPosition, Texture2D foodTexture) {
        float ballRadius = ballTexture.Width / 2f;
        float foodRadius = foodTexture.Width / 2f;

        float distance = Vector2.Distance(ballPosition, foodPosition);

        return distance < (ballRadius + foodRadius);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        // The time since Update was called last.
        float updatedBallSpeed = inputSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        // created another class for controls (keyboard)
        var keyboard = new KeyInput();                      
        keyboard.KInput(ballSpeed, ref ballPosition, ref ballDirection);

        // created another class for window borderl limit
        var windowBorderLimit = new BorderLimit();
        windowBorderLimit.border(ref ballPosition, ref ballTexture, _graphics);

        var infiniteMotion = new Animate();
        infiniteMotion.InfiniteMotion(ref ballPosition, updatedBallSpeed, gameTime);

        if(CheckCollision(ballPosition, ballTexture, foodPosition, foodTexture)) {
            System.Diagnostics.Debug.WriteLine("Collision detected!");
            score++;
            foodPosition = new Vector2(
                Random.Shared.Next(0, _graphics.PreferredBackBufferWidth - foodTexture.Width),
                Random.Shared.Next(0, _graphics.PreferredBackBufferHeight - foodTexture.Height)
            );
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.DarkGreen);

        // TODO: Add your drawing code here
        _spriteBatch.Begin();

        int tileSize = 50;
        int rows = _graphics.PreferredBackBufferHeight / tileSize;
        int cols = _graphics.PreferredBackBufferWidth / tileSize;


        for(int y = 0; y < rows; y++) {
            for(int x = 0; x < cols; x++) {
                Color tileColor = (x+y) % 2 == 0 ? new Color(34,139,34) : new Color(0,100,0);
                _spriteBatch.Draw(squareTexture, new Rectangle(x * tileSize, y * tileSize, tileSize, tileSize), tileColor);
            }
        }

        _spriteBatch.Draw(
            ballTexture, 
            ballPosition, 
            null, 
            Color.White, 
            0f, 
            new Vector2(ballTexture.Width / 2, ballTexture.Height / 2), 
            Vector2.One, 
            SpriteEffects.None, 0f
        );
        _spriteBatch.Draw(
            foodTexture,
            foodPosition,
            Color.White
        );

        _spriteBatch.DrawString(font, $"Score:{score}", new Vector2(10,10), Color.White);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}