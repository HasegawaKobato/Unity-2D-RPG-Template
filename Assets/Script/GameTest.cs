using UnityEngine;

public class GameTest : MonoBehaviour
{
    [SerializeField] private BasePanel panelA;
    [SerializeField] private BasePanel panelB;
    [SerializeField] private BasePanel panelC;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            UIController.Add(panelA);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            UIController.Add(panelB);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            UIController.Add(panelC);
        }
    }
}
