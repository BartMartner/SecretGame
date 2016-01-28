using UnityEngine;
using System.Collections;

public class LazerEyeMiniBoss : Enemy
{
    [Header("LazerEyeMiniBoss")]
    public GameObject lazer;
    public AreaEffector2D push;
    private bool _firingLazer;
}
