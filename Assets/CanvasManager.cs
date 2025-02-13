using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public Text rondaText;
    public Text jugadorActivoText;
    public int ronda = 1;
    public int turno = 0;

    void Start()
    {
        ActualizarUI();
    }

    void ActualizarUI()
    {
        rondaText.text = "Ronda: " + ronda;
        if (turno == 0)
        {
            jugadorActivoText.text = "Turno de: Jugador";
        }
        else
        {
            jugadorActivoText.text = "Turno de: IA";
        }
    }
}

