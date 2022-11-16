using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using UnityEngine;
using Unity.Jobs;
using Unity.Mathematics;

[BurstCompile]
public struct BuoyancyJobParallelFor : IJobParallelFor
{
	public NativeArray<Vector3> ForceAmounts;
	public NativeArray<Vector3> WorldVertPositions;

	[ReadOnly]
	public NativeArray<Vector3> Vertices;
	[ReadOnly]
	public NativeArray<Vector3> MeshVertForceFactor;

	public Vector3 Position;
	public Quaternion Rotation;
	public float WaterLevel;
	public float DeltaTime;

	public void Execute(int index)
	{
		Vector3 worldVertPosition = Position + Rotation * Vertices[index];
		if (worldVertPosition.y < WaterLevel)
		{
			WorldVertPositions[index] = worldVertPosition;
			ForceAmounts[index] = Rotation*MeshVertForceFactor[index] * DeltaTime;
		}
		else
		{
			ForceAmounts[index] = Vector3.zero;
		}
	}
}
