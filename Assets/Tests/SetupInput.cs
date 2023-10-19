using NUnit.Framework;
using UnityEngine.InputSystem;

public static class SetupInput
{
    private static int originalDeviceCount;

    private static Keyboard keyboard;
    private static Mouse mouse;

    public static (Keyboard, Mouse) SetupKeyboard()
    {
        originalDeviceCount = InputSystem.devices.Count;

        if (keyboard != null)
        {
            InputSystem.AddDevice(keyboard);
        }
        else
        {
            keyboard = InputSystem.AddDevice<Keyboard>();
        }
        Assert.That(InputSystem.GetDevice<Keyboard>(), !Is.Null);

        if (mouse != null)
        {
            InputSystem.AddDevice(mouse);
        }
        else
        {
            mouse = InputSystem.AddDevice<Mouse>();
        }
        Assert.That(InputSystem.GetDevice<Mouse>(), !Is.Null);

        Assert.That(InputSystem.devices.Count, Is.EqualTo(originalDeviceCount + 2), "could not add devices");

        return (keyboard, mouse);
    }

    public static void TeardownKeyboard()
    {
        InputSystem.RemoveDevice(mouse);
        InputSystem.RemoveDevice(keyboard);

        Assert.That(InputSystem.devices.Count, Is.EqualTo(originalDeviceCount), "not all devices disconnected");
    }
}

