using UnityEngine;
using UnityEngine.InputSystem;

public class BattleTest : MonoBehaviour
{
    public BattleOpponent target;

    void Update()
    {
        if(Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            target.TakeHit(MoveKind.Loud, 2f);
        }

        else if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            target.TakeHit(MoveKind.Firm, 2f);
        }

         else if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            target.TakeHit(MoveKind.Gentle, 3f);
        }
    }
}
