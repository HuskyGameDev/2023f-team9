using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

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

public class MovementUtil : InputTestFixture
{
    private Keyboard keyboard;

    public MovementUtil(Keyboard keyboard)
    {
        this.keyboard = keyboard;
    }

    public IEnumerator MoveBlockToPosition(int targetX, int targetRot, Board boardScript)
    {
        boardScript.activePiece.stepDelay = 0.25f;
        yield return new WaitForSeconds(boardScript.activePiece.lockDelay);
        yield return new WaitForSeconds(boardScript.activePiece.stepDelay);
        while (boardScript.activePiece.rotationIndex != targetRot)
        {
            PressAndRelease(keyboard.eKey);
            yield return new WaitForFixedUpdate();
        }
        // line up with tower
        while (boardScript.activePiece.position.x != targetX)
        {
            if (boardScript.activePiece.position.x > targetX)
            {
                // move left
                Press(keyboard.aKey);
                yield return new WaitForFixedUpdate();
                yield return new WaitForFixedUpdate();
                Release(keyboard.aKey);
            }
            else
            {
                // move right
                Press(keyboard.dKey);
                yield return new WaitForFixedUpdate();
                yield return new WaitForFixedUpdate();
                Release(keyboard.dKey);
            }
        }
        boardScript.activePiece.stepDelay = 0.01f;
        yield return TestUtil.WaitForReachBottom(boardScript.activePiece);
    }
}

