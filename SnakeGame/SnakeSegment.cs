using Microsoft.Xna.Framework;

public class SnakeSegment {
    public Vector2 Position { get; set; }
    public string Type { get; set; } // head, body or tail
    public string Direction { get; set; } // up , down, left, right etc

    public SnakeSegment(Vector2 position, string type, string direction) {
        Position = position;
        Type = type;
        Direction = direction;
    }
}