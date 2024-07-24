using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Effects;

public class Weapon : MonoBehaviour
{
    [Header("MAIN")]
    public string wpnName;
    public Transform shotPos;

    [System.Serializable]
    public class ShotType
    {
        public enum Type
        {
            Cascate, Individual
        }

        [Header("BULLET CONFIGS")]
        public Type type;
        public float recoilForce = 1;
        public float bulletForce = 1;
        public float bulletDamage = 1;
        public float timePerBullet = 0.2f;
        public int typeOfBullet;
        public string DEBUG_typeActual = "DONT CHANGE ME";

        [Header("OTHERS")]
        public float delayToStart = 0;
        public GameObject enableWhenLoaded;
        public string[] otherAnimatinosFire;

        [Header("EFFECTS")]
        public Vector3 extraOffsetPos;
        public Vector3 extraOffsetRot;
        public Vector3 extraOffsetPosAim;
        public Vector3 extraOffsetRotAim;
        public string reloadAnim;

        [Header("OVERHEAT")]
        public float overheat = 0;
        public bool useOverheatMaterial = false;
        public int overheatMaterialIndex = 0;

        [Header("PREFABS")]
        public GameObject bullet;
        public GameObject fx_Shot;
    }
    public ShotType[] shotTypes;


    [Header("WPN CONFIGS")]
    public int bulletsPerPent = 10;
    public int bulletsMax = 100;

    public float reloadTime = 1;


    [Header("OFFSET")]
    public Vector3 offsetPos;
    public Vector3 offsetRot;

    public Vector3 offsetPosAim;
    public Vector3 offsetRotAim;

    [System.Serializable]
    public class MaterialParts
    {
        public Transform gameObject;
        public int materialIndex;

        public bool hasCustomDefaultTextureAndColor;
        public Texture defaultTexture;
        public Color defaultColor;
        public bool hasCustomColorOverhal;
        public Color colorOverhal;
    }

    [Header("PARTS")]
    public MaterialParts[] materialParts;

    [System.Serializable]
    public class CustoParts
    {
        public string partName;
        public Transform point;
        [System.Serializable]
        public class Mods
        {
            public string name;
            public Sprite image;
            public GameObject prefab;
            public Vector3 offsetPos;
            public Vector3 offsetRot;
            public MaterialParts[] materialParts;
            public bool dontDoWhenUseEffects;

            [Header("EFFECTS")]
            public int sum_bulletPerPent;
            public int sum_bulletMax;
            public Vector3 sum_posOffset;
            public Vector3 sum_rotOffset;
            public Vector3 sum_posOffsetAim;
            public Vector3 sum_rotOffsetAim;
            public ShotType add_shotType;
            public int add_shotTypeIndex;
            public ShotType[] sum_shotTypes;
        }
        public Mods[] mods;

        public GameObject[] disableWhenUse;
        public GameObject[] enableWhenUse;

        [HideInInspector]
        public int actualMod = -1;
        [HideInInspector]
        public GameObject actualModObj;
        [HideInInspector]
        public int lastModObjIndex;
    }
    public CustoParts[] custoParts;

    [System.Serializable]
    public class CustoMaterial
    {
        public string name;
        public Sprite image;
        public Texture texture;
        public Color color = Color.white;
    }
    public CustoMaterial[] custoMaterials;
    [HideInInspector]
    public int actualMat = -1;
    [HideInInspector]
    public int lastMat = -1;

    public RuntimeAnimatorController differentHandAnimController;

    float timeStopShoting;
    float recuo;
    float recuoReload;
    public int ammoInPent;
    [SerializeField]
    int ammoTotal;
    public int shotTypeActual;
    float timeShoting;
    int shotAnimIndex;

    public WpnHolder dono;
    Animator animator;

    public bool DEBUG_CREATESUBOPJECTS;
    public bool DEBUG_GETMATERIALPARTS_FIRST;
    public bool DEBUG_GETMATERIALPARTS_ALL;

