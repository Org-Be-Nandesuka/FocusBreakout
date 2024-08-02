using UnityEngine.UI;
using UnityEngine;

public class Compass : MonoBehaviour {
	[SerializeField] private RawImage _compassImage;
	[SerializeField] private Transform _player; // Does not work with prefab

	private void Update(){
		//Get a handle on the Image's uvRect
		_compassImage.uvRect = new Rect(_player.localEulerAngles.y / 360, 0, 1, 1);

		// Get a copy of your forward vector
		Vector3 forward = _player.transform.forward;

		// Zero out the y component of your forward vector to only get the direction in the X,Z plane
		forward.y = 0;
	}
}
