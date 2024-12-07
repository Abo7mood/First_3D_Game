using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Timer : MonoBehaviour
{
    bool Finish = false;
   public float EnemyKill;
    public Text TimerText;
    public Text KillText;
    private float startTime;
    // Start is called before the first frame update
    void Start()
    {
      
        startTime = Time.time;
    }
    void Update()
    {
      
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (Finish==false)
        {
            float t = Time.time - startTime;
            string hours = ((int)t / 3600).ToString();
            string minutes = ((int)t / 60).ToString();
            string seconds = (t % 60).ToString("f2");

            TimerText.text = hours + ":" + minutes + ":" + seconds;
        }
        else { return; }


      if (EnemyKill > 1000)
        {
            string Kilo= (EnemyKill / 1000).ToString("f3") + "K";
            KillText.text = Kilo;
        }
   
        else
        {
            string defaultNumber = EnemyKill.ToString();
            KillText.text = defaultNumber;
        }

    }

    public void StopTimer()
    {
        Finish = true;
        TimerText.color = Color.yellow;
    }
  
}
