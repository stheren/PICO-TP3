using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
[System.Serializable]
public class TriggerButtonEvent : UnityEvent<bool> {}
public class TriggerButtonWatcher : MonoBehaviour
{
    public TriggerButtonEvent triggerButtonPress;
    public Console console;
    
    private bool lastButtonState = false;
    private bool appState = true;

    private List<InputDevice> devicesWithTriggerButton;

    private void InputDevice_deviceConnected(InputDevice device)
    {
        bool discardedValue;
        if (device.TryGetFeatureValue(CommonUsages.triggerButton, out discardedValue))
        {
            devicesWithTriggerButton.Add(device);
            console.AddLine($"[{this.name}] add device {device.name}");
        }
    }

    private void InputDevice_deviceDisconnected(InputDevice device)
    {
        if (devicesWithTriggerButton.Contains(device))
        {
            devicesWithTriggerButton.Remove(device);
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
        devicesWithTriggerButton.Clear();
    }

    private void OnEnable()
    {
        RegisterDevice();
    }

    void Start()
    {
        devicesWithTriggerButton = new List<InputDevice>();

        RegisterDevice();

    }

    // Update is called once per frame
    void Update()
    {
        bool tempState = false;
        foreach (var device in devicesWithTriggerButton)
        {
            bool primaryButtonState = false;
            tempState =
                device.TryGetFeatureValue(CommonUsages.triggerButton, out primaryButtonState) 
                && primaryButtonState ||
                tempState;
        }

        if (tempState != lastButtonState)
        {
            console.AddLine($"call triggerButtonPress with {tempState}");
            if (tempState)
            {
                appState = !appState;
                triggerButtonPress.Invoke(appState);
            }
            lastButtonState = tempState;
        }
    }
}
