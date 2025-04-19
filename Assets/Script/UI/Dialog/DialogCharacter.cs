using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DialogCharacter : MonoBehaviour
{
    private SpriteRenderer spriteRenderer => GetComponent<SpriteRenderer>();

    private bool isFadeIn = false;
    private bool isFadeOut = false;
    private Color tmpColor = Color.white;

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
    }

    public void FadeIn()
    {
        tmpColor.a = 0;
        spriteRenderer.color = tmpColor;
        isFadeIn = true;
        gameObject.SetActive(true);
    }

    public void FadeOut()
    {
        isFadeOut = true;
    }

}
