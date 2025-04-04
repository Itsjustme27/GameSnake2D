public class BorderLimit
{
    public bool IsGameOver { get; private set; } // Make the setter private

    public void SetGameOver(bool value)
    {
        IsGameOver = value; // Provide a method to update the property
    }
}