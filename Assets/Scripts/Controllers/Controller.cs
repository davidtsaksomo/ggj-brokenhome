using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller : MonoBehaviour
{
    [HideInInspector]
    public float horizontalAxisInput, verticalAxisInput;

    [Header("Movement Variables")]
    [Tooltip("Force applied when moving horizontally")]
    public float moveForce = 50000f;
    [Tooltip("Max run speed")]
    public float maxSpeed = 6.2f;
    [Tooltip("Force applied to deaccelerate")]
    public float horizontalDrag = 15000f;
    [Tooltip("Max speed multiplier when sprinting")]
    [Range(1f, 5f)]
    public float runMultiplier = 1.5f;
    [Range(0f, 1f)]
    [Tooltip("Movement responsiveness while in air")]
    public float airControlMultiplier = 0.5f;
    
    public virtual void Awake()
    {}

    public virtual void Start()
    {}

    public virtual void Update()
    {}

    public virtual void FixedUpdate()
    {}
}
