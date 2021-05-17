using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChange : MonoBehaviour
{
    public List<Material> cellMatList = new List<Material>();
    public IEnumerator StartEffect(float duration)
    {
        Color startCol = new Color(1,0,0,1);
        Color goalCol = new Color(1,1,1,1);

        for(float elapsedTime = 0; elapsedTime < duration; elapsedTime+=Time.deltaTime)
        {
            Color newCol = Color.Lerp(startCol, goalCol, elapsedTime/duration);

            foreach(Material mat in cellMatList)
                mat.color = newCol;

            yield return null;
        }
        foreach(Material mat in cellMatList)
            mat.color = goalCol;
        
    }
}
