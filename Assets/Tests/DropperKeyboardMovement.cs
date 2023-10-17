using System.Collections;
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

        // setup devices
        foreach (InputDevice device in InputSystem.devices)
        {
            if (device != null) InputSystem.RemoveDevice(device);
        }
        Assert.That(InputSystem.devices.Count, Is.EqualTo(0));
        keyboard = InputSystem.AddDevice<Keyboard>();
        Assert.That(InputSystem.GetDevice<Keyboard>(), !Is.Null);
        mouse = InputSystem.AddDevice<Mouse>();
        Assert.That(InputSystem.GetDevice<Mouse>(), !Is.Null);
        Assert.That(InputSystem.devices.Count, Is.EqualTo(2));
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
    }

    [UnityTest]
    public IEnumerator BlockMovesLeftWithKeyboard()
    {
        GameObject boardInstance = Object.Instantiate(board, Vector2.zero, Quaternion.identity);
        Board boardScript = boardInstance.GetComponentInChildren<Board>();

        int originalPosition = boardScript.activePiece.position.x;

        PressAndRelease(keyboard.aKey);
        yield return new WaitForFixedUpdate();

        Assert.That(originalPosition, Is.GreaterThan(boardScript.activePiece.position.x));
    }

    [UnityTest]
    public IEnumerator BlockMovesRightWithKeyboard()
    {
        GameObject boardInstance = Object.Instantiate(board, Vector2.zero, Quaternion.identity);
        Board boardScript = boardInstance.GetComponentInChildren<Board>();

        int originalPosition = boardScript.activePiece.position.x;

        PressAndRelease(keyboard.dKey);
        yield return new WaitForFixedUpdate();

        Assert.That(originalPosition, Is.LessThan(boardScript.activePiece.position.x));
    }


}
