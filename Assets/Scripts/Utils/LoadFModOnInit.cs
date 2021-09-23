using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadFModOnInit : MonoBehaviour
{
    [SerializeField] private string[] phrases;
    [SerializeField] private TMPro.TextMeshProUGUI textMesh;

    private void Awake()
    {
        string phrase = phrases[Random.Range(0, phrases.Length)];
        textMesh.SetText(phrase);
    }

    public IEnumerator Start()
    {
        yield return new WaitForSeconds(2f);
        yield return new WaitUntil(() => FMODUnity.RuntimeManager.HasBanksLoaded);
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
}
