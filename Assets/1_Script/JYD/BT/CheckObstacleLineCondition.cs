using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "Check Obstacle Line", story: "Check [LayTrm] To [Target] Obstacle Line", category: "Conditions", id: "9cf1c8bd09f0c320fa00df19338847a6")]
public partial class CheckObstacleLineCondition : Condition
{
    [SerializeReference] public BlackboardVariable<Transform> LayTrm;
    [SerializeReference] public BlackboardVariable<Transform> Target;

    private LayerMask whatIsTarget;
        
    public override bool IsTrue()
    {
        whatIsTarget = LayerMask.GetMask("Obstacle"); // ������ �κ�

        if (LayTrm == null || Target == null || LayTrm.Value == null || Target.Value == null)
            return false;

        Vector3 direction = (Target.Value.position - LayTrm.Value.position).normalized;

        if (Physics.Raycast(LayTrm.Value.position, direction, out RaycastHit hit, Mathf.Infinity, whatIsTarget))
        {
            Debug.Log(hit.collider.gameObject.name); // ���� ������Ʈ �̸� ����ؼ� Ȯ��
            return false;
        }

        return true;
    }



}
