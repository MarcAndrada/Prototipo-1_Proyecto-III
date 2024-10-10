using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private enum CameraMovement { NONE, ZOOM_IN, ZOOM_OUT };

    [SerializeField]
    private CameraMovement camState = CameraMovement.NONE;


    [Header("Players"), SerializeField]
    private List<Collider> playerColliders;
    [SerializeField]
    private List<PlayerController> playerControllers;

    [Header("Players Variables"), SerializeField]
    private float minYDistance;
    [SerializeField]
    private float zOffset;
    private float playersY;

    [Space, Header("Cameras"), SerializeField]
    private Camera insideCamera;
    [SerializeField]
    private Camera externalCamera;

    [Header("Cameras Variables"), SerializeField, Range(0, 1)]
    private float movementSpeed;
    [SerializeField]
    private float zoomOutSpeed;
    [SerializeField]
    private float zoomInSpeed;
    [SerializeField]
    private float XZSpeed;
    
    



    private void Start()
    {
        //Guardamos la Y del primer Player
        playersY = playerColliders[0].transform.position.y;
        //Seteamos todos los players con la misma posicion en Y
        foreach (Collider item in playerColliders)
            item.transform.position = new Vector3(item.transform.position.x, playersY, item.transform.position.z);
        
        //colocar la camara a la distancia minima
        zOffset = transform.position.z - GetMiddlePointBetweenPlayers().z;
    }

    public void AddPlayer(GameObject _newPlayer)
    {
        playerColliders.Add(_newPlayer.GetComponent<CapsuleCollider>());
        playerControllers.Add(_newPlayer.GetComponent<PlayerController>());
        _newPlayer.transform.position = new Vector3(_newPlayer.transform.position.x, playersY, _newPlayer.transform.position.z);
    }
    public void RemovePlayer(GameObject _removablePlayer)
    {
        playerColliders.Remove(_removablePlayer.GetComponent<CapsuleCollider>());
        playerControllers.Remove(_removablePlayer.GetComponent<PlayerController>());
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        CheckCamDistance();
        MoveCamera();
    }

    private void CheckCamDistance()
    {
        bool zoomIn = true;
        bool zoomOut = false;
        List<Collider> activePlayers = new List<Collider>();
        for (int i = 0; i < playerControllers.Count; i++)
        {
            if (playerControllers[i].isAlive)
                activePlayers.Add(playerColliders[i]);
        }

        foreach (Collider item in activePlayers)
        {
            //Obtenemos los planos que utiliza el fustrum de la camara externa
            Plane[] camFrustrum = GeometryUtility.CalculateFrustumPlanes(externalCamera);
            
            //Comprobamos si el player esta dentro del frustrum
            if (GeometryUtility.TestPlanesAABB(camFrustrum, item.bounds)) //Si esta dentro de la camara
            {
                //Obtenemos los planos que utiliza el fustrum de la camara interna
                camFrustrum = GeometryUtility.CalculateFrustumPlanes(insideCamera);

                if (!GeometryUtility.TestPlanesAABB(camFrustrum, item.bounds)) //Si hay algun player fuera de la camara interna NO hacer ZOOM_IN 
                    zoomIn = false;

            }
            else //Si esta fuera de la camara exterior alejamos la cam                
                zoomOut = true;

        }

        if (zoomOut)
            camState = CameraMovement.ZOOM_OUT;
        else if(zoomIn)
            camState = CameraMovement.ZOOM_IN;
        else
            camState = CameraMovement.NONE;

    }

    private void MoveCamera() 
    {
        Vector3 destinyPos = transform.position;

        Vector3 middlePos = GetMiddlePointBetweenPlayers();

        if (middlePos == Vector3.zero)
            return;

        Vector3 XZDir = new Vector3
            (
            middlePos.x - transform.position.x,
            0,
            (middlePos.z + zOffset) - transform.position.z
            );
        destinyPos += XZDir * XZSpeed;

        if (camState != CameraMovement.NONE)
        {
            float zoomSpeed = 1;
            switch (camState)
            {
                case CameraMovement.ZOOM_IN:
                    zoomSpeed = -zoomInSpeed;
                    break;
                case CameraMovement.ZOOM_OUT:
                    zoomSpeed = zoomOutSpeed;
                    break;
                default:
                    break;
            }
            destinyPos += -transform.forward * zoomSpeed;
            zOffset -= zoomSpeed * Time.fixedDeltaTime;
        }

        Vector3 finalPos = Vector3.Lerp
            (
            transform.position,
            destinyPos,
            movementSpeed * Time.fixedDeltaTime
            );
        finalPos.y = Mathf.Clamp(finalPos.y, minYDistance, Mathf.Infinity);

        transform.position = finalPos;
        
    }
    private Vector3 GetMiddlePointBetweenPlayers()
    {
        Vector3 middlePoint = Vector3.zero;

        foreach (PlayerController item in playerControllers)
        {
            if (item.isAlive)
                middlePoint += item.transform.position;

        }
        if (middlePoint == Vector3.zero)
            return Vector3.zero;

        middlePoint.y = playersY;
        middlePoint /= playerColliders.Count;
        return middlePoint;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(GetMiddlePointBetweenPlayers(),0.4f);
    }


    
}
