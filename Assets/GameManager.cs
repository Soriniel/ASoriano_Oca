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
    public GameObject[] fichas;
    private int[] posiciones = { 0, 0 };


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

        AplicarColoresCasillas();

    }

    private void AplicarColoresCasillas()
    {
        for (int i = 0; i < vectorObjetos.Length; i++)
        {
            if (vectorObjetos[i] != null)
            {
                CambiarColorCasilla(vectorObjetos[i], infoCasillas[i]);
            }
        }
    }

    private void CambiarColorCasilla(GameObject casilla, int tipoCasilla)
    {
        Renderer renderer = casilla.GetComponent<Renderer>();

        if (renderer != null)
        {
            switch (tipoCasilla)
            {
                case 1: // Teleport (Azul)
                    renderer.material.color = Color.blue;
                    break;
                case 2: // Volver a tirar (Verde)
                    renderer.material.color = Color.green;
                    break;
                case -1: // Retroceder (Rojo)
                    renderer.material.color = Color.red;
                    break;
                case 99: // Victoria (Amarillo)
                    renderer.material.color = Color.yellow;
                    break;
                default: // Casilla normal (Blanco)
                    renderer.material.color = Color.white;
                    break;
            }
        }
    }
    public void Start()
    {
        textoTurno.text = "Turno Jugador!";
        botonTirarDado.onClick.AddListener(() => StartCoroutine(TurnoJugador()));
    }

    IEnumerator TurnoJugador()
    {
        if (turnoActual == 0) // Turno del jugador
        {
            textoTurno.text = "Turno Jugador!";
            int resultado = Random.Range(1, 7);
            textoDado.text = "Dado: " + resultado;

            yield return new WaitForSeconds(1); // Pausa antes de mover
            MoverFicha(turnoActual, resultado);

            yield return new WaitForSeconds(1); // Pausa antes de cambiar de turno
            turnoActual = 1; // Cambia a la IA
            StartCoroutine(TurnoIA());
        }
    }

    IEnumerator TurnoIA()
    {
        textoTurno.text = "Turno IA!";
        int resultadoIA = Random.Range(1, 7);
        textoDado.text = "IA sacó: " + resultadoIA;

        yield return new WaitForSeconds(1); // Pausa antes de mover
        MoverFicha(turnoActual, resultadoIA);

        yield return new WaitForSeconds(1); // Pausa antes de cambiar de turno
        turnoActual = 0; // Vuelve el turno al jugador
        textoTurno.text = "Turno Jugador!";
    }

    void MoverFicha(int jugador, int pasos)
    {
        int posicionAnterior = posiciones[jugador];
        int nuevaPos = Mathf.Min(posiciones[jugador] + pasos, vectorObjetos.Length - 1);
        posiciones[jugador] = nuevaPos;

        // Actualizar vectorCasillas
        vectorCasillas[posicionAnterior] = 0; // La casilla anterior queda vacía
        vectorCasillas[nuevaPos] = (jugador == 0) ? 1 : 2; // 1 para jugador, 2 para IA

        // Mover instantáneamente la ficha a la casilla correspondiente
        fichas[jugador].transform.position = vectorObjetos[nuevaPos].transform.position;

        // Aplicar el modificador de la casilla
        AplicarModificador(jugador);
    }

    void AplicarModificador(int jugador)
    {
        int posActual = posiciones[jugador];
        int tipoCasilla = infoCasillas[posActual];

        switch (tipoCasilla)
        {
            case 1: // Casilla teleport
                posiciones[jugador] = EncontrarSiguienteTeleport(posActual);
                fichas[jugador].transform.position = vectorObjetos[posiciones[jugador]].transform.position;
                break;

            case 2: // Casilla de volver a tirar
                StartCoroutine(TurnoExtra(jugador));
                break;

            case -1: // Casilla fallo (retrocede 3 casillas)
                posiciones[jugador] = Mathf.Max(posActual - 3, 0);
                fichas[jugador].transform.position = vectorObjetos[posiciones[jugador]].transform.position;
                break;

            case 99: // Casilla victoria
                textoTurno.text = (jugador == 0) ? "¡Jugador ha ganado!" : "¡IA ha ganado!";
                botonTirarDado.interactable = false;
                break;
        }
    }

    int EncontrarSiguienteTeleport(int posActual)
    {
        for (int i = posActual + 1; i < infoCasillas.Length; i++)
        {
            if (infoCasillas[i] == 1) return i;
        }
        return posActual; // Si no hay otro teleport, se queda en la misma casilla
    }

    IEnumerator TurnoExtra(int jugador)
    {
        yield return new WaitForSeconds(1);
        int resultado = Random.Range(1, 7);
        textoDado.text = "Extra: " + resultado;
        yield return new WaitForSeconds(1);
        MoverFicha(jugador, resultado);
    }
}