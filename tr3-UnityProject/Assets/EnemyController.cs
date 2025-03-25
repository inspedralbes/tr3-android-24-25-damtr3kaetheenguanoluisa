using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public new Rigidbody2D rigidbody { get; private set; }
    private Vector2 direction = Vector2.zero;
    public float speed = 1f; 

    public AnimatorSpriteRenderer spriteRendererUp;
    public AnimatorSpriteRenderer spriteRendererDown;
    public AnimatorSpriteRenderer spriteRendererLeft;
    public AnimatorSpriteRenderer spriteRendererRight;
    public AnimatorSpriteRenderer spriteRendererDeath;

    private AnimatorSpriteRenderer activeSpriteRenderer;

    public float changeDirectionTime = 2f; 
    private float changeDirectionTimer;      

    private Vector2 startPosition;  
    private float movementLimit = 0.3f;  

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        activeSpriteRenderer = spriteRendererDown;
        startPosition = rigidbody.position;  
        changeDirectionTimer = changeDirectionTime; 
    }

    private void Update()
    {
        changeDirectionTimer -= Time.deltaTime;

        if (changeDirectionTimer <= 0f)
        {
            ChangeDirection(); 
            changeDirectionTimer = changeDirectionTime;  
        }

        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        Vector2 position = rigidbody.position;
        Vector2 translation = direction * speed * Time.fixedDeltaTime;

        Vector2 newPosition = position + translation;

        newPosition.x = Mathf.Clamp(newPosition.x, startPosition.x - movementLimit, startPosition.x + movementLimit);
        newPosition.y = Mathf.Clamp(newPosition.y, startPosition.y - movementLimit, startPosition.y + movementLimit);

        rigidbody.MovePosition(newPosition);
    }

    private void ChangeDirection()
    {
        direction = new Vector2(Random.Range(-0.08f, 0.08f), Random.Range(-0.08f, 0.08f)).normalized;  
    }

    private void UpdateAnimation()
    {
        if (direction == Vector2.up)
        {
            SetDirection(spriteRendererUp);
        }
        else if (direction == Vector2.down)
        {
            SetDirection(spriteRendererDown);
        }
        else if (direction == Vector2.left)
        {
            SetDirection(spriteRendererLeft);
        }
        else if (direction == Vector2.right)
        {
            SetDirection(spriteRendererRight);
        }
        else
        {
            SetDirection(activeSpriteRenderer);
        }
    }

    private void SetDirection(AnimatorSpriteRenderer spriteRenderer)
    {
        spriteRendererUp.enabled = spriteRenderer == spriteRendererUp;
        spriteRendererDown.enabled = spriteRenderer == spriteRendererDown;
        spriteRendererLeft.enabled = spriteRenderer == spriteRendererLeft;
        spriteRendererRight.enabled = spriteRenderer == spriteRendererRight;

        activeSpriteRenderer = spriteRenderer;
        activeSpriteRenderer.idle = direction == Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Explosion"))
        {
            Death();
        }
    }

    private void Death()
    {
        enabled = false;
        GetComponent<BombController>().enabled = false;
        spriteRendererUp.enabled = false;
        spriteRendererDown.enabled = false;
        spriteRendererLeft.enabled = false;
        spriteRendererRight.enabled = false;
        spriteRendererDeath.enabled = true;

        Invoke(nameof(OnDeathEnd), 1.25f);
    }

    private void OnDeathEnd()
    {
        gameObject.SetActive(false);
    }
}
