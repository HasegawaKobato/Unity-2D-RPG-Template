using UnityEngine;

public enum MoveDirect
{
    None,
    Left,
    Right,
    Up,
    Down,
}

public class Player : MapCharacterBase
{
    [SerializeField] private MoveModel moveModel = null;

    void Awake()
    {
        moveModel.Init(this);
        moveModel.UpdateTargetPosition(transform.position);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKey(KeyCode.LeftArrow)) moveModel.Move(MoveDirect.Left);
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKey(KeyCode.RightArrow)) moveModel.Move(MoveDirect.Right);
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKey(KeyCode.UpArrow)) moveModel.Move(MoveDirect.Up);
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKey(KeyCode.DownArrow)) moveModel.Move(MoveDirect.Down);

    }

}
