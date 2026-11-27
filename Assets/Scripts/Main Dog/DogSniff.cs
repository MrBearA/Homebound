using UnityEngine;
using UnityEngine.InputSystem;

public class DogSniff : MonoBehaviour
{
    public Transform[] trailPoints;
    public GameObject pawPrintPrefab;
    public float pawSpacing = 1f;
    public float trailDuration = 5f;
    public float strongTrailDuration = 10f;
    public float cooldown = 2f;

    private InputAction sniffAction;
    private float cooldownTimer = 0f;
    private DogEat dogEat;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sniffAction = InputSystem.actions.FindAction("Sniff");
        dogEat = GetComponent<DogEat>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }

        if (sniffAction.WasPerformedThisFrame() && cooldownTimer <= 0f)
        {
            float duration = trailDuration;

            if (dogEat.HasStrongNose())
            {
                duration = strongTrailDuration;
            }

            ShowTrail(duration);
            cooldownTimer = duration + cooldown;
        }
    }

    void ShowTrail(float duration)
    {
        for (int i = 0; i < trailPoints.Length - 1; i++)
        {
            Vector3 start = trailPoints[i].position;
            Vector3 end = trailPoints[i + 1].position;
            int steps = Mathf.Max(1, Mathf.FloorToInt(Vector3.Distance(start, end) / pawSpacing));

            for (int s = 0; s < steps; s++)
            {
                Vector3 position = Vector3.Lerp(start, end, (float)s / steps);
                Quaternion rotation = Quaternion.LookRotation(end - start);
                GameObject pawPrint = Instantiate(pawPrintPrefab, position, rotation);
                Destroy(pawPrint, duration);
            }
        }
    }
}
