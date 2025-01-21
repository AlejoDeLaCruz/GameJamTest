using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFade : MonoBehaviour
{
    public Image blackScreen; // Arrastra la imagen negra aquí desde el inspector.
    public float fadeDuration = 1f; // Tiempo para desvanecer la pantalla.

    private void Start()
    {
        // Asegúrate de que la pantalla esté transparente al inicio.
        SetBlackScreenAlpha(0f);
    }

    // Método para oscurecer y luego mostrar la pantalla nuevamente.
    public void FadeToBlackAndBack(float delay)
    {
        StartCoroutine(FadeSequence(delay));
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
            color.a = elapsedTime / fadeDuration; // Incrementa la opacidad.
            blackScreen.color = color;
            yield return null;
        }

        color.a = 1f; // Asegúrate de que la pantalla esté completamente negra.
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
            color.a = 1f - (elapsedTime / fadeDuration); // Reduce la opacidad.
            blackScreen.color = color;
            yield return null;
        }

        color.a = 0f; // Asegúrate de que la pantalla esté completamente transparente.
        blackScreen.color = color;
    }

    // Método para configurar la transparencia inicial del color.
    private void SetBlackScreenAlpha(float alpha)
    {
        Color color = blackScreen.color;
        color.a = alpha;
        blackScreen.color = color;
    }
}
