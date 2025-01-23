using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFade : MonoBehaviour
{
    public Image blackScreen; // Arrastra la imagen negra aquí desde el inspector.
    public float fadeDuration = 1f; // Tiempo para desvanecer la pantalla.
    public float cooldownTime = 5f; // Tiempo de espera antes de poder volver a usar la transición.

    private float currentFadeAlpha = 0f; // Almacena la opacidad actual del fade.
    private bool canFade = true; // Indica si el jugador puede activar la transición.

    private void Start()
    {
        // Asegúrate de que la pantalla esté transparente al inicio.
        SetBlackScreenAlpha(0f);
    }

    // Método para oscurecer y luego mostrar la pantalla nuevamente.
    public void FadeToBlackAndBack(float delay)
    {
        if (canFade)
        {
            StartCoroutine(FadeSequenceWithCooldown(delay));
        }
    }

    // Secuencia de desvanecimiento con cooldown.
    private IEnumerator FadeSequenceWithCooldown(float delay)
    {
        canFade = false; // Bloquea la transición.
        yield return StartCoroutine(FadeSequence(delay)); // Realiza la secuencia de fade.
        yield return new WaitForSeconds(cooldownTime); // Espera antes de permitir la próxima transición.
        canFade = true; // Permite nuevamente la transición.
    }

    // Secuencia de desvanecimiento.
    private IEnumerator FadeSequence(float delay)
    {
        yield return StartCoroutine(FadeIn()); // Pantalla negra.
        yield return new WaitForSeconds(delay); // Espera con la pantalla negra.
        yield return StartCoroutine(FadeOut()); // Vuelve a la normalidad.
    }

    // Método para desvanecer la pantalla a negro.
    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        Color color = blackScreen.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            currentFadeAlpha = elapsedTime / fadeDuration; // Incrementa la opacidad.
            color.a = currentFadeAlpha;
            blackScreen.color = color;
            yield return null;
        }

        currentFadeAlpha = 1f; // Asegúrate de que la pantalla esté completamente negra.
        color.a = currentFadeAlpha;
        blackScreen.color = color;
    }

    // Método para desvanecer la pantalla a transparente.
    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        Color color = blackScreen.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            currentFadeAlpha = 1f - (elapsedTime / fadeDuration); // Reduce la opacidad.
            color.a = currentFadeAlpha;
            blackScreen.color = color;
            yield return null;
        }

        currentFadeAlpha = 0f; // Asegúrate de que la pantalla esté completamente transparente.
        color.a = currentFadeAlpha;
        blackScreen.color = color;
    }

    // Método para configurar la transparencia inicial del color.
    private void SetBlackScreenAlpha(float alpha)
    {
        Color color = blackScreen.color;
        color.a = alpha;
        blackScreen.color = color;
    }

    // Método para verificar si la pantalla está completamente negra.
    public bool IsScreenBlack()
    {
        return currentFadeAlpha == 1f; // La pantalla está negra si la opacidad es 1.
    }
}