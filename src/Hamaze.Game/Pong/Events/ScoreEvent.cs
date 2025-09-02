using Hamaze.Engine.Core.Events;

namespace Hamaze.Game.Pong;

public record ScoreEvent(int PlayerId) : IEvent;
