using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereGenerator : MonoBehaviour 
{
    [SerializeField]
    private GUISkin _skin;
    private PrimitiveType _type = PrimitiveType.Quad;

    [SerializeField]
    private Material _material;

    private List<GameObject> _pool = new List<GameObject>();
    private float _radius = 10;
    private float _step = .3f;

    private bool _capsule;
    private bool _cube;
    private bool _cylinder;
    private bool _plane;
    private bool _quad = true;
    private bool _sphere;

    private float _currentAngle;

    private void Start()
    {
        GenerateSphere();   
    }

    private void OnGUI()
    {
        GUI.skin = _skin;
        GUI.Label(new Rect(10,10,40,25),"Radius");
        var tempRadius = GUI.HorizontalSlider(new Rect(60,10,200,30),_radius,0,40);
        GUI.Label(new Rect(270,10,40,25),tempRadius.ToString("F"));

        if (Mathf.Abs(tempRadius - _radius) > .1f)
        {
            _radius = tempRadius;
            GenerateSphere();
        }

        GUI.Label(new Rect(10,40,40,25),"Step");
        var tempStep = GUI.HorizontalSlider(new Rect(60,40,200,30),_step,.1f,1);
        GUI.Label(new Rect(270,40,40,25),tempStep.ToString("F"));

        if (Mathf.Abs(tempStep - _step) > .01f)
        {
            _step = tempStep;
            GenerateSphere();
        }

        GUI.Label(new Rect(10,80,40,25),"Type");

        var capsule = GUI.Toggle(new Rect(10,100,70,20), _capsule, "Capsule");
        var cube = GUI.Toggle(new Rect(10,120,70,20), _cube, "Cube");
        var cylinder = GUI.Toggle(new Rect(10,140,70,20), _cylinder, "Cylinder");
        var plane = GUI.Toggle(new Rect(10,160,70,20), _plane, "Plane");
        var quad = GUI.Toggle(new Rect(10,180,70,20), _quad, "Quad");
        var sphere = GUI.Toggle(new Rect(10,200,70,20), _sphere, "Sphere");

        TypeToggleHandler(capsule, cube, cylinder, plane, quad, sphere);

        GUI.Label(new Rect(10,220,40,25),"Angle");
        var tempAngle = GUI.HorizontalSlider(new Rect(60,220,200,30),_currentAngle,-180,180);
        GUI.Label(new Rect(270,220,40,25),tempAngle.ToString("F"));

        if (Mathf.Abs(tempAngle - _currentAngle) > .1f)
        {
            _currentAngle = tempAngle;
            var temp = transform.rotation.eulerAngles;
            temp.y = _currentAngle;
            transform.rotation = Quaternion.Euler(temp);
        }  
    }

    private void TypeToggleHandler(bool capsule, bool cube, bool cylinder, bool plane, bool quad, bool sphere)
    {
        if(capsule != _capsule && capsule)
        {
            _capsule = capsule;
            _cube = false;
            _cylinder = false;
            _plane = false;
            _quad = false;
            _sphere = false;
            _type = PrimitiveType.Capsule;
            GenerateSphere();
            return;
        }

        if(cube != _cube && cube)
        {
            _capsule = false;
            _cube = cube;
            _cylinder = false;
            _plane = false;
            _quad = false;
            _sphere = false;
            _type = PrimitiveType.Cube;
            GenerateSphere();
            return;
        }

        if(cylinder != _cylinder && cylinder)
        {
            _capsule = false;
            _cube = false;
            _cylinder = cylinder;
            _plane = false;
            _quad = false;
            _sphere = false;
            _type = PrimitiveType.Cylinder;
            GenerateSphere();
            return;
        }

        if(plane != _plane && plane)
        {
            _capsule = false;
            _cube = false;
            _cylinder = false;
            _plane = plane;
            _quad = false;
            _sphere = false;
            _type = PrimitiveType.Plane;
            GenerateSphere();
            return;
        }

        if(quad != _quad && quad)
        {
            _capsule = false;
            _cube = false;
            _cylinder = false;
            _plane = false;
            _quad = quad;
            _sphere = false;
            _type = PrimitiveType.Quad;
            GenerateSphere();
            return;
        }

        if(sphere != _sphere && sphere)
        {
            _capsule = false;
            _cube = false;
            _cylinder = false;
            _plane = false;
            _quad = false;
            _sphere = sphere;
            _type = PrimitiveType.Sphere;
            GenerateSphere();
            return;
        }
    }


    private void GenerateSphere()
    {
        DisableObj();

        var temp = transform.rotation.eulerAngles;
        temp.y = 0;
        transform.rotation = Quaternion.Euler(temp);

        for (float i = 0; i < Mathf.PI; i+=_step)
        {
            for (float j = 0; j < Mathf.PI * 2; j+=_step) 
            {
                var x = transform.position.x + _radius * Mathf.Sin(i) * Mathf.Cos(j);
                var y = transform.position.y + _radius * Mathf.Sin(i) * Mathf.Sin(j);
                var z = transform.position.z + _radius * Mathf.Cos(i);
                    SetObj(new Vector3(x,y,z));
            }
        }

        temp.y = _currentAngle;
        transform.rotation = Quaternion.Euler(temp);
    }

    private void SetObj(Vector3 position)
    {
        var strType = _type.ToString();
        foreach (var item in _pool)
        {
            if(!item || item.activeSelf || !item.CompareTag(strType)) continue;
            item.SetActive(true);

            item.transform.localPosition = position;
            item.transform.forward = position - transform.position;

            return;
        }

        var temp = GameObject.CreatePrimitive(_type);
        temp.AddComponent<SphereComponent>();

        temp.tag = strType;
        temp.transform.parent = transform;
        var renderer = temp.GetComponent<Renderer>();
        if(renderer) renderer.material = _material;

        temp.transform.localPosition = position;
        temp.transform.forward = position - transform.position;

        _pool.Add(temp);
    }

    private void DisableObj()
    {
        if(_pool.IsNullOrEmpty()) return;

        foreach (var item in _pool)
        {
            if(!item || !item.activeSelf) continue;
            item.SetActive(false);
        }
    }
}
