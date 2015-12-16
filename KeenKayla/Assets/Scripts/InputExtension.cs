using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AxisDownTracking
{
    public string axisName;
    public float axis;
    public bool pressed;
    public bool justDown;

    public void Update()
    {
        axis = Input.GetAxisRaw(axisName);

        if (axis != 0)
        {
            if (!pressed)
            {
                pressed = true;
                justDown = true;
            }
            else
            {
                justDown = false;
            }
        }
        else if (pressed)
        {
            pressed = false;
            justDown = false;
        }
    }
}

public class InputExtension : MonoBehaviour 
{
    public static InputExtension instance;
    private Dictionary<string, AxisDownTracking> _axes = new Dictionary<string, AxisDownTracking>();
    private string[] _axisStrings = { "TabAxis", "DPadHorizontal", "DPadVertical" };

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        for (int i = 0; i < _axisStrings.Length; i++)
        {
            _axes.Add(_axisStrings[i], new AxisDownTracking { axisName = _axisStrings[i] });
        }
    }

	private void Update () 
    {
        foreach (var axis in _axes.Values)
        {
            axis.Update();
        }
	}

    public float? GetAxisDown(string axis)
    {
        AxisDownTracking tracking;
        if(_axes.TryGetValue(axis, out tracking) && tracking.justDown)
        {
            return tracking.axis;
        }

        return null;
    }
}
