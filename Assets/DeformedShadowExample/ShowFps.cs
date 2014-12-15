using UnityEngine;

public class ShowFps : MonoBehaviour
{
	public float interval = 1f;
	float lastTime;
	int frame;
	[SerializeField] float fps;
	public UnityEngine.UI.Text text;

	void Start ()
	{
		lastTime = Time.realtimeSinceStartup;
	}

	void Update ()
	{
		frame++;
		float time = Time.realtimeSinceStartup - lastTime;
		if (time >= interval) {
			fps = frame / time;
			text.text = fps.ToString("FPS 00.00");
			lastTime = Time.realtimeSinceStartup;
			frame = 0;
		}
	}
}
