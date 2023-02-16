using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwordAttack : MonoBehaviour
{
    public GameObject tip;
    public Image dot;
    public Color enemyDotColor = Color.red;
    public float swordRange = 1.5f;
    GameObject player;
    Color neutralDotColor;
    bool attacked;
    TrailRenderer trail;
    // Start is called before the first frame update
    void Start()
    {
        attacked = false;
        trail = tip.GetComponent<TrailRenderer>();
        trail.enabled = false;
        neutralDotColor = dot.color;
        player = GameObject.FindGameObjectWithTag("Player");

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !attacked)
        {
            Attack();
        }
        

    }

    void Attack()
    {
        StartCoroutine(AttackAnimation());
        RaycastHit hit;
        //Debug.Log("Attack");

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity))
        {
            //Debug.Log("Hitting Anything");
            if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                //Debug.Log("enemy direction correct");
                float distance = Vector3.Distance(player.transform.position, hit.transform.position);
                if (distance <= swordRange)
                {
                    Destroy(hit.collider.gameObject);
                }
            }
        }
    }

    void FixedUpdate()
    {
        dotUpdate();
    }

    void dotUpdate()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                dot.color = Color.Lerp(dot.color, enemyDotColor, Time.deltaTime * 10);

            }
            else
            {
                dot.color = neutralDotColor;
            }
        }
    }



    
    IEnumerator AttackAnimation()
    {
        trail.enabled = true;
        attacked = true;
        GetComponent<Animator>().SetTrigger("Attack");

        yield return new WaitForSeconds(0.6f);
        attacked = false;
        trail.enabled = false;
    }
}
