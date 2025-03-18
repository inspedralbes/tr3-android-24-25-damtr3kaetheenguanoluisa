using UnityEngine;
using UnityEngine.Rendering;

public class DestructibleBlocksController : MonoBehaviour
{
    public float destructionTime = 1f;  
    [Range(0f, 1f)]
    public float itemSpawn = 0.2f;
    public GameObject [] spawnablePowerUps;
    private void Start(){
        Destroy(gameObject, destructionTime);
    }
    private void OnDestroy()
    {
        if (spawnablePowerUps.Length > 0 && Random.value < itemSpawn)
        {
            int randomIndex = Random.Range(0, spawnablePowerUps.Length);
            Instantiate(spawnablePowerUps[randomIndex], transform.position, Quaternion.identity);
        }
    }

}