    void Start()
    {
        animator = GetComponent<Animator>();
        for (int i = 0; i < custoParts.Length; i++)
        {
            custoParts[i].actualMod = -1;
            custoParts[i].lastModObjIndex = -1;
        }
        for (int i = 0; i < materialParts.Length; i++)
        {
            materialParts[i].defaultTexture = materialParts[i].gameObject.GetComponent<Renderer>().materials[materialParts[i].materialIndex].mainTexture;
            materialParts[i].defaultColor = materialParts[i].gameObject.GetComponent<Renderer>().materials[materialParts[i].materialIndex].color;
        }
        ammoInPent = bulletsPerPent;
        ammoTotal = bulletsMax;

        defaultBulletPerPent = bulletsPerPent;
        defaultBulletMax = bulletsMax;
        defaultShotTypes = shotTypes;
        defaultOffsetPos = offsetPos;
        defaultOffsetRot = offsetRot;
        defaultOffsetPosAim = offsetPosAim;
        defaultOffsetRotAim = offsetRotAim;

        if (ammoInPent > 0 && shotTypes[shotTypeActual].enableWhenLoaded)
            shotTypes[shotTypeActual].enableWhenLoaded.SetActive(true);
    }

    void Update()
    {
        if (recuo > 0)
            recuo -= Time.deltaTime;
        if (recuoReload > 0)
            recuoReload -= Time.deltaTime;
        if (timeStopShoting > 0)
            timeStopShoting -= Time.deltaTime;
        else if (timeShoting != 0)
        {
            if(shotTypes[shotTypeActual].overheat > 0)
            {
                if (timeShoting > 0)
                    timeShoting -= Time.deltaTime * 3;
                else if (overheated)
                    overheated = false;

                if (shotTypes[shotTypeActual].useOverheatMaterial)
                {
                    MaterialParts part = materialParts[shotTypes[shotTypeActual].overheatMaterialIndex];
                    part.gameObject.GetComponent<Renderer>().materials[part.materialIndex].color = Color.Lerp(part.defaultColor, overheatColor, timeShoting / shotTypes[shotTypeActual].overheat);
                }

            }
            else
                timeShoting = 0;
        }
    }

    Color overheatColor;
    bool overheated;

    public bool Fire()
    {
        if (recuo > 0 || recuoReload > 0 || overheated)
            return false;


        if (ammoInPent <= 0)
        {
            if (shotTypes[shotTypeActual].enableWhenLoaded)
                shotTypes[shotTypeActual].enableWhenLoaded.SetActive(false);
            dono.Reload();
            return false;
        }

        timeStopShoting = 0.1f;

        timeShoting += Time.deltaTime;

        if (shotTypes[shotTypeActual].useOverheatMaterial && timeShoting > shotTypes[shotTypeActual].overheat/4)
        {
            MaterialParts part = materialParts[shotTypes[shotTypeActual].overheatMaterialIndex];
            overheatColor = Color.Lerp(part.defaultColor, Color.red, timeShoting / shotTypes[shotTypeActual].overheat);
            part.gameObject.GetComponent<Renderer>().materials[part.materialIndex].color = overheatColor;
        }

        if (shotTypes[shotTypeActual].overheat == 0 || timeShoting < shotTypes[shotTypeActual].overheat)
        {
            if (shotTypes[shotTypeActual].delayToStart == 0 || timeShoting >= shotTypes[shotTypeActual].delayToStart)
            {

                GameObject obj = Instantiate(shotTypes[shotTypeActual].bullet, shotPos.position, shotPos.rotation);
                Bullet bul = obj.GetComponentInParent<Bullet>();
                bul.force = shotTypes[shotTypeActual].bulletForce;
                bul.damage = shotTypes[shotTypeActual].bulletDamage;
                bul.dono = this;
                obj.transform.rotation = Quaternion.LookRotation((Camera.main.transform.position + Camera.main.transform.forward * 10000) - transform.position);
                Destroy(obj, 6);
                recuo = shotTypes[shotTypeActual].timePerBullet;
                GameObject objFx = Instantiate(shotTypes[shotTypeActual].fx_Shot, shotPos.position, shotPos.rotation) as GameObject;
                Destroy(objFx, 1);
                ammoInPent -= 1;
            }

            if (animator)
            {
                if(shotTypes[shotTypeActual].otherAnimatinosFire.Length > 0)
                {
                    animator.Play(shotTypes[shotTypeActual].otherAnimatinosFire[shotAnimIndex]);
                    shotAnimIndex = (shotAnimIndex + 1) % shotTypes[shotTypeActual].otherAnimatinosFire.Length;
                }else
                    animator.Play("Shot");
            }
        }
        else
            overheated = true;

        if (ammoInPent <= 0)        
            if (shotTypes[shotTypeActual].enableWhenLoaded)
            shotTypes[shotTypeActual].enableWhenLoaded.SetActive(false);


        return true;

    }

