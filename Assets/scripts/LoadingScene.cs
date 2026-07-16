using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{


    [SerializeField] private float tempoMinimoLoading = 3f;

    private void Start()
    {
        StartCoroutine(LoadAsync());
    }

    private IEnumerator LoadAsync()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(2);
        operation.allowSceneActivation = false;

        // Aguarda o tempo mínimo desejado
        yield return new WaitForSeconds(tempoMinimoLoading);

        // Espera a cena terminar de carregar (90%)
        while (operation.progress < 0.9f)
        {
            yield return null;
        }

        // Ativa a nova cena
        operation.allowSceneActivation = true;
    }
}
