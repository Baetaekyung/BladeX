using System;
using Unity.Behavior;

[BlackboardEnum]
public enum BossState
{
    Idle,
	Move,
	Attack,
	Hurt,
	Groggy,
	Step,
	Dead
}
