using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Trail ������ �����ϱ� ���� Struct
/// </summary>
[System.Serializable]
public class MeshTrailStruct
{
    public GameObject Container;

    public MeshFilter BodyMeshFilter;

    public Mesh bodyMesh;
}

public class MotionTrail : MonoBehaviour
{
    #region Variables & Initializer
    [Header("[PreRequisite]")]
    [SerializeField] private SkinnedMeshRenderer SMR_Body;

    private Transform TrailContainer;
    [SerializeField] private GameObject MeshTrailPrefab;
    private List<MeshTrailStruct> MeshTrailStructs = new List<MeshTrailStruct>();

    private List<GameObject> bodyParts = new List<GameObject>();
    private List<Vector3> posMemory = new List<Vector3>();
    private List<Quaternion> rotMemory = new List<Quaternion>();

    [Header("[Trail Info]")]
    [SerializeField] private int TrailCount;
    [SerializeField] private float TrailGap = 0.2f;
    [SerializeField][ColorUsage(true, true)] private Color frontColor;
    [SerializeField][ColorUsage(true, true)] private Color backColor;
    [SerializeField][ColorUsage(true, true)] private Color frontColor_Inner;
    [SerializeField][ColorUsage(true, true)] private Color backColor_Inner;
    #endregion

    #region MotionTrail

    void Start()
    {
        // ���ο� TailContainer ���ӿ�����Ʈ�� ���� Trail ������Ʈ���� ����
        TrailContainer = new GameObject("TrailContainer").transform;
        TrailContainer.parent = transform;
        for (int i = 0; i < TrailCount; i++)
        {
            // ���ϴ� TrailCount��ŭ ����
            MeshTrailStruct pss = new MeshTrailStruct();

            pss.Container = Instantiate(MeshTrailPrefab, TrailContainer);
            Debug.Log(pss.Container.name);
            pss.BodyMeshFilter = pss.Container.transform.GetChild(0).GetComponent<MeshFilter>();

            pss.bodyMesh = new Mesh();

            // �� mesh�� ���ϴ� skinnedMeshRenderer Bake
            SMR_Body.BakeMesh(pss.bodyMesh);

            // �� MeshFilter�� �˸��� Mesh �Ҵ�
            pss.BodyMeshFilter.mesh = pss.bodyMesh;

            MeshTrailStructs.Add(pss);

            bodyParts.Add(pss.Container);

            // Material �Ӽ� ����
            float alphaVal = (1f - (float)i / TrailCount) * 0.5f;
            pss.BodyMeshFilter.GetComponent<SkinnedMeshRenderer>().material.SetFloat("_Alpha", alphaVal);

            Color tmpColor = Color.Lerp(frontColor, backColor, (float)i / TrailCount);
            pss.BodyMeshFilter.GetComponent<SkinnedMeshRenderer>().material.SetColor("_FresnelColor", tmpColor);

            Color tmpColor_Inner = Color.Lerp(frontColor_Inner, backColor_Inner, (float)i / TrailCount);
            pss.BodyMeshFilter.GetComponent<SkinnedMeshRenderer>().material.SetColor("_BaselColor", tmpColor_Inner);
        }

        StartCoroutine("BakeMeshCoroutine");
    }

    /// <summary>
    /// Trail�� ����� �ڷ�ƾ
    /// </summary>
    IEnumerator BakeMeshCoroutine()
    {
        // Mesh ��ü�� Swap�ϴ� ���� �ƴ϶� vertices, Triangles�� ����
        // ���� triangles�� �������� ������ �޽��� ����� ������� ����
        for (int i = MeshTrailStructs.Count - 2; i >= 0; i--)
        {
            MeshTrailStructs[i + 1].bodyMesh.vertices = MeshTrailStructs[i].bodyMesh.vertices;

            MeshTrailStructs[i + 1].bodyMesh.triangles = MeshTrailStructs[i].bodyMesh.triangles;
        }

        // ù ��° ���� ���� Bake�������
        SMR_Body.BakeMesh(MeshTrailStructs[0].bodyMesh);

        // Snake ����ó�� ������ position�� rotation�� ���
        posMemory.Insert(0, transform.position);
        rotMemory.Insert(0, transform.rotation);

        // Trail Count�� �Ѿ�� ����
        if (posMemory.Count > TrailCount)
            posMemory.RemoveAt(posMemory.Count - 1);
        if (rotMemory.Count > TrailCount)
            rotMemory.RemoveAt(rotMemory.Count - 1);
        // ����ص� Pos, Rot �Ҵ�
        for (int i = 0; i < bodyParts.Count; i++)
        {
            bodyParts[i].transform.position = posMemory[Mathf.Min(i, posMemory.Count - 1)];
            bodyParts[i].transform.rotation = rotMemory[Mathf.Min(i, rotMemory.Count - 1)];
        }

        yield return new WaitForSeconds(TrailGap);
        StartCoroutine("BakeMeshCoroutine");
    }
    #endregion
}