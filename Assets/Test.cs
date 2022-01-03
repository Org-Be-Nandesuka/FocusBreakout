using UnityEngine;

public class Test : MonoBehaviour {
    void Start() {
        
    }

    void Update() {
        transform.Rotate(Vector3.back * 500 * Time.deltaTime);
    }
}
