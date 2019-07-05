using UnityEngine;

public class BezierCurve : MonoBehaviour {

	public Vector3[] points;
    public Vector3 relMoveTo;
    public float speedMultiplier;
    private float speed;
    private float distance;
    private Vector3 startPoint;
    private bool moveTo;
    private void Start()
    {
        startPoint = points[3];
        speedMultiplier *= Time.deltaTime;
        distance = Vector3.Distance(Vector3.zero, relMoveTo);
    }

    private void Update()
    {
        speed = Vector3.Distance(points[3], startPoint);
        speed /= distance;
        speed -= 0.5f;
        speed = Mathf.Abs(speed);
        speed = 0.5f - speed;

        if (speed < 0.05f)
            speed = 0.05f;
        if (moveTo)
            points[3] += relMoveTo * (speed * speedMultiplier);
        if (!moveTo)
            points[3] -= relMoveTo * (speed * speedMultiplier);
        if (Vector3.Distance(points[3], relMoveTo + startPoint) > distance)
            moveTo = true;
        if (Vector3.Distance(points[3], startPoint) > distance)
            moveTo = false;
    }

    public Vector3 GetPoint (float t) {
		return transform.TransformPoint(Bezier.GetPoint(points[0], points[1], points[2], points[3], t));
	}
	
	public Vector3 GetVelocity (float t) {
		return transform.TransformPoint(Bezier.GetFirstDerivative(points[0], points[1], points[2], points[3], t)) - transform.position;
	}
	
	public Vector3 GetDirection (float t) {
		return GetVelocity(t).normalized;
	}
	
	public void Reset () {
		points = new Vector3[] {
			new Vector3(1f, 0f, 0f),
			new Vector3(2f, 0f, 0f),
			new Vector3(3f, 0f, 0f),
			new Vector3(4f, 0f, 0f)
		};
	}
}