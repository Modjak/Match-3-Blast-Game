using System.Collections;
using UnityEngine;

public class ClearablePiece : MonoBehaviour
{
    public AnimationClip clearAnimation;
    public AnimationClip puffAnimation;


    private bool isBeingCleared;
    

    public bool IsBeingCleard
    {
        get { return isBeingCleared; }
    }

    protected GamePiece piece;
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
    public void Clear()
    {
        isBeingCleared = true;
        StartCoroutine(ClearCoroutine());

    }
    public void ClearWithHint()
    {
        isBeingCleared = true;
        Destroy(gameObject);
    }

    private IEnumerator ClearCoroutine()
    {
        Animator animator = GetComponent<Animator>();

        if (animator)
        {
            animator.Play(clearAnimation.name);
            yield return new WaitForSeconds(clearAnimation.length);
            Destroy(gameObject);
        }
    }

    public IEnumerator playPuffAnimation()
    {
        Animator animator = GetComponent<Animator>();

        if (animator)
        {
            animator.Play(puffAnimation.name);
            yield return new WaitForSeconds(puffAnimation.length);
            transform.Find("piece").transform.localScale = new Vector3(1.2f, 1.2f, 0);
        }
    }
}
