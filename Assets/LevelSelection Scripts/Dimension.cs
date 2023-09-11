using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Dimension : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMesh;
    [SerializeField] private Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        slider.onValueChanged.AddListener(UpdateText);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void UpdateText(float value)
    {   
        textMesh.text = ((int)value).ToString();
        if(gameObject.name == "Rows")
        {
            StaticParameters.M = (int)value;
        }
        else if(gameObject.name == "Columns")
        {
            StaticParameters.N = (int)value;
        }
        else if (gameObject.name == "Threshold1")
        {
            StaticParameters.A = (int)value;
        }
        else if (gameObject.name == "Threshold2")
        {
            StaticParameters.B = (int)value;
        }
        else if (gameObject.name == "Threshold3")
        {
            StaticParameters.C = (int)value;
        }

    }

}
