using UnityEngine;

public enum MoveKind{Loud, Firm, Gentle, None}
public enum Mood{Proud, Angry, Scared}

public static class BattleRules
{
    public static MoveKind WeakTo(Mood mood)
    {
        switch (mood)
        {
            case Mood.Proud:
                return MoveKind.Firm;
            case Mood.Angry:
                return MoveKind.Loud;
            default:
                return MoveKind.Gentle;
        }
    }
    
    public static MoveKind Resists(Mood mood)
    {
        switch (mood)
        {
            case Mood.Proud:
                return MoveKind.Gentle;
            case Mood.Angry:
                return MoveKind.Firm;
            default:
                return MoveKind.Loud;
        }
    }

    public static float Multiplier(MoveKind kind, Mood mood)
    {
        if (kind == WeakTo(mood))
        {
            return 2f;
        }
        else if (kind == Resists(mood))
        {
            return 0.5f;
        }
        else
        {
            return 1f;
        }
    }
}
