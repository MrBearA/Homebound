using UnityEngine;

public class BattleTest : MonoBehaviour
{
    void Start()
    {
        Debug.Log("Bark vs Angry: " + BattleRules.Multiplier(MoveKind.Loud, Mood.Angry));
        Debug.Log("Play Bow vs Proud: " + BattleRules.Multiplier(MoveKind.Gentle, Mood.Proud));
        Debug.Log("Hold Ground vs Scared: " + BattleRules.Multiplier(MoveKind.Firm, Mood.Scared));
    }
}
