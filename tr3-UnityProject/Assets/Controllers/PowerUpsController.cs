using UnityEngine;

public class PowerUpsController : MonoBehaviour
{
    public enum ItemType
    {
        ExtraBomb,
        BombRadius,
        Speed    
    }
    public ItemType itemType;
    private void OnPowerUp(GameObject player ){
        switch (itemType){
            case ItemType.BombRadius:
                player.GetComponent<BombController>().ExtraBomb();

            break;
             case ItemType.Speed:
                player.GetComponent<BombermanController>().speed++;
            break;
             case ItemType.ExtraBomb:
                player.GetComponent<BombController>().explosionRadius++;

            break;

        }
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player")){
            OnPowerUp(other.gameObject);
        }
    }
}
