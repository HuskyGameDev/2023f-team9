using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class RunnerTests : InputTestFixture
{
    GameObject runner;
    GameObject board;
    Keyboard keyboard;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        runner = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Player.prefab");
        Assert.That(runner, !Is.Null);
        board = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Board.prefab");
        Assert.That(board, !Is.Null);
    }

    [SetUp]
    public override void Setup()
    {
        SceneManager.LoadScene("Scenes/AutomatedTesting");
        base.Setup();
        keyboard = InputSystem.AddDevice<Keyboard>();

        var mouse = InputSystem.AddDevice<Mouse>();
        Press(mouse.rightButton);
        Release(mouse.rightButton);
    }

    [TearDown]
    public override void TearDown()
    {
        base.TearDown();
        keyboard = null;
    }

    [Test]
    public void RunnerInstantiation()
    {
        GameObject runnerInstance = Object.Instantiate(runner, Vector2.zero, Quaternion.identity);
        Assert.That(runnerInstance, !Is.Null);
    }

    [UnityTest]
    public IEnumerator RunnerDoesNotFallOutOfWorld()
    {
        Object.Instantiate(board, Vector2.zero, Quaternion.identity);
        GameObject runnerInstance = Object.Instantiate(runner, new Vector2(0, -9f), Quaternion.identity);

        yield return new WaitForSeconds(0.5f);

        Assert.That(runnerInstance.GetComponentInChildren<Rigidbody2D>().velocity.y, Is.EqualTo(0));
        Assert.That(runnerInstance.transform.GetChild(0).position.y, Is.GreaterThan(-10f));
    }

    [UnityTest]
    public IEnumerator RunnerMovesLeftWithKeyboard()
    {
        GameObject runnerInstance = Object.Instantiate(runner, Vector2.zero, Quaternion.identity);

        Press(keyboard.leftArrowKey);
        yield return new WaitForFixedUpdate();
        Release(keyboard.leftArrowKey);
        yield return new WaitForFixedUpdate();

        Assert.That(runnerInstance.transform.GetChild(0).position.x, Is.Negative);
    }

    [UnityTest]
    public IEnumerator RunnerMovesRightWithKeyboard()
    {
        GameObject runnerInstance = Object.Instantiate(runner, Vector2.zero, Quaternion.identity);

        Press(keyboard.rightArrowKey);
        yield return new WaitForFixedUpdate();
        Release(keyboard.rightArrowKey);
        yield return new WaitForFixedUpdate();

        Assert.That(runnerInstance.transform.GetChild(0).position.x, Is.Positive);
    }

    [UnityTest]
    public IEnumerator RunnerJumpsWithKeyboard()
    {
        Object.Instantiate(board, Vector2.zero, Quaternion.identity);
        GameObject runnerInstance = Object.Instantiate(runner, new Vector2(0, -9.5f), Quaternion.identity);

        yield return new WaitForSeconds(0.5f);

        PressAndRelease(keyboard.upArrowKey);
        yield return new WaitForFixedUpdate();
        Assert.That(runnerInstance.GetComponentInChildren<Rigidbody2D>().velocity.y, Is.Not.EqualTo(0));
    }

    [UnityTest]
    public IEnumerator RunnerDoesNotDoubleJump()
    {
        Object.Instantiate(board, Vector2.zero, Quaternion.identity);
        GameObject runnerInstance = Object.Instantiate(runner, new Vector2(0, -9.5f), Quaternion.identity);
        Rigidbody2D runnerRigidbody = runnerInstance.GetComponentInChildren<Rigidbody2D>();

        // wait to fall to ground
        while (runnerRigidbody.velocity.y != 0) yield return new WaitForFixedUpdate();

        // jump
        PressAndRelease(keyboard.upArrowKey);
        yield return new WaitForFixedUpdate();
        float vel1 = runnerRigidbody.velocity.y;

        // wait until fall starts
        while (runnerRigidbody.velocity.y >= 0) yield return new WaitForFixedUpdate();

        // try to jump again and check that player is still falling
        PressAndRelease(keyboard.upArrowKey);
        yield return new WaitForFixedUpdate();
        float vel2 = runnerRigidbody.velocity.y;

        Assert.That(vel2, Is.LessThanOrEqualTo(vel1));
    }

    [UnityTest]
    public IEnumerator RunnerChecksForGroundBeforeJump()
    {
        Object.Instantiate(board, Vector2.zero, Quaternion.identity);
        GameObject runnerInstance = Object.Instantiate(runner, new Vector2(0, -9.5f), Quaternion.identity);
        Rigidbody2D runnerRigidbody = runnerInstance.GetComponentInChildren<Rigidbody2D>();

        // wait to fall to ground
        while (runnerRigidbody.velocity.y != 0) yield return new WaitForFixedUpdate();

        // jump and wait for fall to ground
        PressAndRelease(keyboard.upArrowKey);
        while (runnerRigidbody.velocity.y != 0) yield return new WaitForFixedUpdate();

        // jump again and check that player could jump
        PressAndRelease(keyboard.upArrowKey);
        while (runnerRigidbody.velocity.y == 0) yield return new WaitForFixedUpdate();
        Assert.That(runnerRigidbody.velocity.y, Is.GreaterThan(0));
    }

}
