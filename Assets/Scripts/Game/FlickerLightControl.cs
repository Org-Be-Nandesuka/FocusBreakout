using System.Collections;
using UnityEngine;

/// <summary>
/// Controls light flickering on light objects its attached to.
/// 
/// <para>
/// Script should be attached to a GameObject with a realtime Light component.
/// </para>
/// </summary>
public class FlickerLightControl : MonoBehaviour {
    
    [SerializeField] private float minIntensity; // The lower bound of light intensity

    private bool _isFlickering = false; // Bool to handle coroutine cycle
    private float _timeDelay; // Delay between flickering effect
    private float _randomIntensity; // Random light intensity (uses UnityEngine.Random)
    
    // The upper bound of light intensity.
    // Set to the intensity of the light component on Start.
    private float _maxIntensity; 

    void Start() {
        _maxIntensity = GetComponent<Light>().intensity;
    }

    void Update() {
        if(_isFlickering == false) {
            StartCoroutine(FlickeringLight());
        }
    }

    /// <summary>
    /// Coroutine to handle light component flickering based
    /// on random values.
    /// </summary>
    IEnumerator FlickeringLight() {
        _isFlickering = true;

        _randomIntensity = Random.Range(minIntensity, _maxIntensity);
        GetComponent<Light>().intensity = _randomIntensity;
        _timeDelay = Random.Range(0.01f, 0.2f);
            
        yield return new WaitForSeconds(_timeDelay);

        _randomIntensity = Random.Range(minIntensity, _maxIntensity);
        GetComponent<Light>().intensity = _randomIntensity;
        _timeDelay = Random.Range(0.01f, 0.2f);
        
        yield return new WaitForSeconds(_timeDelay);

        _isFlickering = false;
    }
}
