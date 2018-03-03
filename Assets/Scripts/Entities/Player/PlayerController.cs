﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerController.cs" by="Akapagion">
//  © Copyright Dauler Palhares da Costa Vianna 2018.
//          http://github.com/DaulerPalhares
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public bool ToTestRemoveLater;
    /// <summary>
    /// Player Nickname
    /// </summary>
    public string NickName;
    /// <summary>
    /// Player stats.
    /// </summary>
    [HideInInspector]public PlayerStats PlayerStats;
    /// <summary>
    /// This game object.
    /// </summary>
    [HideInInspector]public GameObject MySelf;
    /// <summary>
    /// Movment Speed Amount.
    /// </summary>
    public float Speed;
    /// <summary>
    /// Amount player gold.
    /// </summary>
    public int GoldAmount { get; protected set; }
    /// <summary>
    /// Amount player cash.
    /// </summary>
    public int CashAmount { get; protected set; }
    /// <summary>
    /// NavMesh agent to move.
    /// </summary>
    protected NavMeshAgent NavMeshAgent;
    /// <summary>
    /// The character rigidybody.
    /// </summary>
    protected Rigidbody MyRigidyBody;


    private void Start()
    {
        NavMeshAgent = GetComponent<NavMeshAgent>();
        if (!ToTestRemoveLater)
        {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().Player = this;
        }

        MyRigidyBody = gameObject.GetComponent<Rigidbody>();
        MyRigidyBody.constraints = RigidbodyConstraints.FreezeRotation;
        MyRigidyBody.isKinematic = false;
        PlayerStats.AddHealth(int.MaxValue);
    }

    /// <summary>
    /// Click to move the character in the world.
    /// </summary>
    private void ClickToMove()
    {
        if (Input.GetMouseButton(1))
        {
            Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(cameraRay, out hit))
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    NavMeshAgent.isStopped = true;
                    //Attack;
                }
                else
                {
                    NavMeshAgent.destination = hit.point;
                    NavMeshAgent.isStopped = false;
                }
            }
        }
    }

    /// <summary>
    /// Character aways looking to the mouse cursor.
    /// </summary>
    private void LookToMouse()
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLenght;
        if (groundPlane.Raycast(cameraRay, out rayLenght))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLenght);
            Debug.DrawLine(cameraRay.origin, pointToLook, Color.black);

            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }
    }

    private void Update()
    {
        if (ToTestRemoveLater)
        {
            LookToMouse();
            ClickToMove();
        }
        else if (!GameController.Instance.IsPaused)
        {
            LookToMouse();
            ClickToMove();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            PlayerStats.AddExperience(10);
        }
    }

}