using NullFrameworkException.Core.SortingAlgorithm;
using UnityEngine;
using Random = UnityEngine.Random;
using TMPro;


namespace NullFrameworkException.Test.SortingAlgorithm
{
    public class SortingBehaviour1 : SortingBehaviour, ISort
    {
        [SerializeField] private TMP_Text[] intText;
        [SerializeField] private int[] values;

        private void Start()
        {
            Randomise();
        }

        /// <summary>Randomises The Numbers, and sets the text value</summary>
        protected override void Randomise()
        {
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = Random.Range(0, 99);
                intText[i].SetText(values[i].ToString());
            }
        }

        /// <summary>Sorts all values in the range using a bubble sort, resets text</summary>
        protected override void Sort(int[] inputArray)
        {
            int temp;
            for (int j = 0; j <= inputArray.Length - 2; j++)
            {
                for (int i = 0; i <= values.Length - 2; i++)
                {
                    if (inputArray[i] > inputArray[i + 1])
                    {
                        temp = values[i + 1];
                        values[i + 1] = values[i];
                        values[i] = temp;
                    }
                }
            }

            for (int i = 0; i < values.Length; i++)
            {
                intText[i].SetText(values[i].ToString());
            }
        }

    }
}

