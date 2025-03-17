using System.Collections;
using UnityEngine;

public class BombController : MonoBehaviour
{
    public KeyCode inputKey = KeyCode.Space;
    public GameObject bombPrefab;
    public float bombFuseTime = 3f;
    public int bombAmount = 5;
    private int bombsRemaining;

    private void OnEnable()
    {
        bombsRemaining = bombAmount;
    }
    private void Update()
    {
        if (bombsRemaining > 0 && Input.GetKeyDown(inputKey))
        {
            StartCoroutine(PlaceBomb());
        }
    }
    private IEnumerator PlaceBomb(){
        Vector2 position = transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);
        
        GameObject bomb = Instantiate(bombPrefab, position, Quaternion.identity);
        bombsRemaining--;
        yield return new WaitForSeconds(bombFuseTime);
        Destroy(bomb);
        bombsRemaining++;
    }
}
