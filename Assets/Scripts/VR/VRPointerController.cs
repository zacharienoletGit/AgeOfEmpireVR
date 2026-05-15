using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class VRPointerController : MonoBehaviour
{
    public Camera rayCamera;
    public Transform rightController;
    public float rayDistance = 10f;
    public LayerMask rayMask = ~0;

    bool lastTriggerState;
    readonly List<UnityEngine.XR.InputDevice> rightHandDevices = new List<UnityEngine.XR.InputDevice>();

    void Start()
    {
        if (rayCamera == null)
            rayCamera = Camera.main;
    }

    void Update()
    {
        GameManager manager = GameManager.Instance;
        if (manager == null)
            return;

        HandleKeyboard(manager);

        bool pointerPressed = WasMousePressed() || WasRightTriggerPressed();
        if (pointerPressed)
            HandlePointer(manager);
    }

    void HandleKeyboard(GameManager manager)
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null)
            return;

        if (keyboard.spaceKey.wasPressedThisFrame && manager.CurrentState != GameManager.GameState.Playing)
            manager.StartGame();

        if (keyboard.rKey.wasPressedThisFrame)
            manager.RestartGame();

        if (keyboard.bKey.wasPressedThisFrame)
            manager.ToggleBuildMode();
    }

    void HandlePointer(GameManager manager)
    {
        Ray ray = GetPointerRay();

        if (!Physics.Raycast(ray, out RaycastHit hit, rayDistance, rayMask))
            return;

        SelectableUnit selectable = hit.collider.GetComponentInParent<SelectableUnit>();
        UnitCombat combat = hit.collider.GetComponentInParent<UnitCombat>();
        MapGround ground = hit.collider.GetComponentInParent<MapGround>();

        if (selectable != null)
        {
            manager.SelectUnit(selectable);
            return;
        }

        if (combat != null && combat.team == UnitCombat.Team.Enemy)
        {
            manager.AttackWithSelectedUnit(combat);
            return;
        }

        if (ground != null)
        {
            if (manager.buildSystem != null && manager.buildSystem.IsBuildMode)
            {
                manager.buildSystem.TryPlaceTower(hit.point);
            }
            else
            {
                manager.MoveSelectedUnit(hit.point);
            }
        }
    }

    Ray GetPointerRay()
    {
        Mouse mouse = Mouse.current;
        if (mouse != null && rayCamera != null && mouse.leftButton.wasPressedThisFrame)
            return rayCamera.ScreenPointToRay(mouse.position.ReadValue());

        if (rightController != null)
            return new Ray(rightController.position, rightController.forward);

        if (rayCamera != null)
            return new Ray(rayCamera.transform.position, rayCamera.transform.forward);

        return new Ray(transform.position, transform.forward);
    }

    bool WasMousePressed()
    {
        Mouse mouse = Mouse.current;
        return mouse != null && mouse.leftButton.wasPressedThisFrame;
    }

    bool WasRightTriggerPressed()
    {
        rightHandDevices.Clear();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller, rightHandDevices);

        bool pressed = false;
        for (int i = 0; i < rightHandDevices.Count; i++)
        {
            if (rightHandDevices[i].TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out bool triggerPressed) && triggerPressed)
            {
                pressed = true;
                break;
            }
        }

        bool justPressed = pressed && !lastTriggerState;
        lastTriggerState = pressed;
        return justPressed;
    }
}
