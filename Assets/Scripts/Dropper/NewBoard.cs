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

    public Tile blue;
    public Tile cyan;
    public Tile green;
    public Tile orange;
    public Tile purple;
    public Tile red;
    public Tile yellow;

    private Tile curTile;
    //private List<GameObject> queue;
    // The piece being controlled
    public GameObject activePiece { get; private set; }
    private GameObject chosenPiece;
    public Queue<GameObject> pieceQueue { get; private set; }

    private List<GameObject> chosenRotations;
    
    private bool rotateBuffer;

    private int index = 0;

    // The amount of space the piece is allowed to "clip" through in order to fit tinier places a little bit easier
    // and give the illusion of smoothness
    public float wiggleRoom = 0.3f;

    public bool CanSpawnPieces = true;

    // Power up object
    public GameObject powerUpPrefab;

    public GivePowerUp powerUpCreator;
    public BlockQueue queueUpdater;

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

        pieceQueue = new Queue<GameObject>();

        for (int i = 0; i < 3; i++)
        {
            pieceQueue.Enqueue(pieceChoice[Random.Range(0, pieceChoice.Count)]);
        }
        queueUpdater.updateQueue();
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
            //Choose random block
            chosenPiece = pieceQueue.Dequeue();
            pieceQueue.Enqueue(pieceChoice[Random.Range(0, pieceChoice.Count)]);
            queueUpdater.updateQueue();

            //chosenPiece = Z_Block;

            if (chosenPiece == J_Block)
            {
                chosenRotations = J_Rotate;
                curTile = blue;
            }
            else if (chosenPiece == L_Block)
            {
                chosenRotations = L_Rotate;
                curTile = orange;
            }
            else if (chosenPiece == I_Block)
            {
                chosenRotations = I_Rotate;
                curTile = cyan;
            }
            else if (chosenPiece == T_Block)
            {
                chosenRotations = T_Rotate;
                curTile = purple;
            }
            else if (chosenPiece == S_Block)
            {
                chosenRotations = S_Rotate;
                curTile = green;
            }
            else if (chosenPiece == Z_Block)
            {
                chosenRotations = Z_Rotate;
                curTile = red;
            }
            else if (chosenPiece == O_Block)
            {
                //O_Block does not rotate
                curTile = yellow;
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

            // Randomly spawn power-up around the activePiece
            //SpawnPowerUp(activePiece.transform.position);

        }


    }
    // to display power up
    private void SpawnPowerUp(Vector3 spawnPosition)
    {
        // Randomly determine the position around the activePiece
        Vector2 randomOffset = new Vector2(Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f));

        // Calculate the power-up spawn position
        Vector3 powerUpSpawnPosition = spawnPosition + new Vector3(randomOffset.x, randomOffset.y, 0);

        // Ensure the power-up doesn't intersect with the activePiece
        while (IsIntersecting(powerUpSpawnPosition, activePiece))
        {
            randomOffset = new Vector2(Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f));
            powerUpSpawnPosition = spawnPosition + new Vector3(randomOffset.x, randomOffset.y, 0);
        }

        // Instantiate the power-up prefab at the calculated position
        GameObject powerUpObject = Instantiate(powerUpPrefab, powerUpSpawnPosition, Quaternion.identity);
        // Set the tag for the power-up object
        powerUpObject.tag = "PowerUp";
        powerUpObject.transform.parent = activePiece.transform;
    }
    // check if intersect with active piece
    private bool IsIntersecting(Vector3 position, GameObject activePiece)
    {
        Collider2D powerUpCollider = powerUpPrefab.GetComponent<Collider2D>(); // assuming powerUpPrefab has a collider
        Collider2D[] activePieceColliders = activePiece.GetComponentsInChildren<Collider2D>();

        foreach (Collider2D activePieceCollider in activePieceColliders)
        {
            if (activePieceCollider.OverlapPoint(position))
            {
                return true; // There's an intersection, retry with a new position
            }
        }

        return false; // No intersection
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

        // Detach power-up object before destroying activePiece
        GameObject powerUpObject = null;

        for (int i = 0; i < activePiece.transform.childCount; i++)
        {
            curChild = activePiece.transform.GetChild(i);

            // Check if the current child is the power-up object
            if (curChild.CompareTag("PowerUp")) // Adjust the tag based on your implementation
            {
                powerUpObject = curChild.gameObject;
                // Detach power-up object
                powerUpObject.transform.parent = null;
            }

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
                    tilemap.SetTile(new Vector3Int((int)posX + j, (int)posY+1), curTile);
                }
                else
                {
                    tilemap.SetTile(new Vector3Int((int)posX + j, (int)posY), curTile);
                }
            }

            for (int j = 0; j < scaleY; j++)
            {
                if (posY > 0)
                {
                    tilemap.SetTile(new Vector3Int((int)posX, (int)posY + j + 1), curTile);
                }
                else
                {
                    tilemap.SetTile(new Vector3Int((int)posX, (int)posY + j), curTile);
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
            if (posX < 0)
            {
                posX--;
            }
            //Check scaleX
            for (int j = 0; j < size; j++)
            {
                Debug.Log(curChild.name + ": " + posX + " " + curChild.transform.position.x);
                if (tilemap.HasTile(new Vector3Int((int) posX + j, (int) curChild.position.y, 0)) || tilemap.HasTile(new Vector3Int((int)posX + j, (int)curChild.position.y+1, 0)) 
                    || tilemap.HasTile(new Vector3Int((int)posX + j, (int)curChild.position.y-1, 0)))
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
                if (tilemap.HasTile(new Vector3Int((int)posX, (int)curChild.position.y + j, 0)) || tilemap.HasTile(new Vector3Int((int)posX, (int)curChild.position.y + j+ 1, 0))
                    || tilemap.HasTile(new Vector3Int((int)posX, (int)curChild.position.y + j - 1, 0)))
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
        powerUpCreator.dropperPowerUpRandomizer();
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
        GameManager.Instance.GameOver(true);
    }
}
