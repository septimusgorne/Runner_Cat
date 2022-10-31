using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class CoinController : MonoBehaviour

public class CoinController : Singletone<CoinController>
{

    public float speed = 10f;

    //GameManager gameManager;

    float rotationSpeed = 100;
    void Start()
    {
        /*gameManager = GameObject.FindObject.FindObjectOfType<GameManager>();
        Rigidbody rigidBody = GetComponent<Rigidbody>();
        rigidBody.velocity = new Vector3(0f, speed);*/

        rotationSpeed += Random.Range(0, rotationSpeed / 4.0f);
    }

    void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameManager.Instance.AddScore();
            transform.parent.gameObject.SetActive(false);
            //Destroy(gameObject);///dont destroy! GO FALSE
        }
    }
}