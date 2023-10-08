using UnityEngine;
using UnityEngine.Tilemaps;
public enum Tetromino
{
    I_Block,
    O_Block,
    T_Block,
    J_Block,
    L_Block,
    S_Block,
    Z_Block,
}

[System.Serializable]
public struct TetrominoData
{
    public Tetromino tetromino;
    public Tile tile;
    public Vector2Int[] cells {get; private set;}
    public Vector2Int[,] wallKicks { get; private set; }

    public void Initialize()
    {
        this.cells = Data.Cells[this.tetromino];
        this.wallKicks = Data.WallKicks[this.tetromino];
    }
}