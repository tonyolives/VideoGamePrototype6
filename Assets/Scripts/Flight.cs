using Unity.Mathematics.Geometry;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Flight : MonoBehaviour
{
    //random position variables
    public float speed = 5f;
    public float flyingDuration = 1.5f;
    private float flightTimer = 2f;
    private bool canFly = true;
    private Vector2 currentDirection;
    private float directionChangeInterval = 0.1f; 
    private float directionTimer = 0f;
    private float maxJitter = 2f;  
    [SerializeField] Sprite topView;
    private float damage;
    private bool hazy;
    private Camera mainCamera;
    private Vector3 screenPos;

    private Vector2 spawnPosition;

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        screenPos = mainCamera.WorldToViewportPoint(transform.position);
        spawnPosition = transform.position; 
        flightTimer = flyingDuration;
        currentDirection = Random.insideUnitCircle.normalized; 
        if(gameObject.tag == "Enemy"){

            damage = 1;
            hazy = false;

        } else if (gameObject.tag == "DoubleDamageEnemy"){

            damage = 2;
            hazy = false;

        } else {

            damage = 1;
            hazy = true;

        }
    }

   void Update()
{
    if (canFly)
    {
        //change movement direction at intervals
        directionTimer -= Time.deltaTime;
        if (directionTimer <= 0f)
        {
            currentDirection = Random.insideUnitCircle.normalized * Random.Range(0.5f, maxJitter);  
            directionTimer = directionChangeInterval;
        }

        //movement
        transform.position += (Vector3)(currentDirection * speed * Time.deltaTime);

        //prevent going off-screen
        KeepWithinBounds();

        //stop flying countdown
        flightTimer -= Time.deltaTime;
        if (flightTimer <= 0f)
        {
            canFly = false;
            gameObject.GetComponent<SpriteRenderer>().sprite = topView;
            InvokeRepeating("DoDamage", 0f, 1.5f);
            if (hazy)
            {
                InvokeRepeating("Hazy", 0f, 0.3f);
            }
        }
    }

    FlipSpriteDirection();

    CheckMouseClick();
}

void KeepWithinBounds()
{
    //check if within  screen boundaries
    if (screenPos.x < 0f)
    {
        currentDirection.x = Mathf.Abs(currentDirection.x);
    }
    else if (screenPos.x > 1f)
    {
        currentDirection.x = -Mathf.Abs(currentDirection.x);
    }

    if (screenPos.y < 0f)
    {
        currentDirection.y = Mathf.Abs(currentDirection.y);
    }
    else if (screenPos.y > 1f)
    {
        currentDirection.y = -Mathf.Abs(currentDirection.y);
    }
}

    private void FlipSpriteDirection()
    {
        if (currentDirection.x < 0)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
    }

    //when clicked
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

    void Hazy(){

        //do the hazy thing

    }
    void DoDamage(){

        //lower player health by "damage" var

    }

}
