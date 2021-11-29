using System;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace NullFrameworkException.Test.Core.SceneDragNDropTest
{
    public class SceneDragNDropTest : MonoBehaviour
    {
        [SerializeField, Rename("Scene descriptor")]
        private Text titleSceneText;
        [SerializeField, SceneField] private string[] sceneInt;

        /// <summary> This will make the title = the current scene name. </summary>
        private void Start()
        {
            if(SceneManager.GetActiveScene().buildIndex != 0)
                titleSceneText.text = "Welcome to Scene " + SceneManager.GetActiveScene().buildIndex;
            else
                titleSceneText.text = "Welcome to the Main Menu";
        } 
        
        /// <summary> This will load in the dragged in scene from the inspector.
        ///  Can be changed to either throw an exception or ignore the command</summary>
        /// <param name="index">index of the scene attempted to be loaded</param>>
        public void GoTo_scene(int index)
        {
            try
            {
                SceneManager.LoadScene(sceneInt[index]);
            }
            catch /*(Exception e)*/
            {
                //Console.WriteLine(e.Message);
                //throw new ArgumentOutOfRangeException("index parameter is out of range.", e);
            }

        }
    }
}