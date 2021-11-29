using System.Collections;
using UnityEngine;

namespace NullFrameworkException.Test.Core.SortingAlgorithm
{
    /// <summary>
    /// Space for additional setup at a later date
    /// </summary>
    public abstract class SearchingBehaviour : MonoBehaviour, ISort
    {
        protected abstract void Randomise(params object[] _params);

        public abstract void InputText(params object[] _params);
        
        public abstract IEnumerator Find(params object[][] _params);
    }
}

