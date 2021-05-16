using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomataController : MonoBehaviour
{
    [Header("Cell settings")]
    public GameObject cell;
    public float cellSize = 1.0f;
    public float speed = 1.0f;
    public float timeToDie = 10f;

    [Header("Row settings")]
    public int widthLimit = 10;
    [HideInInspector()]
    public string rule = "00011110";


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
        GameObject rowFollower = GameObject.Find("RowFollower");
        rowFollower.AddComponent<Move>().speed = speed;
        GenerateRow();
    }

    public void StopSimulation()
    {
        row = seed;
        GameObject rowFollower = GameObject.Find("RowFollower");
        Destroy(rowFollower.GetComponent<Move>());
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
        string newRow = (widthExceeded)? "": "00";

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

        if(widthExceeded == false)
            newRow += "00";
        
        return newRow;
    }

    void SpawnCells()
    {
        Vector3 start = Vector3.zero;
        float xOffset = (row.Length / 2) * cellSize;
        start += new Vector3(xOffset, 1f, 0f);

        Vector3 cellOffset = new Vector3(cellSize, 0f, 0f);

        GameObject cellRow = new GameObject("CellRow");
        cellRow.transform.SetParent(cellRows.transform);

        for(int i = 0; i < row.Length; i++)
        {
            if(row[i] == '1')
            {
                Vector3 pos = start + cellOffset * (-i);
                GameObject go =  Instantiate(cell, pos, new Quaternion(), cellRow.transform);                                
            }
        }

        cellRow.AddComponent<Move>().speed = speed;
        Destroy(cellRow, timeToDie);
    }

}
