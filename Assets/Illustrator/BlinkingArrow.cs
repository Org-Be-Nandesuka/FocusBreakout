using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkingArrow : MonoBehaviour {
    [SerializeField] private float _blinkInterval = 0.5f; // Interval in seconds
    [SerializeField] private Image _arrowImage;
    [SerializeField] private bool _isBlinking;

    private Coroutine _blinkingCoroutine;

    void Start() {
        _arrowImage = GetComponent<Image>();
        if (_arrowImage == null) {
            Debug.LogError("BlinkingArrow script requires an Image component on the same GameObject.");
            return;
        }

        _blinkingCoroutine = StartCoroutine(BlinkArrow());
        _isBlinking = true;
    }

    public void ToggleBlinking (bool shouldBlink) {
        if (shouldBlink && !_isBlinking) { 
            _blinkingCoroutine = StartCoroutine(BlinkArrow());
            _isBlinking = true;
        }

        else if (!shouldBlink && _isBlinking) {
            StopCoroutine(_blinkingCoroutine);
            _arrowImage.enabled = true;
            _isBlinking = false;
        }
}


    IEnumerator BlinkArrow() {
        while (true) {
            _arrowImage.enabled = !_arrowImage.enabled;
            yield return new WaitForSeconds(_blinkInterval);
        }
    }
}
