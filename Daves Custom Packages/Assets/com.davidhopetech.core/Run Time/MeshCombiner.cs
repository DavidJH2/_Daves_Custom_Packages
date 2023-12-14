using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCombiner : MonoBehaviour
{
	[SerializeField] private GameObject srcGO;
	[SerializeField] private GameObject firstParent;
	[SerializeField] private List<MeshFilter>	meshFilters;
	// [SerializeField] private MeshFilter			targetMeshFilter;
	// [SerializeField] private MeshRenderer		meshRender;

	private List<Material>						srcMaterials;

	[ContextMenu("Clear Combined Mesh")]
	private void ClearCombinedMesh()
	{
		meshFilters.Clear();
		var meshRender = GetComponent<MeshRenderer>();
		if(meshRender) DestroyImmediate(meshRender);

		var meshFilter = GetComponent<MeshFilter>();
		if(meshFilter) DestroyImmediate(meshFilter);
	}


	[ContextMenu("Deactivate Mesh Renderers")]
	private void DeactivateMeshRenderers()
	{
		MeshRenderer[] renderers = srcGO.GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer meshRenderer in renderers)
		{
			meshRenderer.enabled = false;
		}
	}


	[ContextMenu("Activate Mesh Renderers")]
	private void ActivateMeshRenderers()
	{
		MeshRenderer[] renderers = srcGO.GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer meshRenderer in renderers)
		{
			meshRenderer.enabled = true;
		}
	}

	[ContextMenu("Combine Meshes")]
	private void CombineMeshes()
	{
		Transform initTransform = transform;

		// Remove existing MeshRenderer and MeshFilter
		var combinerMeshRender = GetComponent<MeshRenderer>();
		if (combinerMeshRender) DestroyImmediate(combinerMeshRender);

		var combinerMeshFilter = GetComponent<MeshFilter>();
		if (combinerMeshFilter) DestroyImmediate(combinerMeshFilter);

		// Make a list of all materials
		meshFilters = new List<MeshFilter>(srcGO.GetComponentsInChildren<MeshFilter>());

		var materials = new List<Material>();

		MeshRenderer[] meshRenderers = srcGO.GetComponentsInChildren<MeshRenderer>();

		foreach(Renderer renderer in meshRenderers)
		{
			foreach(Material mat in renderer.sharedMaterials)
			{
				if(!materials.Contains(mat))
				{
					materials.Add(mat);
				}
			}
		}


		// Create Renderer and add all Materials
		combinerMeshRender = gameObject.AddComponent<MeshRenderer>();
		Material[] sharedMaterials = new Material[materials.Count];
		for(var i = 0; i<materials.Count; i++)
		{
			sharedMaterials[i] = materials[i];
		}

		combinerMeshRender.sharedMaterials = sharedMaterials;


		// Create MeshFilter
		var submeshes = new List<Mesh>();
		foreach(Material material in materials)
		{
			var combiners = new List<CombineInstance>();
			foreach(var meshFilter in meshFilters)
			{
				var renderer = meshFilter.GetComponent<Renderer>();

				if(renderer==null)
				{
					Debug.Log(meshFilter.name + "does not have Renderer");
				}
				else
				{
					var meshFilterMaterials = renderer.sharedMaterials;
					for(var meshFilterIndex = 0; meshFilterIndex<meshFilterMaterials.Length; meshFilterIndex++)
					{
						if(meshFilterMaterials[meshFilterIndex] == material)
						{
							var ci = new CombineInstance();

							ci.mesh = meshFilter.sharedMesh;
							ci.subMeshIndex = meshFilterIndex;
							// ci.transform = Matrix4x4.identity;
							Matrix4x4 inverse = meshFilter.transform.localToWorldMatrix.inverse;
							// ci.transform = meshFilter.transform.worldToLocalMatrix.inverse * firstParent.transform.worldToLocalMatrix;
							ci.transform = firstParent.transform.worldToLocalMatrix * meshFilter.transform.localToWorldMatrix;
							combiners.Add(ci);
						}
					}
				}
			}
			// Flatten into a single mesh.
			Mesh mesh = new Mesh();
			mesh.CombineMeshes(combiners.ToArray(), true);
			submeshes.Add(mesh);
		}

		// The final mesh: combine all the material-specific meshes as independent submeshes.
		var finalCombiners = new List<CombineInstance>();
		foreach (Mesh mesh in submeshes)
		{
			CombineInstance ci = new CombineInstance();
			ci.mesh = mesh;
			ci.subMeshIndex = 0;
			ci.transform = Matrix4x4.identity;
			finalCombiners.Add(ci);
		}
		Mesh finalMesh = new Mesh();
		finalMesh.CombineMeshes(finalCombiners.ToArray(), false);

		// Create MeshFilter
		var finalMeshFilter = gameObject.AddComponent<MeshFilter>();
		finalMeshFilter.sharedMesh = finalMesh;
		Debug.Log("Final mesh has " + submeshes.Count + " materials.");
	}
}
