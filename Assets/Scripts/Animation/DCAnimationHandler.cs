using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DCAnimationHandler : MonoBehaviour
{
    public BlinkingArrow blinkingArrow;
    [SerializeField] private bool _isBlinking;

    public void ToggleBlinking()
    {
        _isBlinking = !_isBlinking;
        blinkingArrow.ToggleBlinking(_isBlinking);
    }
}
