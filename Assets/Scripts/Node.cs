//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.EventSystems;

//public class Node : MonoBehaviour
//{
//    public Color hoverColor;
//    public Color notEnoughMoneyColor;
//    public Vector3 positionOffset;
//    public bool isOccupied = false;

//    [Header("Optional")]
//    public GameObject turret;

//    private Renderer rend;
//    private Color startColor;
//    private BuildManager buildManager;

//    void Start()
//    {
//        rend = GetComponent<Renderer>();
//        startColor = rend.material.color;
//        buildManager = BuildManager.instance;
//    }

//    public Vector3 GetBuildPosition()
//    {
//        return transform.position + positionOffset;
//    }

//    void OnMouseDown()
//    {
//        if (EventSystem.current.IsPointerOverGameObject())
//            return;

//        if (buildManager.isCombining)
//        {
//            // 합성 모드가 활성화된 경우, 터렛 합성을 시도합니다.
//            if (turret != null)
//            {
//                buildManager.TryRandomCombineTurrets(this);
//            }
//            else
//            {
//                Debug.Log("No turret on this node to combine.");
//            }
//        }
//        else if (buildManager.CanBuild)
//        {
//            if (turret != null)
//                return;

//            buildManager.BuildTurretOn(this);
//        }
//    }

//    void OnMouseEnter()
//    {
//        if (EventSystem.current.IsPointerOverGameObject())
//            return;

//        if (!buildManager.CanBuild && !buildManager.isCombining)
//            return;

//        rend.material.color = buildManager.HasMoney ? hoverColor : notEnoughMoneyColor;
//    }

//    void OnMouseExit()
//    {
//        rend.material.color = startColor;
//    }

//    public void PlaceTurret(GameObject turretPrefab)
//    {
//        if (turret != null)
//            return;

//        turret = Instantiate(turretPrefab, GetBuildPosition(), Quaternion.identity);
//        isOccupied = true;
//    }

//    public void RemoveTurret()
//    {
//        if (turret == null)
//            return;

//        Destroy(turret);
//        turret = null;
//        isOccupied = false;
//    }
//}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{
    public Color hoverColor;
    public Color notEnoughMoneyColor;
    public Vector3 positionOffset;
    public bool isOccupied = false;

    [Header("Optional")]
    public GameObject turret;

    private Renderer rend;
    private Color startColor;
    private BuildManager buildManager;

    void Start()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
        buildManager = BuildManager.instance;
    }

    public Vector3 GetBuildPosition()
    {
        return transform.position + positionOffset;
    }

    void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (buildManager.isSelling)
        {
            // 판매 모드가 활성화된 경우, 터렛을 판매합니다.
            buildManager.SellTurret(this);
        }
        else if (buildManager.isCombining)
        {
            // 합성 모드가 활성화된 경우, 터렛 합성을 시도합니다.
            if (turret != null)
            {
                buildManager.TryRandomCombineTurrets(this);
            }
            else
            {
                Debug.Log("No turret on this node to combine.");
            }
        }
        else if (buildManager.CanBuild)
        {
            if (turret != null)
                return;

            buildManager.BuildTurretOn(this);
        }
    }

    void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (!buildManager.CanBuild && !buildManager.isCombining && !buildManager.isSelling)
            return;

        rend.material.color = buildManager.HasMoney ? hoverColor : notEnoughMoneyColor;
    }

    void OnMouseExit()
    {
        rend.material.color = startColor;
    }

    public void PlaceTurret(GameObject turretPrefab)
    {
        if (turret != null)
            return;

        turret = Instantiate(turretPrefab, GetBuildPosition(), Quaternion.identity);
        isOccupied = true;
    }

    public void RemoveTurret()
    {
        if (turret == null)
            return;

        Destroy(turret);
        turret = null;
        isOccupied = false;
    }
}