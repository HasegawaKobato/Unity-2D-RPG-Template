using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private Transform mapContent;
    [SerializeField] private Player player;

    void Awake()
    {
        CardBattle.Main.Init();
        MapController.InitAllAsync();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputController.InputActions.Player.Enable();
        InputController.InputActions.UI.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // CardBattle.Main.StartBattle();
            activeMap(MapType.Sample);
        }
    }

    private void activeMap(MapType mapType)
    {
        MapUnit mapUnit = MapController.Get(mapType);
        mapUnit.transform.SetParent(mapContent);
        mapUnit.transform.localPosition = Vector3.zero;
        mapUnit.Active(player);
    }
}
