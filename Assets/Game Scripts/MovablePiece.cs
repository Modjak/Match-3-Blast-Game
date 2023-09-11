using System.Collections;
using UnityEngine;

public class MovablePiece : MonoBehaviour
{
    private GamePiece piece;
    private IEnumerator moveCoroutine;

    private void Awake()
    {
        piece = GetComponent<GamePiece>();


    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move(int newX, int newY, float time)
    {
        if(moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine = MoveCoroutine(newX, newY, time);
        StartCoroutine(moveCoroutine);

    }

    private IEnumerator MoveCoroutine(int newX, int newY, float time)
    {
        piece.X = newX;
        piece.Y = newY;
        transform.Find("piece").GetComponent<SpriteRenderer>().sortingOrder = newY;

        Vector2 startPos = transform.position;
        Vector2 endPos = new Vector2(newX, newY);

        for( float t = 0; t <= 1 * time; t += Time.deltaTime)
        {
            piece.transform.position = Vector2.Lerp(startPos, endPos, t / time);
            yield return 0;
        }
        piece.transform.position = endPos;
    }
}
