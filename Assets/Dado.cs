using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dado : MonoBehaviour
{
 
    public int TirarDado()
    {
        return Random.Range(1, 7); // Genera un número aleatorio entre 1 y 6
    }

    void Start()
    {
        Debug.Log("Resultado del dado: " + TirarDado());
    }
}