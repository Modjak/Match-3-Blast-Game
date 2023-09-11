using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ColorPiece;
using Random = UnityEngine.Random;

public class GameGrid : MonoBehaviour
{
    public enum PieceType // type of block pieces
    {   
        NORMAL,
        EMPTY,
        COUNT,
    };

    [System.Serializable]

    public struct piecePrefab //helper struct to hold type and prefab
    {
        public PieceType type;
        public GameObject prefab;
    };

    public piecePrefab[] piecePrefabs; // In unity editor we add prefabs to this array

    private Dictionary<PieceType, GameObject> piecePrefabDict; // for faster access we use dict 
    public GameObject backgroundPrefab;
    private GamePiece[,] pieces;
    private int xDim;
    private int yDim;
    public float fillTime;
    public bool mouseClickAllowed;

    private int[] dx = { -1, 1, 0, 0 };
    private int[] dy = { 0, 0, -1, 1 };

    private int A, B, C;
    private int[] colorPool;

    [SerializeField]
    private Camera _cam;

    [SerializeField]
    private GameObject topUIManagerObject;
    private TopUIManager topUIManager;

    // Start is called before the first frame update

    private void Awake()
    {
        topUIManager = topUIManagerObject.GetComponent<TopUIManager>();
        LevelSetUp(); 
    }

