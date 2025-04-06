using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;
using System.Collections;
using MyGame.Models;

public class BombermanController : MonoBehaviour
{
    [Header("Player")]
    public int playerNumber; 

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

    public int bombsUsed = 0;
    public int enemiesDefeated = 0;
    public int victories { get; set; }
    public int playerId;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        activeSpriteRenderer = spriteRendererDown;

        bombsUsed = PlayerPrefs.GetInt($"player{playerNumber}BombsUsed", 0);
        enemiesDefeated = PlayerPrefs.GetInt($"player{playerNumber}enemiesDefeated", 0);
        victories = PlayerPrefs.GetInt($"player{playerNumber}victories", 0);
        speed = PlayerPrefs.GetInt($"player{playerNumber}speed", 5);

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
        playerId = PlayerPrefs.GetInt("player" + playerNumber + "Id", -1);

        Debug.Log($"Jugador {playerNumber} inicialitzat amb:\n" +
            $"Bombes utlitzades: {bombsUsed}\n" +
            $"Enemics eliminats: {enemiesDefeated}\n" +
            $"Victories: {victories}\n" +
            $"Velocitat: {speed}");
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
        else if (other.gameObject.CompareTag("Enemy"))
        {
            enemiesDefeated++; 
            Debug.Log($" Enemics derrotats pel jugador{playerNumber}: {enemiesDefeated}");
        }
    }

    public void IncrementBombsUsed()
    {
        bombsUsed++;
        Debug.Log($" Bombes utilitzades per jugador {playerNumber}: {bombsUsed}");
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

        Destroy(gameObject, 1f);

        GameManager.instance.PlayerDied(this);

    }
}
