using UnityEngine;

public class BattleOpponent : MonoBehaviour
{
    public string displayName = "Brutus";
    public float maxHostility = 12f;
    public Mood mood = Mood.Proud;
    public Mood[] moodPattern = { Mood.Proud, Mood.Angry, Mood.Scared };
    public float hostility;
    public bool respected = false;

    private int patternIndex = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hostility = maxHostility;
    }

    public void TakeHit(MoveKind kind, float power  )
    {
        float amount = power * BattleRules.Multiplier(kind, mood);
        hostility -= amount;
        Debug.Log(displayName + " ("+ mood + ") takes " + amount + ". Hostility: " + hostility + "/" + maxHostility);

        if(hostility <= 0)
        {
            hostility = 0;
            respected = true;
            Debug.Log(displayName + " respects you!");
        }
    }

    public void AdvanceMood()
    {
        patternIndex = (patternIndex + 1) % moodPattern.Length;
        mood = moodPattern[patternIndex];
        Debug.Log(displayName + " now looks " + mood);
    }


}
