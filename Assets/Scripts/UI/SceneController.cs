using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    [SerializeField] private Image transitionImage;
    [SerializeField] private float transitionDuration = 1f;
    
    private bool isTransitioning = false;
    private bool isStarting = true;
    private string nextLevel;
    private float timer = 0f;
    
    private Vector3 initialScale = Vector3.one; 
    private Vector3 targetScale = Vector3.zero; 
    private void Start()
    {
        transitionImage.rectTransform.localScale = initialScale;
    }

    public void NextLevel(string level)
    {
        transitionImage.gameObject.SetActive(true);
        isTransitioning = true;
        nextLevel = level; 
        timer = 0f;     
    }
    private void Update()
    {
        if (isStarting)
        {
            timer += Time.deltaTime;
            float progresStart = Mathf.Clamp01(timer / transitionDuration);

            // Reduce la escala de la imagen de (1,1,1) a (0,0,0)
            transitionImage.rectTransform.localScale = Vector3.Lerp(initialScale, targetScale, progresStart);

            if (progresStart >= 1f)
            {
                isStarting = false;
                transitionImage.gameObject.SetActive(false);  // Desactiva la imagen al finalizar
                timer = 0f;  // Reinicia el timer
            }
        }

        if (isTransitioning)
        {
            timer += Time.deltaTime;
            float progresEnd = Mathf.Clamp01(timer / transitionDuration);

            // Aumenta la escala de la imagen de (0,0,0) a (1,1,1)
            transitionImage.rectTransform.localScale = Vector3.Lerp(targetScale, initialScale, progresEnd);
            if (progresEnd >= 1f)
            {
                LoadLevel();  // Carga el siguiente nivel al completar la animación
            }
        }
    }
    private void LoadLevel()
    {
        SceneManager.LoadScene(nextLevel);
    }
}
