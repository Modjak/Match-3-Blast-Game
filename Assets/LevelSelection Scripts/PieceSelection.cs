using UnityEngine;
using UnityEngine.UI;

public class PieceSelection : MonoBehaviour
{
    private bool pieceActive = false;
    public Sprite spriteOn;
    public Sprite spriteOff;
    public Image sourceImage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    public void changeSprite()
    {
        string color = transform.parent.name;
        if(color == "Blue")
        {
            StaticParameters.K[0] = !StaticParameters.K[0];
        }
        else if(color == "Green")
        {
            StaticParameters.K[1] = !StaticParameters.K[1];
        }
        else if (color == "Pink")
        {
            StaticParameters.K[2] = !StaticParameters.K[2];
        }
        else if (color == "Purple")
        {
            StaticParameters.K[3] = !StaticParameters.K[3];
        }
        else if (color == "Red")
        {
            StaticParameters.K[4] = !StaticParameters.K[4];
        }
        else if (color == "Yellow")
        {
            StaticParameters.K[5] = !StaticParameters.K[5];
        }
        
        pieceActive = !pieceActive;
        sourceImage.sprite = pieceActive ? spriteOn : spriteOff;
    }
}
