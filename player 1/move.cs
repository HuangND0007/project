using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class move : MonoBehaviour
{
    public float speed = 5f;
    private void Update()
    {
        float movex = Input.GetAxis("Horizontal");
        float movey = Input.GetAxis("Vertical");
        Vector2 movement=new Vector2(movex, movey);
        transform.Translate(movement * speed * Time.deltaTime);
    }
}
