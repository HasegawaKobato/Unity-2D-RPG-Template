using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DialogCharacter : MonoBehaviour
{
    public SpriteRenderer spriteRenderer => GetComponent<SpriteRenderer>();

    private bool isFadeIn = false;
    private bool isFadeOut = false;
    private bool isMoving = false;
    private bool isShacking = false;
    private float shackTimer = 0;
    private float shackTime = 0.5f;
    private Color tmpColor = Color.white;
    private Vector3 targetPosition = Vector3.zero;
    private Vector3 tmpPosition = Vector3.zero;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isFadeIn)
        {
            tmpColor.a += Time.deltaTime;
            spriteRenderer.color = tmpColor;
            if (tmpColor.a >= 255)
            {
                isFadeIn = false;
            }
        }

        if (isFadeOut)
        {
            tmpColor.a -= Time.deltaTime;
            spriteRenderer.color = tmpColor;
            if (tmpColor.a <= 0)
            {
                isFadeOut = false;
                gameObject.SetActive(false);
            }
        }

        if (isMoving)
        {
            transform.Translate((targetPosition - transform.position) * Time.deltaTime * 10);
            if (Vector3.Distance(transform.position, targetPosition) <= Time.deltaTime)
            {
                transform.position = targetPosition;
                isMoving = false;
            }
        }

        if (isShacking)
        {
            transform.position = tmpPosition + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
            if (Time.time - shackTimer >= shackTime)
            {
                transform.position = tmpPosition;
                isShacking = false;
            }
        }
    }

    public void FadeIn(Vector3 _targetPosition)
    {
        targetPosition = _targetPosition;
        transform.position = targetPosition;
        tmpColor.a = 0;
        spriteRenderer.color = tmpColor;
        isFadeIn = true;
        gameObject.SetActive(true);
    }

    public void FadeOut()
    {
        isFadeOut = true;
    }

    public void MoveTo(Vector3 _targetPosition)
    {
        targetPosition = _targetPosition;
        isMoving = true;
    }

    public void Shack(float _shackTime)
    {
        if (isMoving || isShacking) return;
        shackTime = _shackTime;
        tmpPosition = transform.position;
        shackTimer = Time.time;
        isShacking = true;
    }

}
