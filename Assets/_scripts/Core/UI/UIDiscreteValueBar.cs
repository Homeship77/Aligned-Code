using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class UIDiscreteValueBar: MonoBehaviour
    {
        [SerializeField]
        Image[] _discreteValues;

        public void SetValues(int value)
        {
            for(int i = 0; i < _discreteValues.Length; i++)
            {
                _discreteValues[i].enabled = i < value;
            }
        }

    }
}
