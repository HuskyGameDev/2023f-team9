using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NewPiece : MonoBehaviour
{

    bool hasCollided = false;
    TetrominoData data;
    private NewBoard newBoard;

    private int velocityX = 0;
    private int velocityY;
    private int shiftSpeed;
    Rigidbody2D myRigidBody;

    private int xCount;

    Tilemap tilemap;

    public void Start()
    {
        newBoard = GameObject.Find("Board").GetComponent<NewBoard>();
        velocityY = newBoard.velocityY;
        shiftSpeed = newBoard.shiftSpeed;
        tilemap = newBoard.tilemap;

    }

    private void Awake()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        xCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasCollided)
        {
            if (Mathf.Round(transform.position.x) == xCount)
            {
                velocityX = 0;
                transform.position = new Vector3(xCount, transform.position.y, 0);
            }
            myRigidBody.velocity = new Vector2(velocityX, velocityY);
        }
        else {

            // Prevents piece from accidently being placed on a corner
            if (velocityX != 0)
            {
                transform.position = new Vector3(xCount, transform.position.y, 0);
            }
            
            newBoard.spawnTiles();
        }


        // Controls for left and right
        // Also note that xCount is for keeping track of where the piece is
        // This is a temporary solution to keeping at least the J_Block in bounds
        if (Input.GetKeyDown(KeyCode.A) && xCount - 1 != -5)
        {
            xCount--;
            velocityX = -shiftSpeed;
            Debug.Log("A PRESSED. POSX: " + transform.position.x + " xCount: " + xCount);
        }
        else if (Input.GetKeyDown(KeyCode.D) && xCount + 1 != 5)
        {
            xCount++;
            velocityX = shiftSpeed;
            Debug.Log("D PRESSED. POSX: " + transform.position.x + " xCount: " + xCount);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Tilemap"){
            foreach(ContactPoint2D hitPos in collision.contacts)
            {
                if (hitPos.normal.y > 0)
                {
                    hasCollided = true;
                    break;
                }

                if (hitPos.normal.x > 0)
                {
                    xCount++;
                    velocityX = 0;
                    transform.position = new Vector3(Mathf.Round(transform.position.x), transform.position.y, 0);
                    break;
                } else if (hitPos.normal.x < 0)
                {
                    velocityX = 0;
                    xCount--;
                    transform.position = new Vector3(Mathf.Round(transform.position.x), transform.position.y, 0);
                    break;
                }
            }
            
        }
    }
}
