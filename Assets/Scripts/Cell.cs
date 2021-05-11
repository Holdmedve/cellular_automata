using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum CellState
{
    living,
    dead
}

public class Cell : MonoBehaviour
{   
    public List<GameObject> neighbours = new List<GameObject>();
    
    public int liveNeighbourCount {get; private set;} = 0;

    public CellState state {get; private set;}


    // the verdict decides the fate of this cell
    public delegate void Verdict(Cell c);
    public Verdict verdict;


    public event EventHandler DeathEvent;
    public void OnDeath(EventArgs e)
    {
        state = CellState.dead;
        DeathEvent?.Invoke(this, e);
    }
    public void OnNeighbourDeath(object sender, EventArgs e)
    {
        liveNeighbourCount--;
    }

    public int GetDeathSubscriberCount()
    {
        return DeathEvent.GetInvocationList().Length;
    }


    public event EventHandler  BirthEvent;
    public void OnBirth(EventArgs e)
    {
        state = CellState.living;
        BirthEvent?.Invoke(this, e);
    }
    public void OnNeighbourBirth(object sender, EventArgs e)
    {
        liveNeighbourCount++;
    } 
    public int GetBirthSubscriberCount()
    {
        return BirthEvent.GetInvocationList().Length;
    }


}
