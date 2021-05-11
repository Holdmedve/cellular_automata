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
    public delegate void Verdict(Cell c);
    public Verdict verdict;


    // Start is called before the first frame update
    void Start()
    {
    }


    public delegate void Death();
    public event Death DeathEvent;
    public void OnDeath()
    {
        DeathEvent?.Invoke();
    }
    public void OnNeighbourDeath()
    {
        liveNeighbourCount--;
    }


    public delegate void Birth();
    public event Birth BirthEvent;
    public void OnBirth()
    {
        BirthEvent?.Invoke();
    }
    public void OnNeighbourBirth()
    {
        liveNeighbourCount++;
    } 

    public void OnLive()
    {

    }  
    



    // Update is called once per frame
    void Update()
    {
    }

}
