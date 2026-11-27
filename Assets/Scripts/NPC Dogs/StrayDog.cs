using UnityEngine;

public class StrayDog : MonoBehaviour
{

    public string dogName = "Pip";
    public bool helped = false;
    public float backAwayDistance = 1.5f;

    private bool greeted = false;

    public bool ReceiveFood(FoodItem food)
    {
        if (helped)
        {
            UIManager.Instance.Say(dogName, "Busog na me, Thanks!!!");
            return false;
        }

        if (food.IsSafe())
        {
            helped = true;
            UIManager.Instance.Say(dogName, "Happy eating the " + food.foodName + "! Thank you for helping me!");
            return true;
        }

        Vector3 awayDirection = transform.position - food.transform.position;
        awayDirection.y = 0f;
        awayDirection.Normalize();
        transform.position += awayDirection * backAwayDistance;


        UIManager.Instance.Say(dogName, " Sniffs the " + food.foodName + " Yuck so gross..");
        return false;
    }

    public void Greet()
    {
        if(!greeted)
        {
            UIManager.Instance.Say(dogName, "  Woof! Woof!");
            greeted = true;
        }
        else
        {
            if(helped)
            {
                UIManager.Instance.Say(dogName, "Woof! Thanks for helping me!");
            }
        }

    }
}
