using Assets.Scripts.GameLogics;
using Assets.Scripts.Menu.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SituationsTable : MonoBehaviour
{
    [Header("Achieved length")]
    [SerializeField]
    private InputField LengthField;
    [SerializeField]
    private Dropdown AchievedLengthDropdown;
    [SerializeField]
    private GameObject SnakeNameRowPrefab;
    private Dictionary<string, Toggle> Toggles;
    private const float ROW_HEIGHT = 60;

    [Header("Collision with barrier")]
    [SerializeField]
    private Dropdown CollisionWithBarrierDropdown;

    [Header("Collision with food")]
    [SerializeField]
    private Dropdown CollisionWithFoodDropdown;

    [Header("Collision with snake")]
    [SerializeField]
    private Dropdown CollisionWithSnakeDropdown;

    [Header("Did steps without food")]
    [SerializeField]
    private InputField StepsField;
    [SerializeField]
    private Dropdown DidStepsWithoutFoodDropdown;

    void Start()
    {
        CreateCheckboxes();
        SetUpValues();
    }

    #region SetUp

    private void CreateCheckboxes()
    {
        var names = new AssemblySnakeFactory().GetAllSnakeTypes();
        SnakeNameRowPrefab.transform.parent.GetComponent<RectTransform>()
            .SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, ROW_HEIGHT * names.Count);
        Toggles = new Dictionary<string, Toggle>();

        foreach (var snakeName in names)
        {
            var newRow = Instantiate(SnakeNameRowPrefab);
            newRow.SetActive(true);
            newRow.transform.parent = SnakeNameRowPrefab.transform.parent;
            newRow.transform.GetChild(1).GetComponent<Text>().text = snakeName;
            string name = snakeName;
            newRow.transform.GetChild(2).GetComponent<Toggle>().isOn = SituationsInit.Instance.Names.Contains(name);
            newRow.transform.GetChild(2).GetComponent<Toggle>().onValueChanged.AddListener(new UnityEngine.Events.UnityAction<bool>
            (
                value =>
                {
                    if (value)
                        SituationsInit.Instance.Names.Add(name);
                    else if (SituationsInit.Instance.Names.Contains(name))
                        SituationsInit.Instance.Names.Remove(name);
                }
            ));
            Toggles.Add(name, newRow.transform.GetChild(2).GetComponent<Toggle>());
        }
    }

    private void SetUpValues()
    {
        LengthField.text = SituationsInit.Instance.Length.ToString();
        AchievedLengthDropdown.value = (int)SituationsInit.Instance.AchievedLength;

        CollisionWithBarrierDropdown.value = (int)SituationsInit.Instance.CollisionWithBarrier;

        CollisionWithFoodDropdown.value = (int)SituationsInit.Instance.CollisionWithFood;

        CollisionWithSnakeDropdown.value = (int)SituationsInit.Instance.CollisionWithSnake;

        StepsField.text = SituationsInit.Instance.Steps.ToString();
        DidStepsWithoutFoodDropdown.value = (int)SituationsInit.Instance.DidStepsWithoutFood;
    }

    #endregion
}
