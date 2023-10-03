using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UiDeadStats : UIAbstract
{
    public Text textDead;
    public RawImage ImageSeFudeo;
    public float fadeDuration = 60.0f; // Duração em segundos para o texto desaparecer

    public bool fading = false;


    public bool teste;
    void Start()
    {
        textDead.color = new Color(textDead.color.r, textDead.color.g, textDead.color.b, 0);
    }
    public void ActiveText(string name)
    {
        textDead.text = "Player: " + name + " morreu";

        StartCoroutine(FadeText());
    }
    public void ActiveFadeImage()
    {

        StartCoroutine(FadeImage());
    }
    // Update is called once per frame

    IEnumerator FadeText()
    {
        fading = true;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            // Calcula a nova opacidade com base no tempo passado
            float newOpacity = Mathf.Lerp(1, 0f, elapsedTime / fadeDuration);

            // Define a nova cor do texto com a opacidade atualizada
            textDead.color = new Color(textDead.color.r, textDead.color.g, textDead.color.b, newOpacity);

            // Atualiza o tempo decorrido
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // Garante que o texto seja completamente transparente no final
        textDead.color = new Color(textDead.color.r, textDead.color.g, textDead.color.b, 0f);

        fading = false; // Concluído o fading
    }
    IEnumerator FadeImage()
    {
        fading = true;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            // Calcula a nova opacidade com base no tempo passado
            float newOpacity = Mathf.Lerp(1, 0f, elapsedTime / fadeDuration);

            // Define a nova cor do texto com a opacidade atualizada
            ImageSeFudeo.color = new Color(textDead.color.r, textDead.color.g, textDead.color.b, newOpacity);

            // Atualiza o tempo decorrido
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // Garante que o texto seja completamente transparente no final
        textDead.color = new Color(textDead.color.r, textDead.color.g, textDead.color.b, 0f);

        fading = false; // Concluído o fading
    }
}
