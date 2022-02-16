using MijanTools.Common;
using SpaceInvaders.Util;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SpaceInvaders.Common.Services
{
    // Structs
    [Serializable]
    public enum InputAction
    {
        None,
        MoveLeft,
        MoveRight,
        Shoot
    }

    public struct PlayerInput
    {
        public float MoveDirectionX { get; }
        public bool ShouldShoot { get; }

        public PlayerInput(float moveDirectionX, bool shouldShoot)
        {
            MoveDirectionX = moveDirectionX;
            ShouldShoot = shouldShoot;
        }
    }

    public class InputService
    {
        // Fields
        private readonly KeyCode _moveLeftKeyPrimary = KeyCode.A;
        private readonly KeyCode _moveRightKeyPrimary = KeyCode.D;
        private readonly int _shootMouseButtonPrimary = 0; // Left mouse button.

        private readonly KeyCode _moveLeftKeySecondary = KeyCode.LeftArrow;
        private readonly KeyCode _moveRightKeySecondary = KeyCode.RightArrow;
        private readonly KeyCode _shootKeySecondary = KeyCode.Space;

        private Dictionary<InputAction, bool> _actions;

        public InputService()
        {
            _actions = new Dictionary<InputAction, bool>
            {
                [InputAction.None] = false,
                [InputAction.MoveLeft] = false,
                [InputAction.MoveRight] = false,
                [InputAction.Shoot] = false,
            };
        }

        // Methods
        public PlayerInput GetPlayerInput()
        {
            if (Utils.IsCurrentPlatformEditor || Utils.IsCurrentPlatformStandalone)
            {
                UpdateActionsForStandalone();
            }
            else if (Utils.IsCurrentPlatformMobile)
            {
                // Do nothing, actions are updated from on-screen buttons.
            }

            var moveLeftKey = GetInputAction(InputAction.MoveLeft);
            var moveRightKey = GetInputAction(InputAction.MoveRight);
            var shootKey = GetInputAction(InputAction.Shoot);

            var moveDirectionX = 0f;
            if (moveLeftKey) { moveDirectionX -= 1f; }
            if (moveRightKey) { moveDirectionX += 1f; }
            var shouldShoot = shootKey;

            var playerInput = new PlayerInput(moveDirectionX, shouldShoot);
            return playerInput;
        }

        public void SetInputAction(InputAction action, bool active)
        {
            if (_actions.HasKey(action))
            {
                _actions[action] = active;
            }
        }

        public bool GetInputAction(InputAction type)
        {
            return _actions.HasKey(type) ? _actions[type] : false;
        }

        private void UpdateActionsForStandalone()
        {
            var moveLeftKey = Input.GetKey(_moveLeftKeyPrimary) || Input.GetKey(_moveLeftKeySecondary);
            var moveRightKey = Input.GetKey(_moveRightKeyPrimary) || Input.GetKey(_moveRightKeySecondary);
            var isPointerOverUI = EventSystem.current.IsPointerOverGameObject();
            var shootKey = Input.GetKey(_shootKeySecondary) || (!isPointerOverUI && Input.GetMouseButton(_shootMouseButtonPrimary));

            SetInputAction(InputAction.MoveLeft, moveLeftKey);
            SetInputAction(InputAction.MoveRight, moveRightKey);
            SetInputAction(InputAction.Shoot, shootKey);
        }
    }
}