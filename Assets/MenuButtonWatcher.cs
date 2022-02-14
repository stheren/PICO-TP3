using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

[System.Serializable]
public class MenuButtonEvent : UnityEvent<bool> {}
public class MenuButtonWatcher : MonoBehaviour
{
    public MenuButtonEvent menuButtonPress;
    public Console console;
    
    private bool lastButtonState = false;
    private bool appState = true;

    private List<InputDevice> devicesWithMenuButton;

    private void InputDevice_deviceConnected(InputDevice device)
    {
        bool discardedValue;
        // Changer en CommonUsages.menuButton pour le pico
        if (device.TryGetFeatureValue(CommonUsages.primaryButton, out discardedValue))
        {
            devicesWithMenuButton.Add(device);
            console.AddLine($"[{this.name}] add device {device.name}");
        }
    }

    private void InputDevice_deviceDisconnected(InputDevice device)
    {
        if (devicesWithMenuButton.Contains(device))
        {
            devicesWithMenuButton.Remove(device);
            console.AddLine($"[{this.name}] remove device {device.name}");
        }
    }

    private void RegisterDevice()
    {
        List<InputDevice> allDevices = new List<InputDevice>();
        InputDevices.GetDevices(allDevices);
        foreach (InputDevice device in allDevices)
        {
            InputDevice_deviceConnected(device);
        }

        InputDevices.deviceConnected += InputDevice_deviceConnected;
        InputDevices.deviceDisconnected += InputDevice_deviceDisconnected;
    }

    private void OnDisable()
    {
        InputDevices.deviceConnected -= InputDevice_deviceConnected;
        InputDevices.deviceDisconnected -= InputDevice_deviceDisconnected;
        devicesWithMenuButton.Clear();
    }

    private void OnEnable()
    {
        RegisterDevice();
    }

    void Start()
    {
        devicesWithMenuButton = new List<InputDevice>();

        RegisterDevice();

    }

    // Update is called once per frame
    void Update()
    {
        bool tempState = false;
        foreach (var device in devicesWithMenuButton)
        {
            bool primaryButtonState = false;
            // Changer en CommonUsages.menuButton pour le pico
            tempState =
                device.TryGetFeatureValue(CommonUsages.primaryButton, out primaryButtonState) 
                && primaryButtonState ||
                tempState;
        }

        if (tempState != lastButtonState)
        {
            console.AddLine($"call menuButtonPress with {tempState}");
            if (tempState)
            {
                appState = !appState;
                menuButtonPress.Invoke(appState);
            }
            lastButtonState = tempState;
        }
    }
}
