// GameState: remembers things between scenes.
// "static" means there is only ONE copy, and it is not attached to any GameObject,
// so its values survive when we load a new scene.
public static class GameState
{
    public static string dogName = "Shebu";
    public static int memoriesFound = 0;
    public static bool miloHelped = false;
    public static bool pipHelped = false;

    // Called by the Main Menu when a new game starts.
    public static void ResetAll()
    {
        memoriesFound = 0;
        miloHelped = false;
        pipHelped = false;
    }

    // Replaces "{name}" in a line of text with the dog's name.
    public static string Format(string text)
    {
        return text.Replace("{name}", dogName);
    }
}
