using UnityEngine;

public class DollyAnimationHandler : MonoBehaviour {
    [SerializeField] private bool _isBlinking; // Blinking Toggle

    public BlinkingArrow BlinkingArrow; // Object with BlinkingArrow script
    
    public void ToggleBlinking() {
        _isBlinking = !_isBlinking;
        BlinkingArrow.ToggleBlinking(_isBlinking);
    }
}
