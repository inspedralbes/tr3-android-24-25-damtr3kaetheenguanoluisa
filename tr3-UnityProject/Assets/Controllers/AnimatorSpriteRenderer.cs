using UnityEngine;
using UnityEngine.Animations;

public class AnimatorSpriteRenderer : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public Sprite idleSprites;
    public Sprite[] animationSprites;
    public float animationTime = 0.25f;
    private int animationFrame;
    public bool loop = true;
    public bool idle = true;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        {
            spriteRenderer.enabled = true;
        }   
    }
    private void OnDisable()
    {
        {
            spriteRenderer.enabled = false;
        }
    }
    private void Start()
    {
        InvokeRepeating(nameof(NextFrame),animationTime,animationTime);
    }
    private void NextFrame(){
        animationFrame++;
        if (loop && animationFrame >= animationSprites.Length){
            animationFrame = 0;
        }
        if (idle){
            spriteRenderer.sprite = idleSprites;
        }else if(animationFrame >= 0 && animationFrame <animationSprites.Length){
            spriteRenderer.sprite = animationSprites[animationFrame];
        }
    }
}
