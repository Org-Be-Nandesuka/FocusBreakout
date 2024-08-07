using UnityEngine;

public class RotateY : MonoBehaviour
{
    [SerializeField] private float speed;
    void Update()
    {
        this.gameObject.transform.Rotate(0, speed * 100 * Time.deltaTime, 0, 0);
    }
}
