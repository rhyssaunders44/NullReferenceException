using System.Collections;
using TMPro;
using UnityEngine;
using NullFrameworkException.Test.Core.SortingAlgorithm;

namespace NullFrameworkException.Test.Core.SearchingAlgorithm
{
    public class SearchingBehaviour1 : SearchingBehaviour
    {
        [Header("Input Arrays")]
        [SerializeField] private TMP_Text[] intText;
        [SerializeField] private int[] values;
        
        [Header("Single Inputs")]
        [SerializeField] private TMP_Text foundText;
        [SerializeField] private TMP_InputField input;
        [SerializeField] private string searchValue;
    
        [Header("Unlisted variables")]
        private bool found;
    
        private void Start()
        {
            Randomise();
        }

        /// <summary>Button applicable randomiser</summary>
        public void PressMe()
        {
            Randomise();
        }

        /// <summary>InputField method</summary>
        public void Search()
        {
            InputText();
        }
    
        /// <summary>Randomises the Values of the value array</summary>
        protected override void Randomise(params object[] _params)
        {
            foundText.text = "Numbers have been Randomised!";
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = Random.Range(0, 99);
                intText[i].text = values[i].ToString();
                
                if (values[i] == 69)
                {
                    foundText.text = "position " + (i + 1) + " is nice!";
                }   
            }
            found = false;
        }

        /// <summary>Sets the search number and begins the find routine</summary>
        /// <param name="_params"> the UI input field</param>
        public override void InputText(params object[] _params)
        {
            searchValue = input.text;
            StartCoroutine(Find()); 
        }

        /// <summary>Finds the input number</summary>
        /// <returns>A text value of the findings or that the result could not be found</returns>
        public override IEnumerator Find(params object[][] _params)
        {
            for (int i = 0; i < intText.Length; i++)
            {
                if (intText[i].text == searchValue)
                {
                    var tex = "the score was found at position ";
                    if (intText[i].text != 69.ToString())
                    {
                        foundText.text = tex + (i + 1);
                    }
                    else
                    {
                        foundText.text = tex + (i + 1) + ", NICE!";
                    }
                    found = true;
                }
            }
    
            if (!found)
            {
                foundText.text = "The Score you were looking for could not be found!";
            }
            found = false;
            
            yield break;
        }
    }
}