    public bool Reload()
    {
        if (recuoReload > 0 || ammoInPent >= bulletsPerPent)
            return false;

        shotAnimIndex = 0;

        var ammoNeeded = bulletsPerPent - ammoInPent;

        var ammo = dono.RemoveAmmo(shotTypes[shotTypeActual].typeOfBullet, ammoNeeded);
        if (ammo <= 0)
            return false;

        if (animator)
        {
            if(shotTypes[shotTypeActual].reloadAnim.Length > 0)
                animator.Play(shotTypes[shotTypeActual].reloadAnim);
            else
                animator.Play("Reload");
        }
        var beforeInPent = ammoInPent;        
        ammoInPent = (ammo+ ammoInPent) >= bulletsPerPent ? bulletsPerPent : (ammo + ammoInPent);
        //ammoTotal -= ammoInPent - beforeInPent;
        recuoReload = reloadTime;
        if(shotTypes[shotTypeActual].enableWhenLoaded)
            shotTypes[shotTypeActual].enableWhenLoaded.SetActive(true);
        return true;
    }

    public void ChangeShotType(int witch)
    {
        offsetPos -= shotTypes[shotTypeActual].extraOffsetPos;
        offsetRot -= shotTypes[shotTypeActual].extraOffsetRot;
        offsetPosAim -= shotTypes[shotTypeActual].extraOffsetPosAim;
        offsetRotAim -= shotTypes[shotTypeActual].extraOffsetRotAim;
        if (shotTypes[shotTypeActual].enableWhenLoaded)
            shotTypes[shotTypeActual].enableWhenLoaded.SetActive(false);

        shotTypeActual = witch;
        if (shotTypeActual >= shotTypes.Length)
            shotTypeActual = 0;

        offsetPos += shotTypes[shotTypeActual].extraOffsetPos;
        offsetRot += shotTypes[shotTypeActual].extraOffsetRot;
        offsetPosAim += shotTypes[shotTypeActual].extraOffsetPosAim;
        offsetRotAim += shotTypes[shotTypeActual].extraOffsetRotAim;

        if (shotTypes[shotTypeActual].enableWhenLoaded)
            shotTypes[shotTypeActual].enableWhenLoaded.SetActive(true);

        if (dono)
            dono.EquipWeapon(this);
    }

    int defaultBulletPerPent;
    int defaultBulletMax;
    ShotType[] defaultShotTypes;
    Vector3 defaultOffsetPos;
    Vector3 defaultOffsetRot;
    Vector3 defaultOffsetPosAim;
    Vector3 defaultOffsetRotAim;

