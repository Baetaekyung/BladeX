using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Trail 정보를 저장하기 위한 Struct
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
        // 새로운 TailContainer 게임오브젝트를 만들어서 Trail 오브젝트들을 관리
        TrailContainer = new GameObject("TrailContainer").transform;
        TrailContainer.parent = transform;
        for (int i = 0; i < TrailCount; i++)
        {
            // 원하는 TrailCount만큼 생성
            MeshTrailStruct pss = new MeshTrailStruct();

            pss.Container = Instantiate(MeshTrailPrefab, TrailContainer);
            Debug.Log(pss.Container.name);
            pss.BodyMeshFilter = pss.Container.transform.GetChild(0).GetComponent<MeshFilter>();

            pss.bodyMesh = new Mesh();

            // 각 mesh에 원하는 skinnedMeshRenderer Bake
            SMR_Body.BakeMesh(pss.bodyMesh);

            // 각 MeshFilter에 알맞은 Mesh 할당
            pss.BodyMeshFilter.mesh = pss.bodyMesh;

            MeshTrailStructs.Add(pss);

            bodyParts.Add(pss.Container);

            // Material 속성 설정
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
    /// Trail을 만드는 코루틴
    /// </summary>
    IEnumerator BakeMeshCoroutine()
    {
        // Mesh 자체를 Swap하는 것이 아니라 vertices, Triangles를 복사
        // 만약 triangles를 복사하지 않으면 메쉬가 제대로 복사되지 않음
        for (int i = MeshTrailStructs.Count - 2; i >= 0; i--)
        {
            MeshTrailStructs[i + 1].bodyMesh.vertices = MeshTrailStructs[i].bodyMesh.vertices;

            MeshTrailStructs[i + 1].bodyMesh.triangles = MeshTrailStructs[i].bodyMesh.triangles;
        }

        // 첫 번째 것은 새로 Bake해줘야함
        SMR_Body.BakeMesh(MeshTrailStructs[0].bodyMesh);

        // Snake 게임처럼 이전의 position과 rotation을 기억
        posMemory.Insert(0, transform.position);
        rotMemory.Insert(0, transform.rotation);

        // Trail Count를 넘어서면 제거
        if (posMemory.Count > TrailCount)
            posMemory.RemoveAt(posMemory.Count - 1);
        if (rotMemory.Count > TrailCount)
            rotMemory.RemoveAt(rotMemory.Count - 1);
        // 기억해둔 Pos, Rot 할당
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