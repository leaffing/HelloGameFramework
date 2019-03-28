using System;
using GameFramework.Event;

class GameBeganEventArgs : GameEventArgs
{
    public static readonly int EventId = typeof(GameBeganEventArgs).GetHashCode();

    public override int Id
    {
        get
        {
            return EventId;
        }
    }

    public override void Clear()
    {
        
    }
}