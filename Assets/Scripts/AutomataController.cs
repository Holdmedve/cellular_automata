using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutomataController : MonoBehaviour
{
    [Header("Cell settings")]
    public GameObject cell;
    public float cellSize = 1.0f;
    public Dropdown speedSelector;

    [Header("Row settings")]
    public int widthLimit = 10;
    [HideInInspector()]
    public string rule = "00011110";


    // one cell in the middle as a starting point
    const string seed = "00100";
    string row;
    
    GameObject cellRows;
    void Start()
    {
        cellRows = GameObject.Find("CellRows");
    }
    
    public void StartSimulation()
    {
        row = seed;
        GenerateRow();
    }

    public void StopSimulation()
    {
        int rowCount = cellRows.transform.childCount;
        Transform youngestRow = cellRows.transform.GetChild(rowCount - 1);
        // won't collide with BirthGate, therefore no new row
        youngestRow.gameObject.GetComponent<BoxCollider>().enabled = false;
    }

    public void GenerateRow()
    {
        SpawnCells();
        bool widthExceeded = CheckWidth();
        row = ApplyRule(widthExceeded);
    }

    bool CheckWidth()
    {
        if(row.Length > widthLimit)
            return true;
        return false;
    }

    string ApplyRule(bool widthExceeded)
    {
        string newRow = (widthExceeded) ? "" : "00";

        for(int i = 0; i < row.Length - 2; i++)
        {
            // without "" it wants to add the asci value of the chars
            // other solution: string h = word.Substring(start, length); in case current one is slow
            string pattern = "" + row[i] + row[i + 1] + row[i + 2];

            // 111 110 101 100 011 010 001 000
            switch(pattern)
            {
                case "111": newRow += rule[0]; break;
                case "110": newRow += rule[1]; break;
                
                case "101": newRow += rule[2]; break;
                case "100": newRow += rule[3]; break;

                case "011": newRow += rule[4]; break;
                case "010": newRow += rule[5]; break;
                
                case "001": newRow += rule[6]; break;
                case "000": newRow += rule[7]; break;
            }
        }

        newRow += (widthExceeded) ? "" : "00";
        
        return newRow;
    }

    void SpawnCells()
    {
        // holds a row of cells 
        // more efficient than moving individual cells
        GameObject cellRow = new GameObject("CellRow");        
        cellRow.transform.SetParent(cellRows.transform);
        cellRow.tag = "CellRow";

        // left end pos of the row
        Vector3 leftEnd;
        float x = (row.Length / 2) * cellSize;
        leftEnd = new Vector3(x, 1f, 0f);
        Vector3 cellOffset = new Vector3(cellSize, 0f, 0f);

        // string together new row
        for(int i = 0; i < row.Length; i++)
        {
            if(row[i] == '1')
            {
                Vector3 pos = leftEnd + cellOffset * (-i);
                GameObject go =  Instantiate(cell, pos, new Quaternion(), cellRow.transform);                                
            }
        }

        StartMovingRow(cellRow);
    }

    void StartMovingRow(GameObject cellRow)
    {
        // start moving the new row
        cellRow.AddComponent<Move>().speed = InputController.GetSpeed();
        Move move = cellRow.GetComponent<Move>();
        speedSelector.onValueChanged.AddListener(delegate {move.OnSpeedChange();});
        
        // to detect collision with the gates
        cellRow.AddComponent<BoxCollider>();
    }
}
