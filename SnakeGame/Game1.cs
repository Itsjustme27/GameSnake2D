using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace SnakeGame;

public class Game1 : Game
{
    // Texture2D ballTexture;
    Texture2D foodTexture;
    Texture2D squareTexture;
    // Vector2 ballPosition;
    // Vector2 ballDirection;
    Vector2 foodPosition;
    // float ballSpeed;
    // float inputSpeed;
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private SpriteFont font;
    private int score = 0;
    private int highscore;
    private BorderLimit borderLimit;
    private Snake snake;

    private Dictionary<string, Texture2D> headSprites;
    private Dictionary<string, Texture2D> bodySprites;
    private Dictionary<string, Texture2D> tailSprites;

    
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // Set the initial position of the food
        foodPosition = new Vector2(100, 100);

        // Initialize the border limit
        borderLimit = new BorderLimit();

        // Read the high score from the file
        if (File.Exists("HighScore.txt")) {
            ReadHighScoreFile();
        } else {
            // If the file doesn't exist, create it with an initial high score of 0
            HighScoreFile(0);
            highscore = 0;
        }

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // Load textures
        foodTexture = Content.Load<Texture2D>("apple");
        font = Content.Load<SpriteFont>("GameFont");
        squareTexture = new Texture2D(GraphicsDevice, 1, 1);
        squareTexture.SetData(new[] { Color.LightGreen });

        // Initialize dictionaries
        headSprites = new Dictionary<string, Texture2D>();
        bodySprites = new Dictionary<string, Texture2D>();
        tailSprites = new Dictionary<string, Texture2D>();

        // Load head sprites
        headSprites.Add("up", Content.Load<Texture2D>("head_up"));
        headSprites.Add("down", Content.Load<Texture2D>("head_down"));
        headSprites.Add("left", Content.Load<Texture2D>("head_left"));
        headSprites.Add("right", Content.Load<Texture2D>("head_right"));

        // Load body sprites
        bodySprites.Add("horizontal", Content.Load<Texture2D>("body_horizontal"));
        bodySprites.Add("vertical", Content.Load<Texture2D>("body_vertical"));
        bodySprites.Add("topleft", Content.Load<Texture2D>("body_topleft"));
        bodySprites.Add("topright", Content.Load<Texture2D>("body_topright"));
        bodySprites.Add("bottomleft", Content.Load<Texture2D>("body_bottomleft"));
        bodySprites.Add("bottomright", Content.Load<Texture2D>("body_bottomright"));

        // Add missing body directions
        bodySprites.Add("up", Content.Load<Texture2D>("body_vertical"));
        bodySprites.Add("down", Content.Load<Texture2D>("body_vertical"));
        bodySprites.Add("left", Content.Load<Texture2D>("body_horizontal"));
        bodySprites.Add("right", Content.Load<Texture2D>("body_horizontal"));

        // Load tail sprites
        tailSprites.Add("up", Content.Load<Texture2D>("tail_up"));
        tailSprites.Add("down", Content.Load<Texture2D>("tail_down"));
        tailSprites.Add("left", Content.Load<Texture2D>("tail_left"));
        tailSprites.Add("right", Content.Load<Texture2D>("tail_right"));

        // Create the snake
        snake = new Snake(headSprites, bodySprites, tailSprites);
    }

    private void HighScoreFile(int highscore) {
        string fileName = "HighScore.txt";

        string intString = highscore.ToString();
        File.WriteAllText(fileName, intString);
    }

    private string ReadHighScoreFile() {
        string fileName = "HighScore.txt";
        
        string readText = File.ReadAllText(fileName);
        highscore = Convert.ToInt32(readText);
        return readText;
    }

    protected override void Update(GameTime gameTime) {
        if (!borderLimit.IsGameOver) {
            snake.Update(gameTime, _graphics, borderLimit);

            // Check for collision with food
            if (CheckCollision(snake.GetHeadPosition(), foodTexture, foodPosition, foodTexture)) {
                score++; // Increment score
                if (score > highscore) {
                    highscore = score; // Update high score
                    HighScoreFile(highscore); // Save high score to file
                }

                // Move food to a new random position
                Random random = new Random();
                foodPosition = new Vector2(
                    random.Next(0, _graphics.PreferredBackBufferWidth / 20) * 20,
                    random.Next(0, _graphics.PreferredBackBufferHeight / 20) * 20
                );

                // Grow the snake
                snake.Grow();
            }

            // Check for self-collision
            if (snake.CheckSelfCollision()) {
                Console.WriteLine("Game Over! Snake collided with itself.");
                borderLimit.SetGameOver(true); // End the game
            }
        }

        base.Update(gameTime);
    }

    // Method to check collision between two objects
    private bool CheckCollision(Vector2 obj1Position, Texture2D obj1Texture, Vector2 obj2Position, Texture2D obj2Texture) {
        Rectangle obj1Rect = new Rectangle((int)obj1Position.X, (int)obj1Position.Y, obj1Texture.Width, obj1Texture.Height);
        Rectangle obj2Rect = new Rectangle((int)obj2Position.X, (int)obj2Position.Y, obj2Texture.Width, obj2Texture.Height);

        return obj1Rect.Intersects(obj2Rect);
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

        // _spriteBatch.Draw(
        //     ballTexture, 
        //     ballPosition, 
        //     null, 
        //     Color.White, 
        //     0f, 
        //     new Vector2(ballTexture.Width / 2, ballTexture.Height / 2), 
        //     Vector2.One, 
        //     SpriteEffects.None, 0f
        // );
        _spriteBatch.Draw(
            foodTexture,
            foodPosition,
            Color.White
        );

        snake.Draw(_spriteBatch);

        string highscore = ReadHighScoreFile();
        _spriteBatch.DrawString(font, $"Score:{score}", new Vector2(10,10), Color.White);
        _spriteBatch.DrawString(font, $"Current High Score: {highscore} ", new Vector2(300,10), Color.White);
        
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}