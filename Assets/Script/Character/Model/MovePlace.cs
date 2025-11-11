using UnityEngine;

public class MovePlace : MonoBehaviour
{
    public MapCharacterBase Character => applyCharacter;

    private MapCharacterBase applyCharacter = null;
    private BoxCollider2D boxCollider2D = null;
    private Rigidbody2D rgd2D = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init(MapCharacterBase characterBase)
    {
        applyCharacter = characterBase;
        boxCollider2D = gameObject.AddComponent<BoxCollider2D>();
        rgd2D = gameObject.AddComponent<Rigidbody2D>();
        rgd2D.bodyType = RigidbodyType2D.Kinematic;
        Disable();
    }

    public void Enable(Vector3 position)
    {
        boxCollider2D.enabled = true;
        transform.position = position;
    }

    public void Disable()
    {
        boxCollider2D.enabled = false;
    }
}
