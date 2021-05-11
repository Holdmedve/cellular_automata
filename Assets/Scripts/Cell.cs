using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CellState
{
    living,
    dead
}

public class Cell : MonoBehaviour
{
    

    public List<GameObject> neighbours = new List<GameObject>();
    public int liveNeighbourCount {get; private set;}

    public CellState state {get; private set;}

    // the verdict decides the fate of this cell
    public delegate void Verdict();
    public Verdict verdict;

    // Start is called before the first frame update
    void Start()
    {
    }
    public int GetLiveNeighbourCount()
    {
        return 0;
    }



    public void OnDeath()
    {

    }

    public void OnBeBorn()
    {

    } 

    public void OnLive()
    {

    }  
    



    // Update is called once per frame
    void Update()
    {
    }

}
