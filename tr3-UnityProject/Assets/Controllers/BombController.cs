using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BombController : MonoBehaviour
{
    [Header("Jugador")]
    public int playerID = 1;  

    [Header("Bomb")]
    public KeyCode inputKey = KeyCode.Space; 
    public GameObject bombPrefab;
    public float bombFuseTime = 3f;
    public int bombAmount = 5;
    private int bombsRemaining;

    [Header("Explosion")]
    public Explosion explosionPrefab;
    public LayerMask obstacleLayer;
    public float explosionDuration = 1f;
    public int explosionRadius = 1;

    [Header("Destructible-Blocks")]
    public Tilemap destructibleTiles;
    public DestructibleBlocksController destructiblePrefab;

    private void OnEnable()
    {
        bombsRemaining = bombAmount;
    }

    private void Start()
    {
        Debug.Log("Jugador asignado: " + playerID);
        AsignarTeclas();
        bombAmount = PlayerPrefs.GetInt($"player{playerID}Bombs", 5);
        bombsRemaining = bombAmount;
    }

    private void Update()
    {
        if (bombsRemaining > 0 && Input.GetKeyDown(inputKey))
        {
            StartCoroutine(PlaceBomb());
        }
    }

    private IEnumerator PlaceBomb()
    {
        Vector2 position = transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        Debug.Log("Jugador " + playerID + " colocando bomba en: " + position);
        GameObject bomb = Instantiate(bombPrefab, position, Quaternion.identity);
        bombsRemaining--;

        yield return new WaitForSeconds(bombFuseTime);

        position = bomb.transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        Debug.Log("Explosión en posición: " + position);
        Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        explosion.SetActivateRenderer(explosion.start);
        explosion.DestroyAfter(explosionDuration);

        Explode(position, Vector2.up, explosionRadius);
        Explode(position, Vector2.down, explosionRadius);
        Explode(position, Vector2.left, explosionRadius);
        Explode(position, Vector2.right, explosionRadius);

        Destroy(bomb);
        bombsRemaining++;
    }

    private void Explode(Vector2 position, Vector2 direction, int length)
    {
        if (length == 0)
        {
            return;
        }
        position += direction;

        if (Physics2D.OverlapBox(position, Vector2.one / 2f, 0f, obstacleLayer))
        {
            ClearDestructibleBlocks(position);
            return;
        }

        Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        explosion.SetActivateRenderer(length > 1 ? explosion.middle : explosion.end);
        explosion.SetDirection(direction);
        explosion.DestroyAfter(explosionDuration);

        Explode(position, direction, length - 1);
    }

    private void ClearDestructibleBlocks(Vector2 position)
    {
        Vector3Int cell = destructibleTiles.WorldToCell(position);
        TileBase tile = destructibleTiles.GetTile(cell);
        if (tile != null)
        {
            destructibleTiles.SetTile(cell, null);
            Instantiate(destructiblePrefab, position, Quaternion.identity);
        }
    }

    public void ExtraBomb()
    {
        bombAmount++;
        bombsRemaining++;
    }
     private void AsignarTeclas()
    {
        inputKey = playerID == 1 ? KeyCode.Space : KeyCode.Mouse0;
    }
}
