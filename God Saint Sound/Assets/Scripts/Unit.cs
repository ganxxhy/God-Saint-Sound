using UnityEngine;

public class Unit : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UnitSelectionManager.instance.allUnitList.Add(gameObject);
    }

    private void OnDestroy()
    {
        UnitSelectionManager.instance.allUnitList.Remove(gameObject);
    }


}
