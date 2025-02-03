using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Gun Data")]
    [SerializeField] private Transform gunPoint;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private GameObject bulletPrefab;

    [Header("Movement Data")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float  rotationSpeed;
    
    private float verticalInput;
    private float horizontalInput;

    [Header("Tower Data")]
    [SerializeField] private Transform towerTransform;
    [SerializeField] private float towerRotationSpeed;

    [Header("Aim Data")]
    [SerializeField] private LayerMask whatIsAimMask;
    [SerializeField] private Transform aimTransform;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    void Update()
    {
        UpdateAim();
        CheckInputs();
    }

    private void CheckInputs()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
            Shoot();

        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");

        if (verticalInput < 0)
            horizontalInput = -horizontalInput;

        
    }

    private void FixedUpdate()
    {
        ApplyMovement();

        transform.Rotate(0, horizontalInput * rotationSpeed, 0);
        ApplyTowerRotation();
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, gunPoint.position, gunPoint.rotation);

        bullet.GetComponent<Rigidbody>().velocity = gunPoint.forward * bulletSpeed;

        Destroy(bullet, 7);
    }

    private void ApplyTowerRotation()
    {
        Vector3 deirection = aimTransform.position - towerTransform.position;
        deirection.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(deirection);

        towerTransform.rotation = Quaternion.RotateTowards(towerTransform.rotation, targetRotation, towerRotationSpeed);
    }

    private void ApplyMovement()
    {
        Vector3 movement = transform.forward * moveSpeed * verticalInput;
        rb.velocity = movement;
    }

    private void UpdateAim()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, whatIsAimMask))
        {
            float fixedY = aimTransform.position.y;
            aimTransform.position = new Vector3(hit.point.x, fixedY, hit.point.z);
        }
    }
}
