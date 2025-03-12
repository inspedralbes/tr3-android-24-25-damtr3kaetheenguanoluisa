using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BombermanController : MonoBehaviour
{

    public new Rigidbody2D rigidbody { get; private set;}
    private Vector2 direction = Vector2.down;
    public float speed = 5f;

    public KeyCode inputUp = KeyCode.W;
    public KeyCode inputDown = KeyCode.S;
    public KeyCode inputLeft = KeyCode.A;
    public KeyCode inputRight = KeyCode.D;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(Input.GetKey(inputUp)){
            SetDirection(Vector2.up);
  
        }else if (Input.GetKey(inputDown)){
            SetDirection(Vector2.down);

        }else if (Input.GetKey(inputLeft)){
            SetDirection(Vector2.left);

        }else if (Input.GetKey(inputRight)){
            SetDirection(Vector2.right);
        }else{
            SetDirection(Vector2.zero);
        }
    } 

    private void FixedUpdate()
    {
        Vector2 position = rigidbody.position;
        Vector2 translation = direction * speed * Time.fixedDeltaTime;

        rigidbody.MovePosition(position + translation);
    }

    private void SetDirection(Vector2 newDirection)
    {
        direction = newDirection;
    }



    // public float velocidad;
    // [Range(5f, 5f)]
    // Vector3 targetPosition;


    // // Update is called once per frame
    // void Update()
    // {
    //     if (Input.GetMouseButton(0)) {
    //         targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //         targetPosition.z = 0;
    //     } else {
    //         targetPosition = transform.position + new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
    //     }

    //     transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);

    // }

}
