using UnityEngine;

public class VisionCone : MonoBehaviour
{
    public Transform target; // Esta variable indica el objetivo que va a detectar
    public float detectionRange = 10f; // Distancia máxima para detectar el objetivo
    public float fieldOfView = 45f; // Ángulo del cono de visión
    public Color detectedColor = Color.red; // Color cuando detecta al objetivo
    public Color nodetection = Color.green; // Color cuando no detecta al objetivo
    public bool showVisionCone = true; // Para mostrar u ocultar la visualización del cono de visión

    private Renderer _renderer; // Renderer para cambiar el color

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        // Revisar si el objetivo está en el cono de visión
        if (IsTargetInVisionCone())
        {
            _renderer.material.color = detectedColor; // Cambia de color si detecta el objetivo
        }
        else
        {
            _renderer.material.color = nodetection; // Cambia de color cuando no detecta el objetivo
        }
    }

    // Función para verificar si el objetivo está en el cono de visión
    private bool IsTargetInVisionCone()
    {
        Vector3 directionToTarget = target.position - transform.position;
        float distanceToTarget = directionToTarget.magnitude;

        // Verificar si el objetivo está dentro del rango de detección
        if (distanceToTarget > detectionRange)
            return false;

        // Calcular el ángulo entre la dirección frontal del agente y la dirección hacia el objetivo
        float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

        // Verificar si el ángulo está dentro del campo de visión
        return angleToTarget < fieldOfView / 2;
    }

    // Método para dibujar el cono de visión en la vista de la Escena
    private void OnDrawGizmos()
    {
        if (showVisionCone)
        {
            Gizmos.color = nodetection;
            Gizmos.DrawWireSphere(transform.position, detectionRange); // Dibujar el rango de detección como esfera

            // Dibujar el cono de visión
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