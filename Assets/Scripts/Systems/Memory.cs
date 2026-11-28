using UnityEngine;

// Memory: a glowing ball. When the dog touches it, count it and show the memory line.
public class Memory : MonoBehaviour
{
    [TextArea] public string line = "A happy memory.";

    void OnTriggerEnter(Collider other)
    {
        DogMovement dog = other.GetComponent<DogMovement>();

        if (dog != null)
        {
            GameState.memoriesFound++;
            UIManager.Instance.Say("Memory " + GameState.memoriesFound, GameState.Format(line));
            gameObject.SetActive(false);   // hide it so it can only be found once
        }
    }
}