    public void UpdateCustoParts()
    {
        for (int i = 0; i < custoParts.Length; i++)
        {
            CustoParts part = custoParts[i];
            if (part.mods.Length > 0 && part.point)
            {
                if (part.lastModObjIndex != part.actualMod)
                {
                    if (part.lastModObjIndex != -1)
                    {
                        bulletsPerPent -= part.mods[part.lastModObjIndex].sum_bulletPerPent;
                        bulletsMax -= part.mods[part.lastModObjIndex].sum_bulletMax;
                        offsetPos -= part.mods[part.lastModObjIndex].sum_posOffset;
                        offsetRot -= part.mods[part.lastModObjIndex].sum_rotOffset;
                        offsetPosAim -= part.mods[part.lastModObjIndex].sum_posOffsetAim;
                        offsetRotAim -= part.mods[part.lastModObjIndex].sum_rotOffsetAim;
                        if (part.mods[part.lastModObjIndex].add_shotType.bullet)
                        {
                            RemoveAtFromArray(ref shotTypes, part.mods[part.lastModObjIndex].add_shotTypeIndex);
                            if (shotTypeActual >= shotTypes.Length)
                                ChangeShotType(0);
                        }
                        if (part.mods[part.lastModObjIndex].sum_shotTypes.Length > 0)
                        {
                            int index = part.mods[part.lastModObjIndex].sum_shotTypes.Length == shotTypes.Length ? -1 : 0;
                            for (int j = 0; j < shotTypes.Length; j++)
                            {
                                if (index < 0)
                                    index = j;
                                shotTypes[j].bullet = defaultShotTypes[j].bullet;
                                shotTypes[j].fx_Shot = defaultShotTypes[j].fx_Shot;
                                shotTypes[j].bulletDamage -= part.mods[part.lastModObjIndex].sum_shotTypes[index].bulletDamage;
                                shotTypes[j].bulletForce -= part.mods[part.lastModObjIndex].sum_shotTypes[index].bulletForce;
                                shotTypes[j].delayToStart -= part.mods[part.lastModObjIndex].sum_shotTypes[index].delayToStart;
                                shotTypes[j].overheat -= part.mods[part.lastModObjIndex].sum_shotTypes[index].overheat;
                                shotTypes[j].recoilForce -= part.mods[part.lastModObjIndex].sum_shotTypes[index].recoilForce;
                                //shotTypes[j].type -= defaultShotTypes[j].type;
                            }
                        }
                    }

                    if (part.actualModObj)
                        Destroy(part.actualModObj);

                    if (part.actualMod != -1)
                    {
                        GameObject obj = Instantiate(part.mods[part.actualMod].prefab, part.point.position, part.point.rotation, part.point);
                        obj.transform.Translate(part.mods[part.actualMod].offsetPos);
                        obj.transform.Rotate(part.mods[part.actualMod].offsetRot);
                        part.actualModObj = obj;


                        bulletsPerPent += part.mods[part.actualMod].sum_bulletPerPent;
                        bulletsMax = part.mods[part.actualMod].sum_bulletMax;
                        offsetPos += part.mods[part.actualMod].sum_posOffset;
                        offsetRot += part.mods[part.actualMod].sum_rotOffset;
                        offsetPosAim += part.mods[part.actualMod].sum_posOffsetAim;
                        offsetRotAim += part.mods[part.actualMod].sum_rotOffsetAim;
                        if (part.mods[part.actualMod].sum_shotTypes.Length > 0)
                        {
                            int index = part.mods[part.actualMod].sum_shotTypes.Length == shotTypes.Length ? -1 : 0;
                            for (int j = 0; j < shotTypes.Length; j++)
                            {
                                if (index < 0)
                                    index = j;
                                if (part.mods[part.actualMod].sum_shotTypes[index].bullet)
                                    shotTypes[j].bullet = part.mods[part.actualMod].sum_shotTypes[index].bullet;
                                if (part.mods[part.actualMod].sum_shotTypes[index].fx_Shot)
                                    shotTypes[j].fx_Shot = part.mods[part.actualMod].sum_shotTypes[index].fx_Shot;
                                shotTypes[j].bulletDamage += part.mods[part.actualMod].sum_shotTypes[index].bulletDamage;
                                shotTypes[j].bulletForce += part.mods[part.actualMod].sum_shotTypes[index].bulletForce;
                                shotTypes[j].delayToStart += part.mods[part.actualMod].sum_shotTypes[index].delayToStart;
                                shotTypes[j].overheat += part.mods[part.actualMod].sum_shotTypes[index].overheat;
                                shotTypes[j].recoilForce += part.mods[part.actualMod].sum_shotTypes[index].recoilForce;
                                //shotTypes[j].type -= part.mods[part.actualMod].sum_shotTypes[0].type;
                            }
                        }
                        if (part.mods[part.actualMod].add_shotType.bullet)
                        {
                            shotTypes = AddtoArray(shotTypes, part.mods[part.actualMod].add_shotType);
                            part.mods[part.actualMod].add_shotTypeIndex = shotTypes.Length - 1;
                        }


                        for (int j = 0; j < part.mods[part.actualMod].materialParts.Length; j++)
                        {
                            MaterialParts mat = part.mods[part.actualMod].materialParts[j];
                            Transform realObj = part.actualModObj.transform.Find(mat.gameObject.name);
                            if (!realObj)
                                realObj = part.actualModObj.transform;
                            print(part.actualModObj);
                            print(realObj.gameObject.name);
                            print(realObj.GetComponent<Renderer>().materials[mat.materialIndex]);
                            if (actualMat != -1)
                            {
                                realObj.GetComponent<Renderer>().materials[mat.materialIndex].mainTexture = custoMaterials[actualMat].texture;
                                realObj.GetComponent<Renderer>().materials[mat.materialIndex].color = custoMaterials[actualMat].color;
                            }
                            else
                            {
                                if (part.mods[part.actualMod].materialParts[j].hasCustomDefaultTextureAndColor)
                                {
                                    realObj.GetComponent<Renderer>().materials[mat.materialIndex].mainTexture = part.mods[part.actualMod].materialParts[j].defaultTexture;
                                    realObj.GetComponent<Renderer>().materials[mat.materialIndex].color = part.mods[part.actualMod].materialParts[j].defaultColor;
                                }
                                else
                                {
                                    realObj.GetComponent<Renderer>().materials[mat.materialIndex].mainTexture = materialParts[0].defaultTexture;
                                    realObj.GetComponent<Renderer>().materials[mat.materialIndex].color = materialParts[0].defaultColor;
                                }
                                if (part.mods[part.actualMod].materialParts[j].hasCustomColorOverhal)
                                {
                                    realObj.GetComponent<Renderer>().materials[mat.materialIndex].color = part.mods[part.actualMod].materialParts[j].colorOverhal;
                                }
                            }
                        }
                    }

                    for (int j = 0; j < part.disableWhenUse.Length; j++)
                        part.disableWhenUse[j].SetActive((part.actualMod != -1 && !part.mods[part.actualMod].dontDoWhenUseEffects) ? false : true);
                    for (int j = 0; j < part.enableWhenUse.Length; j++)
                        part.enableWhenUse[j].SetActive((part.actualMod != -1 && !part.mods[part.actualMod].dontDoWhenUseEffects) ? true : false);



                    part.lastModObjIndex = part.actualMod;
                    print("changed part");
                }
            }
        }
        print("updated parts");
    }

