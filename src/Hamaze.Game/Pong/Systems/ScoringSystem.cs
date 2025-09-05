using Hamaze.Engine.Core;
using Hamaze.Engine.Core.Events;

namespace Hamaze.Game.Pong.Systems;

public class ScoringSystem : WorldSystem
{
  int player1Score = 0;
  int player2Score = 0;

  public override void Initialize(IEnumerable<IEntity> Entities)
  {
    EventManager.Subscribe<ScoreEvent>(OnScoreEvent);
  }

  private void OnScoreEvent(ScoreEvent @event)
  {
    if (@event.PlayerId == 1)
    {
      player1Score++;
    }
    else if (@event.PlayerId == 2)
    {
      player2Score++;
    }

    if (player1Score >= 5 || player2Score >= 5)
    {
      Console.WriteLine($"Player {(@event.PlayerId == 1 ? 1 : 2)} wins!");
    }
  }

  public override void Draw(IEnumerable<IEntity> Entities)
  {
    base.Draw(Entities);
    DrawScores();
  }

  private void DrawScores()
  {
    // Draw player scores on the screen
    DrawText($"Player 1: {player1Score}", 10, 10, 20, Color.White);
    DrawText($"Player 2: {player2Score}", (int)Viewport.ScreenSize.X - 150, 10, 20, Color.White);
  }
}
