using UnityEngine;

public enum FoodType { Clean, Dirty, Spoiled, Unsafe }

public class FoodItem : MonoBehaviour
{
    public string foodName = "Pandesal";
    public FoodType type = FoodType.Clean;

    public bool IsSafe()
    {
        return type == FoodType.Clean;
    }
}