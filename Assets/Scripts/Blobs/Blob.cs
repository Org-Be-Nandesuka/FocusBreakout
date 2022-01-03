using System;
using UnityEngine;

/// <summary>
/// Base Blob class that just handles their health.
/// Die() method still needs to be implemented.
/// </summary>
public abstract class Blob : MonoBehaviour {
    [SerializeField] private int _maxHealth;

    private int _currentHealth;

    private void Awake() {
        _currentHealth = _maxHealth;
    }

    public virtual void TakeDamage(int dmg) {
        if (dmg <= 0) {
            throw new ArgumentException("Can't deal " + dmg + " damage, needs to be at least 0");
        }

        _currentHealth -= dmg;
        if (_currentHealth <= 0) {
            Die();
        }
    }

    public virtual void Heal(int num) {
        if (num <= 0) {
            throw new ArgumentException("Can't heal " + num + ", needs to be at least 0");
        }

        _currentHealth += num;
        if (_currentHealth > MaxHealth) {
            _currentHealth = MaxHealth;
        }
    }

    /// <summary>
    /// Sets Current Health to Max Health.
    /// </summary>
    protected void ResetCurrentHealth() {
        _currentHealth = _maxHealth;
    }

    protected abstract void Die();

    public int CurrentHealth {
        get { return _currentHealth; }
    }

    public int MaxHealth {
        get { return _maxHealth; }
        protected set { 
            if (value < 1) {
                throw new ArgumentException("Max Health must be at least 1.");
            } else {
                _maxHealth = value;
            }
        }
    }

    protected virtual void OnValidate() {
        if (_maxHealth < 1) {
            _maxHealth = 1;
        }
    }
}
