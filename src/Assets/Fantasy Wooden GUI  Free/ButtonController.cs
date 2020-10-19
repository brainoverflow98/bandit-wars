using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    public void pressStartButton()
    {
        SceneManager.LoadScene("CustomizationMenu");
    }
    


}
