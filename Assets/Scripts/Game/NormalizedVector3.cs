using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalizedVector3 {
    private float _x;
    private float _y;
    private float _z;

    private void IsNormalized(float num) {
        if (num < -1 || num > 1) {
            throw new ArgumentException("");
        }
    }

    public float X { 
        get { return _x; }
        set { 
            IsNormalized(_x);
            _x = value;
        }
    }

    public float Y {
        get { return _y; }
        set {
            IsNormalized(_y);
            _y = value;
        }
    }

    public float Z {
        get { return _z; }
        set {
            IsNormalized(_z);
            _z = value;
        }
    }
}
