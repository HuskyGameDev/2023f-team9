using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class RunnerTests : InputTestFixture
{
    GameObject runner = Resources.Load<GameObject>("Prefabs/Player");
    GameObject board = Resources.Load<GameObject>("Prefabs/Board");
    Keyboard keyboard;

    public override void Setup()
    {
        SceneManager.LoadScene("Scenes/AutomatedTesting");
        base.Setup();
        keyboard = InputSystem.AddDevice<Keyboard>();

        var mouse = InputSystem.AddDevice<Mouse>();
        Press(mouse.rightButton);
        Release(mouse.rightButton);
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
        GameObject runnerInstance = Object.Instantiate(runner, new Vector2(0, -5f), Quaternion.identity);

        yield return new WaitForSeconds(1f);

        Assert.That(runnerInstance.GetComponentInChildren<Rigidbody2D>().velocity.y, Is.EqualTo(0));
        Assert.That(runnerInstance.transform.GetChild(0).position.y, Is.GreaterThan(-10f));
    }

    [UnityTest]
    public IEnumerator RunnerMovesLeftWithKeyboard()
    {
        GameObject runnerInstance = Object.Instantiate(runner, Vector2.zero, Quaternion.identity);

        Press(keyboard.leftArrowKey);
        yield return new WaitForSeconds(0.5f);
        Release(keyboard.leftArrowKey);
        yield return new WaitForSeconds(1f);

        Assert.That(runnerInstance.transform.GetChild(0).position.x, Is.LessThan(-1.5f));
    }

    [UnityTest]
    public IEnumerator RunnerMovesRightWithKeyboard()
    {
        GameObject runnerInstance = Object.Instantiate(runner, Vector2.zero, Quaternion.identity);

        Press(keyboard.rightArrowKey);
        yield return new WaitForSeconds(0.5f);
        Release(keyboard.rightArrowKey);
        yield return new WaitForSeconds(1f);

        Assert.That(runnerInstance.transform.GetChild(0).position.x, Is.GreaterThan(1.5f));
    }

    [UnityTest]
    public IEnumerator RunnerJumpsWithKeyboard()
    {
        Object.Instantiate(board, Vector2.zero, Quaternion.identity);
        GameObject runnerInstance = Object.Instantiate(runner, new Vector2(0, -8f), Quaternion.identity);

        yield return new WaitForSeconds(0.5f);

        PressAndRelease(keyboard.upArrowKey);
        yield return new WaitForSeconds(0.25f);
        Assert.That(runnerInstance.GetComponentInChildren<Rigidbody2D>().velocity.y, Is.Not.EqualTo(0));
    }

    [UnityTest]
    public IEnumerator RunnerDoesNotDoubleJump()
    {
        Object.Instantiate(board, Vector2.zero, Quaternion.identity);
        GameObject runnerInstance = Object.Instantiate(runner, new Vector2(0, -8f), Quaternion.identity);

        yield return new WaitForSeconds(0.5f);

        PressAndRelease(keyboard.upArrowKey);
        yield return new WaitForSeconds(0.25f);
        float vel1 = runnerInstance.GetComponentInChildren<Rigidbody2D>().velocity.y;

        PressAndRelease(keyboard.upArrowKey);
        yield return new WaitForSeconds(0.25f);
        float vel2 = runnerInstance.GetComponentInChildren<Rigidbody2D>().velocity.y;

        Assert.That(vel2, Is.LessThanOrEqualTo(vel1));
    }

    [UnityTest]
    public IEnumerator RunnerChecksForGroundBeforeJump()
    {
        Object.Instantiate(board, Vector2.zero, Quaternion.identity);
        GameObject runnerInstance = Object.Instantiate(runner, new Vector2(0, -8f), Quaternion.identity);

        yield return new WaitForSeconds(0.5f);

        PressAndRelease(keyboard.upArrowKey);
        yield return new WaitForSeconds(1f);

        PressAndRelease(keyboard.upArrowKey);
        yield return new WaitForSeconds(0.25f);
        Assert.That(runnerInstance.GetComponentInChildren<Rigidbody2D>().velocity.y, Is.GreaterThan(0));
    }

}
