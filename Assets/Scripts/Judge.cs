using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Judge : MonoBehaviour
{
    // the judge decides who lives and who dies

    public GameObject cellPrefab;
    public int gridSize = 100;
    float gridPointDst = 1.0f;
    GameObject[,,] cells; // A TYPE LEHETNE CELL


    // Start is called before the first frame update
    void Start()
    {
        InitPopulation();
        RandomKilling();
    }

    void RandomKilling()
    {
        foreach(GameObject cellGo in cells)
        {

        }
    }

    void InitPopulation()
    {
        cells = new GameObject[gridSize, gridSize, gridSize];
        
        // x, z are ground coordinates in unity
        // y is up-down
        int cellIdx = 0;
        for(int x = 0; x < gridSize; x++)
        {
            for(int z = 0; z < gridSize; z++)
            {
                for(int y = 0; y < gridSize; y++)
                {
                    Vector3 pos = CalcCellPos(x, z, y);
                    cells[x, z, y] = Instantiate(cellPrefab, pos, new Quaternion());
                    cells[x, z, y].AddComponent<Cell>();
                    cellIdx++;
                }
            }
        }

        ConnectNeighbours();
    }

    void ConnectNeighbours()
    {
        // offsets in all 6 directions
        int[,] offsets = {{ 1, 0, 0},
                          {-1, 0, 0},
                          { 0, 1, 0},
                          { 0,-1, 0},
                          { 0, 0, 1},
                          { 0, 0,-1}};

        for(int x = 0; x < gridSize; x++)
        {
            for(int z = 0; z < gridSize; z++)
            {
                for(int y = 0; y < gridSize; y++)
                {
                    Cell cell = cells[x, y, z].GetComponent<Cell>();
                    for(int i = 0; i < 6; i++)
                    {
                        int[] neighbour = {x + offsets[i, 0], z + offsets[i, 1], y + offsets[i, 2]};
                        if(ValidCoordinates(neighbour))
                        {
                            int _x = neighbour[0];
                            int _z = neighbour[1];
                            int _y = neighbour[2];
                            cell.neighbours.Add(cells[_x, _z, _y]);
                            cells[_x, _z, _y].GetComponent<Cell>().DeathEvent += cell.OnNeighbourDeath;
                            cells[_x, _z, _y].GetComponent<Cell>().BirthEvent += cell.OnNeighbourBirth;
                        }
                    }
                }
            }
        }
    }

    bool ValidCoordinates(int[] coordinates)
    {
        foreach(int coo in coordinates)
            if(coo < 0 || coo >= gridSize)
                return false;

        return true;
    }

    Vector3 CalcCellPos(int xIdx, int zIdx, int yIdx)
    {
        float xPos = gridPointDst * xIdx;
        float zPos = gridPointDst * zIdx;
        float yPos = gridPointDst * yIdx;
        Vector3 pos = new Vector3(xPos, zPos, yPos);

        return pos;
    }

    // Update is called once per frame
    void Update()
    {        
    }

    void Death(Cell c)
    {
        c.gameObject.SetActive(false);
        c.OnDeath();
    }

    void Birth(Cell c)
    {
        c.gameObject.SetActive(true);
        c.OnBirth();
    }


    void ExecuteVerdict()
    {
        foreach(GameObject cellGo in cells)
        {
            Cell c = cellGo.GetComponent<Cell>();
            c.verdict(c);
        }
    }

    void GenerateVerdict()
    {
        /*
            1. Any live cell with fewer than two live neighbors dies, as if caused by under population.
            2. Any live cell with two or three live neighbors lives on to the next generation.
            3. Any live cell with more than three live neighbors dies, as if by overpopulation.
            4. Any dead cell with exactly three live neighbors becomes a live cell, as if by reproduction.
        */

        foreach(GameObject cellGo in cells)
        {
            Cell c = cellGo.GetComponent<Cell>();
            if(c.state == CellState.living)
            {
                if(c.liveNeighbourCount < 2 || c.liveNeighbourCount > 3)
                    c.verdict = Death;
            }
            else
            {
                if(c.liveNeighbourCount == 3)
                    c.verdict = Birth;
            }            
        }
        
    } 
}
