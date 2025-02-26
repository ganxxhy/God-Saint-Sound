using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

public class UnitSelectionManager : MonoBehaviour
{
    public static UnitSelectionManager instance { get; set; }

    public List<GameObject> allUnitList = new List<GameObject>();
    public List<GameObject> unitsSelected = new List<GameObject>();

    public LayerMask Clickable;
    public LayerMask ground;
    public GameObject groundMarker;
    private Camera cam;

    public void Awake()
    {
        if(instance !=  null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            //If we click on a clickable object
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, Clickable))
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    MultiSelect(hit.collider.gameObject);
                }
                else
                {
                    SelectByClick(hit.collider.gameObject);
                }

            }
            else //if we are not clicking on a clickable object
            {
                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    DeselectAll();
                }

            }
        }


        if (Input.GetMouseButtonDown(1) && unitsSelected.Count > 0)
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            //If we click on a clickable object
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground))
            {
                groundMarker.transform.position = hit.point;
                groundMarker.SetActive(false);
                groundMarker.SetActive(true);
            }
        }
    }

    private void MultiSelect(GameObject unit)
    {
        if(unitsSelected.Contains(unit) == false)
        {
            unitsSelected.Add(unit);
            triggerSelectionIndicator(unit, true);
            EnableUnitMovement(unit, true);
            
        }
        else
        {
            EnableUnitMovement(unit, false);
            triggerSelectionIndicator(unit, false);
            unitsSelected.Remove(unit);
            
        }
    }

    private void DeselectAll()
    {
        foreach (var unit in unitsSelected)
        {
            EnableUnitMovement(unit, false);
            triggerSelectionIndicator(unit, false);
        }
        groundMarker.SetActive(false);
        unitsSelected.Clear();
    }
    
    private void SelectByClick(GameObject unit)
    {
        DeselectAll();
        unitsSelected.Add(unit);

        triggerSelectionIndicator(unit, true);

        EnableUnitMovement(unit, true);
    }

    private void EnableUnitMovement(GameObject unit, bool willMove)
    {
        unit.GetComponent<UnitMovement>().enabled = willMove;
    }

    private void triggerSelectionIndicator(GameObject unit, bool isVisible)
    {
        unit.transform.GetChild(0).gameObject.SetActive(isVisible);
    }
}
