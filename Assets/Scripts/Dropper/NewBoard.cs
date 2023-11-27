using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NewBoard : MonoBehaviour
{
    public Tilemap tilemap { get; private set; }
    // Probably won't need this variable too much longer but rn, its being used to get the Tiles
    public TetrominoData[] tetrominoes;
    // The x coordinate in which the piece will be spawned at
    public int SpawnX = 0;
    // The board size for easy reference
    private static Vector2Int boardSize = new Vector2Int(10, 20);

    // How fast the piece will shift left and right
    public int shiftSpeed = 5;
    // The downward velocity of the next piece that is spawned
    public int velocityY = -5;
    // The prefab of the J_Block
    public GameObject J_Block;
    public GameObject J_BlockLR;
    public GameObject J_BlockRR;
    public GameObject J_BlockUD;
    private List<GameObject> J_Rotate;


    public GameObject L_Block;
    public GameObject L_BlockLR;
    public GameObject L_BlockRR;
    public GameObject L_BlockUD;
    private List<GameObject> L_Rotate;

    public GameObject I_Block;
    public GameObject I_BlockLR;
    private List<GameObject> I_Rotate;

    public GameObject O_Block;

    public GameObject S_Block;
    public GameObject S_BlockLR;
    private List<GameObject> S_Rotate;

    public GameObject Z_Block;
    public GameObject Z_BlockLR;
    private List<GameObject> Z_Rotate;

    public GameObject T_Block;
    public GameObject T_BlockLR;
    public GameObject T_BlockRR;
    public GameObject T_BlockUD;
    private List<GameObject> T_Rotate;


    private List<GameObject> pieceChoice;
    private List<GameObject> queue;
    // The piece being controlled
    private GameObject activePiece;
    private GameObject chosenPiece;
    private List<GameObject> chosenRotations;

    private int index = 0;

    // The amount of space the piece is allowed to "clip" through in order to fit tinier places a little bit easier
    // and give the illusion of smoothness
    public float wiggleRoom = 0.3f;

    public bool CanSpawnPieces = true;



    public static RectInt Bounds
    {
        get
        {
            Vector2Int position = new Vector2Int(-boardSize.x / 2, -boardSize.y / 2);
            return new RectInt(position, boardSize);
        }
    }

    private void Awake()
    {
        this.tilemap = GetComponentInChildren<Tilemap>();
        pieceChoice = new List<GameObject>() { J_Block, L_Block, T_Block, Z_Block, S_Block, O_Block, I_Block };
        J_Rotate = new List<GameObject>() { J_Block, J_BlockLR, J_BlockUD, J_BlockRR };
        L_Rotate = new List<GameObject>() { L_Block, L_BlockLR, L_BlockUD, L_BlockRR };
        I_Rotate = new List<GameObject>() { I_Block, I_BlockLR};
        S_Rotate = new List<GameObject>() { S_Block, S_BlockLR};
        Z_Rotate = new List<GameObject>() { Z_Block, Z_BlockLR};
        T_Rotate = new List<GameObject>() { T_Block, T_BlockLR, T_BlockUD, T_BlockRR };

        chosenPiece = S_Block;
        chosenRotations = S_Rotate;

        for (int i = 0; i < this.tetrominoes.Length; i++)
        {
            this.tetrominoes[i].Initialize();
        }
    }

    private void Start()
    {

        SpawnPiece();
    }


    // As of right now, only spawns a J block but does keep track of lost condition for dropper
    public void SpawnPiece()
    {
        if (!CanSpawnPieces) return;
        if (tilemap.HasTile(new Vector3Int(SpawnX, 9, 0)))
        {
            GameOver();
        }
        else
        {

            if (chosenPiece == S_Block || chosenPiece == Z_Block)
            {
                activePiece = Instantiate(chosenPiece, new Vector3(SpawnX + 1, 12, 0), Quaternion.identity);
            }
            else
            {
                activePiece = Instantiate(chosenPiece, new Vector3(SpawnX + 0.5f, 12, 0), Quaternion.identity);
            }

            
        }


    }



    // This is called my the piece when the piece is ready to be "set down" or has collided with a tile, meaning its ready to be replaced with tiles
    // This is where we might tell the UI that a new piece is going to be queued up
    public void spawnTiles()
    {
        /*Transform curChild;
        for (int i = 0; i < activePiece.transform.childCount; i++)
        {
            curChild = activePiece.transform.GetChild(i).transform;
            //Debug.Log(curChild.name + ": \n POSITION: " + curChild.position + "\nSIZE: " + curChild.GetComponent<SpriteRenderer>().size);
            int x;
            int y;
            if (curChild.position.x > 0)
            {
                x = (int)curChild.position.x;
            }
            else
            {
                x = (int)curChild.position.x - 1;
            }

            if (Mathf.Round(curChild.position.y * 10) / 10 == -.5)
            {
                y = -1;
            }
            else if (curChild.position.y < 0)
            {
                y = (int)curChild.position.y - 1;
            }

            else
            {
                y = (int)curChild.position.y;
            }

            int width = (int)curChild.GetComponent<SpriteRenderer>().size.x;
            int height = (int)curChild.GetComponent<SpriteRenderer>().size.y;

            //Debug.Log("X: " + x + " Y: " + y);

            if (width % 2 != 0)
            {
                tilemap.SetTile(new Vector3Int(x, y, 0), tetrominoes[2].tile);
            }
            int amount = width / 2;

            for (int j = 1; j <= amount; j++)
            {
                tilemap.SetTile(new Vector3Int(x + j, y, 0), tetrominoes[2].tile);
                tilemap.SetTile(new Vector3Int(x - j, y, 0), tetrominoes[2].tile);
            }

            amount = height / 2;
            for (int j = 1; j <= amount; j++)
            {
                tilemap.SetTile(new Vector3Int(x, y + j, 0), tetrominoes[2].tile);
                tilemap.SetTile(new Vector3Int(x, y - j, 0), tetrominoes[2].tile);
            }


        }
        */

        Transform curChild;
        float scaleX;
        float scaleY;
        float posX;
        float posY;

        for (int i = 0; i < activePiece.transform.childCount; i++)
        {
            curChild = activePiece.transform.GetChild(i);


            posX = curChild.position.x;
            posY = curChild.position.y;
            scaleX = (int)curChild.GetComponent<SpriteRenderer>().size.x;
            scaleY = (int)curChild.GetComponent<SpriteRenderer>().size.y;

            posX -= scaleX / 2;
            posY -= scaleY / 2;
            Destroy(activePiece);

            for (int j = 0; j < scaleX; j++)
            {
                if (posY > 0)
                {
                    tilemap.SetTile(new Vector3Int((int)posX + j, (int)posY+1), tetrominoes[2].tile);
                }
                else
                {
                    tilemap.SetTile(new Vector3Int((int)posX + j, (int)posY), tetrominoes[2].tile);
                }
            }

            for (int j = 0; j < scaleY; j++)
            {
                if (posY > 0)
                {
                    tilemap.SetTile(new Vector3Int((int)posX, (int)posY + j + 1), tetrominoes[2].tile);
                }
                else
                {
                    tilemap.SetTile(new Vector3Int((int)posX, (int)posY + j), tetrominoes[2].tile);
                }
            }
        }
        GameManager.Instance.BlockPlaced();

        SpawnPiece();
    }

    public void rotatePiece(int direction)
    {
        index -= direction;
        if (index == -1)
        {
            index = chosenRotations.Count - 1;
        }
        else if (index == chosenRotations.Count) {
            index = 0;
        }
        Vector3 positions = activePiece.transform.position;
        Destroy(activePiece);
        chosenPiece = chosenRotations[index];
        if (chosenRotations == S_Rotate || chosenRotations == Z_Rotate)
        {
            Debug.Log("RAR");
            if (chosenPiece == S_Block || chosenPiece == Z_Block)
            {
                activePiece = Instantiate(chosenPiece, new Vector3(positions.x - 0.5f, positions.y, 0), Quaternion.identity);
            }
            else if (chosenPiece == S_BlockLR || chosenPiece == Z_BlockLR)
            {
                activePiece = Instantiate(chosenPiece, new Vector3(positions.x + 0.5f, positions.y, 0), Quaternion.identity);
            }
        }
        else
        {
            activePiece = Instantiate(chosenPiece, new Vector3(positions.x, positions.y, 0), Quaternion.identity);
        }

        activePiece.GetComponent<NewPiece>().setNextY((int)positions.y);
    }

    private bool IsLineFull(int row)
    {
        RectInt bounds = Bounds;

        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);

            if (!this.tilemap.HasTile(position))
            {
                return false;
            }
        }
        return true;
    }

    private void LineClear(int row)
    {
        RectInt bounds = Bounds;

        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);
            this.tilemap.SetTile(position, null);
        }

        while (row < bounds.yMax)
        {
            for (int col = bounds.xMin; col < bounds.xMax; col++)
            {
                Vector3Int position = new Vector3Int(col, row + 1, 0);
                TileBase above = this.tilemap.GetTile(position);

                position = new Vector3Int(col, row, 0);
                this.tilemap.SetTile(position, above);
            }
            row++;
        }
    }

    private void GameOver()
    {
        for (int i = 0; i < 20; i++)
        {
            LineClear(-9);
        }
        SpawnPiece();
    }
}
