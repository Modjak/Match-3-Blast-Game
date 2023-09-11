using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevelScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPlayButtonClick()
    {
        if(checkForDimension() && checkForColors() && checkForThresholds())
        {
            SceneManager.LoadScene("Gamescene");
        }
    }

    private bool checkForDimension()
    {
        if(StaticParameters.N > StaticParameters.M)
        {
            print("The number of rows must be greater than or equal to the number of columns");
            return false;
        }
        return true;
    }
    private bool checkForThresholds()
    {
        int A = StaticParameters.A;
        int B = StaticParameters.B;
        int C = StaticParameters.C;


        if(A >= B)
        {
            print("The threshold B must be greater than the threshold A");
            return false;

        }
        if (B >= C)
        {
            print("The threshold C must be greater than the threshold B");
            return false;
        }

        return true;
    }
    private bool checkForColors()
    {
        int count = 0;
        for(int i = 0; i < 6; i++)
        {
            if (StaticParameters.K[i] == true)
            {
                count += 1;
            }
        }
        if(count < 2)
        {
            print("You must pick atleast 2 colors");
            return false;

        }
        return true;
    }
}
