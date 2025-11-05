using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TileCharacterDirectionSprites
{
    public Sprite sprite1;
    public Sprite sprite2;
    public Sprite sprite3;
}

[Serializable]
public class TileCharacterSpitesheet
{
    public TileCharacterDirectionSprites down;
    public TileCharacterDirectionSprites right;
    public TileCharacterDirectionSprites up;
}

[Serializable]
public class TileCharacter
{
    public string name;
    public TileCharacterSpitesheet spitesheet;
}

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteModel : CharacterModelBase
{
    public SpriteRenderer spriteRenderer => GetComponent<SpriteRenderer>();

    [Tooltip("至少要有一個name為空字串的作為預設")]
    [SerializeField] private List<TileCharacter> characters = new List<TileCharacter>();

    private float intervalTime => moveModel.IsRunning ? 0.25f / 2 : 0.25f;

    private Dictionary<string, TileCharacterSpitesheet> characterMap = new Dictionary<string, TileCharacterSpitesheet>();
    private MoveModel moveModel;
    private bool preIsMoving = false;
    private string useSpritesheet = "";
    /// <summary>
    /// 播放序號，範圍為0~3
    /// 0: sprite1
    /// 1: sprite2
    /// 2: sprite1
    /// 3: sprite3
    /// </summary>
    private int index = 0;
    private float timer;

    protected override void Awake()
    {
        base.Awake();

        characters.ForEach(character =>
        {
            if (!characterMap.ContainsKey(character.name))
            {
                characterMap.Add(character.name, character.spitesheet);
            }
        });
    }

    protected override void Update()
    {
        base.Update();

        if (moveModel.IsMoving)
        {
            if (Time.time - timer > intervalTime)
            {
                index++;
                timer = Time.time;
                if (index > 3) index = 0;
                onDirectionChanged(moveModel.Direction);
            }
        }
        else
        {
            if (preIsMoving != false)
            {
                resetSprite();
            }
        }
        preIsMoving = moveModel.IsMoving;
    }

    public void Init(MapCharacterBase characterBase, MoveModel _moveModel)
    {
        base.Init(characterBase);
        moveModel = _moveModel;

        timer = Time.time;

        moveModel.onDirectionChanged.AddListener(onDirectionChanged);
        onDirectionChanged(moveModel.Direction);
    }

    public void SetSpritesheet(string newName)
    {
        useSpritesheet = newName;
    }

    private void resetSprite()
    {
        spriteRenderer.sprite = getDefaultSprite(moveModel.Direction);
        if (moveModel.Direction == MoveDirect.Left)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = Vector3.one;
        }
    }

    private void onDirectionChanged(MoveDirect direct)
    {
        spriteRenderer.sprite = getSpriteFromDirection(direct);
        if (direct == MoveDirect.Left)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = Vector3.one;
        }
    }

    private Sprite getSpriteFromDirection(MoveDirect direct)
    {
        if (direct == MoveDirect.Left || direct == MoveDirect.Right)
        {
            if (index == 0 | index == 2) return characterMap[useSpritesheet].right.sprite1;
            if (index == 1) return characterMap[useSpritesheet].right.sprite2;
            if (index == 3) return characterMap[useSpritesheet].right.sprite3;
        }
        else if (direct == MoveDirect.Up)
        {
            if (index == 0 | index == 2) return characterMap[useSpritesheet].up.sprite1;
            if (index == 1) return characterMap[useSpritesheet].up.sprite2;
            if (index == 3) return characterMap[useSpritesheet].up.sprite3;
        }
        else if (direct == MoveDirect.Down)
        {
            if (index == 0 | index == 2) return characterMap[useSpritesheet].down.sprite1;
            if (index == 1) return characterMap[useSpritesheet].down.sprite2;
            if (index == 3) return characterMap[useSpritesheet].down.sprite3;
        }
        return null;
    }

    private Sprite getDefaultSprite(MoveDirect direct)
    {
        if (direct == MoveDirect.Left || direct == MoveDirect.Right)
        {
            return characterMap[useSpritesheet].right.sprite1;
        }
        else if (direct == MoveDirect.Up)
        {
            return characterMap[useSpritesheet].up.sprite1;
        }
        else if (direct == MoveDirect.Down)
        {
            return characterMap[useSpritesheet].down.sprite1;
        }
        return null;
    }

}
