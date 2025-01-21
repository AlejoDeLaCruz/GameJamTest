using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public PlayerMovement controller;

    [SerializeField] private float baseSpeed = 5f; // Velocidad base del jugador
    [SerializeField] private float iceSpeedMultiplier; // Multiplicador de velocidad en hielo.

    private float currentRunSpeed; // Velocidad actual que puede ser modificada por objetos.

    float horizontalMove = 0f;
    bool jump = false;
    bool crouch = false;

    private bool onIceFloor = false; // Indica si el jugador está sobre hielo.

    private void Start()
    {
        currentRunSpeed = baseSpeed; // Inicializa la velocidad actual con la velocidad base.
    }

    void Update()
    {
        // Ajusta la velocidad según el estado (sobre hielo o no).
        currentRunSpeed = onIceFloor ? baseSpeed * iceSpeedMultiplier : baseSpeed;

        horizontalMove = Input.GetAxisRaw("Horizontal") * currentRunSpeed;

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }
    }

    void FixedUpdate()
    {
        // Mover al personaje.
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }

    // Detectar si el jugador entra en contacto con el hielo.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("iceFloor"))
        {
            onIceFloor = true;
        }
    }

    // Detectar si el jugador sale del contacto con el hielo.
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("iceFloor"))
        {
            onIceFloor = false;
        }
    }

    // Método para ajustar la velocidad.
    public void AdjustSpeed(float speedAdjustment)
    {
        currentRunSpeed = baseSpeed + speedAdjustment; // Asegúrate de que se sume correctamente
        Debug.Log("Velocidad ajustada: " + currentRunSpeed); // Para ver en consola si la velocidad cambia
    }
}