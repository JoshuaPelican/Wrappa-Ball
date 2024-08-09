using UnityEngine;
using UnityEngine.SceneManagement;

public class DestroySceneChange : ActionOnDestroy
{
    [SerializeField] int SceneIndex;

    protected override void DestroyAction()
    {
        SceneManager.LoadScene(SceneIndex);
    }
}
