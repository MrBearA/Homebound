using UnityEngine;
using UnityEngine.InputSystem;

public class DogEat : MonoBehaviour
{
    public float sickDuration = 30f;
    public float sickSpeed = 0.5f;
    public float strongNoseDuration = 60f;
    public float eatHoldTime = 0.5f;
    public Transform mouthPoint;

    private InputAction interactAction;
    private DogMovement movement;
    private FoodItem nearbyfood;
    private FoodItem carriedFood;
    private StrayDog nearbyStray;
    private float sickTimer = 0f;
    private float strongNoseTimer = 0f;
    private float holdTimer = 0f;
    private bool holdUsed = false;

    void Start()
    {
        interactAction = InputSystem.actions.FindAction("Interact");
        movement = GetComponent<DogMovement>();
    }

    void Update()
    {
        UpdateTimers();
        HandleInteractInput();
        UpdatePrompt();
    }

    void UpdatePrompt()
    {
        if(carriedFood != null && nearbyStray != null)
        {
            UIManager.Instance.ShowPrompt("Tap E: give to " + nearbyStray.dogName);
        }
        
        else if (carriedFood != null)
        {
            UIManager.Instance.ShowPrompt("Tap E: drop Hold E: eat");
        }

        else if (nearbyfood != null)
        {
            UIManager.Instance.ShowPrompt("Tap E: pick up the: " + nearbyfood.foodName + " Hold E: eat the:  " + nearbyfood.foodName);
        }
        
        else
        {
            UIManager.Instance.HidePrompt();
        }
    }

    void UpdateTimers()
    {
        if (sickTimer > 0f)
        {
            sickTimer -= Time.deltaTime;
            if (sickTimer <= 0f)
            {
                movement.speedMultiplier = 1f;
                Debug.Log("I feel better now!");
            }
        }

        if (strongNoseTimer > 0f)
        {
            strongNoseTimer -= Time.deltaTime;
        }
    }

    void HandleInteractInput()
    {
        if (interactAction.IsPressed())
        {
            holdTimer += Time.deltaTime;

            if (holdTimer >= eatHoldTime && !holdUsed)
            {
                holdUsed = true;
                if (carriedFood != null)
                {
                    Eat(carriedFood);
                }
                else if (nearbyfood != null)
                {
                    Eat(nearbyfood);
                }
            }
        }

        if (interactAction.WasReleasedThisFrame())
        {
            if (!holdUsed)
            {
                Tap();
            }
            holdTimer = 0f;
            holdUsed = false;
        }
    }

    void Tap()
    {
        if (carriedFood != null)
        {
            if (nearbyStray != null)
            {
                GiveFood();
            }
            else
            {
                DropFood();
            }
        }
        else if (nearbyfood != null)
        {
            PickUp(nearbyfood);
        }
    }

    void PickUp(FoodItem food)
    {
        carriedFood = food;
        nearbyfood = null;
        food.GetComponent<Collider>().enabled = false;
        food.transform.SetParent(mouthPoint);
        food.transform.localPosition = Vector3.zero;
        Debug.Log("Picked up the " + food.foodName);
    }

    void DropFood()
    {
        carriedFood.transform.SetParent(null);
        Vector3 dropPosition = mouthPoint.position;
        dropPosition.y = 0.15f;
        carriedFood.transform.position = dropPosition;
        carriedFood.GetComponent<Collider>().enabled = true;
        Debug.Log("Dropped the " + carriedFood.foodName);
        carriedFood = null;
    }

    void GiveFood()
    {
        bool accepted = nearbyStray.ReceiveFood(carriedFood);
        if (accepted)
        {
            Destroy(carriedFood.gameObject);
            carriedFood = null;
        }
    }

    void Eat(FoodItem food)
    {
        if (food.IsSafe())
        {
            strongNoseTimer = strongNoseDuration;
            Debug.Log("Yum, " + food.foodName + "! My nose is stronger now!");
        }
        else
        {
            sickTimer = sickDuration;
            movement.speedMultiplier = sickSpeed;
            Debug.Log("Yuck, the " + food.foodName + " was " + food.type + ". I feel sick.");
        }

        if (food == carriedFood)
        {
            carriedFood = null;
        }
        if (food == nearbyfood)
        {
            nearbyfood = null;
        }
        Destroy(food.gameObject);
    }

    public bool HasStrongNose()
    {
        return strongNoseTimer > 0f;
    }

    void OnTriggerEnter(Collider other)
    {
        FoodItem food = other.GetComponent<FoodItem>();
        if (food != null)
        {
            nearbyfood = food;
            Debug.Log("Tap E to pick up, hold E to eat the " + food.foodName);
        }

        StrayDog stray = other.GetComponent<StrayDog>();
        if (stray != null)
        {
            nearbyStray = stray;
            stray.Greet();
        }
    }

    void OnTriggerExit(Collider other)
    {
        FoodItem food = other.GetComponent<FoodItem>();
        if (food != null && food == nearbyfood)
        {
            nearbyfood = null;
        }

        StrayDog stray = other.GetComponent<StrayDog>();
        if (stray != null && stray == nearbyStray)
        {
            nearbyStray = null;
        }
    }
}