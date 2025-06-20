using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AutoNPC : MonoBehaviour
{
    public Transform[] puntosRuta;
    public float velocidad = 5f;
    public float velocidadGiro = 2f;

    // Para animar las ruedas
    public Transform[] rotatingWheelsLeft;
    public Transform[] rotatingWheelsRight;
    public float rotationSpeed = 300f; // velocidad para rotar las ruedas al avanzar
    public float steerAngleMax = 30f;  // ángulo máximo de giro de las ruedas delanteras

    private int indiceActual = 0;
    private Rigidbody rb;

    private Vector3 direccionAnterior;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        direccionAnterior = transform.forward;
    }

    void FixedUpdate()
    {
        if (puntosRuta.Length == 0) return;

        Transform objetivo = puntosRuta[indiceActual];
        Vector3 direccion = (objetivo.position - transform.position).normalized;

        // Rotación suave
        if (direccion != Vector3.zero)
        {
            Quaternion rotacionDeseada = Quaternion.LookRotation(direccion);
            Quaternion rotacionSuave = Quaternion.Slerp(rb.rotation, rotacionDeseada, velocidadGiro * Time.fixedDeltaTime);
            rb.MoveRotation(rotacionSuave);
        }

        // Movimiento hacia adelante
        Vector3 movimiento = transform.forward * velocidad * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movimiento);

        // Cambiar al siguiente punto
        if (Vector3.Distance(transform.position, objetivo.position) < 1f)
        {
            indiceActual = (indiceActual + 1) % puntosRuta.Length;
        }

        // --- Animar ruedas ---

        // 1. Rotar ruedas que giran para avanzar
        foreach (Transform wheel in rotatingWheelsLeft)
        {
            // Rotar en torno al eje X (Vector3.left)
            wheel.Rotate(Vector3.right, rotationSpeed * Time.fixedDeltaTime);
        }
        foreach (Transform wheel in rotatingWheelsRight)
        {
            // Rotar en torno al eje X (Vector3.left)
            wheel.Rotate(Vector3.left, rotationSpeed * Time.fixedDeltaTime);
        }

        direccionAnterior = transform.forward;
    }
}