    public void UpdateCustoMats()
    {
        if (actualMat != lastMat)
        {
            for (int i = 0; i < materialParts.Length; i++)
            {
                if (actualMat != -1)
                {
                    materialParts[i].gameObject.GetComponent<Renderer>().materials[materialParts[i].materialIndex].mainTexture = custoMaterials[actualMat].texture;
                    materialParts[i].gameObject.GetComponent<Renderer>().materials[materialParts[i].materialIndex].color = custoMaterials[actualMat].color;

                }
                else
                {
                    materialParts[i].gameObject.GetComponent<Renderer>().materials[materialParts[i].materialIndex].mainTexture = materialParts[i].defaultTexture;
                    materialParts[i].gameObject.GetComponent<Renderer>().materials[materialParts[i].materialIndex].color = materialParts[i].defaultColor;

                }
            }
            lastMat = actualMat;
            print("updated mats");
            for (int i = 0; i < custoParts.Length; i++)
            {
                CustoParts part = custoParts[i];
                if (part.mods.Length > 0 && part.point)
                {
                    if (part.actualMod != -1)
                    {
                        for (int j = 0; j < part.mods[part.actualMod].materialParts.Length; j++)
                        {
                            MaterialParts mat = part.mods[part.actualMod].materialParts[j];
                            Transform realObj = part.actualModObj.transform.Find(mat.gameObject.name);
                            if (!realObj)
                                realObj = part.actualModObj.transform;
                            if (actualMat != -1)
                            {
                                realObj.GetComponent<Renderer>().materials[mat.materialIndex].mainTexture = custoMaterials[actualMat].texture;
                                realObj.GetComponent<Renderer>().materials[mat.materialIndex].color = custoMaterials[actualMat].color;
                            }
                            else
                            {
                                if (part.mods[part.actualMod].materialParts[j].hasCustomDefaultTextureAndColor)
                                {
                                    realObj.GetComponent<Renderer>().materials[mat.materialIndex].mainTexture = part.mods[part.actualMod].materialParts[j].defaultTexture;
                                    realObj.GetComponent<Renderer>().materials[mat.materialIndex].color = part.mods[part.actualMod].materialParts[j].defaultColor;
                                }
                                else
                                {
                                    realObj.GetComponent<Renderer>().materials[mat.materialIndex].mainTexture = materialParts[0].defaultTexture;
                                    realObj.GetComponent<Renderer>().materials[mat.materialIndex].color = materialParts[0].defaultColor;
                                }
                                if (part.mods[part.actualMod].materialParts[j].hasCustomColorOverhal)
                                {
                                    realObj.GetComponent<Renderer>().materials[mat.materialIndex].color = materialParts[0].colorOverhal;
                                }
                            }
                        }
                    }
                }

            }
        }
    }


