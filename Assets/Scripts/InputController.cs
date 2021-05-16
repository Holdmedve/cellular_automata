using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum State
{
    idle,
    running
}

public class InputController : MonoBehaviour
{
    public InputField ruleInput;
    public AutomataController automataController;
    State state;

    public void OnStart()
    {
        if(state == State.running)
            return;

        if(ruleInput.text == "")
            return;
        
        int rule = int.Parse(ruleInput.text);
        if(OutsideBounds(rule))
            return;
        
        automataController.rule = ConvertTo8Bit(rule);
        automataController.StartSimulation();
    }

    bool OutsideBounds(int i)
    {
        if(i < 0 || i > 255)
            return true;
        return false;
    }

    string ConvertTo8Bit(int decimalRule)
    {
        string binaryRule = "";
        for(int exponent = 7; exponent >= 0; exponent--)
        {
            int divisor = (int)Mathf.Pow(2, exponent);

            if((int)(decimalRule / divisor) > 0)
            {
                binaryRule += "1";
                decimalRule -= divisor;
            }
            else
            {
                binaryRule += "0";
            }
        }

        return binaryRule;
    }

    public void OnStop()
    {
        if(state == State.idle)
            return;
            
        automataController.StopSimulation();
    }
}
