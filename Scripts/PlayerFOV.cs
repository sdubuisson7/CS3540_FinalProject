using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFOV : MonoBehaviour
{
    public float distance = 5.0f;
    public float fov = 100;
    public Transform enemyEyes;
    GameObject player;
    Canvas enemyHUD;
    // EnemyAI EnemyAI;

    // Start is called before the first frame update
    void Start()
    {
        enemyHUD = GetComponentInChildren<Canvas>();
        player = GameObject.FindGameObjectWithTag("Player");

    }

    // Update is called once per frame
    void Update()
    {

        // for (int i = 0; i < enemies.Length; i++) {
        if (inClearFOV()) {
            enemyHUD.gameObject.SetActive(true);
        }
        else {
            enemyHUD.gameObject.SetActive(false);
        }
    }


    private void OnDrawGizmos() 
    {

        // Line from eyes to chaseDistance
        Vector3 frontRayPoint = enemyEyes.position + (enemyEyes.forward * distance);

        // left/right rays in fov. Transforms front ray by 22.5 degrees
        Vector3 leftRayPoint = Quaternion.Euler(0, fov * 0.5f, 0) * frontRayPoint;
        Vector3 rightRayPoint = Quaternion.Euler(0, -fov * 0.5f, 0) * frontRayPoint;

        // lines for FOV
        Debug.DrawLine(enemyEyes.position, frontRayPoint, Color.cyan);
        Debug.DrawLine(enemyEyes.position, leftRayPoint, Color.yellow);
        Debug.DrawLine(enemyEyes.position, rightRayPoint, Color.yellow);
    }

    // check to see if player is in clear FOV of the NPC
    bool inClearFOV() 
    {
        Vector3 directionToPlayer = player.transform.position - enemyEyes.position;
        RaycastHit hit;
        if(Vector3.Angle(directionToPlayer, enemyEyes.forward) <= fov) 
        {
            // check to see that the line between the two is clear by using raycast
            if (Physics.Raycast(enemyEyes.position, directionToPlayer, out hit, distance)) 
            {
                if(hit.collider.CompareTag("Player"))
                {
                    print("Player in sight!");
                    return true;
                }
                return false;
            }
            return false;
        }
        return false;

    }
}