    void OnDrawGizmos()
    {
        if (DEBUG_CREATESUBOPJECTS)
        {
            
            GameObject obj = new GameObject();
            obj.name = "shotPos";
            obj.transform.position = transform.position;
            obj.transform.rotation = transform.rotation;
            obj.transform.localScale = new Vector3(0.32f, 0.32f, 0.32f);
            obj.transform.parent = transform;
            shotPos = obj.transform;

            custoParts = new CustoParts[6];

            for(int i = 0; i < 6; i++)
            {
                obj = new GameObject();
                obj.transform.position = transform.position;
                obj.transform.rotation = transform.rotation;
                obj.transform.localScale = new Vector3(0.32f, 0.32f, 0.32f);
                obj.transform.parent = transform;
                obj.name = "custoPart (" + i.ToString() + ")";
                custoParts[i] = new CustoParts();
                custoParts[i].point = obj.transform;
            }
            DEBUG_CREATESUBOPJECTS = false;
        }
        if (DEBUG_GETMATERIALPARTS_FIRST || DEBUG_GETMATERIALPARTS_ALL)
        {
            if (materialParts.Length == 0)
            {
                List<MaterialParts> ps = new List<MaterialParts>();
                for (int i = 0; i < transform.childCount; i++)
                {
                    if (transform.GetChild(i).GetComponent<Renderer>())
                    {
                        MaterialParts p = new MaterialParts();
                        p.gameObject = transform.GetChild(i);
                        ps.Add(p);
                        if (DEBUG_GETMATERIALPARTS_FIRST)
                            break;
                    }
                }
                materialParts = ps.ToArray();
            }

            DEBUG_GETMATERIALPARTS_ALL = false;
            DEBUG_GETMATERIALPARTS_FIRST = false;
        }

        DataBase dt = FindObjectOfType<DataBase>();
        if (dt) {
            for (int i = 0; i < shotTypes.Length; i++)
            {
                if (shotTypes[i].typeOfBullet >= 0 && shotTypes[i].typeOfBullet < dt.bulletTypes.Length)
                {
                    if (shotTypes[i].DEBUG_typeActual != dt.bulletTypes[shotTypes[i].typeOfBullet].name)
                    {
                        shotTypes[i].DEBUG_typeActual = dt.bulletTypes[shotTypes[i].typeOfBullet].name;
                    }
                }
                else
                    shotTypes[i].DEBUG_typeActual = "NOT VALID";
            }
            for (int i = 0; i < custoParts.Length; i++)
            {
                for (int j = 0; j < custoParts[i].mods.Length; j++)
                {
                    CustoParts.Mods mod = custoParts[i].mods[j];
                    if (mod.add_shotType.typeOfBullet >= 0 && mod.add_shotType.typeOfBullet < dt.bulletTypes.Length)
                    {
                        if (mod.add_shotType.DEBUG_typeActual != dt.bulletTypes[mod.add_shotType.typeOfBullet].name)
                        {
                            mod.add_shotType.DEBUG_typeActual = dt.bulletTypes[mod.add_shotType.typeOfBullet].name;
                        }
                    }
                    else
                        mod.add_shotType.DEBUG_typeActual = "NOT VALID";

                   /* for (int k = 0; k < custoParts[i].mods[j].sum_shotTypes.Length; k++)
                    {
                        ShotType shot = custoParts[i].mods[j].sum_shotTypes[k];
                        if (shot.typeOfBullet >= 0 && shot.typeOfBullet < dt.bulletTypes.Length)
                        {
                            if (shot.DEBUG_typeActual != dt.bulletTypes[shot.typeOfBullet].name)
                            {
                                shot.DEBUG_typeActual = dt.bulletTypes[shot.typeOfBullet].name;
                            }
                        }
                        else
                            shot.DEBUG_typeActual = "NOT VALID";
                    }*/
                }
            }
        }

    }

    T[] AddtoArray<T>(T[] Org, T New_Value)
    {
        T[] New = new T[Org.Length + 1];
        Org.CopyTo(New, 0);
        New[Org.Length] = New_Value;
        return New;
    }

    public static void RemoveAtFromArray <T>(ref T[] arr, int index)
    {
        for (int a = index; a < arr.Length - 1; a++)
        {
            // moving elements downwards, to fill the gap at [index]
            arr[a] = arr[a + 1];
        }
        // finally, let's decrement Array's size by one
        Array.Resize(ref arr, arr.Length - 1);
    }

}
