using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFade : MonoBehaviour
{
    public Image blackScreen; // Arrastra la imagen negra aqu� desde el inspector.
    public float fadeDuration = 1f; // Tiempo para desvanecer la pantalla.
    public float cooldownTime = 5f; // Tiempo de espera antes de poder volver a usar la transici�n.

    private float currentFadeAlpha = 0f; // Almacena la opacidad actual del fade.
    private bool canFade = true; // Indica si el jugador puede activar la transici�n.

    private void Start()
    {
        // Aseg�rate de que la pantalla est� transparente al inicio.
        SetBlackScreenAlpha(0f);
    }

    // M�todo para oscurecer y luego mostrar la pantalla nuevamente.
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
        canFade = false; // Bloquea la transici�n.
        yield return StartCoroutine(FadeSequence(delay)); // Realiza la secuencia de fade.
        yield return new WaitForSeconds(cooldownTime); // Espera antes de permitir la pr�xima transici�n.
        canFade = true; // Permite nuevamente la transici�n.
    }

    // Secuencia de desvanecimiento.
    private IEnumerator FadeSequence(float delay)
    {
        yield return StartCoroutine(FadeIn()); // Pantalla negra.
        yield return new WaitForSeconds(delay); // Espera con la pantalla negra.
        yield return StartCoroutine(FadeOut()); // Vuelve a la normalidad.
    }

    // M�todo para desvanecer la pantalla a negro.
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

        currentFadeAlpha = 1f; // Aseg�rate de que la pantalla est� completamente negra.
        color.a = currentFadeAlpha;
        blackScreen.color = color;
    }

    // M�todo para desvanecer la pantalla a transparente.
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

        currentFadeAlpha = 0f; // Aseg�rate de que la pantalla est� completamente transparente.
        color.a = currentFadeAlpha;
        blackScreen.color = color;
    }

    // M�todo para configurar la transparencia inicial del color.
    private void SetBlackScreenAlpha(float alpha)
    {
        Color color = blackScreen.color;
        color.a = alpha;
        blackScreen.color = color;
    }

    // M�todo para verificar si la pantalla est� completamente negra.
    public bool IsScreenBlack()
    {
        return currentFadeAlpha == 1f; // La pantalla est� negra si la opacidad es 1.
    }
}