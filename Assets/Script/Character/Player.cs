using UnityEngine;
using UnityEngine.InputSystem;

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
    public MoveModel MoveModel => moveModel;
    public EventModel EventModel => eventModel;
    public SpriteModel SpriteModel => spriteModel;

    [SerializeField] private MoveModel moveModel = null;
    [SerializeField] private EventModel eventModel = null;
    [SerializeField] private SpriteModel spriteModel = null;

    private Vector2 moveOffset = Vector2.zero;

    void Awake()
    {
        InputController.InputActions.Player.Interact.performed += onClickInteract;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        moveModel.Init(this);
        moveModel.UpdateTargetPosition(transform.position);

        eventModel.Init(this, moveModel);
        spriteModel.Init(this, moveModel);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        moveOffset = InputController.InputActions.Player.Move.ReadValue<Vector2>();

        if (moveOffset.x < 0) moveModel.Move(MoveDirect.Left);
        if (moveOffset.x > 0) moveModel.Move(MoveDirect.Right);
        if (moveOffset.y > 0) moveModel.Move(MoveDirect.Up);
        if (moveOffset.y < 0) moveModel.Move(MoveDirect.Down);

    }

    public override void ApplyMap(MapUnit mapUnit)
    {
        locateMap = mapUnit;
        moveModel.InitTileMap(mapUnit.Maps);
    }

    private void onClickInteract(InputAction.CallbackContext context)
    {
        eventModel.TryTriggerEvent();
    }

}
