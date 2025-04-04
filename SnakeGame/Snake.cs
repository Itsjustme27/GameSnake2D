using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

public class Snake {
    private List<SnakeSegment> segments;
    private Dictionary<string, Texture2D> headSprites;
    private Dictionary<string, Texture2D> bodySprites;
    private Dictionary<string, Texture2D> tailSprites;
    private Vector2 direction;
    private float moveTimer;
    private float moveDelay = 0.1f; // move every 100 ms

    public Snake(Dictionary<string, Texture2D> headSprites,
                 Dictionary<string, Texture2D> bodySprites,
                 Dictionary<string, Texture2D> tailSprites) 
    {
        this.headSprites = headSprites;
        this.bodySprites = bodySprites;
        this.tailSprites = tailSprites;

        // Initialize with 3 segments
        segments = new List<SnakeSegment>
        {
            new SnakeSegment(new Vector2(100, 100), "head", "right"),
            new SnakeSegment(new Vector2(80, 100), "body" , "horizontal"),
            new SnakeSegment(new Vector2(60, 100), "tail", "right")
        };

        direction = new Vector2(1, 0);  // Moving right initially
    }

    public void Update(GameTime gameTime, GraphicsDeviceManager graphics, BorderLimit borderLimit) {
        // Handle input to change direction
        KeyboardState keyState = Keyboard.GetState();
        
        if ((keyState.IsKeyDown(Keys.Up) || keyState.IsKeyDown(Keys.W)) && direction.Y != 1)
        {
            direction = new Vector2(0, -1);
        }
        else if ((keyState.IsKeyDown(Keys.Down) || keyState.IsKeyDown(Keys.S)) && direction.Y != -1)
        {
            direction = new Vector2(0, 1);
        }
        else if ((keyState.IsKeyDown(Keys.Left) || keyState.IsKeyDown(Keys.A)) && direction.X != 1)
        {
            direction = new Vector2(-1, 0);
        }
        else if ((keyState.IsKeyDown(Keys.Right) || keyState.IsKeyDown(Keys.D)) && direction.X != -1)
        {
            direction = new Vector2(1, 0);
        }

        moveTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (moveTimer >= moveDelay) {
            moveTimer = 0;
            Move();

            // Check for border collision
            if (CheckBorderCollision(graphics, borderLimit)) {
                Console.WriteLine("Game Over! Snake hit the border.");
                System.Environment.Exit(0); // End the game
            }
        }
    }

    public void Move() {
        List<Vector2> prevPositions = segments.Select(s => s.Position).ToList();

        // Move head
        Vector2 newHeadPos = segments[0].Position + new Vector2(direction.X * 20, direction.Y * 20);
        segments[0].Position = newHeadPos;

        // Update Head direction
        if(direction.X == 1) segments[0].Direction = "right";
        else if(direction.X == -1) segments[0].Direction = "left";
        else if(direction.Y == 1) segments[0].Direction = "down";
        else if(direction.Y == -1) segments[0].Direction = "up";

        // Move body segments
        for(int i = 1; i < segments.Count; i++) {
            segments[i].Position = prevPositions[i-1];
        }

        // Update body segment directions
        UpdateBodySegmentDirections();

        // Update Tail direction
        UpdateTailDirection();

    }

    private void UpdateBodySegmentDirections() {
        // For each body segment (excluding head and tail) 
        for(int i = 1; i < segments.Count - 1; i++) {
            Vector2 prev = segments[i -1].Position;
            Vector2 current = segments[i].Position;
            Vector2 next = segments[i + 1].Position;

            // Determine if it's a straight segment or a corner
            if(prev.X == next.X) {                  // Vertical Straight
                segments[i].Direction = "vertical";
            } else if(prev.Y == next.Y) {           // Horizontal Straight
                segments[i].Direction = "horizontal";
            } else {                                // It's a corner
                // Determine which corner
                if (prev.X > current.X && next.Y < current.Y || prev.Y < current.Y && next.X > current.X)
                {
                    segments[i].Direction = "bottomright";
                }
                else if (prev.X < current.X && next.Y < current.Y || prev.Y < current.Y && next.X < current.X)
                {
                    segments[i].Direction = "bottomleft";
                }
                else if (prev.X > current.X && next.Y > current.Y || prev.Y > current.Y && next.X > current.X)
                {
                    segments[i].Direction = "topright";
                }
                else
                {
                    segments[i].Direction = "topleft";
                }
            }
        }
    }

    public void UpdateTailDirection() {
        int lastIndex = segments.Count - 1;
        int secondLastIndex = lastIndex - 1;

        Vector2 tailPos = segments[lastIndex].Position;
        Vector2 beforeTailPos = segments[secondLastIndex].Position;

        if(beforeTailPos.X > tailPos.X) {
            segments[lastIndex].Direction = "right";
        } else if(beforeTailPos.X < tailPos.X) {
            segments[lastIndex].Direction = "left";
        } else if(beforeTailPos.Y > tailPos.Y) {
            segments[lastIndex].Direction = "down";
        } else if(beforeTailPos.Y < tailPos.Y) {
            segments[lastIndex].Direction = "up";
        }
    }

    public void Draw(SpriteBatch spriteBatch) {
        foreach (var segment in segments) {
            Texture2D texture = null;

            switch (segment.Type) {
                case "head":
                    if (headSprites.ContainsKey(segment.Direction)) {
                        texture = headSprites[segment.Direction];
                    } else {
                        texture = headSprites.Values.FirstOrDefault();
                        Console.WriteLine($"Missing head sprite direction: {segment.Direction}");
                    }
                    break;
                case "body":
                    if (bodySprites.ContainsKey(segment.Direction)) {
                        texture = bodySprites[segment.Direction];
                    } else {
                        texture = bodySprites.Values.FirstOrDefault();
                        Console.WriteLine($"Missing body sprite direction: {segment.Direction}");
                    }
                    break;
                case "tail":
                    if (tailSprites.ContainsKey(segment.Direction)) {
                        texture = tailSprites[segment.Direction];
                    } else {
                        texture = tailSprites.Values.FirstOrDefault();
                        Console.WriteLine($"Missing tail sprite direction: {segment.Direction}");
                    }
                    break;
            }

            if (texture != null) {
                spriteBatch.Draw(texture, segment.Position, Color.White);
            }
        }
    }

    private bool CheckBorderCollision(GraphicsDeviceManager graphics, BorderLimit borderLimit) {
        Vector2 headPosition = segments[0].Position;

        // Check if the head is outside the game window
        if (headPosition.X < 0 || headPosition.X >= graphics.PreferredBackBufferWidth ||
            headPosition.Y < 0 || headPosition.Y >= graphics.PreferredBackBufferHeight) {
            borderLimit.SetGameOver(true); // Use the method to set IsGameOver
            return true;
        }

        return false;
    }

    public void Grow() {
        // Add a new segment at the tail position
        SnakeSegment tail = segments[segments.Count - 1];
        SnakeSegment newTail = new SnakeSegment(tail.Position, "tail", tail.Direction);

        // Change the old tail to a body segment
        tail.Type = "body";

        // Add the new tail
        segments.Add(newTail);
    }

    public Vector2 GetHeadPosition() {
        return segments[0].Position;
    }

    public bool CheckSelfCollision() {
        Vector2 headPosition = segments[0].Position;

        // Check if the head collides with any body segment
        for (int i = 1; i < segments.Count; i++) {
            if (segments[i].Position == headPosition) {
                return true;
            }
        }

        return false;
    }
}



