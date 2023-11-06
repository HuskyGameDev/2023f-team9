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

    private List<Vector2Int> right;
    private List<Vector2Int> left;
    private List<Vector2Int> bottom;
    private List<Vector2Int> top;

    private int leftTileY;
    private int rightTileY;
    private int bottomTileY;

    bool leftTileDetected;
    bool rightTileDetected;
    bool bottomTileDetected;


    Rigidbody2D myRigidBody;
    int xCount;
    int nextY;
    float offSetY;

    Tilemap tilemap;

    public void Start()
    {
        newBoard = GameObject.Find("Board").GetComponent<NewBoard>();
        velocityY = newBoard.velocityY;
        shiftSpeed = newBoard.shiftSpeed;
        tilemap = newBoard.tilemap;

        myRigidBody = GetComponent<Rigidbody2D>();

        rightTileY = 99;
        leftTileY = 99;
        bottomTileY = 99;


    }

    private void Awake()
    {
        right = new List<Vector2Int>();
        left = new List<Vector2Int>();
        bottom = new List<Vector2Int>();
        top = new List<Vector2Int>();

        

        getDimensions();
        nextY = (int)(9 - offSetY);
        Debug.Log("OFFSET: " + offSetY);
        xCount = 0;

        leftTileDetected = false;
        rightTileDetected = false;
        bottomTileDetected = false;
    }

    public void getDimensions()
    {
        Transform curChild;
        int scaleX;
        int scaleY;
        int posX;
        int posY;

        for (int i = 0; i < transform.childCount; i++)
        {
            curChild = transform.GetChild(i);
            if (curChild.position.x > 0)
            {
                posX = (int)curChild.position.x;
            }
            else
            {
                posX = (int)curChild.position.x - 1;
            }

            
            posY = (int)curChild.position.y;
            
            scaleX = (int) curChild.GetComponent<SpriteRenderer>().size.x;
            scaleY = (int) curChild.GetComponent<SpriteRenderer>().size.y;

            posX = posX - (scaleX / 2);
            posY = posY - (scaleY / 2);
            if (scaleY/2 > offSetY)
            {
                offSetY = (float) scaleY/2;
            }

            //Debug.Log(posX + " " + posY);


            //Debug.Log(curChild.name);
            for (int j = 0; j < scaleX; j++)
            {
                bottom.Add(new Vector2Int(j + posX, posY-1));

                top.Add(new Vector2Int(j + posX, posY + 1));
            }

            
            
            for (int j = 0; j < scaleY; j++)
            {
                left.Add(new Vector2Int(posX-1, j + posY));
                right.Add(new Vector2Int(posX + 1, j + posY));
            }

            
        }
    }

    private void updateX(int x)
    {
        for (int i = 0; i < 4; i++)
        {
            if (i < left.Count)
            {
                left[i] = new Vector2Int(x + left[i].x, left[i].y);
                //Debug.Log("LEFT: " + left[i]);
                
            }

            if (i < right.Count)
            {
                right[i] = new Vector2Int(x + right[i].x, right[i].y);
                //Debug.Log("RIGHT: " + right[i]);
            }

            if (i < bottom.Count)
            {
                bottom[i] = new Vector2Int(x + bottom[i].x, bottom[i].y);
                //Debug.Log("BOTTOM: " + bottom[i]);
            }

            if (i < top.Count)
            {
                top[i] = new Vector2Int(x + top[i].x, top[i].y);
            }

            
            if (i >= left.Count && i >= right.Count && i >= bottom.Count)
            {
                break;
            }

        }
    }

    private void updateY(int y)
    {
        for (int i = 0; i < 4; i++)
        {
            if (i < left.Count)
            {
                left[i] = new Vector2Int(left[i].x, left[i].y + y);
                //Debug.Log("LEFT: " + left[i]);

            }

            if (i < right.Count)
            {
                right[i] = new Vector2Int(right[i].x, right[i].y + y);
                //Debug.Log("RIGHT: " + right[i]);
            }

            if (i < bottom.Count)
            {
                bottom[i] = new Vector2Int(bottom[i].x, bottom[i].y + y);
                //Debug.Log("BOTTOM: " + bottom[i]);
            }

            if (i < top.Count)
            {
                top[i] = new Vector2Int(top[i].x, top[i].y + y);
            }


            if (i >= left.Count && i >= right.Count && i >= bottom.Count)
            {
                break;
            }

        }
    }

    private void checkTiles()
    {
        for (int i = 0; i < right.Count; i++)
        {
            //Debug.Log("No tile detected at position " + right[i] + " on the RIGHT (" + i + ")");
            if (tilemap.HasTile((Vector3Int) right[i]))
            {
                
                rightTileDetected = true;
                //Debug.Log("Position " + right[i] + " on the RIGHT has a tile (" + i + ")");
                rightTileY = (int) (right[i].y - 1.5);
                break;
            }
            rightTileDetected = false;
        }

        for (int i = 0; i < left.Count; i++)
        {
            //Debug.Log("No tile detected at position " + left[i] + " on the LEFT (" + i + ")");
            if (tilemap.HasTile((Vector3Int)left[i]))
            {
                //Debug.Log("Position " + left[i] + " on the LEFT has a tile (" + i + ")");
                leftTileDetected = true;
                leftTileY = (int) (left[i].y - 1.5);
                break;
            }
            leftTileDetected = false;
        }

        for (int i = 0; i < bottom.Count; i++)
        {
            //Debug.Log("No tile detected at position " + bottom[i] + " on the BOTTOM (" + i + ")");
            if (tilemap.HasTile((Vector3Int)bottom[i]))
            {
                //Debug.Log("Position " + bottom[i] + " on the BOTTOM has a tile (" + i + ")");
                bottomTileDetected = true;
                break;
            }
            bottomTileDetected = false;
        }
    }


    

    // Update is called once per frame
    void Update()
    {
        
        if ( (int) (transform.position.y - offSetY) == nextY)
        {
            //Debug.Log(transform.position.y + " - " + offSetY + " compared to " + nextY);
            updateY(-1);
            checkTiles();

            if (leftTileY < nextY)
            {
                leftTileDetected = true;
            }

            if (rightTileY < nextY)
            {
                rightTileDetected = true;
            }
            nextY--;
            
        }

            
        if (Mathf.Round(transform.position.x) == xCount)
        {
            if (velocityX != 0)
            {
                velocityX = 0;
                transform.position = new Vector3(xCount, transform.position.y, 0);

            }
        }
            
            
        myRigidBody.velocity = new Vector2(velocityX, velocityY);
        
        

            // Controls for left and right
            // Also note that xCount is for keeping track of where the piece is
            // This is a temporary solution to keeping at least the J_Block in bounds    
        if (Input.GetKeyDown(KeyCode.A) && !leftTileDetected)
        {
            // If can move left
            updateX(-1);
            checkTiles();
            Debug.Log(transform.position.y + offSetY);
            xCount--;
            transform.position = new Vector3(transform.position.x - 1, transform.position.y, 0);
            //Debug.Log("A PRESSED. POSX: " + transform.position.x + " xCount: " + xCount);    
        }
            
        else if (Input.GetKeyDown(KeyCode.D) && !rightTileDetected)    
        {
            updateX(1);
            checkTiles();
            xCount++;
            transform.position = new Vector3(transform.position.x + 1, transform.position.y, 0);
            //velocityX = shiftSpeed;
            //Debug.Log("D PRESSED. POSX: " + transform.position.x + " xCount: " + xCount);
            }
        
    }


    
}
