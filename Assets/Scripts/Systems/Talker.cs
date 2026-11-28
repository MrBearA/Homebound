using System.Collections;
using UnityEngine;

// Talker: put this on a trigger zone around an NPC. While the dog is inside,
// the NPC says each line, one after another.
public class Talker : MonoBehaviour
{
    public string speakerName = "Someone";
    [TextArea] public string[] lines;

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<DogMovement>() != null && lines != null && lines.Length > 0)
        {
            StopAllCoroutines();
            StartCoroutine(SayLines());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<DogMovement>() != null)
        {
            StopAllCoroutines();   // the dog walked away, so stop talking
        }
    }

    IEnumerator SayLines()
    {
        for (int i = 0; i < lines.Length; i++)
        {
            UIManager.Instance.Say(speakerName, GameState.Format(lines[i]));
            yield return new WaitForSeconds(UIManager.Instance.dialogueTime);
        }
    }
}
