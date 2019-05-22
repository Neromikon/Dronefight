using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//	this class is needed cause Selectable.IsPressed() is not accessible
//	through Button reference

public class VirtualKey : Button
{
    public bool pressed { get; private set; }
	
    void Update()
    {
		pressed = IsPressed();
    }
}
