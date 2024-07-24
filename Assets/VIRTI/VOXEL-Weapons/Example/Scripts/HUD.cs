using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Transform hud_custoMode_main;
    public GameObject hud_custoMode_partIcon;
    public Transform hud_custoMode_content;
    public GameObject hud_custoMode_item;
    public Text hud_custoMode_wpnName;
    public Text hud_custoMode_partName;
    bool custoMode;
    Weapon custoMode_Wpn;
    int custoMode_actualPart;

    List<Transform> hud_items_partSelection = new List<Transform>();
    List<Transform> hud_items_modSelection = new List<Transform>();

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (custoMode && custoMode_Wpn)
        {
            for(int i = 0; i < hud_items_partSelection.Count; i++)
            {
                Transform obj = hud_items_partSelection[i];
                obj.position = Camera.main.WorldToScreenPoint(custoMode_Wpn.custoParts[i].point.position);
            }
        }
    }

    public void EnterCustoMode(Weapon wpn)
    {
        custoMode = true;
        custoMode_Wpn = wpn;
        hud_custoMode_main.gameObject.SetActive(true);
        foreach (Transform obj in hud_items_partSelection)
            Destroy(obj.gameObject);
        hud_items_partSelection = new List<Transform>();


        hud_custoMode_wpnName.text = custoMode_Wpn.wpnName;

        for (int i = 0; i < wpn.custoParts.Length; i++)
        {
            Weapon.CustoParts part = wpn.custoParts[i];
            if(part.point)
            {
                print(part.partName + " has " + part.mods.Length + " mods");
                GameObject obj = Instantiate(hud_custoMode_partIcon, hud_custoMode_main.position, hud_custoMode_main.rotation, hud_custoMode_main);
                obj.SetActive(true);
                obj.name = i.ToString();
                hud_items_partSelection.Add(obj.transform);
            }
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ChoosePart(GameObject item)
    {
        print("choosed");
        foreach (Transform obj in hud_items_modSelection)
            Destroy(obj.gameObject);
        hud_items_modSelection = new List<Transform>();        

        int index = -1;
        if (!int.TryParse(item.name, out index) || index == -1)
            return;

        custoMode_actualPart = index;
        

        Weapon.CustoParts part = custoMode_Wpn.custoParts[index];

        hud_custoMode_partName.text = part.partName;

        GameObject empty = Instantiate(hud_custoMode_item, hud_custoMode_content.position, hud_custoMode_content.rotation, hud_custoMode_content);
        empty.SetActive(true);
        empty.transform.GetChild(0).GetComponent<Text>().text = "Nenhum";
        empty.transform.GetChild(1).GetComponent<Image>().sprite = null;
        empty.name = "-1";
        hud_items_modSelection.Add(empty.transform);

        for (int i = 0; i < part.mods.Length; i++)
        {
            Weapon.CustoParts.Mods mod = part.mods[i];
            GameObject obj = Instantiate(hud_custoMode_item, hud_custoMode_content.position, hud_custoMode_content.rotation, hud_custoMode_content);
            obj.SetActive(true);
            obj.transform.GetChild(0).GetComponent<Text>().text = mod.name;
            obj.transform.GetChild(1).GetComponent<Image>().sprite = mod.image;
            obj.name = i.ToString();
            hud_items_modSelection.Add(obj.transform);
        }
    }

    public void ChooseMaterialPart()
    {
        foreach (Transform obj in hud_items_modSelection)
            Destroy(obj.gameObject);
        hud_items_modSelection = new List<Transform>();
        custoMode_actualPart = -2;


        hud_custoMode_partName.text = "Material";

        GameObject empty = Instantiate(hud_custoMode_item, hud_custoMode_content.position, hud_custoMode_content.rotation, hud_custoMode_content);
        empty.SetActive(true);
        empty.transform.GetChild(0).GetComponent<Text>().text = "Nenhum";
        empty.transform.GetChild(1).GetComponent<Image>().sprite = null;
        empty.name = "-1";
        hud_items_modSelection.Add(empty.transform);

        for (int i = 0; i < custoMode_Wpn.custoMaterials.Length; i++)
        {
            Weapon.CustoMaterial mat = custoMode_Wpn.custoMaterials[i];
            GameObject obj = Instantiate(hud_custoMode_item, hud_custoMode_content.position, hud_custoMode_content.rotation, hud_custoMode_content);
            obj.SetActive(true);
            obj.transform.GetChild(0).GetComponent<Text>().text = mat.name;
            obj.transform.GetChild(1).GetComponent<Image>().sprite = mat.image;
            obj.name = i.ToString();
            hud_items_modSelection.Add(obj.transform);
        }
    }

    public void ChooseMod(GameObject item)
    {
        if(custoMode_actualPart == -2)
        {
            ChooseMaterial(item);
            return;
        }

        int index = -1;
        if (!int.TryParse(item.name, out index))
            return;

        print("choosed mod " + index);

        custoMode_Wpn.custoParts[custoMode_actualPart].actualMod = index;
        custoMode_Wpn.UpdateCustoParts();
    }

    public void ChooseMaterial(GameObject item)
    {
        int index = -1;
        if (!int.TryParse(item.name, out index))
            return;

        custoMode_Wpn.actualMat = index;

        custoMode_Wpn.UpdateCustoMats();
    }

    public void ExitCustoMode()
    {
        custoMode_Wpn = null;
        hud_custoMode_main.gameObject.SetActive(false);
        custoMode = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
