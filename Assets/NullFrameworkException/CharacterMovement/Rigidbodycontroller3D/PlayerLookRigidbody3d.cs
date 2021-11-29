using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NullFrameworkException.CharacterMovement.RigidbodyController3D
{
    /// <summary> Attach to the player for first person camera view. </summary>
    public class PlayerLookRigidbody3d : MonoBehaviour
    {
        /// <summary> Horizontal sensitivity. </summary>
        private float sensX = 1.5f;
        /// <summary> Vertical sensitivity. </summary>
        private float sensY = 1.5f;

        /// <summary>Allows the camera to move independently</summary>
        [SerializeField] private Transform cameraManager = null;
        [SerializeField] private Transform orientation = null;

        float mouseX;
        float mouseY;

        float xRotation;
        float yRotation;
        
        private void Start()
        {
            CursorSettings();
        }

        private void Update()
        {
            LookInput();

            //clamps rotation of the x axis
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            
            cameraManager.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            //orientation.transform.rotation = cameraManager.transform.rotation;
        }

        /// <summary> Mouse Input of the player. </summary>
        private void LookInput()
        {
            mouseX = Input.GetAxisRaw("Mouse X");
            mouseY = Input.GetAxisRaw("Mouse Y");
            
            yRotation += mouseX * sensX;
            xRotation -= mouseY * sensY;
        }

        /// <summary>Sets up the cursor for non cursor Gameplay</summary>
        private void CursorSettings()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}