using UnityEngine;

public class BombermanController : MonoBehaviour
{
    [Header("Player")]
    public int playerNumber; // 1 para Jugador 1, 2 para Jugador 2

    public new Rigidbody2D rigidbody { get; private set; }
    private Vector2 direction = Vector2.down;
    public float speed = 5f;

    private KeyCode inputUp;
    private KeyCode inputDown;
    private KeyCode inputLeft;
    private KeyCode inputRight;

    public AnimatorSpriteRenderer spriteRendererUp;
    public AnimatorSpriteRenderer spriteRendererDown;
    public AnimatorSpriteRenderer spriteRendererLeft;
    public AnimatorSpriteRenderer spriteRendererRight;
    public AnimatorSpriteRenderer spriteRendererDeath;

    private AnimatorSpriteRenderer activeSpriteRenderer;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        activeSpriteRenderer = spriteRendererDown;

        // Asignar controles según el jugador
        if (playerNumber == 1)
        {
            inputUp = KeyCode.W;
            inputDown = KeyCode.S;
            inputLeft = KeyCode.A;
            inputRight = KeyCode.D;
        }
        else if (playerNumber == 2)
        {
            inputUp = KeyCode.UpArrow;
            inputDown = KeyCode.DownArrow;
            inputLeft = KeyCode.LeftArrow;
            inputRight = KeyCode.RightArrow;
        }
    }

    private void Update()
    {
        if (Input.GetKey(inputUp))
        {
            SetDirection(Vector2.up, spriteRendererUp);
        }
        else if (Input.GetKey(inputDown))
        {
            SetDirection(Vector2.down, spriteRendererDown);
        }
        else if (Input.GetKey(inputLeft))
        {
            SetDirection(Vector2.left, spriteRendererLeft);
        }
        else if (Input.GetKey(inputRight))
        {
            SetDirection(Vector2.right, spriteRendererRight);
        }
        else
        {
            SetDirection(Vector2.zero, activeSpriteRenderer);
        }
    }

    private void FixedUpdate()
    {
        Vector2 position = rigidbody.position;
        Vector2 translation = direction * speed * Time.fixedDeltaTime;

        rigidbody.MovePosition(position + translation);
    }

    private void SetDirection(Vector2 newDirection, AnimatorSpriteRenderer spriteRenderer)
    {
        direction = newDirection;

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

        // Notificar al GameManager sobre la muerte
        GameManager.Instance.PlayerDied(playerNumber);
    }
}
