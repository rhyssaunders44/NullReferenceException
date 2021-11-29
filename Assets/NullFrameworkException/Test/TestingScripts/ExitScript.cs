using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NullFrameworkException.Test.CharacterController.RigidbodyController3D
{
    /// <summary> Just exit the game on Esc. </summary>
    public class ExitScript : MonoBehaviour
    {
        /// <summary>This key can be rebound in inspector, or potentially in a rebinding script when made public</summary>
        [SerializeField] private KeyCode chosenKey = KeyCode.Escape;

        /// <summary>check every frame to see if the quit key was pressed</summary>
        private void Update()
        {
            if (Input.GetKeyDown(chosenKey))
            {
                Quit();
            }
        }

        /// <summary> Basic Quit function to exit the game, quits play mode in editor. Assignable to a UI button</summary>
        public void Quit()
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #endif
                Application.Quit();
        }
    }
}