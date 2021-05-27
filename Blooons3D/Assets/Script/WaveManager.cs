using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    int health = 200;
    int difficulty = 1;
    public GameObject bloon;
    public BezierSpline track1;
    public Text hpText;
    public Text cashText;
    public Text waveText;
    private bool waiting = true;

    public int cash = 400;
    private bool doneSpawning;
    private int currentWave = 1;
    Dictionary<int, List<string>> Waves;


    void Start()
    {
       
        dict();
        Physics.IgnoreLayerCollision(9, 10);
        Physics.IgnoreLayerCollision(10, 10);
        Physics.IgnoreLayerCollision(9, 14);
        Physics.IgnoreLayerCollision(14, 15);
        Physics.IgnoreLayerCollision(11, 10);
        Physics.IgnoreLayerCollision(11, 14);
        Physics.IgnoreLayerCollision(10, 13);
        Physics.IgnoreLayerCollision(10, 15);
        FindObjectOfType<UImanager>().CreateHotkeyUI("R", "Start Wave");

    }
    private void Update()
    {
        hpText.text = health.ToString();
        cashText.text = cash.ToString();
        waveText.text = currentWave.ToString();

        if (Input.GetKeyDown("r")){
            if (waiting)
            {
            StartNextWave();

            }
        }
    }
    public void TakeDamage(int amount) {
        health = health - amount;
    
    }

    void SpawnBloon(int health, string type)
    {
        GameObject newbloon = Instantiate(bloon);
        newbloon.GetComponent<Bloon>().health = health;
        newbloon.GetComponent<Bloon>().spline = track1;
        if (type.Contains("camo"))
        {
            newbloon.GetComponent<Bloon>().camo = true;
        }
        if (type.Contains("white"))
        {
            newbloon.GetComponent<Bloon>().white = true;
        }
        if (type.Contains("black"))
        {
            newbloon.GetComponent<Bloon>().black = true;
        }
        if (type.Contains("metal"))
        {
            newbloon.GetComponent<Bloon>().metal = true;
        }

    }
    IEnumerator SpawnCluster(int health, float delay, int amount, string type)
    {
        for (int i = 1; i < amount; i++)
        {
            SpawnBloon(health, type);
            yield return new WaitForSeconds(delay);
        }
    }



    public void AddCash(int amount)
    {
        cash = cash + amount;
    }



    bool checkbloons()
    {
        Bloon[] boons = FindObjectsOfType<Bloon>();
        if(boons.Length == 0)
        {
            return true;
        }
        else { return false; }
    }



    IEnumerator wavechoose(List<string> waveorder)
    {
        doneSpawning = false;
        for(int i = 0; i < waveorder.Count; i++)
        {
            if(waveorder[i].Contains("SC"))//Spawn Cluster
            {
                string special = "";
                if (waveorder[i].Contains("camo"))
                {
                    special += " camo";
                }
                if (waveorder[i].Contains("metal"))
                {
                    special += " metal";
                }
                if (waveorder[i].Contains("white"))
                {
                    special += " white";
                }
                if (waveorder[i].Contains("black"))
                {
                    special += " black";
                }
                if (waveorder[i].Contains("purple"))
                {
                    special += " purple";
                }

                int bloontype = int.Parse(waveorder[i + 1]);
                int bloonAmount = int.Parse(waveorder[i + 2]);
                StartCoroutine(SpawnCluster(bloontype, .1f, bloonAmount + 1, special)); //spawns one less for some reason??(temp fixed)
            }
       
            if (waveorder[i] == "DD")
            {
                int delaytime = int.Parse(waveorder[i + 1]);
                yield return new WaitForSeconds(delaytime);
            }
            else
            {
            }
        }
        doneSpawning = true;

        yield return new WaitUntil(() => checkbloons());
        EndRound();

    }

    void EndRound()
    {
        AddCash(100);
        currentWave++;
        waiting = true;
        FindObjectOfType<UImanager>().CreateHotkeyUI("R", "Start Wave");
    }
    public void StartNextWave() {

        FindObjectOfType<UImanager>().DeleteHotkeyUI("R", "Start Wave");
        StartCoroutine(wavechoose(Waves[currentWave]));
        waiting = false;
    }


    void dict()
    {
        Waves = new Dictionary<int, List<string>>();
        Waves.Add(1, new List<string> { "SC metal", "1", "20" });
        Waves.Add(2, new List<string> { "SC", "1", "35", "SC", "1", "35" });
        Waves.Add(3, new List<string> { "SC", "1", "25", "SC", "2", "5" });
        Waves.Add(4, new List<string> { "SC", "1", "35", "SC", "2", "18" });
        Waves.Add(5, new List<string> { "SC", "1", "5", "SC", "2", "27" });
        Waves.Add(6, new List<string> { "SC", "1", "15", "SC", "2", "15", "SC", "3", "4" });
        Waves.Add(7, new List<string> { "SC", "1", "20", "SC", "2", "20", "SC", "3", "5" });
        Waves.Add(8, new List<string> { "SC", "1", "10", "SC", "2", "20", "SC", "3", "14" });
        Waves.Add(9, new List<string> { "SC", "3", "30" });
        Waves.Add(10, new List<string> { "SC", "2", "102" });
        Waves.Add(11, new List<string> { "SC", "1", "6", "SC", "2", "12", "SC", "3", "12","SC", "4", "3" });
        Waves.Add(12, new List<string> { "SC", "1", "6", "SC", "2", "13", "SC", "3", "12", "SC", "4", "3" });
        Waves.Add(13, new List<string> { "SC", "2", "50", "SC", "3", "23" });
        Waves.Add(14, new List<string> { "SC", "1", "49", "SC", "2", "15", "SC", "3", "10", "SC", "4", "9" });
        Waves.Add(15, new List<string> { "SC", "1", "20", "SC", "2", "15", "SC", "3", "12", "SC", "4", "10", "SC", "5", "5"});
        Waves.Add(16, new List<string> { "SC", "3", "40", "SC", "4", "8" });
        Waves.Add(17, new List<string> { "SC ", "4", "12" });
        Waves.Add(18, new List<string> { "SC ", "3", "80" });
        Waves.Add(19, new List<string> { "SC", "3", "10", "SC", "4", "8", "SC", "5", "15"});
        Waves.Add(20, new List<string> { "SC ", "6", "6" });
        Waves.Add(21, new List<string> { "SC", "4", "40", "SC", "5", "14" });
        Waves.Add(22, new List<string> { "SC white", "6", "16"});
        Waves.Add(23, new List<string> { "SC white", "6", "7", "SC black", "6", "7" });
        Waves.Add(24, new List<string> { "SC camo", "3", "1", "SC", "2", "20" });
        Waves.Add(25, new List<string> { "SC purple", "6", "10", "SC", "4", "25" });
        Waves.Add(26, new List<string> { "SC", "7", "4", "SC", "5", "23" });
        Waves.Add(27, new List<string> { "SC", "1", "100", "SC", "2", "60", "SC", "3", "45", "SC", "4", "45" });
        Waves.Add(28, new List<string> { "SC metal", "7", "6" });
        Waves.Add(29, new List<string> { "SC", "4", "70"});
        Waves.Add(30, new List<string> { "SC metal", "7", "9"});
        Waves.Add(31, new List<string> { "SC black", "6", "8", "SC white", "6", "8","SC", "7", "10" });
        Waves.Add(32, new List<string> { "SC black", "6", "15", "SC white", "6", "20", "SC purple", "6", "10" });
        Waves.Add(33, new List<string> { "SC camo", "1", "20", "SC camo", "4", "13"});
        Waves.Add(34, new List<string> { "SC", "4", "160", "SC", "7", "6" });
        Waves.Add(35, new List<string> { "SC", "5", "35", "SC black", "6", "30", "SC white", "6", "25", "SC", "8", "5" });
        Waves.Add(36, new List<string> { "SC", "5", "140", "SC camo", "3", "20" });
        Waves.Add(37, new List<string> { "SC black", "6", "25", "SC white", "6", "25", "SC camo white", "6", "7", "SC metal", "7", "15" });
        Waves.Add(38, new List<string> { "SC", "5", "42", "SC white", "6", "17", "SC", "7", "10", "SC metal", "7", "14", "SC", "9", "2" });
        Waves.Add(39, new List<string> { "SC black", "6", "10", "SC white", "6", "10", "SC", "7", "20", "SC", "8", "18" });
        Waves.Add(40, new List<string> { "SC", "10", "1" });














    }
}
