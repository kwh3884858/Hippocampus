using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

	public float m_length = 2f;
	public float m_velocityHorizontal = 1;

	[Tooltip ("每相邻的子弹之间的间隔角度")]
	public float m_angleInterval = 15;

	[Tooltip ("半个扇形的子弹数量，最后射出的总数量为 2 * 半边 + 1")]
	public int m_halfSideBulletNumber = 2;

	private int [] m_shootingSide = new int [] { 1, -1 };

	public LayerMask m_layer;

	//public float m_halfAngle = 40f;
	public float m_rayLength = 2f;
	//public float m_interval = 10f;

	private float m_current = 0;
	private RaycastHit2D hit;

	MeshFilter filter;

	Vector2 [] m_vertices;
	// Use this for initialization
	void Start ()
	{
		m_current = m_length;
		//m_vertices = new Vector2 [m_halfSideBulletNumber * 6];


		// Set up game object with mesh;
		//var meshRenderer = gameObject.AddComponent<MeshRenderer> ();
		//meshRenderer.material = new Material (Shader.Find ("Sprites/Default"));
		//filter = gameObject.AddComponent<MeshFilter> ();
	}

	// Update is called once per frame
	void Update ()
	{
		Vector3 vec = Vector3.right * transform.localScale.x;
		//int offset = 0;


		for (int i = 0; i < m_shootingSide.Length; i++) {

			for (int j = 0; j < m_halfSideBulletNumber; j++) {
				Vector3 angle = Quaternion.AngleAxis (m_shootingSide [i] * (j + 1) * m_angleInterval, Vector3.forward) * vec;
				Vector3 end = transform.position + angle * m_rayLength;
				Debug.DrawLine (transform.position, end, Color.red);

				hit = Physics2D.Raycast (transform.position, angle, m_rayLength, m_layer);
				if (hit.collider != null) {
					//m_vertices [i * m_halfSideBulletNumber + j + offset] = new Vector2 (hit.point.x - transform.position.x, hit.point.y - transform.position.y);
					if (hit.collider.name == "Hero") {
						LoadController load = GameObject.Find ("SavaPoint").GetComponent<LoadController> ();
						load.Reload (hit.collider.gameObject);
					}
				} else {
					//m_vertices [i * m_halfSideBulletNumber + j + offset] = angle * m_rayLength;
				}
				//input 
				//求法线向量与物体上方向向量点乘，结果为1或-1，修正旋转方向
				//float aAngle = Vector3.Angle (Vector3.right, angle) * Mathf.Sign (Vector3.Dot (angle, Vector3.up));
				//aSimpleBullet.transform.Rotate (Vector3.forward, aAngle);
				//aSimpleBullet.m_isPlayerFire = m_isPlayerFire;
				//aSimpleBullet.InitBullet (angle, m_bulletSpeed, m_bulletAttack, m_bulletDuration, m_bulletRepelDistance);


			}
			//offset += 1;

		}

		Debug.DrawLine (transform.position, (transform.position + vec * m_rayLength), Color.red);
		hit = Physics2D.Raycast (transform.position, vec * m_rayLength, m_rayLength, m_layer);
		if (hit.collider != null) {
			//m_vertices [m_halfSideBulletNumber -2] = new Vector2 (hit.point.x - transform.position.x, hit.point.y - transform.position.y);

			if (hit.collider.name == "Hero") {
				LoadController load = GameObject.Find ("SavaPoint").GetComponent<LoadController> ();
				load.Reload (hit.collider.gameObject);
			}
		} else {
			//m_vertices [(i + 1) * m_halfSideBulletNumber] = vec * m_rayLength;

		}
		//m_vertices [m_vertices.Length - 1] = Vector3.zero;

		//Vector2 temp = m_vertices [1];
		//m_vertices [1] = m_vertices [0];
		//m_vertices [0] = temp;

		//Vector3 [] vertices3D = System.Array.ConvertAll<Vector2, Vector3> (m_vertices, v => v);

		//// Use the triangulator to get indices for creating triangles
		//Triangulator triangulator = new Triangulator (m_vertices);
		//int [] indices = triangulator.Triangulate ();


		//Color [] colors = new Color [vertices3D.Length];
		//for (int i = 0; i < colors.Length; i++) {
		//	colors [i] = Color.red;
		//	colors [i].a = 0.5f;
		//}

		//// Create the mesh
		//var mesh = new Mesh {
		//	vertices = vertices3D,
		//	triangles = indices,
		//	colors = colors
		//};


		//mesh.RecalculateNormals ();
		//mesh.RecalculateBounds ();



		//filter.mesh = mesh;

	}

	private void FixedUpdate ()
	{
		if (m_current > 0) {
			m_current -= Time.fixedDeltaTime;
			transform.localPosition = new Vector3 (
			transform.localPosition.x + m_velocityHorizontal * Time.fixedDeltaTime,
			transform.localPosition.y,
			transform.localPosition.z);
			//transform.position = new Vector3(transform.position.x+  )
		} else {
			m_current = m_length;
			m_velocityHorizontal = -m_velocityHorizontal;
			transform.localScale = new Vector3 (
		 -transform.localScale.x,
		 transform.localScale.y,
		 transform.localScale.z);

		}


	}



}
