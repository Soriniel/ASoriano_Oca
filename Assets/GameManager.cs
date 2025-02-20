using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public int[] vectorCasillas;
    public int[] infoCasillas;
    public GameObject[] vectorObjetos;


    public TextMeshProUGUI textoTurno;
    public TextMeshProUGUI textoDado;
    public Button botonTirarDado; // Botón de la UI para tirar el dado

    private int turnoActual = 0; // 0 = Jugador, 1 = IA
    private bool esperandoDecision = false;

    private void Awake()
    {

        vectorCasillas = new int[21];
        infoCasillas = new int[21];

        // RELLENAMOS EL VECTOR DE CASILLAS
        for (int i = 0; i < vectorCasillas.Length; i++)
            vectorCasillas[i] = 0;

        // RELLENAMOS EL VECTOR DE INFO CASILLAS
        for (int i = 0; i < infoCasillas.Length; i++)
            infoCasillas[i] = 0;

        // teleports
        infoCasillas[0] = 1;
        infoCasillas[5] = 1;

        // volver a tirar
        infoCasillas[11] = 2;
        infoCasillas[17] = 2;

        // retroceder 3 casillas
        infoCasillas[4] = -1;
        infoCasillas[9] = -1;
        infoCasillas[13] = -1;
        infoCasillas[18] = -1;
        infoCasillas[19] = -1;

        // victoria
        infoCasillas[20] = 99;

        // RELLENAMOS EL VECTOR DE GAMEOBJECTS
        vectorObjetos = GameObject.FindGameObjectsWithTag("casilla");

        // METODO 2: RELLENAR CON UN FOR Y UN FIND
        vectorObjetos = new GameObject[21];

        for (int i = 0; i < vectorObjetos.Length; i++)
        {
            vectorObjetos[i] = GameObject.Find("casilla" + i);
        }

        // 21 CASILLAS DESORDENADAS
        GameObject[] vectorGOCasillas = GameObject.FindGameObjectsWithTag("casilla");

        for (int i = 0; i < vectorGOCasillas.Length; i++)
        {
            GameObject casilla = vectorGOCasillas[i];

            // Suponemos que el nombre tiene el formato "casillaX"
            // Extraemos la parte numérica del nombre. Ajusta el índice si tu prefijo cambia.
            string casillaString = casilla.name.Substring(7); // "casilla" tiene 7 caracteres
            if (int.TryParse(casillaString, out int numeroCasilla))
            {
                // Si el número está dentro del rango, lo asignamos en esa posición del vector
                if (numeroCasilla >= 0 && numeroCasilla < vectorObjetos.Length)
                {
                    vectorObjetos[numeroCasilla] = casilla;
                }

            }
        }
    }
}
/*
    public void Start()
    {
        IniciarTurno();
    }

    public void IniciarTurno()
    {

        if (turnoActual == 0)
        {
            StartCoroutine(TurnoJugador());
        }
        else
        {
            StartCoroutine(TurnoIA());
        }
    }

    IEnumerator TurnoJugador()
    {
        Debug.Log("Turno jugador!");
    }
    IEnumerator TurnoIA()
    {
        Debug.Log("Turno IA!");
    }
}*/
