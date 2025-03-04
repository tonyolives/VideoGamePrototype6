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

        CheckMouseClick();
    }
    
    // when clicked
    private void CheckMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePosition2D = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

            Collider2D hitCollider = Physics2D.OverlapPoint(mousePosition2D);

            if (hitCollider != null && hitCollider.gameObject == gameObject)
            {
                Destroy(gameObject);
            }
        }
    }
}
