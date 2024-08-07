using System;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

// Have you seen DisneyPlus' Loki
// Well unfortunately, we don't have access to that many fonts
public class TextGlitchEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    [Header("Initial Text Glitch")]
    // How long this effect lasts
    [SerializeField] private float _minTotalTime = 0f;
    [SerializeField] private float _maxTotalTime = 1f;
    // How long for a char to change
    [SerializeField] private float _minChangeTime = 0f;
    [SerializeField] private float _maxChangeTime = 0.1f;

    [Header("Continuous Text Glitch")]
    [SerializeField] private int _charAmount = 3;

    [Header("On Hover Text Glitch")]
    [SerializeField] private float _hoverEffectLength = 0.2f;
    [SerializeField] private float _hoverEffectSpeed = 0.01f;
    [SerializeField] private float _hoverEffectStrength = 250f;

    [Header("Unity's Debug Logger")]
    // Disables Debug.Log because converting unicode decimal above 126 to char gives a
    // lot of log warnings that can be safely ignored. However, for debugging purposes
    // it is recommended to change the MaxUnicodeValue to 126 and EnableLogger to true.
    //[SerializeField] private bool _enableLogger = true;

    // Using unicode decimal format
    private const int _minUnicodeValue = 33;
    private const int _maxUnicodeValue = 126; // use 126 for no log warnings

    private string _originalText;
    private string _targetText;
    private float _originalCharacterSpacing;
    private TextMeshProUGUI _text;
    private StringBuilder _stringBuilder;
    private Coroutine _hoverCoroutine;
    private Coroutine[] _initialRandCoroutineArray;
    private Coroutine[] _contRandCoroutineArray;

    void OnEnable() {
        int originalLength;

        // If object has GameWinScoreManager update the text on TMPGUI before glitch effect
        if (TryGetComponent(out GameWinScoreManager gameWinScoreManager))
        {
            gameWinScoreManager.UpdateText();
        }

        _text = GetComponent<TextMeshProUGUI>();
        _originalText = _text.text;
        originalLength = _text.text.Length;
        _originalCharacterSpacing = _text.characterSpacing;
        _stringBuilder = new StringBuilder(_originalText, originalLength);
        _initialRandCoroutineArray = new Coroutine[originalLength];
        _contRandCoroutineArray = new Coroutine[_charAmount];

        TextChange(_originalText);

        for (int i = 0; i < _charAmount; i++) {
            _contRandCoroutineArray[i] = StartCoroutine(ContinuousRandCharCoroutine());
        }
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    // Randomly change a specific character in a string
    IEnumerator StartRandCharCoroutine(int idx, string text) {
        while (true) {
            float time = Random.Range(_minChangeTime, _maxChangeTime);

            _stringBuilder.Length = text.Length;
            _stringBuilder[idx] = GetRandomChar();
            _text.text = _stringBuilder.ToString();
            yield return new WaitForSecondsRealtime(time);
        }
    }

    // Return character to what it was originally
    IEnumerator EndRandCharCoroutine(int idx, float min, float max, string text) {
        float time = Random.Range(min, max);
        yield return new WaitForSecondsRealtime(time);
        StopCoroutine(_initialRandCoroutineArray[idx]);
        _stringBuilder[idx] = text[idx];
    }

    // There is currently a bug where not all of the characters
    // are returned back to normal (usually 1 or 2), this is used
    // as a temporary fix.
    IEnumerator FinalRandCharCoroutine(float time, string text) {
        yield return new WaitForSecondsRealtime(time);
        _text.text = text;
    }

    // Occassionally switch a char to another random char and back.
    IEnumerator ContinuousRandCharCoroutine() {
        while (true) {
            int idx = Random.Range(0, _targetText.Length);
            float glitchTime = Random.Range(0f, 0.25f);
            float restTime = Random.Range(0f, 8f);

            _stringBuilder[idx] = GetRandomChar();
            _text.text = _stringBuilder.ToString();
            yield return new WaitForSecondsRealtime(glitchTime);

            _stringBuilder[idx] = _targetText[idx]; // reset StringBuilder for next iteration
            _text.text = _targetText;
            yield return new WaitForSecondsRealtime(restTime);
        }
    }

    private char GetRandomChar() {
        int num = Random.Range(_minUnicodeValue, _maxUnicodeValue + 1);
        return Convert.ToChar(num);
    }

    // Ideally I would like to change the text whenever I want with the
    // "glitch effect", but unfortunately I do not understand coroutines
    // well enough so this can only be used at Start() :c
    private void TextChange(string text) {
        _targetText = text;
        for (int i = 0; i < text.Length; i++) {
            _initialRandCoroutineArray[i] = StartCoroutine(StartRandCharCoroutine(i, text));
            StartCoroutine(EndRandCharCoroutine(i, _minTotalTime, _maxTotalTime, text));
        }

        StartCoroutine(FinalRandCharCoroutine(_maxTotalTime, text));
    }

    IEnumerator OnHoverCoroutine() {
        float currentTime = 0f;

        while (currentTime < _hoverEffectLength) {
            float spacing = Random.Range(_hoverEffectStrength * -1, _hoverEffectStrength);
            _text.characterSpacing = spacing;
            currentTime += _hoverEffectSpeed;
            yield return new WaitForSecondsRealtime(_hoverEffectSpeed);
        }

        _text.characterSpacing = _originalCharacterSpacing;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        _hoverCoroutine = StartCoroutine(OnHoverCoroutine());
    }

    public void OnPointerExit(PointerEventData eventData) {
        StopCoroutine(_hoverCoroutine);
        _text.characterSpacing = _originalCharacterSpacing;
    }

    private void OnValidate() {
        // Total Time
        if (_minTotalTime < 0) {
            _minTotalTime = 0;
        }
        if (_maxTotalTime < _minTotalTime) {
            _maxTotalTime = _minTotalTime;
        }
        // Change Time
        if (_minChangeTime < 0) {
            _minChangeTime = 0;
        }
        if (_maxChangeTime < _minChangeTime) {
            _maxChangeTime = _minChangeTime;
        }
        // Character Amount
        if (_charAmount < 0) {
            _charAmount = 0;
        }
        // Hover Effect
        if (_hoverEffectLength < 0) {
            _hoverEffectLength = 0;
        }
        if (_hoverEffectSpeed < 0.01) {
            _hoverEffectSpeed = 0.01f;
        }
        if (_hoverEffectStrength < 0) {
            _hoverEffectStrength = 0;
        }
    }
}
