using UnityEngine;  

namespace NullFrameworkException.Core.SortingAlgorithm
{
    public abstract class SortingBehaviour : MonoBehaviour
    {

        /// <summary>Randomises The Numbers, and sets the text value</summary>
        protected abstract void Randomise();

        /// <summary>Sorts all values in the range using a bubble sort, resets text</summary>
        protected abstract void Sort(int[] inputArray);
        
    }
}

