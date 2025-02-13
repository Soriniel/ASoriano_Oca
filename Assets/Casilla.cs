using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class Casilla : MonoBehaviour
{
    public int numeroCasilla;

    void Awake()
    {
        int.TryParse(Regex.Match(gameObject.name, "\\d+").Value, out numeroCasilla);
    }

}
