using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TextBlink : MonoBehaviour
{
    Text blinkText;
    

    // Start is called before the first frame update
    void Start()
    {
        blinkText = GetComponent<Text>();
        StartCoroutine(BilnkText());
    }

    public IEnumerator BilnkText() //0.5초마다 깜빡이게
    {
        while(true)
        {
            blinkText.text = "";   //아무글자도 안뛰워주고 
            yield return new WaitForSeconds(0.5f);
            blinkText.text = "Press Any Key";   //글자를 뛰워주면 
            yield return new WaitForSeconds(0.5f);

        }

    }

  
}
