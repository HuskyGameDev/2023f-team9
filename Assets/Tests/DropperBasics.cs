using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DropperBasics : InputTestFixture
{
    GameObject board;
    Keyboard keyboard;
    Mouse mouse;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        // get prefabs
        board = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Board.prefab");
        Assert.That(board, !Is.Null);

        (keyboard, mouse) = SetupInput.SetupKeyboard();
    }

    [SetUp]
    public override void Setup()
    {
        SceneManager.LoadScene("Scenes/AutomatedTesting");

        Assert.That(InputSystem.GetDevice<Mouse>(), !Is.Null);
        PressAndRelease(mouse.rightButton);
    }

    [TearDown]
    public override void TearDown()
    {
        base.TearDown();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        SetupInput.TeardownKeyboard();
    }

    [UnityTest]
    public IEnumerator BlockStopsAtHorizontalBorder()
    {
        GameObject boardInstance = Object.Instantiate(board, Vector2.zero, Quaternion.identity);
        Board boardScript = boardInstance.GetComponentInChildren<Board>();

        yield return new WaitForFixedUpdate();
        for (int i = 0; i < boardScript.Bounds.width; i++)
        {
            Press(keyboard.aKey);
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            Release(keyboard.aKey);
        }
        yield return new WaitForFixedUpdate();

        Assert.That(boardScript.activePiece.position.x, Is.GreaterThanOrEqualTo(boardScript.Bounds.xMin));

        yield return new WaitForFixedUpdate();
        for (int i = 0; i < boardScript.Bounds.width; i++)
        {
            Press(keyboard.dKey);
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            Release(keyboard.dKey);
        }
        yield return new WaitForFixedUpdate();

        Assert.That(boardScript.activePiece.position.x, Is.LessThanOrEqualTo(boardScript.Bounds.xMax));
    }

    [UnityTest]
    public IEnumerator BlockStopsAtVerticalBorder()
    {
        GameObject boardInstance = Object.Instantiate(board, Vector2.zero, Quaternion.identity);
        Board boardScript = boardInstance.GetComponentInChildren<Board>();

        yield return new WaitForFixedUpdate();
        for (int i = 0; i < boardScript.Bounds.height; i++)
        {
            Press(keyboard.sKey);
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            Release(keyboard.sKey);
        }
        yield return new WaitForFixedUpdate();

        Assert.That(boardScript.activePiece.position.y, Is.GreaterThanOrEqualTo(boardScript.Bounds.yMin));
    }

    [UnityTest]
    public IEnumerator BlockDoesNotRotateOutOfFrame()
    {
        GameObject boardInstance = Object.Instantiate(board, Vector2.zero, Quaternion.identity);
        Board boardScript = boardInstance.GetComponentInChildren<Board>();
        TestUtil.OnlySpawnIBlocks(boardScript);

        // rotate left (rotation index = 3)
        yield return new WaitForFixedUpdate();
        Press(keyboard.qKey);
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        Release(keyboard.qKey);
        yield return new WaitForFixedUpdate();
        Assert.That(boardScript.activePiece.rotationIndex, Is.EqualTo(3));

        // move to far left
        while (boardScript.activePiece.position.x > boardScript.Bounds.xMin)
        {
            Press(keyboard.aKey);
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            Release(keyboard.aKey);
        }
        yield return new WaitForFixedUpdate();

        // rotate left (and stay in bounds)
        yield return new WaitForFixedUpdate();
        Press(keyboard.qKey);
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        Release(keyboard.qKey);
        yield return new WaitForFixedUpdate();
        Assert.That(boardScript.activePiece.position.x, Is.GreaterThanOrEqualTo(boardScript.Bounds.xMin));
        Assert.That(boardScript.activePiece.rotationIndex, Is.EqualTo(2));

        // rotate right (rotation index = 3)
        yield return new WaitForFixedUpdate();
        Press(keyboard.eKey);
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        Release(keyboard.eKey);
        yield return new WaitForFixedUpdate();
        Assert.That(boardScript.activePiece.rotationIndex, Is.EqualTo(3));

        // move to far right
        for (int i = 0; i < boardScript.Bounds.width; i++)
        {
            Press(keyboard.dKey);
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            Release(keyboard.dKey);
        }
        yield return new WaitForFixedUpdate();

        // rotate right (and stay in bounds)
        yield return new WaitForFixedUpdate();
        Press(keyboard.eKey);
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        Release(keyboard.eKey);
        yield return new WaitForFixedUpdate();
        Assert.That(boardScript.activePiece.position.x, Is.LessThanOrEqualTo(boardScript.Bounds.xMax));
        Assert.That(boardScript.activePiece.rotationIndex, Is.EqualTo(0));
    }

    [UnityTest]
    public IEnumerator DetectScreenFull()
    {
        GameObject boardInstance = Object.Instantiate(board, Vector2.zero, Quaternion.identity);
        Board boardScript = boardInstance.GetComponentInChildren<Board>();
        TestUtil.OnlySpawnIBlocks(boardScript);

        for (int i = 0; i < 4; i++)
        {
            boardScript.activePiece.stepDelay = 0.25f;
            yield return new WaitForSeconds(boardScript.activePiece.lockDelay);
            yield return new WaitForSeconds(boardScript.activePiece.stepDelay);
            PressAndRelease(keyboard.eKey);
            yield return new WaitForFixedUpdate();
            // line up with tower
            while (boardScript.activePiece.position.x != 0)
            {
                if (boardScript.activePiece.position.x > 0)
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

        while (!boardScript.LostGame) yield return new WaitForFixedUpdate();

        Assert.That(boardScript.LostGame, Is.True);
    }
}
