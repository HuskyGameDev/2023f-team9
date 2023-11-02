using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NewPiece : MonoBehaviour
{

    bool hasCollided = false;
    TetrominoData data;
    private NewBoard newBoard;


    [SerializeField] int velocity = -5;
    Rigidbody2D myRigidBody;

    public void Start()
    {
        newBoard = GameObject.Find("Board").GetComponent<NewBoard>();

    }

    private void Awake()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasCollided)
        {
            myRigidBody.velocity = new Vector2(0, velocity);
        }
        else {
            
            newBoard.spawnTiles();
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
                }
            }
            
        }
    }
}
