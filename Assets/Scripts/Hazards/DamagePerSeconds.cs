using System.Collections;
using UnityEngine;

public class DamagePerSeconds : MonoBehaviour {
    public bool DealingDamage { get; set; }
    
    [SerializeField] private int _damage;       // Amount of damage
    [SerializeField] private float _interval;   // Time between each instance of damage

    private Coroutine _instance;    // Instance of coroutine that controls damage application

    public void StartApplyingDamage(Player target) {
        if (target == null) {
            Debug.LogError($"The target {this.name} is trying to attack does not have the " +
                $"Player.cs script attached or it is null");
            return;
        }
        _instance = StartCoroutine(ApplyDamage(target));
    }

    public void StopApplyingDamage() {
        if (_instance != null) {
            DealingDamage = false;
            StopCoroutine(_instance);
        }
    }

    private IEnumerator ApplyDamage(Player target) {
        do {
            DealingDamage = true;
            target.TakeDamage(_damage);
            yield return new WaitForSeconds(_interval);
        } while (true);
    }
}
