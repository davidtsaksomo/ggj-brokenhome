using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NadineController : Controller
{
    public override void Awake()
    {}

    public override void Start()
    {}

    public override void Update()
    {}

    public override void FixedUpdate()
    {
        horizontalAxisInput = Input.GetAxis("Horizontal");
        verticalAxisInput = Input.GetAxis("Vertical");

        if(horizontalAxisInput < 0.1f && horizontalAxisInput > -0.1f)
        {
            horizontalAxisInput = 0;
        }
    }
}
