using System;
using Unity.Collections;
using UnityEngine;
using Unity.Jobs;
using Unity.Mathematics;

// Cams mostly hack buoyancy
public class Buoyancy : MonoBehaviour
{
	public float forceScalar;
	public float waterLevel;

	public int underwaterVerts;
	public float dragScalar;

	// Luke added
	public Transform t;
	public Rigidbody rb;
	public Mesh mesh;

	public float dragFactor;
	public int totalVerts;
	
	public NativeArray<Vector3> Vertices;
	public NativeArray<Vector3> MeshVertForceFactor;
	private NativeArray<Vector3> _forceAmounts;
	private NativeArray<Vector3> _worldVertPositions;

	private JobHandle _handle;
	private BuoyancyJobParallelFor _job;
	
	private void Start()
	{
		t = transform;
		rb = GetComponent<Rigidbody>();
		mesh = GetComponent<MeshFilter>().mesh;
		waterLevel = SplashTrigger.Singleton.waterLevel;
		if (totalVerts == 0) Precalculate();
		dragFactor = dragScalar/totalVerts;
		_forceAmounts = new NativeArray<Vector3>(totalVerts, Allocator.Persistent);
		_worldVertPositions = new NativeArray<Vector3>(totalVerts, Allocator.Persistent);
	}

	private void Precalculate()
	{
		totalVerts = mesh.normals.Length;
		Vertices = new NativeArray<Vector3>(mesh.vertices, Allocator.Persistent);
		MeshVertForceFactor = new NativeArray<Vector3>(totalVerts, Allocator.Persistent);

		for (var i=0; i < totalVerts; i++)
		{
			MeshVertForceFactor[i] = mesh.normals[i]*-forceScalar;
		}
	}

	private void Update()
	{
		CalculateForces();
	}

	private void CalculateForces()
	{
		underwaterVerts = 0;
		var position = t.position;
		var rotation = t.rotation;

		_job = new BuoyancyJobParallelFor
		{
			ForceAmounts = _forceAmounts,
			WorldVertPositions = _worldVertPositions,
			Vertices = Vertices,
			MeshVertForceFactor = MeshVertForceFactor,
			Position = position,
			Rotation = rotation,
			WaterLevel = waterLevel,
			DeltaTime = Time.deltaTime
		};

		_handle = _job.Schedule(totalVerts, 512);

		_handle.Complete();

		for (var i = 0; i < totalVerts; i++)
		{
			if (_forceAmounts[i] == Vector3.zero) continue;
			rb.AddForceAtPosition(_forceAmounts[i],_worldVertPositions[i], ForceMode.Force);
			underwaterVerts++;
		}
		
		var drag = underwaterVerts * dragFactor;
		rb.drag = drag;
		rb.angularDrag = drag;
	}

	private void OnDestroy()
	{
		Vertices.Dispose();
		MeshVertForceFactor.Dispose();
		if (!_forceAmounts.IsCreated) return;
		_forceAmounts.Dispose();
		_worldVertPositions.Dispose();
	}
}
