namespace KamSoft.Patterns;

public sealed class Singleton
{
    private static Singleton? _instance;

    public Singleton()
    {
        
    }

    public static Singleton Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new Singleton();
            }

            return _instance;
        }
    }
}