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
    public GameObject activePiece { get; private set; }
    private GameObject chosenPiece;
    private List<GameObject> chosenRotations;
    
    private bool rotateBuffer;

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

        rotateBuffer = false;
        chosenPiece = J_Block;
        chosenRotations = J_Rotate;

        for (int i = 0; i < this.tetrominoes.Length; i++)
        {
            this.tetrominoes[i].Initialize();
        }
    }

    private void Start()
    {
        SpawnPiece();
    }

    public void SpawnPiece()
    {
        if (tilemap.HasTile(new Vector3Int(SpawnX, 9, 0)))
        {
            CanSpawnPieces = false;
            GameOver();
        }
        else
        {
            CanSpawnPieces = true;
        }
        
        if (CanSpawnPieces)
        {
            chosenPiece = pieceChoice[Random.Range(0, pieceChoice.Count)];
            //chosenPiece = I_Block;

            if (chosenPiece == J_Block)
            {
                chosenRotations = J_Rotate;
            }
            else if (chosenPiece == L_Block)
            {
                chosenRotations = L_Rotate;
            }
            else if (chosenPiece == I_Block)
            {
                chosenRotations = I_Rotate;
            }
            else if (chosenPiece == T_Block)
            {
                chosenRotations = T_Rotate;
            }
            else if (chosenPiece == S_Block)
            {
                chosenRotations = S_Rotate;
            }
            else if (chosenPiece == Z_Block)
            {
                chosenRotations = Z_Rotate;
            }
            else if (chosenPiece == O_Block)
            {
                //O_Block does not rotate
            }

            activePiece = chosenPiece;
            if (chosenPiece == S_Block || chosenPiece == Z_Block)
            {
                
                //activePiece.transform.position = new Vector3(SpawnX + 1, 12, 0);
                activePiece = Instantiate(chosenPiece, new Vector3(SpawnX + 1, 12, 0), Quaternion.identity);
            }
            else
            {
                //activePiece.transform.position = new Vector3(SpawnX + 0.5f, 12, 0);
                activePiece = Instantiate(chosenPiece, new Vector3(SpawnX + 0.5f, 12, 0), Quaternion.identity);

            }



        }


    }



    // This is called my the piece when the piece is ready to be "set down" or has collided with a tile, meaning its ready to be replaced with tiles
    // This is where we might tell the UI that a new piece is going to be queued up
    public void spawnTiles()
    {
        

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
        RectInt bounds = Bounds;
        int row = bounds.yMin;

        while (row < bounds.yMax)
        {
            if (IsLineFull(row))
            {
                LineClear(row);
            }
            else
            {
                row++;
            }
        }

        index = 0;
        SpawnPiece();
    }

    public void rotatePiece(int direction)
    {
        if (activePiece.transform.name == "O-Block(Clone)")
        {
            return;
        }
        if (activePiece.GetComponent<NewPiece>().bottomTileDetected)
        {
            return;
        }
        while (rotateBuffer)
        {

        }
        int previousIndex = index;
        index -= direction;
        if (index == -1)
        {
            index = chosenRotations.Count - 1;
        }
        else if (index == chosenRotations.Count) {
            index = 0;
        }
        //Vector3 positions = activePiece.transform.position;
        Vector3 positions = new Vector3(activePiece.transform.position.x, (int) activePiece.transform.position.y, 0);

        GameObject oldPiece = activePiece;

        chosenPiece = chosenRotations[index];
        if (chosenRotations == S_Rotate || chosenRotations == Z_Rotate)
        {
            if (chosenPiece == S_Block || chosenPiece == Z_Block)
            {
                //activePiece.transform.position = new Vector3(positions.x - 0.5f, positions.y, 0);
                activePiece = Instantiate(chosenPiece, new Vector3(positions.x - 0.5f, positions.y, 0), Quaternion.identity);

            }
            else if (chosenPiece == S_BlockLR || chosenPiece == Z_BlockLR)
            {
                //activePiece.transform.position = new Vector3(positions.x + 0.5f, positions.y, 0);
                activePiece = Instantiate(chosenPiece, new Vector3(positions.x + 0.5f, positions.y, 0), Quaternion.identity);
            }
        }
        else
        {
            activePiece = Instantiate(chosenPiece, positions, Quaternion.identity);
            
        }
        NewPiece activeScript = activePiece.GetComponent<NewPiece>();
        //activeScript.getDimensions();

        Transform curChild;

        for (int i = 0; i < activePiece.transform.childCount; i++)
        {
            curChild = activePiece.transform.GetChild(i);


            int size = (int)curChild.GetComponent<SpriteRenderer>().size.x;
            
            int posX = (int) curChild.position.x - size / 2;
            //Check scaleX
            for (int j = 0; j < size; j++)
            {
                if (tilemap.HasTile(new Vector3Int((int) posX + j, (int) curChild.position.y, 0)))
                {
                    index = previousIndex;
                    Destroy(activePiece);
                    activePiece = oldPiece;
                    return;
                }
            }
            

            size = (int)curChild.GetComponent<SpriteRenderer>().size.y;
            
            posX = (int)curChild.position.y - size / 2;
            //Check scaleY
            for (int j = 0; j < size; j++)
            {
                if (tilemap.HasTile(new Vector3Int((int)posX, (int)curChild.position.y + j, 0)))
                {
                    index = previousIndex;
                    Destroy(activePiece);
                    activePiece = oldPiece;
                    return;
                }
            }
            
        }

        Destroy(oldPiece);
        //rotateBuffer = true;

    }

    private void Update()
    {
        if (rotateBuffer)
        {
            //WaitForSeconds(.1);
        }
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
        /*
        for (int i = 0; i < 20; i++)
        {
            LineClear(-9);
        }
        SpawnPiece();
        */
    }
}
