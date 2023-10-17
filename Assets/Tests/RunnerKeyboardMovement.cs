using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class RunnerKeyboardMovement : InputTestFixture
{
    readonly ArrayList initialDevices = new();

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

        // setup devices
        foreach (InputDevice device in InputSystem.devices)
        {
            if (device != null)
            {
                initialDevices.Add(device);
                InputSystem.RemoveDevice(device);
            }
        }
        Assert.That(InputSystem.devices.Count, Is.EqualTo(0), "not all initial devices disconnected");
        keyboard = InputSystem.AddDevice<Keyboard>();
        Assert.That(InputSystem.GetDevice<Keyboard>(), !Is.Null);
        mouse = InputSystem.AddDevice<Mouse>();
        Assert.That(InputSystem.GetDevice<Mouse>(), !Is.Null);
        Assert.That(InputSystem.devices.Count, Is.EqualTo(2), "could not add devices");
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
        // remove devices
        InputSystem.RemoveDevice(mouse);
        InputSystem.RemoveDevice(keyboard);

        Assert.That(InputSystem.devices.Count, Is.EqualTo(0), () =>
        {
            string returnString = "not all devices disconnected (";
            foreach (InputDevice device in InputSystem.devices)
                returnString += device + ", ";
            returnString += ")";
            return returnString;
        });

        foreach (InputDevice device in initialDevices)
        {
            if (device != null)
            {
                InputSystem.AddDevice(device);
            }
        }
        initialDevices.Clear();
    }

    [UnityTest]
    public IEnumerator RunnerMovesLeftWithKeyboard()
    {
        GameObject runnerInstance = Object.Instantiate(runner, Vector2.zero, Quaternion.identity);

        Press(keyboard.leftArrowKey);
        yield return new WaitForSeconds(0.25f);
        Release(keyboard.leftArrowKey);
        yield return new WaitForFixedUpdate();

        Assert.That(runnerInstance.GetComponentInChildren<Rigidbody2D>().velocity.x, Is.Negative);
    }

    [UnityTest]
    public IEnumerator RunnerMovesRightWithKeyboard()
    {
        GameObject runnerInstance = Object.Instantiate(runner, Vector2.zero, Quaternion.identity);

        Press(keyboard.rightArrowKey);
        yield return new WaitForSeconds(0.25f);
        Release(keyboard.rightArrowKey);
        yield return new WaitForFixedUpdate();

        Assert.That(runnerInstance.GetComponentInChildren<Rigidbody2D>().velocity.x, Is.Positive);
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
}
