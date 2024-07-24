using UnityEngine;

public class CanvasNavigation : MonoBehaviour
{
    [SerializeField] private GameObject parentCanvas;
    [SerializeField] private GameObject targetCanvas;

    public void LoadCanvas()
    {
        targetCanvas.SetActive(true);
        parentCanvas.SetActive(false);
    }
}
