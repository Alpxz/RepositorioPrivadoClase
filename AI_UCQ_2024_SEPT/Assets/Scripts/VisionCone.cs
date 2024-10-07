using UnityEngine;

public class VisionCone : MonoBehaviour
{
    public Transform target; // Esta variable indica el objetivo que va a detectar
    public float detectionRange = 10f; // Distancia m�xima para detectar el objetivo
    public float fieldOfView = 45f; // �ngulo del cono de visi�n
    public Color detectedColor = Color.red; // Color cuando detecta al objetivo
    public Color nodetection = Color.green; // Color cuando no detecta al objetivo
    public bool showVisionCone = true; // Para mostrar u ocultar la visualizaci�n del cono de visi�n

    private Renderer _renderer; // Renderer para cambiar el color

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        // Revisar si el objetivo est� en el cono de visi�n
        if (IsTargetInVisionCone())
        {
            _renderer.material.color = detectedColor; // Cambia de color si detecta el objetivo
        }
        else
        {
            _renderer.material.color = nodetection; // Cambia de color cuando no detecta el objetivo
        }
    }

    // Funci�n para verificar si el objetivo est� en el cono de visi�n
    private bool IsTargetInVisionCone()
    {
        Vector3 directionToTarget = target.position - transform.position;
        float distanceToTarget = directionToTarget.magnitude;

        // Verificar si el objetivo est� dentro del rango de detecci�n
        if (distanceToTarget > detectionRange)
            return false;

        // Calcular el �ngulo entre la direcci�n frontal del agente y la direcci�n hacia el objetivo
        float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

        // Verificar si el �ngulo est� dentro del campo de visi�n
        return angleToTarget < fieldOfView / 2;
    }

    // M�todo para dibujar el cono de visi�n en la vista de la Escena
    private void OnDrawGizmos()
    {
        if (showVisionCone)
        {
            Gizmos.color = nodetection;
            Gizmos.DrawWireSphere(transform.position, detectionRange); // Dibujar el rango de detecci�n como esfera

            // Dibujar el cono de visi�n
            Vector3 forward = transform.forward * detectionRange;
            Quaternion leftRayRotation = Quaternion.AngleAxis(-fieldOfView / 2, Vector3.up);
            Quaternion rightRayRotation = Quaternion.AngleAxis(fieldOfView / 2, Vector3.up);

            Vector3 leftRayDirection = leftRayRotation * forward;
            Vector3 rightRayDirection = rightRayRotation * forward;

            Gizmos.color = _renderer != null && _renderer.material.color == detectedColor ? detectedColor : nodetection;
            Gizmos.DrawRay(transform.position, leftRayDirection); // Lado izquierdo del cono
            Gizmos.DrawRay(transform.position, rightRayDirection); // Lado derecho del cono
        }
    }
}