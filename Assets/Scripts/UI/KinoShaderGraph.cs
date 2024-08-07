using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[AddComponentMenu("Kino Effects/Digital Glitch")]
public class KinoShaderGraph : MonoBehaviour {
    [SerializeField] private Shader _shader;
    [SerializeField] private float _minEffectTime = 1f;  // Minimum duration for the glitch effect
    [SerializeField] private float _minEffectInterval = 1f;  // Minimum interval between glitches
    [SerializeField, Range(0, 1)] private float _intensity = 0;

    //public float intensity
    //{
    //    get { return _intensity; }
    //    set { _intensity = value; }
    //}

    // Digital glitch properties
    private Material _material;
    private Texture2D _noiseTexture;
    private Texture2D _trashTexture1;
    private Texture2D _trashTexture2;
    private RenderTexture _renderTexture1;
    private RenderTexture _renderTexture2;
    private Image _image;

    // Time and Bool check
    private float _startTime;
    private float _effectEndTime;
    private bool _isGlitching;

    void Start() {
        _startTime = Time.time;
        _isGlitching = false;
        _effectEndTime = Time.time;

        _image = GetComponent<Image>();
        if (_image == null) {
            Debug.LogError("No Image component found on the GameObject. Please ensure the script is attached to a UI element with an Image component.");
            return;
        }
    }

    void Update() {        
        if (_image == null) return;

        if (Random.value > Mathf.Lerp(0.9f, 0.5f, _intensity) && _isGlitching) {
            SetUpResources();
            UpdateNoiseTexture();
            ApplyShader();
            _effectEndTime = Time.time;
        }

        if(Time.time - _startTime > _minEffectTime) {
            _isGlitching = false;
        }

        if(Time.time - _effectEndTime > _minEffectInterval) {
            _isGlitching = true;
            _startTime = Time.time;
        }

        if(_isGlitching == false) {
            ResetShader();
        }

    }

    void ApplyShader() {
        if (_material == null || _image == null) return;

        // Update trash frames on a constant interval.
        var fcount = Time.frameCount;
        if (fcount % 13 == 0) {
            Graphics.Blit(null, _renderTexture1, _material);
        }
        if (fcount % 73 == 0) {
            Graphics.Blit(null, _renderTexture2, _material);
        }
          
        CopyRenderTextureToTexture2D(_renderTexture1, _trashTexture1);
        CopyRenderTextureToTexture2D(_renderTexture2, _trashTexture2);

        _material.SetFloat("_Intensity", _intensity);
        _material.SetTexture("_NoiseTex", _noiseTexture);
        var trashTexture = Random.value > 0.5f ? _trashTexture1 : _trashTexture2;
        _material.SetTexture("_TrashTex", trashTexture);

        _image.material = _material; // Apply the material to the Image
    }

    void ResetShader() {
        if (_material == null || _image == null) return;

        _material.SetTexture("_NoiseTex", null);
        _material.SetTexture("_TrashTex", null);
        _image.material = _material;
    }

    static Color RandomColor() {
        return new Color(Random.value, Random.value, Random.value, Random.value);
    }

    void SetUpResources() {
        if (_material != null) return;

        // Assuming you have assigned the material in the inspector.
        // If not, create a new material with the Shader Graph shader here.
        _material = new Material(_shader);
        _material.hideFlags = HideFlags.DontSave;

        _noiseTexture = new Texture2D(64, 32, TextureFormat.ARGB32, false);
        _noiseTexture.hideFlags = HideFlags.DontSave;
        _noiseTexture.wrapMode = TextureWrapMode.Clamp;
        _noiseTexture.filterMode = FilterMode.Point;

        _renderTexture1 = new RenderTexture(Screen.width, Screen.height, 0);
        _renderTexture2 = new RenderTexture(Screen.width, Screen.height, 0);
        _renderTexture1.hideFlags = HideFlags.DontSave;
        _renderTexture2.hideFlags = HideFlags.DontSave;

        _trashTexture1 = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, false);
        _trashTexture2 = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, false);
        _trashTexture1.hideFlags = HideFlags.DontSave;
        _trashTexture2.hideFlags = HideFlags.DontSave;

        UpdateNoiseTexture();
    }

    void UpdateNoiseTexture() {
        var color = RandomColor();

        for (var y = 0; y < _noiseTexture.height; y++) {
            for (var x = 0; x < _noiseTexture.width; x++) {
                if (Random.value > 0.89f) {
                    color = RandomColor();
                }
                _noiseTexture.SetPixel(x, y, color);
            }
        }

        _noiseTexture.Apply();
    }

    void CopyRenderTextureToTexture2D(RenderTexture renderTexture, Texture2D texture2D) {
        RenderTexture.active = renderTexture;
        texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture2D.Apply();
        RenderTexture.active = null;
    }

}
