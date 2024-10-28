using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    public float mimamount;
    public float maxamount;
    [Range(0f, 10f)]
    public float smoothamount;

    private Vector3 initialPos;
    void Start()
    {
        initialPos = transform.localPosition; // Posicao local do objeto herdada pelo objeto pai
    }

    // Update is called once per frame
    void Update()
    {
        // Moviimentar a arma para posições diferentes com suavizacao
        float moveX = -Input.GetAxis("Mouse X") * mimamount;
        float moveY = -Input.GetAxis("Mouse Y") * mimamount;

        moveX = Mathf.Clamp(moveX,- maxamount, maxamount);
        moveY = Mathf.Clamp(moveY, -maxamount, maxamount);

        Vector3 finalPos = new Vector3(moveX, moveY, 0f);

        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPos + initialPos, Time.deltaTime * smoothamount);
    }
}
