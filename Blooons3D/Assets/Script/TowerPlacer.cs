using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TowerPlacer : MonoBehaviour
{
    private WaveManager waveMan;
    private GameObject tower;
    private bool selector;
    private int selectedButton;
    private Dictionary<int, GameObject> Towers;
    private List<GameObject> towericons = new List<GameObject>();
    private GameObject holo;
    private Vector3 raypoint;

    public Text levelText;
    public Text monkeyName;
    public Text monkeyCost;
    public Text nameText;
    public Text popCount;
    public Text sellText;
    public Text targetText;
    public CanvasGroup towersCanvas;
    public CanvasGroup infoCanvas;
    public GameObject[] towersArray;
    public GameObject hologram;
    public Transform head;
    public Tower selectedTower;
    public GameObject icon;
    public Transform icons;
    public Transform placer;

    public GameObject hoveredTower;

    GameObject mountedTower;
    bool mounted;

    UImanager uimanager;
    bool infoShown;


    private void Start()
    {
        dict();
        waveMan = FindObjectOfType<WaveManager>();
        uimanager = FindObjectOfType<UImanager>();
        uimanager.CreateHotkeyUI("X", "Tower");

    }
    void dict()
    {
        Towers = new Dictionary<int, GameObject>();
        for (int i = 0; i < towersArray.Length; i++)
        {
            Towers.Add(i,towersArray[i]);
        }
        for (int i = 0; i < towersArray.Length; i++)
        {
            GameObject newicon = Instantiate(icon, icons.transform);
            newicon.GetComponent<TowerIcon>().ID = i;
            towericons.Add(newicon);
        }
    }

void Update()
    {
        if (!mounted)
        {
        FindFloor();
        findmonky();

        }
        if (Input.GetKeyDown("x"))
        {
            if (!selector && !mounted)
            {
                uimanager.CreateHotkeyUI("MB1", "Place Tower");
                uimanager.DeleteHotkeyUI("X", "Tower");
                selector = true; 
                tower = towersArray[selectedButton];
                UpdateHologram();
                ;
            }
            else { selector = false;
                uimanager.CreateHotkeyUI("X", "Tower");
                uimanager.DeleteHotkeyUI("MB1", "Place Tower");
            }
        }
        if (selector) {
            TowerChoser();
            uimanager.ShowUI(towersCanvas);
        }
        else
        {
            if (holo)
            {
                Destroy(holo.gameObject);
                TowerRadius[] rads = FindObjectsOfType<TowerRadius>();
                foreach(TowerRadius r in rads)
                {
                    r.radiuses.Clear();
                }
            }
            uimanager.HideUI(towersCanvas);     
        }
        if(selector || infoShown)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        if (Input.GetKeyDown("e"))
        {
            if (mounted) { Dismount(); }
            else if (hoveredTower)
            {
                if (hoveredTower.GetComponentInParent<MinigunTower>() != null && !mounted)
                {
                    Mount(hoveredTower);                
                }
            }
        }
        if (hoveredTower)
        {
        if (hoveredTower.GetComponentInParent<MinigunTower>() != null && !mounted && !selector)
        {
            uimanager.CreateHotkeyUI("E", "Mount");
        }

        }
    }
    void Mount(GameObject towertomount)
    {
        
        if (!towertomount.gameObject.GetComponentInParent<MinigunTower>().Stalling)
        {

            if (!mounted && towertomount.gameObject.GetComponentInParent<MinigunTower>().canBeSelceted && !selector)
            {
                TowerRadius[] radi = FindObjectsOfType<TowerRadius>();
                foreach (TowerRadius r in radi)
                {
                    r.hovered = false;
                }
                uimanager.DeleteHotkeyUI("Q", "Tower Info");
                uimanager.CreateHotkeyUI("E", "Dismount");
                uimanager.DeleteHotkeyUI("E", "Mount");
                selector = false;
                FindObjectOfType<UImanager>().DeleteHotkeyUI("MB1", "Place Tower");
                uimanager.ChangeUIState(UImanager.UIstates.mounted);

                gameObject.transform.parent = towertomount.GetComponentInParent<MinigunTower>().playerSpot;
                gameObject.transform.localPosition = new Vector3(0, 0, 0);
               
                towertomount.gameObject.GetComponentInParent<MinigunTower>().playerControl = true;
                FindObjectOfType<HomieCamera>().SwitchCamera(2);
               
                mounted = true;
                mountedTower = towertomount;
            }                 
        }
        StartCoroutine(towertomount.gameObject.GetComponentInParent<MinigunTower>().stall());
    }

    void Dismount()
    {
            uimanager.DeleteHotkeyUI("E", "Dismount");
            uimanager.ChangeUIState(UImanager.UIstates.ingame);
            gameObject.transform.parent = null;
            mountedTower.GetComponentInParent<MinigunTower>().playerControl = false;
            FindObjectOfType<HomieCamera>().SwitchCamera(1);
            mounted = false;
        
    }

    void PlaceTower()
    {
        if (tower.GetComponent<Tower>().cost <= waveMan.cash && holo.GetComponent<Tower>().radius.beingTouched == false)
        {
            waveMan.cash -= tower.GetComponent<Tower>().cost;
            Quaternion rotationtower = Quaternion.Euler(0, 0, 0);
            GameObject newtower = Instantiate(tower, raypoint, rotationtower, null);
            newtower.GetComponent<Tower>().activated = true;
            StartCoroutine(holo.GetComponent<Tower>().radius.PlaceTower());
            selector = false;
            uimanager.DeleteHotkeyUI("MB1", "Place Tower");
            uimanager.CreateHotkeyUI("X", "Tower");

        }
    }
    void UpdateHologram()
    {
        if (holo)
        {
            Destroy(holo.gameObject);
        }    
        holo = Instantiate(tower, placer.transform.position, Quaternion.Euler(0, 0, 0), placer);
        MeshRenderer[] rendererers = holo.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer m in rendererers)
        {
            m.material.color = new Color(m.material.color.r, m.material.color.g, m.material.color.b, 0.4f);
        }
    }

    void ShowTowerInfo(Tower tower)
    {
        if (tower.activated)
        {
            nameText.text = tower.name.ToString();
            popCount.text = tower.popcount.ToString();
            sellText.text = (tower.cost / 2).ToString();
            selectedTower = tower;
            levelText.text = ("Level "+tower.level.ToString());
            switch (tower.targeting.targetMode)
            {
                case 1:
                    targetText.text = "First";
                    break;
                case 2:
                    targetText.text = "Last";
                    break;
                case 3:
                    targetText.text = "Close";
                    break;
                case 4:
                    targetText.text = "Strong";
                    break;
            }
        }
}
    public void selltower()
    {
        selectedTower.Sell();
        uimanager.HideUI(infoCanvas);
        uimanager.ChangeUIState(UImanager.UIstates.ingame);
        infoShown = false;//sus
    }
    public void UpgradeTower()
    {
        if (selectedTower.level <3)
        {
            waveMan.cash = waveMan.cash - 500;
        }
        selectedTower.Upgrade();
        ShowTowerInfo(selectedTower);
        
    }
    public void SwapTargeting()
    {
        selectedTower.SwapTargeting();
        ShowTowerInfo(selectedTower);
    }
    void TowerChoser()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            selectedButton--;
            if (selectedButton < 0)
            {
                selectedButton = towersArray.Length-1;
            }
            tower = towersArray[selectedButton];
            UpdateHologram();
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            selectedButton++;
            if (selectedButton > towersArray.Length-1)
            {
                selectedButton = 0;
            }
            tower = towersArray[selectedButton];
            UpdateHologram();
        }
        if (Input.GetKeyDown("1"))
        {
            selectedButton = 0;
            tower = towersArray[selectedButton];
            UpdateHologram();
        }
        if (Input.GetKeyDown("2"))
        {
            selectedButton = 1;
            tower = towersArray[selectedButton];
            UpdateHologram();

        }
        if (Input.GetKeyDown("3"))
        {
            selectedButton = 2;
            tower = towersArray[selectedButton];
            UpdateHologram();

        }
        if (Input.GetKeyDown("4"))
        {
            selectedButton = 3;
            tower = towersArray[selectedButton];
            UpdateHologram();

        }
        if (Input.GetKeyDown("5"))
        {
            selectedButton = 4;
            tower = towersArray[selectedButton];
            UpdateHologram();

        }
        if (Input.GetKeyDown("0"))
        {
            tower = null;
        }
        if (Input.GetMouseButtonDown(0))
        {
            PlaceTower();
        }
        foreach (GameObject i in towericons)
        {
            i.GetComponent<TowerIcon>().UpdateInfo(selectedButton);
        }

        monkeyCost.text = towersArray[selectedButton].GetComponent<Tower>().cost.ToString();
        monkeyName.text = towersArray[selectedButton].gameObject.name;


    }
    void findmonky()
    {
        RaycastHit BodyRaycast;
        int layerMask = 1 << 15;
        float rayDistance = 50f;
        if (Physics.Raycast(head.position, head.TransformDirection(Vector3.forward), out BodyRaycast, rayDistance, layerMask))
        {
            Tower foundTower = BodyRaycast.collider.gameObject.GetComponentInParent<Tower>();
            hoveredTower = foundTower.gameObject;
            Debug.DrawRay(head.position, head.TransformDirection(Vector3.forward) * rayDistance, Color.red);
            if (foundTower.activated)
            {
                uimanager.CreateHotkeyUI("Q","Tower Info");


                BodyRaycast.collider.gameObject.GetComponentInParent<Tower>().radius.hovered = true;
                if (Input.GetKeyDown("q"))
                {
                    if (infoShown)
                    {
                        uimanager.ChangeUIState(UImanager.UIstates.ingame);
                        uimanager.HideUI(infoCanvas);
                        infoShown = false;
                        selectedTower.radius.selected = false;
                    }
                    else
                    {
                        if (!mounted)
                        {
                        uimanager.ChangeUIState(UImanager.UIstates.menu);
                        uimanager.ShowUI(infoCanvas);                  
                        foundTower.radius.selected = true;
                        ShowTowerInfo(foundTower);
                        infoShown = true;
                        }
                    }
                }

            }
            
        }
        else
        {
            
            uimanager.DeleteHotkeyUI("E", "Mount");
            hoveredTower = null;
            Debug.DrawRay(head.position, head.TransformDirection(Vector3.forward) * rayDistance, Color.white);
            TowerRadius[] radi = FindObjectsOfType<TowerRadius>();
            foreach(TowerRadius r in radi)
            {
                r.hovered = false;
            }
            uimanager.DeleteHotkeyUI("Q", "Tower Info");

        }

    }

    void FindFloor()
    {

        if (holo)
        {
            holo.transform.position = raypoint;
        }
        RaycastHit BodyRaycast;
        int layerMask = 1 << 12;
        float grabdistance = 50f;
        if (Physics.Raycast(head.position, head.TransformDirection(Vector3.forward), out BodyRaycast, grabdistance, layerMask))
        {
            Debug.DrawRay(head.position, head.TransformDirection(Vector3.forward) * grabdistance, Color.red);
            if (BodyRaycast.collider.gameObject.CompareTag("TowerSurface"))
            {
                raypoint = BodyRaycast.point;
                placer.position = BodyRaycast.point;
            }
        }
        //else {Debug.DrawRay(head.position, head.TransformDirection(Vector3.forward) * grabdistance, Color.white);}
    }
}
