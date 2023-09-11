using System.Collections.Generic;
using UnityEngine;

public class ColorPiece : MonoBehaviour
{
    public enum ColorType
    {
        BLUE,
        GREEN,
        PINK,
        PURPLE,
        RED,
        YELLOW,
        BLUE_A,
        BLUE_B,
        BLUE_C,
        GREEN_A,
        GREEN_B,
        GREEN_C,
        PINK_A,
        PINK_B,
        PINK_C,
        PURPLE_A,
        PURPLE_B,
        PURPLE_C,
        RED_A,
        RED_B,
        RED_C,
        YELLOW_A,
        YELLOW_B,
        YELLOW_C,
        COUNT
    };

    public int NumColors
    {
        get { return colorSprites.Length; }
    }
    [System.Serializable]
    public struct ColorSprite
    {
        public ColorType color;
        public Sprite sprite;
    };

    public ColorSprite[] colorSprites;

    private ColorType color;

    public ColorType Color
    {
        get { return color; }
        set { SetColor(value); }
    }
    private SpriteRenderer sprite;

    public SpriteRenderer Sprite
    {
        get { return sprite; }
    }

    private Dictionary<ColorType, Sprite> colorSpriteDict; //for faster look up we use dict

    private void Awake()
    {
        sprite = transform.Find("piece").GetComponent<SpriteRenderer>();

        colorSpriteDict = new Dictionary<ColorType, Sprite>();

        for(int i = 0; i < colorSprites.Length; i++)
        {
            if (!colorSpriteDict.ContainsKey(colorSprites[i].color))
            {
                colorSpriteDict.Add(colorSprites[i].color , colorSprites[i].sprite);
            }
        }



    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetColor(ColorType newColor)
    {
        sprite.sortingOrder = GetComponent<GamePiece>().Y;
        color = newColor;
        if (colorSpriteDict.ContainsKey(newColor))
        {
            sprite.sprite = colorSpriteDict[newColor];
        }
    }
}
