using System.Collections;
using System.Collections.Generic;
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
}
