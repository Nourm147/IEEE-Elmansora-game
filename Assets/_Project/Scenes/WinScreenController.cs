using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class WinScreenController : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("«·Êﬁ  »«·ÀÊ«‰Ì ﬁ»· «·⁄Êœ… ··„‘Âœ «·√Ê·")]
    public float waitTime = 15f;

    [Tooltip("—ﬁ„ „‘Âœ «·»œ«Ì… ›Ì «·‹ Build Settings")]
    public int startSceneIndex = 0;

    void Start()
    {
        StartCoroutine(WaitAndReturn());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            ReturnToStartScene();
        }
    }

    IEnumerator WaitAndReturn()
    {
        yield return new WaitForSeconds(waitTime);

        ReturnToStartScene();
    }

    private void ReturnToStartScene()
    {
        SceneManager.LoadScene(startSceneIndex);
    }
}
