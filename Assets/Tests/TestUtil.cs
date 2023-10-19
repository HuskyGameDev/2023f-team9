using UnityEngine;
using System.Collections;

public static class TestUtil
{
    // Board Settings
    public static void OnlySpawnIBlocks(Board boardScript)
    {
        TetrominoData iBlock = boardScript.tetrominoes[0];
        boardScript.tetrominoes = new TetrominoData[1];
        boardScript.tetrominoes[0] = iBlock;
    }
    public static void MakeFast(Board boardScript)
    {
        boardScript.activePiece.stepDelay = 0.001f;
        boardScript.activePiece.lockDelay = 0.01f;
    }

    // Waits
    public static IEnumerator WaitForReachBottom(Piece piece)
    {
        int prevPos = piece.position.y + 100;

        while (piece.position.y < prevPos)
        {
            prevPos = piece.position.y;
            yield return new WaitForSeconds(piece.stepDelay);
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(piece.lockDelay);
        yield return new WaitForFixedUpdate();
    }
}

