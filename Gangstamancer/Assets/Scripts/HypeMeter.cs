using UnityEngine;
using UnityEngine.UI;

public class HypeMeter : MonoBehaviour
{
    public Slider _slider;
    public float _winOMeter = .5f;

    
    private void Update()
    {
        _slider.value = _winOMeter;
    }
}
