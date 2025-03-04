using Unity.Mathematics.Geometry;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FlyingCircle : MonoBehaviour
{
    // random position variables
    public float speed = 5f;
    public float flyingDuration = 1.5f;
    private float flightTimer = 0f;
    private bool canFly = true;

    void Start()
    {
        flightTimer = flyingDuration;
    }

    void Update()
    {
        if (canFly) {
            // Jittery random mosquito-like movement
            Vector2 randomDirection = Random.insideUnitCircle.normalized;  // Random direction each frame
            transform.position += (Vector3)(randomDirection * speed * Time.deltaTime);

            // Countdown to stop flying
            flightTimer -= Time.deltaTime;
            if (flightTimer <= 0f)
            {
                canFly = false;
            }
        }
    }
    // when clicked
    private void OnMouseDown()
    {
        Destroy(gameObject);
    }
}
