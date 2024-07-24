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

    private bool isFlickering = false; // Bool to handle coroutine cycle
    private float timeDelay; // Delay between flickering effect
    private float randomIntensity; // Random light intensity (uses UnityEngine.Random)
    
    // The upper bound of light intensity.
    // Set to the intensity of the light component on Start.
    private float maxIntensity; 

    private void Start() {
        maxIntensity = this.gameObject.GetComponent<Light>().intensity;
    }

    private void Update() {
        if(isFlickering == false)
        {
            StartCoroutine(FlickeringLight());
        }
    }

    /// <summary>
    /// Coroutine to handle light component flickering based
    /// on random values.
    /// </summary>
    IEnumerator FlickeringLight() {
        isFlickering = true;

        randomIntensity = UnityEngine.Random.Range(minIntensity, maxIntensity);
        this.gameObject.GetComponent<Light>().intensity = randomIntensity;
        timeDelay = UnityEngine.Random.Range(0.01f, 0.2f);
        
        yield return new WaitForSeconds(timeDelay);

        randomIntensity = UnityEngine.Random.Range(minIntensity, maxIntensity);
        this.gameObject.GetComponent<Light>().intensity = randomIntensity;
        timeDelay = UnityEngine.Random.Range(0.01f, 0.2f);
        
        yield return new WaitForSeconds(timeDelay);

        isFlickering = false;
    }
}
