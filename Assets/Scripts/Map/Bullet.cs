using System;
using UnityEngine;

public class Bullet : Hazard
{
    private CharacterController _controller;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Update() {
        _controller.Move(Direction * Time.deltaTime * Speed);
    }
}
