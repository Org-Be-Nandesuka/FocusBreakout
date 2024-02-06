using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

[RequireComponent(typeof(GlitchStick))]
public class GlitchStick : MonoBehaviour {
    // Determines where a GlitchStick can teleport to based on its TeleportRadius.
    // Cube: Random point within or on a cube (cube length is 2x of the Teleport "Radius").
    // Sphere: Random point within or on a sphere based on its TeleportRadius.
    // SphereSurface: Random point on the surface of a sphere based on its MaxTeleportRadius, 
    // please note that SphereSurface does NOT use MinTeleportRadius.
    [SerializeField] private ShapeEnum _shape;
    // Used if there are multiple GlitchSticks the GlitchBase needs to choose from.
    // Ex: if GlitchStick1 has Priority = 1 and GlitchStick2 has Priority = 5 then
    // GlitchStick1 will have a 1/6 and GlitchStick2 will have a 5/6 chance of being spawned.
    [SerializeField] private int _probability;
    [Space(Constants.InpectorSpaceAttribute)]

    [SerializeField] private float _minWidth;
    [SerializeField] private float _maxWidth;
    [Space(Constants.InpectorSpaceAttribute)]

    [SerializeField] private float _minLength;
    [SerializeField] private float _maxLength;
    [Space(Constants.InpectorSpaceAttribute)]

    [SerializeField] private float _minHeight;
    [SerializeField] private float _maxHeight;
    [Space(Constants.InpectorSpaceAttribute)]

    [SerializeField] [Range(0, 1)] private float _minOpacity;
    [SerializeField] [Range(0, 1)] private float _maxOpacity;
    [Space(Constants.InpectorSpaceAttribute)]

    [SerializeField] private float _minTeleportRadius;
    [SerializeField] private float _maxTeleportRadius;
    [Space(Constants.InpectorSpaceAttribute)]

    [SerializeField] private float _minTeleportTime;
    [SerializeField] private float _maxTeleportTime;
    [Space(Constants.InpectorSpaceAttribute)]
    // Used in GetRandomFloat() to add some extra randomness with Glitch Sticks.
    // BonusRarity is the probability calculated via (1/BonusRarity). Set this
    // to 0 if you want to disable it.
    [SerializeField] private int _bonusRarity;
    [SerializeField] private float _bonusMultiplier;

    private Material _material;

    // TESTING STUFF
    private float _height;
    private bool _goingUp;

    void Start() {
        if (transform.parent == null) {
            throw new ArgumentException("GlitchStick needs to have a parent.");
        }

        _height = -0.5f;
        _goingUp = true;

        _material = GetComponent<Renderer>().material;
        Color color = _material.color;
        float num = Random.Range(_minOpacity, _maxOpacity);

        color.a = num;
        _material.color = color;

        transform.localScale = GetRanomScale();
        StartCoroutine(TeleportCoroutine());
    }

    private Vector3 GetRanomScale() {
        float x = Random.Range(_minWidth, _maxWidth);
        float y = Random.Range(_minHeight, _maxHeight);
        float z = Random.Range(_minLength, _maxLength);

        return new Vector3(x, y, z);
    }

    IEnumerator TeleportCoroutine() {
        while (true) {
            float time = Random.Range(_minTeleportTime, _maxTeleportTime);
            float radius; // For Sphere and SphereSurface

            switch (_shape) {
                case ShapeEnum.Cube:
                    float x = GetRandomFloat(_minTeleportRadius, _maxTeleportRadius, true);
                    float y = GetRandomFloat(_minTeleportRadius, _maxTeleportRadius, true);
                    float z = GetRandomFloat(_minTeleportRadius, _maxTeleportRadius, true);

                    transform.localPosition = new Vector3(x, y, z);
                    break;
                case ShapeEnum.Sphere:
                    radius = GetRandomFloat(_minTeleportRadius, _maxTeleportRadius, false);
                    transform.localPosition = Random.insideUnitSphere * radius;
                    break;
                case ShapeEnum.SphereSurface:
                    radius = GetRandomFloat(_maxTeleportRadius, _maxTeleportRadius, false);
                    transform.localPosition = Random.onUnitSphere * radius;
                    break;
                case ShapeEnum.Test:
                    float yValue = Random.Range(-0.5f, _height);
                    
                    float spread = Mathf.Abs(yValue) - 0.6f; // -0.5 for bottom of 0.5 radius sphere
                    
                    Vector2 v2 = Random.insideUnitCircle * spread;

                    Vector3 v3 = new Vector3(v2.x, yValue, v2.y);
                    time = _minTeleportTime;

                    transform.localPosition = v3;

                    if (_goingUp) {
                        _height += 0.05f;

                        if (_height >= 0.5) {
                            _goingUp = false;
                        }
                    } else {
                        _height -= 0.05f;

                        if (_height <= -0.5) {
                            _goingUp = true;
                        }
                    }
                    
                    break;
                default:
                    throw new ArgumentException("Enum \"" + _shape + "\" is not recognized.");
            }

            yield return new WaitForSeconds(time);
        }
    }

    // Returns a random float between min and max (both inclusive)
    // If isFlipple is enabled, there is a 50% chance its value's sign will be flipped.
    private float GetRandomFloat(float min, float max, bool isFlippable) {
        float num = Random.Range(min, max);
        if (isFlippable && Random.Range(0, 2) == 0) {
            num *= -1;
        }

        // Bonus Values
        if (_bonusRarity != 0 && Random.Range(0, _bonusRarity) == 0) {
            num *= _bonusMultiplier;
        }

        return num;
    }

    public int Probability { 
        get { return _probability; }
    }

    private void OnValidate() {
        // Priority
        if (_probability < 1) {
            _probability = 1;
        }
        // Width and Length
        if (_minWidth < 0) {
            _minWidth = 0;
        }
        if (_maxWidth < _minWidth) {
            _maxWidth = _minWidth;
        }
        if (_minLength < 0) {
            _minLength = 0;
        }
        if (_maxLength < _minLength) {
            _maxLength = _minLength;
        }
        // Height
        if (_minHeight < 0) {
            _minHeight = 0;
        }
        if (_maxHeight < _minHeight) {
            _maxHeight = _minHeight;
        }
        // Opacity
        if (_maxOpacity < _minOpacity) {
            _maxOpacity = _minOpacity;
        }
        // TP Radius
        if (_minTeleportRadius < 0) {
            _minTeleportRadius = 0;
        }
        if (_maxTeleportRadius < _minTeleportRadius) {
            _maxTeleportRadius = _minTeleportRadius;
        }
        // TP Time
        if (_minTeleportTime < 0) {
            _minTeleportTime = 0;
        }
        if (_maxTeleportTime < _minTeleportTime) {
            _maxTeleportTime = _minTeleportTime;
        }
        // Bonus Values
        if (_bonusRarity < 0) {
            _bonusRarity = 0;
        }
        if (_bonusMultiplier < 1.1) {
            _bonusMultiplier = 1.1f;
        }
    }

    private enum ShapeEnum {
        Cube,
        Sphere,
        SphereSurface,
        Test
    }
}