using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IModule<T> where T : MonoBehaviour
{
    public void UpdateModule();
    public void Init(T controller);
}
