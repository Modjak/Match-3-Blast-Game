using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class GamePiece : MonoBehaviour
{
    private int x;
    public int X
    {
        get { return x; }
        set
        {
            if (IsMovable())
            {
                x = value;
            }
        }
    }

    private int y;
    public int Y
    {
        get { return y; }
        set
        {
            if (IsMovable())
            {
                y = value;
            }
        }
    }

    private GameGrid.PieceType type;
    public GameGrid.PieceType Type
    {
        get { return type; }
    }

    private GameGrid grid;
    public GameGrid GridRef
    {
        get { return grid; }
    }

    private MovablePiece movableComponent;
    public MovablePiece MovableComponent
    {
        get { return movableComponent; }
    }

    private ColorPiece colorComponent;
    public ColorPiece ColorComponent
    {
        get { return colorComponent; }
    }

    private ClearablePiece clearableComponent;
    public ClearablePiece ClearableComponent
    {
        get { return clearableComponent; }
    }

    private bool hintActicated;
    public bool HintActicated
    {
        get { return hintActicated; }
        set { hintActicated = value; }
    }

    private void Awake()
    {
        movableComponent = GetComponent<MovablePiece>();
        colorComponent = GetComponent<ColorPiece>();
        clearableComponent = GetComponent<ClearablePiece>();
    }
    public void Init(int _x,  int _y,  GameGrid _grid , GameGrid.PieceType _type)
    {
        x = _x;
        y = _y;
        grid = _grid;
        type = _type;

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnMouseDown()
    {
        if (grid.mouseClickAllowed)
        {
            var matchedPieces = grid.BFS(this);
            if(matchedPieces.Count > 1)
            {
                grid.ClearAllMatchedPieces(matchedPieces,x,y);
            }
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public bool IsMovable()
    {
        return movableComponent != null;
    }

    public bool IsColored()
    {
        return colorComponent != null;
    }
    public bool IsClearable()
    {
        return clearableComponent != null;
    }
}
