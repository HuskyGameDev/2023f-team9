using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class RunnerBasics : InputTestFixture
{
    GameObject runner;
    GameObject board;
    Keyboard keyboard;
    Mouse mouse;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        // get prefabs
        runner = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Player.prefab");
        Assert.That(runner, !Is.Null);
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
        while (runnerRigidbody.velocity.y > 0) yield return new WaitForFixedUpdate();

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
        GameObject runnerInstance = Object.Instantiate(runner, new Vector2(0, -8f), Quaternion.identity);
        Rigidbody2D runnerRigidbody = runnerInstance.GetComponentInChildren<Rigidbody2D>();

        // check jump doesn't work
        yield return new WaitForFixedUpdate();
        PressAndRelease(keyboard.upArrowKey);
        yield return new WaitForFixedUpdate();
        Assert.That(runnerRigidbody.velocity.y, Is.Negative);

        // wait to fall to ground
        while (runnerRigidbody.velocity.y != 0) yield return new WaitForFixedUpdate();

        // check jump does work
        PressAndRelease(keyboard.upArrowKey);
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();

        Assert.That(runnerRigidbody.velocity.y, Is.Positive);
    }

}
