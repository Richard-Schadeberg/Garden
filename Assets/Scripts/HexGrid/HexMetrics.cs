using UnityEngine;

public static class HexMetrics {
	public const float outerRadius = 10f;
	public const float innerRadius = outerRadius * 0.866025404f;
	public static Vector3[] corners = {
		new Vector3(0f, 0f, outerRadius),
		new Vector3(innerRadius, 0f, 0.5f * outerRadius),
		new Vector3(innerRadius, 0f, -0.5f * outerRadius),
		new Vector3(0f, 0f, -outerRadius),
		new Vector3(-innerRadius, 0f, -0.5f * outerRadius),
		new Vector3(-innerRadius, 0f, 0.5f * outerRadius),
		new Vector3(0f, 0f, outerRadius)
	};
	public static Vector3 edgeCornerCCW(HexDirection direction) {
		return corners[(int)direction];
	}
	public static Vector3 edgeCornerCW(HexDirection direction) {
		return corners[(int)direction+1];
	}
	public static Vector3 edgeMidpoint(HexDirection direction) {
		return (edgeCornerCW(direction) + edgeCornerCCW(direction))/2;
	}
	public static Vector3 edgeVectorCW(HexDirection direction) {
		return edgeCornerCW(direction) - edgeCornerCCW(direction);
	}
}