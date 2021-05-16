using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputController : MonoBehaviour
{
    public InputField ruleInput;
    public AutomataController automataController;

    public void OnStart()
    {
        if(ruleInput.text == "")
            return;
        
        int rule = int.Parse(ruleInput.text);
        if(OutsideBounds(rule))
            return;
        
        automataController.rule = ConvertTo8Bit(rule);
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

    }
}
