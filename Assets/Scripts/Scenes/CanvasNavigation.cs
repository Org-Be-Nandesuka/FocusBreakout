using UnityEngine;

public class CanvasNavigation : MonoBehaviour
{
    [SerializeField] private GameObject _parentCanvas;
    [SerializeField] private GameObject _targetCanvas;

    public void LoadCanvas() {
        _targetCanvas.SetActive(true);
        _parentCanvas.SetActive(false);
    }
}
