using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadFModOnInit : MonoBehaviour
{
    public IEnumerator Start()
    {
        yield return new WaitUntil(() => FMODUnity.RuntimeManager.HasBankLoaded("Master"));
        
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
}
