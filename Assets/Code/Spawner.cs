
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	public int count;
	public GameObject prefab;
	public float randomAmount;
	public float randomThrowAmount;

	public float rotationMin = -180;
	public float rotationMax = 180;

	// Luke added.
	private int _totalVerts;
	private Vector3[] _meshVertLocalDisplacements;
	private Vector3[] _meshVertForceFactor;

	void Awake()
	{
		Precalculate();
	}
	
	// Use this for initialization
	void Start()
	{
		for (int i = 0; i < count; i++)
		{
			Spawn();
		}
	}
	
	void Precalculate()
	{
		Mesh mesh = prefab.GetComponent<MeshFilter>().sharedMesh;
		_totalVerts = mesh.normals.Length;
		_meshVertLocalDisplacements = mesh.vertices;
		_meshVertForceFactor = new Vector3[_totalVerts];

		float forceScalar = prefab.GetComponent<Buoyancy>().forceScalar;
		
		for (int i=0; i < _totalVerts; i++)
		{
			_meshVertForceFactor[i] = mesh.normals[i]*-forceScalar;
		}
	}

	private void Spawn()
	{
		GameObject o = Instantiate(prefab, transform.position + new Vector3(Random.Range(-randomAmount, randomAmount), Random.Range(-randomAmount, randomAmount)/2f, Random.Range(-randomAmount, randomAmount)), Quaternion.Euler(new Vector3(Random.Range(rotationMin, rotationMax), Random.Range(rotationMin, rotationMax), Random.Range(rotationMin, rotationMax))));
		Buoyancy b = o.GetComponent<Buoyancy>();
		b.totalVerts = _totalVerts;
		b.MeshVertForceFactor = new NativeArray<Vector3>(_meshVertForceFactor, Allocator.Persistent);
		b.Vertices = new NativeArray<Vector3>(_meshVertLocalDisplacements, Allocator.Persistent);
		if (Random.value > 0.3f)
		{
			o.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-randomThrowAmount, randomThrowAmount), 0,
				Random.Range(-randomThrowAmount, randomThrowAmount));
		}

		if (Random.value > 0.5f)
		{
			o.GetComponent<Rigidbody>().angularVelocity = new Vector3(Random.Range(-randomThrowAmount, randomThrowAmount), Random.Range(-randomThrowAmount, randomThrowAmount), Random.Range(-randomThrowAmount, randomThrowAmount));
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKey(KeyCode.Space))
		{
			Spawn();
		}
	}
}
