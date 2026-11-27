using UnityEngine;

public class HomeGoal : MonoBehaviour
{

    private bool reached = false;

    void OnTriggerEnter(Collider other)
    {
        DogMovement dog = other.GetComponent<DogMovement>();

        if (dog != null && !reached)
        {
            reached = true;
            UIManager.Instance.Say("You", "I made it home! I can finally rest now.");
        }
    }
}
