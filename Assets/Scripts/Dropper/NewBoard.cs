using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NewBoard : MonoBehaviour
{
    public Tilemap tilemap { get; private set; }
    public TetrominoData[] tetrominoes;
    public Vector3Int spawnPosition;
    public Vector2Int boardSize = new Vector2Int(10, 20);

    public GameObject J_Block;
    private GameObject activePiece;
    public bool LostGame { get; private set; }

    public RectInt Bounds
    {
        get
        {
            Vector2Int position = new Vector2Int(-this.boardSize.x / 2, -this.boardSize.y / 2);
            return new RectInt(position, this.boardSize);
        }
    }

    private void Awake()
    {
        this.tilemap = GetComponentInChildren<Tilemap>();


        for (int i = 0; i < this.tetrominoes.Length; i++)
        {
            this.tetrominoes[i].Initialize();
        }
    }

    private void Start()
    {
        SpawnPiece();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //
    public void SpawnPiece()
    {
        activePiece = Instantiate(J_Block, new Vector3(-4, 9, 0), Quaternion.identity);
        
    }

    public void spawnTiles()
    {
        Transform curChild;
        for (int i = 0; i < activePiece.transform.childCount; i++)
        {
            curChild = activePiece.transform.GetChild(i).transform;
            Debug.Log(curChild.name + ": \n POSITION: " + curChild.position + "\nSIZE: " + curChild.GetComponent<SpriteRenderer>().size);
            int x;
            int y;
            if (curChild.position.x > 0)
            {
                x = (int)curChild.position.x;
            }
            else
            {
                x = (int)curChild.position.x -1;
            }

            if (Mathf.Round(curChild.position.y*10)/10 == -.5)
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

            int width = (int) curChild.GetComponent<SpriteRenderer>().size.x;
            int height = (int)curChild.GetComponent<SpriteRenderer>().size.y;

            Debug.Log("X: " + x + " Y: " + y);

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
                tilemap.SetTile(new Vector3Int(x, y+j, 0), tetrominoes[2].tile);
                tilemap.SetTile(new Vector3Int(x, y-j, 0), tetrominoes[2].tile);
            }
            
            
        }
        Destroy(activePiece);
        SpawnPiece();
    }

    private bool IsLineFull(int row)
    {
        RectInt bounds = this.Bounds;

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
        RectInt bounds = this.Bounds;

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
}
