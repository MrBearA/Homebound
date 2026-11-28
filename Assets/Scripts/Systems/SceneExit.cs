using UnityEngine;

// SceneExit: put this on a trigger collider. When the dog walks in, go to the next scene.
public class SceneExit : MonoBehaviour
{
    public string nextScene = "Scene2_Barangay";
    public string titleCard = "Scene 2 · Barangay";

    private bool used = false;

    void OnTriggerEnter(Collider other)
    {
        DogMovement dog = other.GetComponent<DogMovement>();

        if (dog != null && !used)
        {
            used = true;
            dog.enabled = false;   // stop the dog walking while the screen fades
            SceneFader.FadeToScene(nextScene, titleCard);
        }
    }
}