    void Start()
    {
        SetCamera();
        pieces = new GamePiece[xDim, yDim];
        piecePrefabDict = new Dictionary<PieceType, GameObject>();

        for (int i = 0; i < piecePrefabs.Length; i++)
        {
            if (!piecePrefabDict.ContainsKey(piecePrefabs[i].type))
            {
                piecePrefabDict.Add(piecePrefabs[i].type, piecePrefabs[i].prefab);
            }
        }

        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                GameObject background = Instantiate(backgroundPrefab, new Vector3(x, y, 0), Quaternion.identity);
                background.transform.parent = transform;
            }
        }

        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                SpawnNewPiece(x, y, PieceType.EMPTY);
            }
        }

        StartCoroutine(Fill());
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void LevelSetUp()
    {
        xDim = StaticParameters.N;
        yDim = StaticParameters.M;
        A = StaticParameters.A;
        B = StaticParameters.B;
        C = StaticParameters.C;

        int count = 0;
        for (int i = 0; i < 6; i++)
        {
            if (StaticParameters.K[i] == true)
            {
                count += 1;
            }
        }
        colorPool = new int[count];
        int j = 0;
        for (int i = 0; i < 6; i++)
        {
            if (StaticParameters.K[i] == true)
            {
                colorPool[j++] = i;
            }
        }
    }

    private ColorType getRandomColor()
    {
        int index = Random.Range(0, colorPool.Length);
        return (ColorPiece.ColorType)colorPool[index];
    }


    public IEnumerator Fill()
    {
        mouseClickAllowed = false;
        while (FillStep())
        {
            yield return new WaitForSeconds(fillTime);
        }
        yield return new WaitForSeconds(fillTime);

        while(DetecetDeadlock())
        {
            yield return new WaitForSeconds(1);
            ShuffleBoard();
        }
        ResetAllHints();

        DetecetHints();

        mouseClickAllowed = true;

    }


    public bool FillStep()
    {
        bool movedPiece = false;

        for(int y = 1; y < yDim; y++)
        {
            for(int x = 0; x < xDim; x++)
            {
                GamePiece piece = pieces[x, y];

                if (piece.IsMovable())
                {
                    GamePiece pieceBelow = pieces[x, y - 1];

                    if(pieceBelow.Type == PieceType.EMPTY)
                    {
                        Destroy(pieceBelow.gameObject); 
                        piece.MovableComponent.Move(x, y - 1, fillTime);
                        pieces[x, y - 1] = piece;
                        SpawnNewPiece(x, y, PieceType.EMPTY);
                        movedPiece = true;

                    }
                }
            }
        }

        for(int x = 0; x < xDim; x++)
        {
            GamePiece pieceBelow = pieces[x, yDim - 1];

            if(pieceBelow.Type == PieceType.EMPTY)
            {
                Destroy(pieceBelow.gameObject);
                GameObject newPiece = Instantiate(piecePrefabDict[PieceType.NORMAL], new Vector3(x, yDim+1), Quaternion.identity);
                newPiece.transform.parent = transform;

                pieces[x, yDim - 1] = newPiece.GetComponent<GamePiece>();
                pieces[x, yDim - 1].Init(x, yDim+1, this,PieceType.NORMAL);
                pieces[x, yDim - 1].MovableComponent.Move(x, yDim - 1,fillTime);
                //pieces[x, yDim - 1].ColorComponent.SetColor((ColorPiece.ColorType)Random.Range(0,K));
                pieces[x, yDim - 1].ColorComponent.SetColor(getRandomColor());
                pieces[x, yDim - 1].name = pieces[x, yDim - 1].ColorComponent.Color.ToString();
                movedPiece = true;
            }
        }

        return movedPiece;
    }


    private void SetCamera()
    {
        _cam.orthographicSize = yDim * 0.9f;
        _cam.transform.position = new Vector3((float)xDim / 2 - 0.5f, (float)yDim / 2 - 0.5f, -10);
    }

    public GamePiece SpawnNewPiece(int x, int y, PieceType type)
    {
        GameObject newPiece = Instantiate(piecePrefabDict[type], new Vector3(x, y, 0), Quaternion.identity);
        newPiece.transform.parent = transform;
        pieces[x, y] = newPiece.GetComponent<GamePiece>();
        pieces[x, y].Init(x, y, this, type);
        return pieces[x, y];
    }

    public List<GamePiece> BFS(GamePiece piece)
    {
        ColorPiece.ColorType targetColor = piece.ColorComponent.Color;
        HashSet<GamePiece> visited = new HashSet<GamePiece>();
        List<GamePiece> adjacentPieces = new List<GamePiece>(); // For return purpose
        Queue<GamePiece> queue = new Queue<GamePiece>();
        adjacentPieces.Add(piece);
        queue.Enqueue(piece);
        visited.Add(piece);
        GamePiece currentPiece;

        while (queue.Count > 0)
        {
            currentPiece = queue.Dequeue();
            int currentX = currentPiece.X;
            int currentY = currentPiece.Y;

            // Check neighboring cells
            for (int i = 0; i < 4; i++)
            {
                int newX = currentX + dx[i];
                int newY = currentY + dy[i];

                if(!isValidPiece(newX, newY))
                {
                    continue;
                }
                currentPiece = pieces[newX, newY];

                if (!visited.Contains(currentPiece) && pieces[newX, newY].ColorComponent.Color == targetColor)
                {

                    adjacentPieces.Add(pieces[newX,newY]);
                    queue.Enqueue(pieces[newX, newY]);
                    visited.Add(currentPiece);
                }
            }
        }
        return adjacentPieces;
    }

    public bool isValidPiece(int x, int y)
    {
        return x >= 0 && x < xDim && y >= 0 && y < yDim;
    }
    public void ClearPiece(int x , int y)
    {
        if (pieces[x,y].IsClearable() && !pieces[x, y].ClearableComponent.IsBeingCleard)
        {
            pieces[x, y].ClearableComponent.Clear();
            SpawnNewPiece(x, y, PieceType.EMPTY);
        }
    }

    public IEnumerator ClearPieceWithHint(int x, int y, int clickX, int clickY)
    {
        if (pieces[x, y].IsClearable() && !pieces[x, y].ClearableComponent.IsBeingCleard)
        {
            yield return pieces[x, y].ClearableComponent.playPuffAnimation();
            pieces[x, y].MovableComponent.Move(clickX, clickY, 0.3f);
            yield return new WaitForSeconds(0.3f);
            pieces[x, y].ClearableComponent.ClearWithHint();
            SpawnNewPiece(x, y, PieceType.EMPTY);
        }
    }

    public void ClearAllMatchedPieces(List<GamePiece> matchedPieces,int clickX, int clickY)
    {
        topUIManager.IncreaseMoveCount();
        topUIManager.IncreaseColorCount(matchedPieces.Count, matchedPieces[0].ColorComponent.Color.ToString());
        StartCoroutine(ClearAndFill(matchedPieces,clickX,clickY));
    }


    private IEnumerator ClearAndFill(List<GamePiece> matchedPieces, int clickX, int clickY)
    {
        mouseClickAllowed = false;

        if(matchedPieces.Count > A)
        {
            for (int i = 0; i < matchedPieces.Count; i++)
            {
                StartCoroutine(ClearPieceWithHint(matchedPieces[i].X, matchedPieces[i].Y, clickX, clickY));
            }
        }
        else
        {
            for (int i = 0; i < matchedPieces.Count; i++)
            {
                ClearPiece(matchedPieces[i].X, matchedPieces[i].Y);
            }
        }


        yield return new WaitForSeconds(0.6f); 

        yield return StartCoroutine(Fill());                                           
    }


    private bool DetecetDeadlock()
    {
        for(int x = 0; x < xDim; x++)
        {
            for(int y = 0; y < yDim; y++)
            {
                if (HasSameColorAdjacent(x, y))
                {
                    return false;
                }
            }
        }
        return true;
    }

    private bool HasSameColorAdjacent(int x, int y)
    {
        for (int i = 0; i < 4; i++)
        {
            int newX = x + dx[i];
            int newY = y + dy[i];

            if (!isValidPiece(newX, newY))
            {
                continue;
            }
            if (pieces[x, y].ColorComponent.Color == pieces[newX, newY].ColorComponent.Color)
            {
                return true;
            }
        }
        return false;
    }

    private void Shuffle()
    {
        List<GamePiece> allPieces = new List<GamePiece>();
        
        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                allPieces.Add(pieces[x, y]);
            }
        }

        // Use Fisher-Yates shuffle to shuffle the list.
        System.Random random = new System.Random();
        int n = allPieces.Count;
        for (int i = n - 1; i > 0; i--)
        {
            int j = random.Next(0, i + 1);
            GamePiece temp = allPieces[i];
            allPieces[i] = allPieces[j];
            allPieces[j] = temp;
        }

        // Update the pieces array with the shuffled pieces.
        int index = 0;
        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                pieces[x, y] = allPieces[index];
                pieces[x, y].MovableComponent.Move(x, y, fillTime);
                index++;
            }
        }
    }

    private void ShuffleBoard()
    {
        if (SameColorObject())
        {
            Shuffle();
        }
        else
        {
            ColorPiece.ColorType colorUsedToChange = pieces[0, 0].ColorComponent.Color;
            int randX = Random.Range(1, xDim);
            int randY = Random.Range(1, yDim);
            pieces[randX, randY].ColorComponent.SetColor(colorUsedToChange);
            Shuffle();
;        }
    }


    private bool SameColorObject()
    {
        //Dictionary<ColorPiece.ColorType, int> colorCountDict = new Dictionary<ColorPiece.ColorType, int>();

        List<ColorPiece.ColorType> colorCountList = new List<ColorPiece.ColorType>();
        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                if (!colorCountList.Contains(pieces[x, y].ColorComponent.Color))
                {
                    colorCountList.Add(pieces[x, y].ColorComponent.Color); 
                }
                else
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void DetecetHints()
    {
        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                if(pieces[x, y].HintActicated)
                {
                    continue;
                }
                var adjacentPieces = BFS(pieces[x, y]);
                int count = adjacentPieces.Count;
                ColorType strToColorType;
                string colorBefore = adjacentPieces[0].ColorComponent.Color.ToString();

                if (count > C)
                {
                    strToColorType = (ColorType) Enum.Parse(typeof(ColorType), colorBefore + "_C");
                }
                else if(count > B)
                {
                    strToColorType = (ColorType)Enum.Parse(typeof(ColorType), colorBefore + "_B");
                }
                else if(count > A)
                {
                    strToColorType = (ColorType)Enum.Parse(typeof(ColorType), colorBefore + "_A");
                }
                else
                {
                    continue;
                }

                ChangeHintColor(adjacentPieces, count, strToColorType);

            }
        }
    }

    
    private void ChangeHintColor(List<GamePiece> hintedPieces,int count,ColorPiece.ColorType hintColor)
    {
        for(int i = 0; i < count; i++){
            pieces[hintedPieces[i].X, hintedPieces[i].Y].ColorComponent.Color = hintColor;
            pieces[hintedPieces[i].X, hintedPieces[i].Y].HintActicated = true;
        }
    }

    private void ResetAllHints()
    {
        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                if (pieces[x, y].HintActicated)
                {
                    pieces[x, y].HintActicated = false;
                    string colorBefore = pieces[x, y].ColorComponent.Color.ToString();
                    pieces[x, y].ColorComponent.Color = (ColorType)Enum.Parse(typeof(ColorType), colorBefore.Substring(0, colorBefore.Length - 2));
                }

            }
        }
    }


}
    
