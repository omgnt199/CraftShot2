using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;


public class WpnHolder : MonoBehaviour
{
    public Transform hand;
    public Weapon wpnActual;

    public Vector3 interactOffset = new Vector3(0,0,1);
    public float interactRadius = 1;
    public LayerMask interactLayer;

    [System.Serializable]
    public class InventoryItem
    {
        public enum Type
        {
            Ammo, Other
        }
        public Type type;
        public int itemIndex;
        public int qtd;
    }
    public List<InventoryItem> inventory_bulletTypes = new List<InventoryItem>();

    public List<Weapon> weapons = new List<Weapon>();
    public int indexWpnActual = -1;
    int lastWpn = -1;

    public RuntimeAnimatorController defaultHandAnimController;

    float horizontal;
    float vertical;
    float xMouse;
    float yMouse;
    [SerializeField]
    float zMove;
    float xMove;

    bool aiming;
    bool crouched;
    bool custoMode;

    bool changingWpn;
    // float recuoChaningWpn;

    GameObject ui_interact;
    GameObject ui_ammo;

    public Vector3 handOffset;
    public Vector3 handOffsetAiming;

    public Animator handAnimator;
    CharacterController controller;

    public bool DEBUG;
    public bool DEBUGAIM;

    public GameObject debug_cam;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (wpnActual)
        {
            AddWeapon(wpnActual);
            EquipWeapon(wpnActual);
            indexWpnActual = IndexOfWpnActual(wpnActual);
        }
        ui_interact = GameObject.Find("ui_interact");
        ui_interact.SetActive(false);
        ui_ammo = GameObject.Find("ui_ammo");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            DEBUG = !DEBUG;
            print("DEBUG (ALWAYS SET POS OF WPN) is now: " + (DEBUG ? "ON" : "OFF"));
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            DEBUGAIM = !DEBUGAIM;
            print("DEBUGAIM (ALWAYS AIMING WPN) is now: " + (DEBUGAIM ? "ON" : "OFF"));
        }
        if (Input.GetKeyDown(KeyCode.F3))
        { 
            GetComponent<FirstPersonController>().enabled = !GetComponent<FirstPersonController>().enabled;
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            if (Time.timeScale == 1)
                Time.timeScale = 0;
            else
                Time.timeScale = 1;
        }
        if (Input.GetKeyDown(KeyCode.F5))
        {
            if (GameObject.Find("Canvas").GetComponent<CanvasGroup>().alpha == 1)
                GameObject.Find("Canvas").GetComponent<CanvasGroup>().alpha = 0;
            else
                GameObject.Find("Canvas").GetComponent<CanvasGroup>().alpha = 1;
        }
        if (Input.GetKeyDown(KeyCode.F6))
        {
            if (!debug_cam.activeInHierarchy) 
            {
                debug_cam.SetActive(true);
                GetComponent<FirstPersonController>().enabled = false;
            }
            else
            {
                debug_cam.SetActive(false);
                GetComponent<FirstPersonController>().enabled = true;
            }

        }

        if (DEBUG && wpnActual)
        {
            EquipWeapon(wpnActual);
            wpnActual.ChangeShotType(wpnActual.shotTypeActual);
        }

        if (wpnActual)
        {
            float debug_run = Input.GetKey(KeyCode.KeypadMultiply) ? 2 : 1;
            float debug_vel = (DEBUGAIM ? 0.25f : 2) * debug_run;

            if (DEBUG && !DEBUGAIM)
            {
                if (Input.GetKey(KeyCode.Keypad6))
                    wpnActual.offsetPos += new Vector3(debug_vel, 0, 0);
                if (Input.GetKey(KeyCode.Keypad4))
                    wpnActual.offsetPos += new Vector3(-debug_vel, 0, 0);
                if (Input.GetKey(KeyCode.Keypad8))
                    wpnActual.offsetPos += new Vector3(0, 0, debug_vel);
                if (Input.GetKey(KeyCode.Keypad2))
                    wpnActual.offsetPos += new Vector3(0, 0, -debug_vel);
                if (Input.GetKey(KeyCode.KeypadPlus))
                    wpnActual.offsetPos += new Vector3(0, debug_vel, 0);
                if (Input.GetKey(KeyCode.KeypadMinus))
                    wpnActual.offsetPos += new Vector3(0, -debug_vel, 0);
            }else if(DEBUG && DEBUGAIM)
            {
                if (Input.GetKey(KeyCode.Keypad6))
                    wpnActual.offsetPosAim += new Vector3(debug_vel, 0, 0);
                if (Input.GetKey(KeyCode.Keypad4))
                    wpnActual.offsetPosAim += new Vector3(-debug_vel, 0, 0);
                if (Input.GetKey(KeyCode.Keypad8))
                    wpnActual.offsetPosAim += new Vector3(0, debug_vel, 0);
                if (Input.GetKey(KeyCode.Keypad2))
                    wpnActual.offsetPosAim += new Vector3(0, -debug_vel, 0);
                if (Input.GetKey(KeyCode.KeypadPlus))
                    wpnActual.offsetPosAim += new Vector3(0, 0, debug_vel);
                if (Input.GetKey(KeyCode.KeypadMinus))
                    wpnActual.offsetPosAim += new Vector3(0, 0, -debug_vel);
            }
        }



        //input
        float mov = 0;
        bool run = Input.GetButton("Fire3");
        vertical = Mathf.MoveTowards(vertical, Input.GetAxis("Vertical") * (run ? 2 : 1), Time.deltaTime * 3);
        horizontal = Mathf.MoveTowards(horizontal, Input.GetAxis("Horizontal") * (run ? 2 : 1), Time.deltaTime * 3);
        xMouse = Mathf.MoveTowards(xMouse, Input.GetAxis("Mouse X"), Time.deltaTime * 5);
        yMouse = Mathf.MoveTowards(yMouse, Input.GetAxis("Mouse Y"), Time.deltaTime * 10f);

        if (vertical != 0)
        {
            if (horizontal == 0)
                mov = Mathf.Abs(vertical);
            else
                mov = (Mathf.Abs(vertical) + Mathf.Abs(horizontal)) / 2;
        }
        else
            mov = Mathf.Abs(horizontal);

        DoInteract(Input.GetKeyDown("e"));


        if (Input.GetKeyDown("r"))
            Reload();
        if (Input.GetKeyDown(KeyCode.LeftControl))
            Crouch();
        else if (!run && !Input.GetKey(KeyCode.LeftControl))
        {
            switch (wpnActual.shotTypes[wpnActual.shotTypeActual].type)
            {
                case Weapon.ShotType.Type.Cascate:
                    if (Input.GetButton("Fire1"))
                        Fire();
                    break;
                case Weapon.ShotType.Type.Individual:
                    if (Input.GetButtonDown("Fire1"))
                        Fire();
                    break;
            }
        }

        for (int i = 0; i < 10; i++)
        {
            if (Input.GetKeyDown(i == 9 ? "0" : (i+1).ToString()))
                ChangeWeapon(i);
        }

        if (Input.GetKeyDown(KeyCode.Equals))
            ChangeWeapon(indexWpnActual == weapons.Count ? 0 : indexWpnActual + 1);
        if (Input.GetKeyDown(KeyCode.Minus))
            ChangeWeapon((indexWpnActual == 0 ? weapons.Count-1 : indexWpnActual - 1));


        if (Input.GetKeyDown("h"))
        {
            if (!custoMode)
                EnterCustoMode();
            else
                ExitCustoMode();
        }

        if (Input.GetKeyDown("v"))
        {
            if(wpnActual)
                wpnActual.ChangeShotType(wpnActual.shotTypeActual+1);   
        }

        //

        ui_ammo.transform.GetChild(0).GetComponent<Text>().text = wpnActual.ammoInPent.ToString();
        ui_ammo.transform.GetChild(1).GetComponent<Text>().text = GetAmmoAmount(wpnActual.shotTypes[wpnActual.shotTypeActual].typeOfBullet).ToString();



        //anims
        handAnimator.SetFloat("Movement", mov);
        handAnimator.SetBool("OnGround", controller.isGrounded);
        if (controller.isGrounded)
        {
            if (Input.GetButton("Jump"))
                handAnimator.Play(run ? "JumpRun" : "Jump");
        }
        aiming = Input.GetButton("Fire2") || DEBUGAIM;

        //
        

        //movement
        Vector3 rot = hand.localEulerAngles;
        rot.x = Mathf.LerpAngle(rot.x, (yMouse * 6f) * (aiming ? 0.2f : 1), Time.deltaTime*4); //vertical
        rot.y = Mathf.LerpAngle(rot.y, (-xMouse * 3f) * (aiming ? 0.2f : 1), Time.deltaTime*8); //horizontal
        hand.localEulerAngles = rot;

        Vector3 offSetWpnAim = wpnActual.offsetPosAim / 100;

        Vector3 pos = hand.localPosition;
        pos.x = Mathf.Lerp(pos.x, (aiming ? handOffsetAiming.x + offSetWpnAim.x : handOffset.x) + (xMove * 25 * (aiming ? 0.5f : 1)), Time.deltaTime * 4); //horizontal
        pos.z = Mathf.Lerp(pos.z, (aiming ? handOffsetAiming.z + offSetWpnAim.z : handOffset.z) + (zMove * 25 * (aiming ? 0.5f : 1)), Time.deltaTime * 4); //vertical
        pos.y = Mathf.Lerp(pos.y, handOffset.y+(crouched ? 0 : 0)+ (aiming ? offSetWpnAim.y : 0), Time.deltaTime * 4); //vertical
        hand.localPosition = Vector3.MoveTowards(hand.localPosition, pos, Time.deltaTime);
        xMove = Mathf.MoveTowards(xMove, 0, Time.deltaTime*10);
        zMove = Mathf.MoveTowards(zMove, 0, Time.deltaTime*10);



        if (changingWpn)
        {
           // recuoChaningWpn += Time.deltaTime;
          //  if (recuoChaningWpn > 2)
           // {
           //     recuoChaningWpn = 0;
           //     changingWpn = false;
           // }
        }

        //
    }

    private void LateUpdate()
    {
        hand.GetChild(0).localScale = new Vector3(1.6f, 1.6f, 1.6f);
    }

    public void Fire()
    {
        if (!wpnActual || custoMode)
            return;

        if (wpnActual.Fire())
        {
            if(zMove < 0.01f)
                zMove = -0.01f*wpnActual.shotTypes[wpnActual.shotTypeActual].recoilForce;
            yMouse = -1.4f* wpnActual.shotTypes[wpnActual.shotTypeActual].recoilForce;
        }
    }

    public void Reload()
    {
        if (!wpnActual || !controller.isGrounded || custoMode)
            return;

        if (wpnActual.Reload())
        {
            string r = wpnActual.shotTypes[wpnActual.shotTypeActual].reloadAnim;
            handAnimator.CrossFade(r.Length > 0 ? r : "Reload", 0.3f);
        }
    }

    public void Crouch()
    {
        if (custoMode)
            return;
        crouched = !crouched;
        if (crouched)
            controller.height /= 2;
        else
            controller.height *= 2;
    }

    bool inte;

    public void DoInteract(bool pressing)
    {
            Collider[] cols = Physics.OverlapSphere(hand.position + hand.TransformDirection(interactOffset), interactRadius, interactLayer);
        if (cols.Length > 0)
        {
            inte = false;
            ui_interact.SetActive(true);
            if(pressing)
                cols[0].SendMessage("Interact", this);
        }
        else if (!inte)
        {
            ui_interact.SetActive(false);
            inte = true;
        }

    }

    public void ChangeWeapon(int witch)
    {
        if (custoMode || witch == indexWpnActual || changingWpn)
            return;
        handAnimator.Play("ChangeWpn", 0, 0.2f);
        lastWpn = indexWpnActual;
        indexWpnActual = witch;
        changingWpn = true;
        print(changingWpn);
    }

    public void DoChangeWeapon() { 
        if(lastWpn >= 0 && lastWpn < weapons.Count)
            UnequipWeapon(weapons[lastWpn]);
        if (indexWpnActual > -1 && indexWpnActual < weapons.Count)
        {
            wpnActual = weapons[indexWpnActual];
            EquipWeapon(wpnActual);            
        }
        
        changingWpn = false;
        print(changingWpn);
    }

    public void AddWeapon(Weapon wpn)
    {
        if (wpn.dono || weapons.Contains(wpn) || weapons.Count <= 0 || !wpn || custoMode)
            return;

        var slot = FindSlotInWpns();
        if (slot == -1)
        {
            slot = indexWpnActual;
            RemoveWeapon(indexWpnActual);
        }
        else
            UnequipWeapon(wpnActual);

        weapons[slot] = wpn;
        wpn.dono = this;
        
        EquipWeapon(wpn);
        ChangeWeapon(slot);
    }

    public void RemoveWeapon(int index)
    {
        if (index <= -1 || index > weapons.Count || custoMode)
            return;
        weapons[index].dono = null;
        weapons[index] = null;
        weapons[index].transform.SetParent(null);
    }

    public void EquipWeapon(Weapon wpn)
    {
        if (!wpn || custoMode)
            return;

        Transform tran = wpn.transform;
        tran.SetParent(hand.GetChild(0));
        tran.localPosition = wpn.offsetPos/1000;
        tran.rotation = hand.rotation;
        tran.localEulerAngles = wpn.offsetRot;

        wpn.dono = this;

        if (wpn.differentHandAnimController)
            handAnimator.runtimeAnimatorController = wpn.differentHandAnimController;
        else
            handAnimator.runtimeAnimatorController = defaultHandAnimController;

        wpn.gameObject.SetActive(true);
    }

    public void UnequipWeapon(Weapon wpn)
    {
        if (!wpn || custoMode)
            return;
        print("unnedquipped");
        wpn.gameObject.SetActive(false);
    }

    public int FindSlotInWpns()
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            if (!weapons[i])
                return i;
        }

        return -1;
    }

    public void EnterCustoMode()
    {
        custoMode = true;
        handAnimator.SetBool("CustoMode", true);
        GameObject.FindObjectOfType<HUD>().EnterCustoMode(wpnActual);
    }

    public void ExitCustoMode()
    {
        custoMode = false;
        handAnimator.SetBool("CustoMode", false);
        GameObject.FindObjectOfType<HUD>().ExitCustoMode();
    }

    public int IndexOfWpnActual(Weapon wpn)
    {
        for (int i = 0; i < weapons.Count; i++)
            if (weapons[i] == wpn)
                return i;

        return -1;
    }

    public void AddAmmo(int type, int amount)
    {
        int index = GetIndexAmmo(type);
        if (index == -1)
        {
            InventoryItem i = new InventoryItem();
            i.itemIndex = type;
            i.qtd = amount;
            i.type = InventoryItem.Type.Ammo;
            inventory_bulletTypes.Add(i);
        }
        else
        {
            inventory_bulletTypes[index].qtd += amount;
        }
    }

    public int RemoveAmmo(int type, int amount)
    {
        int index = GetIndexAmmo(type);
        if (index == -1)
            return 0;

        if (inventory_bulletTypes[index].qtd > amount)
        {
            inventory_bulletTypes[index].qtd -= amount;
            return amount;
        }
        else
        {
            int qtd = inventory_bulletTypes[index].qtd;
            inventory_bulletTypes[index].qtd = 0;
            return qtd;
        }
    }

    public int GetAmmoAmount(int type)
    {
        int index = GetIndexAmmo(type);
        if (index == -1)
            return 0;

        return inventory_bulletTypes[index].qtd;
    }

    public int GetIndexAmmo(int type)
    {
        for (int i = 0; i < inventory_bulletTypes.Count; i++)
            if (inventory_bulletTypes[i].itemIndex == type)
                return i;

        return -1;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(hand.position + hand.TransformDirection(interactOffset), interactRadius);
    }
}
