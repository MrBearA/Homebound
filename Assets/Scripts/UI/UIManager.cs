using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public TMPro.TMP_Text promptText;
    public GameObject dialoguePanel;
    public TMPro.TMP_Text speakerText;
    public TMPro.TMP_Text dialogueText;
    public float dialogueTime = 3f;

    private float dialogueTimer = 0f;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        HidePrompt();
        dialoguePanel.SetActive(false);
    }

    public void ShowPrompt(string message)
    {
        promptText.text = message;
        promptText.gameObject.SetActive(true);
    }

    public void HidePrompt()
    {
        promptText.gameObject.SetActive(false);
    }

    public void Say(string speaker, string message)
    {
        speakerText.text = speaker;
        dialogueText.text = message;
        dialoguePanel.SetActive(true);

        dialogueTimer = dialogueTime;
    }

    void Update()
    {
        if(dialogueTimer > 0)
        {
            dialogueTimer -= Time.deltaTime;

            if(dialogueTimer <= 0)
            {
                dialoguePanel.SetActive(false);
            }
        }

    }
}
