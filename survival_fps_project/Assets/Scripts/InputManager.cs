﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] PlayerInput playerInput;
    [SerializeField] private PlayerMove player;
    [SerializeField] float mouseSensitivity = 10f;
    [SerializeField] float padSensitivity = 40;

    // InputActionsの登録と破棄
    PlayerInputActions inputs;
    private void Awake() => inputs = new PlayerInputActions();
    private void OnDisable() => inputs.Disable();

    // Inputの有効・無効化
    void OnDestroy() => inputs.Disable();
    void OnEnable() => inputs.Enable();

    // Update is called once per frame
    void Update()
    {
        player.moveVec2 = inputs.Player.Move.ReadValue<Vector2>();
        
        Debug.Log(playerInput.defaultControlScheme == "Keyboard&Mouse");
        
        player.lookVec2 = inputs.Player.Look.ReadValue<Vector2>();

        player.jump = inputs.Player.Jump.ReadValue<float>() > 0;
    }
}
