using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[AddComponentMenu("Kino Effects/Digital Glitch")]
public class KinoShaderGraph : MonoBehaviour
{
    #region Public Properties

    [SerializeField] private float minEffectTime = 1f;  // Minimum duration for the glitch effect
    [SerializeField] private float maxEffectTime = 3f;  // Maximum duration for the glitch effect
    [SerializeField] private float minEffectInterval = 1f;  // Minimum interval between glitches
    [SerializeField] private float maxEffectInterval = 3f;  // Maximum interval between glitches

    [SerializeField, Range(0, 1)]
    float _intensity = 0;

    public float intensity
    {
        get { return _intensity; }
        set { _intensity = value; }
    }

    #endregion

    #region Private Properties

    [SerializeField] Shader _shader;
    //[SerializeField] Material _material;

    Material _material;
    Texture2D _noiseTexture;
    Texture2D _trashTexture1;
    Texture2D _trashTexture2;
    RenderTexture _renderTexture1;
    RenderTexture _renderTexture2;
    Image _image;

    float startTime;
    float effectEndTime;
    bool isGlitching;

    #endregion

    #region Private Functions

    static Color RandomColor()
    {
        return new Color(Random.value, Random.value, Random.value, Random.value);
    }

    void SetUpResources()
    {
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

    void UpdateNoiseTexture()
    {
        var color = RandomColor();

        for (var y = 0; y < _noiseTexture.height; y++)
        {
            for (var x = 0; x < _noiseTexture.width; x++)
            {
                if (Random.value > 0.89f) color = RandomColor();
                _noiseTexture.SetPixel(x, y, color);
            }
        }

        _noiseTexture.Apply();
    }

    void CopyRenderTextureToTexture2D(RenderTexture renderTexture, Texture2D texture2D)
    {
        RenderTexture.active = renderTexture;
        texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture2D.Apply();
        RenderTexture.active = null;
    }

    #endregion

    #region MonoBehaviour Functions

    void Start()
    {
        startTime = Time.time;
        isGlitching = false;
        effectEndTime = Time.time;

        _image = GetComponent<Image>();
        if (_image == null)
        {
            Debug.LogError("No Image component found on the GameObject. Please ensure the script is attached to a UI element with an Image component.");
            return;
        }
    }

    void Update()
    {        
        if (_image == null)
            return;

        if (Random.value > Mathf.Lerp(0.9f, 0.5f, _intensity) && isGlitching)
        {
            SetUpResources();
            UpdateNoiseTexture();
            ApplyShader();
            effectEndTime = Time.time;
        }

        if(Time.time - startTime > minEffectTime)
        {
            isGlitching = false;
        }

        if(Time.time - effectEndTime > minEffectInterval)
        {
            isGlitching = true;
            startTime = Time.time;
        }

        if(isGlitching == false)
        {
            ResetShader();
        }

    }

    void ApplyShader()
    {
        if (_material == null || _image == null)
            return;

        // Update trash frames on a constant interval.
        var fcount = Time.frameCount;
        if (fcount % 13 == 0) Graphics.Blit(null, _renderTexture1, _material);
        if (fcount % 73 == 0) Graphics.Blit(null, _renderTexture2, _material);

        CopyRenderTextureToTexture2D(_renderTexture1, _trashTexture1);
        CopyRenderTextureToTexture2D(_renderTexture2, _trashTexture2);

        _material.SetFloat("_Intensity", _intensity);
        _material.SetTexture("_NoiseTex", _noiseTexture);
        var trashTexture = Random.value > 0.5f ? _trashTexture1 : _trashTexture2;
        _material.SetTexture("_TrashTex", trashTexture);

        // Apply the material to the Image
        _image.material = _material;
    }

    void ResetShader()
    {
        if (_material == null || _image == null)
            return;

        _material.SetTexture("_NoiseTex", null);
        _material.SetTexture("_TrashTex", null);
        _image.material = _material;
    }

    #endregion
}
