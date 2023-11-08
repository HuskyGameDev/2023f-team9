using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NewPiece : MonoBehaviour
{


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


    private bool leftTileDetected;
    private bool rightTileDetected;
    private bool bottomTileDetected;

    private bool leftShiftClear;
    private bool rightShiftClear;


    private Rigidbody2D myRigidBody;
    private float nextY;
    private float offSetY;
    private float bottomOfPiece;

    private Tilemap tilemap;

    private float wiggleRoom;
    public void Start()
    {
        newBoard = GameObject.Find("Board").GetComponent<NewBoard>();
        velocityY = newBoard.velocityY;
        shiftSpeed = newBoard.shiftSpeed;
        tilemap = newBoard.tilemap;
        wiggleRoom = newBoard.wiggleRoom;

        myRigidBody = GetComponent<Rigidbody2D>();

        rightTileY = 99;
        leftTileY = 99;
        bottomTileY = -99;


    }

    private void Awake()
    {
        right = new List<Vector2Int>();
        left = new List<Vector2Int>();
        bottom = new List<Vector2Int>();



        getDimensions();
        nextY = 12 - offSetY;

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

            scaleX = (int)curChild.GetComponent<SpriteRenderer>().size.x;
            scaleY = (int)curChild.GetComponent<SpriteRenderer>().size.y;

            posX = posX - (scaleX / 2);
            posY = posY - ((scaleY + 1) / 2);
            if (scaleY / 2 > offSetY)
            {
                offSetY = (float)scaleY / 2;
            }

            //Debug.Log(posX + " " + posY);


            //Debug.Log(curChild.name);
            for (int j = 0; j < scaleX; j++)
            {
                bottom.Add(new Vector2Int(j + posX, posY));
            }



            for (int j = 0; j < scaleY + 1; j++)
            {
                left.Add(new Vector2Int(posX - 1, j + posY));
                right.Add(new Vector2Int(posX + 1, j + posY));
            }


        }
    }

    private void updateX(int x)
    {
        for (int i = 0; i < 8; i++)
        {
            if (i < left.Count)
            {
                left[i] = new Vector2Int(left[i].x + x, left[i].y);
                //Debug.Log("LEFT: " + left[i]);

            }

            if (i < right.Count)
            {
                right[i] = new Vector2Int(right[i].x + x, right[i].y);
                //Debug.Log("RIGHT: " + right[i]);
            }

            if (i < bottom.Count)
            {
                bottom[i] = new Vector2Int(bottom[i].x + x, bottom[i].y);
                //Debug.Log("BOTTOM: " + bottom[i]);
            }


            if (i >= left.Count && i >= right.Count && i >= bottom.Count)
            {
                break;
            }

        }
    }

    private void updateY(int y)
    {
        for (int i = 0; i < 8; i++)
        {

            if (i < left.Count)
            {

                left[i] = new Vector2Int(left[i].x, left[i].y + y);


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


            if (i >= left.Count && i >= right.Count && i >= bottom.Count)
            {
                break;
            }

        }
    }

    private void checkRight()
    {
        for (int i = 0; i < right.Count; i++)
        {
            //Debug.Log("No tile detected at position " + right[i] + " on the RIGHT (" + i + ")");
            if (tilemap.HasTile((Vector3Int)right[i]))
            {
                //Debug.Log("DETECTED: Position " + right[i] + " on the RIGHT has a tile (" + i + ")");
                rightTileDetected = true;
                rightTileY = right[i].y;

                break;
            }
            rightTileDetected = false;
        }
    }

    private void checkLeft()
    {
        //Debug.Log("------------------");
        for (int i = 0; i < left.Count; i++)
        {
            //Debug.Log("Checking position " + left[i] + " on the LEFT//// Parent PosY: " + transform.position.y +  " (" + i + ")");
            if (tilemap.HasTile((Vector3Int)left[i]))
            {
                //Debug.Log("DETECTED: Position " + left[i] + " on the LEFT has a tile (" + i + ")");
                leftTileY = left[i].y;
                leftTileDetected = true;

                break;

            }
            leftTileDetected = false;

        }
    }
    private void checkBottom()
    {
        //Debug.Log("------------------");
        for (int i = 0; i < bottom.Count; i++)
        {
            //Debug.Log("No tile detected at position " + bottom[i] + " on the BOTTOM//// Parent PosY: " + transform.position.y + " (" + i + ")");
            if (tilemap.HasTile((Vector3Int)bottom[i]))
            {
                //Debug.Log("DETECTED Position " + bottom[i] + " on the BOTTOM//// Parent PosY: " + transform.position.y + " (" + i + ")");
                bottomTileDetected = true;
                bottomTileY = bottom[i].y + 1;

                break;
            }
            bottomTileDetected = false;
        }
    }

    private void checkAll()
    {
        checkLeft();
        checkRight();
        checkBottom();
    }


    // Update is called once per frame
    void Update()
    {
        bottomOfPiece = transform.position.y - offSetY;

        if (bottomOfPiece <= nextY - 0.5f)
        {
            //Debug.Log(transform.position.y + " - " + offSetY + " = " + (transform.position.y -  offSetY) + " compared to " + (nextY-0.5f));
            updateY(-1);
            checkAll();
            if (bottomTileDetected)
            {
                if (bottomTileY >= transform.position.y - offSetY)
                {
                    newBoard.spawnTiles();
                }
            }
            nextY--;

        }





        myRigidBody.velocity = new Vector2(velocityX, velocityY);

        if (leftShiftClear)
        {
            if (bottomOfPiece <= nextY - 0.5f + wiggleRoom)
            {

                leftTileDetected = false;
            }

        }

        if (rightShiftClear)
        {
            if (bottomOfPiece <= nextY - 0.5f + wiggleRoom)
            {
                rightTileDetected = false;
            }

        }

        // Controls for left and right
        // Also note that xCount is for keeping track of where the piece is
        // This is a temporary solution to keeping at least the J_Block in bounds    
        if (Input.GetKeyDown(KeyCode.A) && !leftTileDetected)
        {
            // If can move left
            updateX(-1);
            checkAll();

            transform.position = new Vector3(transform.position.x - 1, transform.position.y, 0);
            //Debug.Log("A PRESSED. POSX: " + transform.position.x + " xCount: " + xCount);    
        }

        else if (Input.GetKeyDown(KeyCode.D) && !rightTileDetected)
        {
            updateX(1);
            checkAll();
            transform.position = new Vector3(transform.position.x + 1, transform.position.y, 0);
            //velocityX = shiftSpeed;
            //Debug.Log("D PRESSED. POSX: " + transform.position.x + " xCount: " + xCount);
        }

    }



}
