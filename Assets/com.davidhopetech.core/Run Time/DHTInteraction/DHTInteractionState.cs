using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract class DHTInteractionState
{
    DHTPlayerController _controller;
        
    public abstract void Update();
}

