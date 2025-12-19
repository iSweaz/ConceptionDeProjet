using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MB_Start : MonoBehaviour
{
    public Button create,join;
    void Start()
    {
        create.gameObject.SetActive(false);
        join.gameObject.SetActive(false);
        if(C_NetworkUtils.IsServerRunning(C_NetworkUtils.GetLocalIP(),5001))
            join.gameObject.SetActive(true);
        else
            create.gameObject.SetActive(true);
    }

    public void OnJoinCliked()
    {
        SceneManager.LoadScene("Test_Multi");
    }

    public void OnCreateCliked()
    {
        SceneManager.LoadScene("Menu");
    }
}
