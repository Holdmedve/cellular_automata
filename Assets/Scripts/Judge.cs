using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Judge : MonoBehaviour
{
    // the judge decides who lives and who dies

    public GameObject cellPrefab;
    public int gridSize = 100;
    public float gridPointDst = 1.0f;
    public bool wallsArePortals = false;
    GameObject[,,] cells; // A TYPE LEHETNE CELL



    // Start is called before the first frame update
    void Start()
    {
        InitPopulation();
        ConnectNeighbours();
        PopulationBirth();
        RandomKilling();
    }
    void PopulationBirth()
    {
        foreach(GameObject cellGo in cells)
            Birth(cellGo.GetComponent<Cell>());
    }

    void RandomKilling()
    {
        foreach(GameObject cellGo in cells)
            if(UnityEngine.Random.Range(0.0f, 1.0f) < 0.5f)
                Death(cellGo.GetComponent<Cell>());
    }

    void InitPopulation()
    {
        cells = new GameObject[gridSize, gridSize, gridSize];
        
        for(int x = 0; x < gridSize; x++)
        {
            for(int y = 0; y < gridSize; y++)
            {
                for(int z = 0; z < gridSize; z++)
                {
                    Vector3 pos = CalcCellPos(x, y, z);
                    cells[x, y, z] = Instantiate(cellPrefab, pos, new Quaternion());
                    cells[x, y, z].AddComponent<Cell>();
                }
            }
        }
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
            for(int y = 0; y < gridSize; y++)
            {
                for(int z = 0; z < gridSize; z++)
                {
                    Cell cell = cells[x, y, z].GetComponent<Cell>();
                    for(int i = 0; i < 6; i++)
                    {
                        int[] neighbour = {x + offsets[i, 0], y + offsets[i, 1], z + offsets[i, 2]};
                        if(ValidIndeces(ref neighbour))
                        {
                            int _x = neighbour[0];
                            int _y = neighbour[1];
                            int _z = neighbour[2];
                            cell.neighbours.Add(cells[_x, _y, _z]);
                            cells[_x, _y, _z].GetComponent<Cell>().DeathEvent += cell.OnNeighbourDeath;
                            cells[_x, _y, _z].GetComponent<Cell>().BirthEvent += cell.OnNeighbourBirth;
                        }
                    }
                }
            }
        }
    }

    bool ValidIndeces(ref int[] coordinates)
    {
        if(wallsArePortals)
        {
            for(int i = 0; i < 3; i++)
            {
                if (coordinates[i] == -1)
                    coordinates[i] = gridSize -1;
                else if(coordinates[i] == gridSize)
                    coordinates[i] = 0;
            }
            return true;
        }

        foreach(int coo in coordinates)
            if(coo < 0 || coo >= gridSize)
                return false;

        return true;
    }

    Vector3 CalcCellPos(int xIdx, int yIdx, int zIdx)
    {
        float xPos = gridPointDst * xIdx;
        float yPos = gridPointDst * yIdx;
        float zPos = gridPointDst * zIdx;
        Vector3 pos = new Vector3(xPos, yPos, zPos);

        return pos;
    }

    int counter = 1;
    // Update is called once per frame
    void FixedUpdate() {
        if(counter % 5 == 0){
            GenerateVerdict();
            ExecuteVerdict();
        }
        counter++;
    }

    void Death(Cell c)
    {
        c.gameObject.GetComponent<MeshRenderer>().enabled = false;
        c.OnDeath(EventArgs.Empty);
    }

    void Birth(Cell c)
    {
        c.gameObject.GetComponent<MeshRenderer>().enabled = true;
        c.OnBirth(EventArgs.Empty);
    }


    void ExecuteVerdict()
    {
        foreach(GameObject cellGo in cells)
        {
            Cell c = cellGo.GetComponent<Cell>();
            if(c.verdict != null)
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

        foreach(GameObject cellGo in cells) // EZ HÜLYESÉG VÉGIGMENNI MINDEGYIKEN
        // INKÁBB GENERÁLD A VERDICTET AZ EVENTHANDLER-ekben
        {
            Cell c = cellGo.GetComponent<Cell>();
            c.verdict = null;
            if(c.state == CellState.living)
            {
                if(c.liveNeighbourCount < 2 || c.liveNeighbourCount > 3)
                    c.verdict = Death;
            }
            else if(c.state == CellState.dead)
            {
                if(c.liveNeighbourCount == 3)
                    c.verdict = Birth;
            }            
        }
        
    } 
}
