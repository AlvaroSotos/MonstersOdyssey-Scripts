using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FilteredFlockBehavior : Flock_Behavior //por ser abstracta no necesita implementar la clase abstracta
{
    public ContextFilter filter;
}
