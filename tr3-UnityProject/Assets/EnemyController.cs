using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public new Rigidbody2D rigidbody { get; private set; }
    private Vector2 direction = Vector2.zero;
    public float speed = 1f;  // Velocidad más baja para movimiento sutil

    public AnimatorSpriteRenderer spriteRendererUp;
    public AnimatorSpriteRenderer spriteRendererDown;
    public AnimatorSpriteRenderer spriteRendererLeft;
    public AnimatorSpriteRenderer spriteRendererRight;
    public AnimatorSpriteRenderer spriteRendererDeath;

    private AnimatorSpriteRenderer activeSpriteRenderer;

    // Parámetros de aleatorización
    public float changeDirectionTime = 2f;  // Tiempo para cambiar de dirección
    private float changeDirectionTimer;      // Temporizador para cambiar la dirección

    private Vector2 startPosition;  // Posición inicial del enemigo
    private float movementLimit = 0.5f;  // El límite máximo de movimiento (dentro de su casilla)

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        activeSpriteRenderer = spriteRendererDown;
        startPosition = rigidbody.position;  // Guardar la posición inicial
        changeDirectionTimer = changeDirectionTime;  // Inicializa el temporizador
    }

    private void Update()
    {
        // Actualiza el temporizador y cambia la dirección aleatoriamente
        changeDirectionTimer -= Time.deltaTime;

        if (changeDirectionTimer <= 0f)
        {
            ChangeDirection();  // Cambiar dirección aleatoriamente
            changeDirectionTimer = changeDirectionTime;  // Reiniciar el temporizador
        }

        // Actualiza la animación según la dirección
        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        Vector2 position = rigidbody.position;
        Vector2 translation = direction * speed * Time.fixedDeltaTime;

        // Limitamos el movimiento del enemigo para que no se aleje de su casilla
        Vector2 newPosition = position + translation;

        // Asegurarse de que el enemigo no se mueva más allá de un rango limitado de la posición inicial
        newPosition.x = Mathf.Clamp(newPosition.x, startPosition.x - movementLimit, startPosition.x + movementLimit);
        newPosition.y = Mathf.Clamp(newPosition.y, startPosition.y - movementLimit, startPosition.y + movementLimit);

        rigidbody.MovePosition(newPosition);
    }

    // Cambia la dirección de forma aleatoria dentro de un rango limitado
    private void ChangeDirection()
    {
        // Direcciones aleatorias muy pequeñas dentro de un rango
        direction = new Vector2(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f)).normalized;  // Movimiento más sutil
    }

    // Actualiza la animación según la dirección
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
