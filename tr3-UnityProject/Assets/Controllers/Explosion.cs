using UnityEngine;

public class Explosion : MonoBehaviour
{
    public AnimatorSpriteRenderer start;
    public AnimatorSpriteRenderer middle;
    public AnimatorSpriteRenderer end;
    public void SetActivateRenderer(AnimatorSpriteRenderer renderer)
    {
        start.enabled = renderer == start;
        middle.enabled = renderer == middle;
        end.enabled = renderer == end;
    }

    public void SetDirection(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) ;
        transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
    }
     public void DestroyAfter(float seconds)
    {
        Destroy(gameObject, seconds);
    }
}
