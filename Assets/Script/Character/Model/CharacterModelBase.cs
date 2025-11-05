using UnityEngine;

public class CharacterModelBase : MonoBehaviour
{
    protected MapCharacterBase applyCharacter = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Awake()
    {

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {

    }

    // Update is called once per frame
    protected virtual void Update()
    {

    }

    public virtual void Init(MapCharacterBase characterBase)
    {
        applyCharacter = characterBase;
    }
}
