using UnityEngine;

public class DogController : MonoBehaviour
{
    public string dogName = "Shebu";
    public int age = 2;
    private float timeAlive = 0f;
    public float waitTime = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log(" Woof! My name is " + dogName + " and I am " + age + " years old ");
    }

    // Update is called once per frame
    void Update()
    {
        timeAlive += Time.deltaTime;

        if(timeAlive > waitTime)
        {
            Debug.Log(" Ive been waiting for " + waitTime + " seconds for you, where is my family? ");
            timeAlive = 0f;
        }

    }
}
