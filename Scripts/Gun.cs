using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] PlayerControl playerControl;
    [SerializeField] Camera playerCamera;
    [SerializeField] Transform gun;
    [SerializeField] GameObject bullet;

    [SerializeField] float bulletSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(playerControl.dstFromTarget > 0)
        {
            Ray mouseRay = playerCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(mouseRay, out RaycastHit mouseHit))
            {
                gun.rotation = Quaternion.LookRotation(mouseHit.point - gun.position, transform.up);
                Debug.DrawLine(gun.position, mouseHit.point, Color.green, 0.1f);
            }
            else
            {
                gun.rotation = Quaternion.LookRotation(mouseRay.direction, transform.up);
                Debug.DrawRay(mouseRay.origin, mouseRay.direction, Color.red, 0.1f);
            }
        }
        else
        {
            gun.rotation = playerCamera.transform.rotation;
        }

        if (Input.GetMouseButtonDown(0))
        {
            GameObject newBullet = Instantiate(bullet, gun.position + gun.forward * 0.5f, gun.rotation);
            newBullet.GetComponent<Rigidbody>().velocity = gun.forward * bulletSpeed;
        }
    }
}
