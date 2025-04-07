using System;
using Unity.Behavior;

[BlackboardEnum]
public enum BossState
{
    Idle,
    Recovery,
	Move,
	Attack,
	Hurt,
	Step,
	Dead
}
