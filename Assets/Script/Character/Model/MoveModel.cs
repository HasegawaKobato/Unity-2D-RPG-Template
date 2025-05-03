using UnityEngine;

public class MoveModel : CharacterModelBase
{
    [Tooltip("每步的距離，取絕對值。(0, 0)表示無最小步數，可以隨意移動")]
    [SerializeField] private Vector2 stepSize = Vector2.one;
    [SerializeField] private float moveSpeed = 10;

    /// <summary>
    /// 目標位置
    /// </summary>
    private Vector2 targetPosition = Vector2.zero;

    /// <summary>
    /// 移動方向
    /// </summary>
    private MoveDirect moveDirect = MoveDirect.None;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (moveDirect != MoveDirect.None)
        {
            switch (moveDirect)
            {
                case MoveDirect.Left:
                    applyCharacter.transform.Translate(Vector2.left * Time.deltaTime * moveSpeed);
                    if (applyCharacter.transform.position.x <= targetPosition.x)
                    {
                        applyCharacter.transform.position = targetPosition;
                        moveDirect = MoveDirect.None;
                    }
                    ;
                    break;
                case MoveDirect.Right:
                    applyCharacter.transform.Translate(Vector2.right * Time.deltaTime * moveSpeed);
                    if (applyCharacter.transform.position.x >= targetPosition.x)
                    {
                        applyCharacter.transform.position = targetPosition;
                        moveDirect = MoveDirect.None;
                    }
                    ;
                    break;
                case MoveDirect.Up:
                    applyCharacter.transform.Translate(Vector2.up * Time.deltaTime * moveSpeed);
                    if (applyCharacter.transform.position.y >= targetPosition.y)
                    {
                        applyCharacter.transform.position = targetPosition;
                        moveDirect = MoveDirect.None;
                    }
                    ;
                    break;
                case MoveDirect.Down:
                    applyCharacter.transform.Translate(Vector2.down * Time.deltaTime * moveSpeed);
                    if (applyCharacter.transform.position.y <= targetPosition.y)
                    {
                        applyCharacter.transform.position = targetPosition;
                        moveDirect = MoveDirect.None;
                    }
                    ;
                    break;
            }
        }
    }

    public MoveModel Init(MapCharacterBase characterBase, Vector2 _stepSize)
    {
        base.Init(characterBase);
        stepSize = _stepSize;
        return this;
    }

    public void UpdateTargetPosition(Vector2 _targetPosition)
    {
        targetPosition = _targetPosition;
    }

    public void Move(MoveDirect direct)
    {
        if (moveDirect != MoveDirect.None) return;
        moveDirect = direct;
        switch (direct)
        {
            case MoveDirect.Left:
                targetPosition += new Vector2(-Mathf.Abs(stepSize.x), 0);
                break;
            case MoveDirect.Right:
                targetPosition += new Vector2(Mathf.Abs(stepSize.x), 0);
                break;
            case MoveDirect.Up:
                targetPosition += new Vector2(0, Mathf.Abs(stepSize.y));
                break;
            case MoveDirect.Down:
                targetPosition += new Vector2(0, -Mathf.Abs(stepSize.y));
                break;
        }
    }

}
