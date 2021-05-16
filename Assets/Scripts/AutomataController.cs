using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomataController : MonoBehaviour
{
    public GameObject cell;
    public float cellSize = 1.0f;
    public float stepTime = 1.0f;
    
    
    public string rule = "00011110";
    string row = "00100";

    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Simulate());    
    }

    IEnumerator Simulate()
    {
        while(false)
        {
            SpawnCells();
            yield return new WaitForSeconds(stepTime);
            row = ApplyRule();
        }
    }

    string ApplyRule()
    {
        string newRow = "00";
        for(int i = 0; i < row.Length - 2; i++)
        {
            // without "" it wants to add the asci value of the chars
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

        newRow += "00";

        // other solution: string h = word.Substring(start, length);
        return newRow;
    }

    void SpawnCells()
    {
        Vector3 start = Vector3.zero;
        float xOffset = (row.Length / 2) * cellSize;
        float zOffset = row.Length / 2 - 1;
        start += new Vector3(xOffset, 1f, zOffset);

        Vector3 cellOffset = new Vector3(cellSize, 0f, 0f);

        for(int i = 0; i < row.Length; i++)
        {
            if(row[i] == '1')
            {
                Vector3 pos = start + cellOffset * (-i);
                Instantiate(cell, pos, new Quaternion());
            }
        }
    }

}
