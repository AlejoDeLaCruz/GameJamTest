using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText; // Referencia al TextMeshPro para mostrar el tiempo.
    public float currentTime = 0f; // Tiempo restante.
    private bool isTimerRunning = true; // Controla si el reloj está corriendo.

    // Actualiza el reloj cada frame.
    private void Update()
    {
        if (isTimerRunning)
        {
            currentTime -= Time.deltaTime; // Resta el tiempo cada frame.

            // Si el tiempo llega a 0, lo mantenemos en 0.
            if (currentTime < 0f)
            {
                currentTime = 0f;
            }

            // Mostrar el tiempo en formato 00:00
            timerText.text = Mathf.Floor(currentTime / 60).ToString("00") + ":" + Mathf.Floor(currentTime % 60).ToString("00");
        }
    }

    // Método para aumentar el tiempo del reloj.
    public void AddTime(float seconds)
    {
        currentTime += seconds; // Aumenta el tiempo en segundos.
    }

    // Método para iniciar el reloj con un tiempo determinado.
    public void StartTimer(float startTime)
    {
        currentTime = startTime;
    }

    // Método para detener el reloj.
    public void StopTimer()
    {
        isTimerRunning = false;
    }
}
