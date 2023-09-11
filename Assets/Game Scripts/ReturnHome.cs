using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnHome : MonoBehaviour
{
    [SerializeField] private GameObject returnPanel;
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject gridGameObject;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LevelSelectionMenuClick()
    {
        gridGameObject.SetActive(false);
        canvas.sortingOrder = 120;
        returnPanel.SetActive(true);
        
    }

    public void NoClick()
    {
        gridGameObject.SetActive(true);
        canvas.sortingOrder = -20;
        returnPanel.SetActive(false);

    }
    public void YesClick()
    {
        ResetStaticFields();
        SceneManager.LoadScene("LevelSelectionScene");
    }


    private void ResetStaticFields()
    {   
        StaticParameters.M = 2;
        StaticParameters.N = 2;
        StaticParameters.A = 2;
        StaticParameters.B = 2;
        StaticParameters.C = 2;

        for(int i = 0; i < 6; i++)
        {
            StaticParameters.K[i] = false;
        }
    }

}
