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

    public void Start()
    {
#if !(DEVELOPMENT_BUILD || UNITY_EDITOR)
        Debug.unityLogger.logEnabled = false; 
#endif
        StartCoroutine(WaitForBankLoading());
    }

    private IEnumerator WaitForBankLoading()
    {
        yield return new WaitForSecondsRealtime(5f);
        yield return new WaitUntil(() => FMODUnity.RuntimeManager.HasBanksLoaded);
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
}
