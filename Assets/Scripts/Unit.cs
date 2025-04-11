using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    UnitSelectionManager unitSelectionManager;
    [SerializeField] GameObject unitSelectionIndicator;

    public bool isPlayerUnit = true; // true for player, false for enemy
    // Start is called before the first frame update
    void Start()
    {
        unitSelectionManager = UnitSelectionManager.Instance;
        unitSelectionManager.AddUnit(gameObject);
    }

    private void OnDestroy()
    {
        unitSelectionManager.RemoveUnit(gameObject);
    }

    public void IndicateSelection(bool active)
    {
        unitSelectionIndicator.SetActive(active);
    }
}
