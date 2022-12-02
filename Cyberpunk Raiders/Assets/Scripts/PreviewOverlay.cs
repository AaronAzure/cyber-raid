using UnityEngine;
using UnityEngine.SceneManagement;

public class PreviewOverlay : MonoBehaviour
{
	[SerializeField] string sceneName;
	
	
    // Start is called before the first frame update
    void Start()
    {
		if (sceneName != "")
        	SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    }
}
