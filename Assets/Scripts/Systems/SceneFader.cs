using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// SceneFader: fades the screen to black, loads the next scene, shows a title card, and fades back in.
// It builds its own black screen in code, so you don't have to add anything to your scenes.
// DontDestroyOnLoad keeps it alive while the next scene loads.
public class SceneFader : MonoBehaviour
{
    public float fadeTime = 0.8f;
    public float titleTime = 1.5f;

    static SceneFader instance;

    CanvasGroup group;
    Image background;
    TMPro.TextMeshProUGUI titleText;
    TMPro.TextMeshProUGUI bodyText;
    bool busy = false;

    // Finds the fader, or creates one the first time it is needed.
    static SceneFader Get()
    {
        if (instance == null)
        {
            GameObject go = new GameObject("SceneFader");
            instance = go.AddComponent<SceneFader>();
            DontDestroyOnLoad(go);
            instance.BuildScreen();
        }
        return instance;
    }

    public static void FadeToScene(string sceneName, string title)
    {
        SceneFader fader = Get();
        if (!fader.busy)
        {
            fader.StartCoroutine(fader.FadeRoutine(sceneName, title));
        }
    }

    // Shows a big title and text over the game (used by the endings). It stays until a new scene loads.
    public static void ShowCard(string title, string text)
    {
        SceneFader fader = Get();
        fader.StopAllCoroutines();
        fader.busy = false;
        fader.titleText.text = title;
        fader.bodyText.text = text;
        fader.background.color = new Color(0f, 0f, 0f, 0.75f);
        fader.StartCoroutine(fader.Fade(0f, 1f));
    }

    IEnumerator FadeRoutine(string sceneName, string title)
    {
        busy = true;
        titleText.text = "";
        bodyText.text = "";
        background.color = Color.black;

        yield return Fade(0f, 1f);                 // fade to black

        AsyncOperation load = SceneManager.LoadSceneAsync(sceneName);
        while (!load.isDone)
        {
            yield return null;                     // wait one frame, then check again
        }

        titleText.text = title;                    // show the title card on the black screen
        yield return new WaitForSecondsRealtime(titleTime);

        yield return Fade(1f, 0f);                 // fade back in
        titleText.text = "";
        busy = false;
    }

    IEnumerator Fade(float from, float to)
    {
        float t = 0f;
        while (t < fadeTime)
        {
            t += Time.unscaledDeltaTime;
            group.alpha = Mathf.Lerp(from, to, t / fadeTime);
            yield return null;
        }
        group.alpha = to;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // If a scene loads some other way (like pressing R), hide any ending card.
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!busy && group != null)
        {
            group.alpha = 0f;
        }
    }

    // Makes a Canvas with a full-screen Image and two texts.
    void BuildScreen()
    {
        Canvas canvas = gameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100;                 // draw on top of the game UI
        CanvasScaler scaler = gameObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        group = gameObject.AddComponent<CanvasGroup>();
        group.alpha = 0f;
        group.blocksRaycasts = false;              // never block clicks on menu buttons

        background = new GameObject("Black").AddComponent<Image>();
        background.transform.SetParent(transform, false);
        Stretch(background.rectTransform, 0f, 1f);

        titleText = MakeText("Title", 72, 0.5f, 0.75f);
        bodyText = MakeText("Body", 36, 0.15f, 0.5f);
    }

    TMPro.TextMeshProUGUI MakeText(string name, float size, float bottom, float top)
    {
        TMPro.TextMeshProUGUI text = new GameObject(name).AddComponent<TMPro.TextMeshProUGUI>();
        text.transform.SetParent(transform, false);
        Stretch(text.rectTransform, bottom, top);
        text.fontSize = size;
        text.alignment = TMPro.TextAlignmentOptions.Center;
        text.color = Color.white;
        text.text = "";
        return text;
    }

    void Stretch(RectTransform rt, float bottom, float top)
    {
        rt.anchorMin = new Vector2(0.1f, bottom);
        rt.anchorMax = new Vector2(0.9f, top);
        if (bottom == 0f && top == 1f)
        {
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
        }
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }
}
