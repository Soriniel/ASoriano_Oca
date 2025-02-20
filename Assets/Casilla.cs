using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Casilla : MonoBehaviour
{
    public int numeroCasilla;
    public GameManager gameManager;

    void Awake()
    {
        string casillaString = this.gameObject.name.Substring(7);
        numeroCasilla = int.Parse(casillaString);
    }
}
