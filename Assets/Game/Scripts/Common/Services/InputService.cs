using SpaceInvaders.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SpaceInvaders.Common
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
        public float MoveDirectionY { get; }
        public bool ShouldShoot { get; }

        public PlayerInput(float moveDirectionY, bool shouldShoot)
        {
            MoveDirectionY = moveDirectionY;
            ShouldShoot = shouldShoot;
        }
    }

    public class InputService
    {
        // Fields
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
            OverrideActionsForEditor();

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

        private void OverrideActionsForEditor()
        {
#if UNITY_EDITOR
            var moveLeftKey = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
            var moveRightKey = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
            var isPointerOverUI = EventSystem.current.IsPointerOverGameObject();
            var shootKey = Input.GetKey(KeyCode.Space) || (!isPointerOverUI && Input.GetMouseButton(0));

            SetInputAction(InputAction.MoveLeft, moveLeftKey);
            SetInputAction(InputAction.MoveRight, moveRightKey);
            SetInputAction(InputAction.Shoot, shootKey);
#endif
        }
    }
}