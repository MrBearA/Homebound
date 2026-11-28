using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

// The three endings from the GDD.
public enum EndingType { Reunion, NewPack, OpenRoad }

// EndingTrigger: when the dog walks in, the game ends with this ending's title and text.
public class EndingTrigger : MonoBehaviour
{
    public EndingType ending = EndingType.Reunion;
    public string title = "THE END";
    [TextArea] public string text = "";

    private bool reached = false;

    void Start()
    {
        // "A New Pack" only appears if you helped both Milo and Pip.
        if (ending == EndingType.NewPack && !(GameState.miloHelped && GameState.pipHelped))
        {
            gameObject.SetActive(false);   // this also hides Milo and Pip waiting here
        }
    }

    void OnTriggerEnter(Collider other)
    {
        DogMovement dog = other.GetComponent<DogMovement>();

        if (dog != null && !reached)
        {
            reached = true;

            // Stop the dog: no walking, sniffing or eating behind the ending card.
            dog.enabled = false;
            dog.GetComponent<DogEat>().enabled = false;
            dog.GetComponent<DogSniff>().enabled = false;

            // Clear the game UI so nothing overlaps the ending card.
            UIManager.Instance.HidePrompt();
            UIManager.Instance.dialoguePanel.SetActive(false);

            SceneFader.ShowCard(title, GameState.Format(text) + "\n\n<size=70%>Press R to return to the menu</size>");
        }
    }

    void Update()
    {
        // After the ending, R goes back to the Main Menu.
        if (reached && Keyboard.current.rKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene("Main Menu");
        }
    }
}
