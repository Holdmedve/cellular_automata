using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChange : MonoBehaviour
{
    public IEnumerator StartEffect(float duration)
    {
        Material mat = transform.GetComponent<MeshRenderer>().material;
        Color startCol = new Color(1,0,0,1);
        Color goalCol = new Color(1,1,1,1);

        for(float elapsedTime = 0; elapsedTime < duration; elapsedTime+=Time.deltaTime)
        {
            Color newCol = Color.Lerp(startCol, goalCol, elapsedTime/duration);
            mat.color = newCol;
            yield return null;
        }

        mat.color = goalCol;
    }
}
