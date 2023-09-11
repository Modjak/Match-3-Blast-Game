using System.Collections.Generic;
using TMPro;
using UnityEngine;



public class TopUIManager : MonoBehaviour
{
    [SerializeField] private GameObject[] colorPieces;
    [SerializeField] private TextMeshProUGUI moveCount;

    private Dictionary<string, int> colorIndexDict = new Dictionary<string, int>
    {
        {"BLUE",0 },
        {"GREEN",1 },
        {"PINK",2 },
        {"PURPLE",3},
        {"RED",4 },
        {"YELLOW" , 5 },
    };

    private void Awake()
    {
        DeleteNullColors();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void DeleteNullColors()
    {
        for (int i = 0; i < 6; i++)
        {
            if (StaticParameters.K[i] == false)
            {
                Destroy(colorPieces[i]);
            }

        }
    }

    public void IncreaseColorCount(int count, string color )
    {

        if (color.EndsWith("_A") || color.EndsWith("_B") || color.EndsWith("_C"))
        {
            color = color.Substring(0, color.Length - 2);
        }

        TextMeshProUGUI colorTextMesh = colorPieces[colorIndexDict[color]].transform.Find("Text").GetComponent<TextMeshProUGUI>();
        
        int currentCount = int.Parse(colorTextMesh.text);

        currentCount += count;

        colorTextMesh.text = currentCount.ToString();
    }

    public void IncreaseMoveCount()
    {
         moveCount.text = (int.Parse(moveCount.text) + 1).ToString();
    }



}
