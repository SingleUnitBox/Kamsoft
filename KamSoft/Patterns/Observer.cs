namespace KamSoft.Patterns;

public abstract class Publisher
{
    public event EventHandler Handler;
    
    public void Publish(EventArgs args)
    { 
        Handler?.Invoke(this, args);
    }
}

public class ScoreEventArgs : EventArgs
{
    public int Score { get; }

    public ScoreEventArgs(int score)
    {
        Score = score;
    }
}

public class Game : Publisher
{
    public event EventHandler<ScoreEventArgs> ScoreReached;
    public event EventHandler<bool> HomeTeamWon; 
    
    public void AddPoints(int points)
    {
        Console.WriteLine($"Added {points} points.");
        if (points >= 100)
        {
            ScoreReached?.Invoke(this, new ScoreEventArgs(points));
            //Publish(EventArgs.Empty);
        }
    }

    public void CheckHomeTeam(bool victory)
    {
        Console.WriteLine($"Game is publishing who won the game.");
        HomeTeamWon?.Invoke(this, victory);
    }
}

public abstract class Subscriber
{
    public abstract void OnEventRaised(object sender, EventArgs args);
    
    // add more events
    public void Subscribe(Publisher publisher)
    {
        publisher.Handler += OnEventRaised;
    }

    public void Unsubscribe(Publisher publisher)
    {
        publisher.Handler -= OnEventRaised;
    }
}

public class Player : Subscriber
{
    public override void OnEventRaised(object sender, EventArgs args)
    {
        Console.WriteLine("Player has been notified that game raised an event.");
    }

    public void OnScoreReached(object sender, ScoreEventArgs args)
    {
        Console.WriteLine($"Player reached a score of {args.Score}.");
    }
    

    public void SubscribeToGame(Game game)
    {
        game.ScoreReached += OnScoreReached;
    }
}

public class Spectator : Subscriber
{
    public void Subscribe(Game game)
    {
        game.HomeTeamWon += (obj, tof) =>
        {
            if (tof)
            {
                Console.WriteLine("Spectator - Indeed, home team won the game.");
            }
            else
            {
                Console.WriteLine("Spectator - Home team lost the game.");
            }
        };
    }

    public override void OnEventRaised(object sender, EventArgs args)
    {
        throw new NotImplementedException();
    }
}