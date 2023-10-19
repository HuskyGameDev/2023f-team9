﻿using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DropperKeyboardMovement : InputTestFixture
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
    public IEnumerator BlockMovesLeftWithKeyboard()
    {
        GameObject boardInstance = Object.Instantiate(board, Vector2.zero, Quaternion.identity);
        Board boardScript = boardInstance.GetComponentInChildren<Board>();

        yield return new WaitForFixedUpdate();
        int originalPosition = boardScript.activePiece.position.x;

        Press(keyboard.aKey);
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        Release(keyboard.aKey);
        while (boardScript.activePiece.position.x == originalPosition) yield return new WaitForFixedUpdate();

        int finalPosition = boardScript.activePiece.position.x;

        Assert.That(finalPosition, Is.LessThan(originalPosition));
    }

    [UnityTest]
    public IEnumerator BlockMovesRightWithKeyboard()
    {
        GameObject boardInstance = Object.Instantiate(board, Vector2.zero, Quaternion.identity);
        Board boardScript = boardInstance.GetComponentInChildren<Board>();

        yield return new WaitForFixedUpdate();
        int originalPosition = boardScript.activePiece.position.x;

        Press(keyboard.dKey);
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        Release(keyboard.dKey);
        while (boardScript.activePiece.position.x == originalPosition) yield return new WaitForFixedUpdate();

        int finalPosition = boardScript.activePiece.position.x;

        Assert.That(finalPosition, Is.GreaterThan(originalPosition));
    }

    [UnityTest]
    public IEnumerator BlockMovesDownWithKeyboard()
    {
        GameObject boardInstance = Object.Instantiate(board, Vector2.zero, Quaternion.identity);
        Board boardScript = boardInstance.GetComponentInChildren<Board>();

        yield return new WaitForFixedUpdate();
        int originalPosition = boardScript.activePiece.position.y;

        for (int i = 0; i < 8; i++)
        {
            Press(keyboard.sKey);
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            Release(keyboard.sKey);
        }
        yield return new WaitForFixedUpdate();

        int finalPosition = boardScript.activePiece.position.y;

        Assert.That(finalPosition, Is.LessThan(originalPosition - 2));
    }

    [UnityTest]
    public IEnumerator BlockRotatesLeftWithKeyboard()
    {
        GameObject boardInstance = Object.Instantiate(board, Vector2.zero, Quaternion.identity);
        Board boardScript = boardInstance.GetComponentInChildren<Board>();

        yield return new WaitForFixedUpdate();
        Assert.That(boardScript.activePiece.rotationIndex, Is.EqualTo(0));

        Press(keyboard.qKey);
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        Release(keyboard.qKey);
        while (boardScript.activePiece.rotationIndex == 0) yield return new WaitForFixedUpdate();

        Assert.That(boardScript.activePiece.rotationIndex, Is.EqualTo(3));
    }

    [UnityTest]
    public IEnumerator BlockRotatesRightWithKeyboard()
    {
        GameObject boardInstance = Object.Instantiate(board, Vector2.zero, Quaternion.identity);
        Board boardScript = boardInstance.GetComponentInChildren<Board>();

        yield return new WaitForFixedUpdate();
        Assert.That(boardScript.activePiece.rotationIndex, Is.EqualTo(0));

        Press(keyboard.eKey);
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        Release(keyboard.eKey);
        while (boardScript.activePiece.rotationIndex == 0) yield return new WaitForFixedUpdate();

        Assert.That(boardScript.activePiece.rotationIndex, Is.EqualTo(1));
    }
}
