using UnityEngine;

public class PlayerCollision : MonoBehaviour
{

    public GameObject Car;
    private Rigidbody rb;
    // This function runs when we hit another object.
    // We get information about the collision and call it "collisionInfo".
    public void Start()
    {
        rb = Car.GetComponent<Rigidbody>();
    }
    void OnTriggerEnter(Collider collisionInfo)
    {
        
        // We check if the object we collided with has a tag called "WaterBasicDaytime".
        if (collisionInfo.gameObject.CompareTag("Water"))
        {
           
            rb.isKinematic = true;
            //FindObjectOfType<GameManager>().EndGame();
        }
        
        // We check if the object we collided with has a tag called "rocks".
        if (collisionInfo.gameObject.CompareTag("rocks"))
        {
            rb.isKinematic = true;// Disable the car movement.
            //FindObjectOfType<GameManager>().EndGame();
        }
        
    }

}

