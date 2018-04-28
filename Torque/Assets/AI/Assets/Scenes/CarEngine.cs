using System.Collections.Generic;
using UnityEngine;


public class CarEngine : MonoBehaviour {
    public Transform path;
    public float maxsteerangle=45f;
    public WheelCollider wheelFR;
    public WheelCollider wheelFL;
    public float maxengintorqe=300f;
    public float maxspeed=250f;
    public float currendspeed;
    public List<Transform> nodes;
    public int currentnode = 0;
    [Header("sensor")]
    public float sensorlength = 5f;
    public Vector3 frontsensorpos=new Vector3(0,0.2f,0.5f);
    public float frontSidesensorPos = 0.2f;
    public float frontsensorangle=30f;
    RaycastHit Hit;
    public bool avoid=false;
    public float smoothsteerangle;
    public float speedturn = 5f;

    public float targetsteerangle = 0f;
    void Start() {
        
        Transform[] PathTarnsform = path.GetComponentsInChildren<Transform>();
        nodes = new List<Transform>();
        for (int i = 0; i < PathTarnsform.Length; i++)
        {
            if (PathTarnsform[i] != path.transform)
            {
                nodes.Add(PathTarnsform[i]);
            }

        }
    }

    // Update is called once per frame
    private void FixedUpdate ()
    {
        sensor();
        ApplySteers();
        Drive();
        checkwaypoint();
       // lerpsteerangle(); 
       //lerpsteerangele for smoth curve when i call it it make the car in node 12 turn out 
       //node 16 and 15 got problem to
    }
    private void ApplySteers()
    {  if (avoid) { return; }
        Vector3 relativevector = transform.InverseTransformPoint(nodes[currentnode].position);
       float newsteer= (relativevector.x /  relativevector.magnitude)*maxsteerangle;
       
        wheelFL.steerAngle = newsteer;
        wheelFR.steerAngle = newsteer;
         
    }
    private void Drive ()
    {
        currendspeed = 2 * Mathf.PI * wheelFL.radius * wheelFL.rpm * 60 / 1000;
        if (currendspeed < maxspeed)
        {
            wheelFR.motorTorque = maxengintorqe;
            wheelFL.motorTorque = maxengintorqe;
        }else
        {
            wheelFR.motorTorque = 0;
            wheelFL.motorTorque = 0;
        }
    }
    void checkwaypoint()
    {
        if (Vector3.Distance(transform.position,nodes[currentnode].position)<0.5f)
        {
            if (currentnode == nodes.Count - 1)
            {
                currentnode = 0;
            }
            else
                currentnode++;
        }
    }
    void sensor()
    {
        float avoiding = 0;
        avoid = false;
        Vector3 startsensorpos = transform.position;
        startsensorpos += transform.forward *frontsensorpos.z;
        startsensorpos += transform.up * frontsensorpos.y;
        //front sensor
        if (avoiding == 0)
        {
            if (Physics.Raycast(startsensorpos, transform.forward, out Hit, sensorlength))
            {
                Debug.DrawLine(startsensorpos, Hit.point);
                avoid = true;
                if(Hit.normal.x<0)
                {
                    avoiding -= .5f;

                }
                else
                {
                    avoiding += .5f;
                }
            }
        }

        //front rightsensor

        startsensorpos += transform.right * frontSidesensorPos;
        if (Physics.Raycast(startsensorpos, transform.forward, out Hit, sensorlength))
        {
            Debug.DrawLine(startsensorpos, Hit.point);
            avoid = true;
            avoiding -= .5F;
        }
        //front right angle sensor

       else if (Physics.Raycast(startsensorpos, Quaternion.AngleAxis(frontsensorangle,transform.up)*transform.forward, out Hit, sensorlength))
        {
          Debug.DrawLine(startsensorpos, end: Hit.point);
            avoid = true;
            avoiding -= .2F;
        }
        //front left sensor
        startsensorpos -= transform.right * frontSidesensorPos;
        if (Physics.Raycast(startsensorpos, transform.forward, out Hit, sensorlength))
        {
            Debug.DrawLine(startsensorpos, Hit.point);
            avoid = true;
            avoiding += .5F;

        }
        //frint left angle sensor
       else if (Physics.Raycast(startsensorpos, Quaternion.AngleAxis(-frontsensorangle, transform.up) * transform.forward, out Hit, sensorlength))
        {
            Debug.DrawLine(startsensorpos, Hit.point);
            avoid = true;
            avoiding += .2F;
        }
        if (avoid)
        {   targetsteerangle= maxsteerangle * avoiding;
            
        }
    }
    private void lerpsteerangle()
    {
        wheelFL.steerAngle = Mathf.Lerp(wheelFL.steerAngle, targetsteerangle, Time.deltaTime * speedturn);
        wheelFR.steerAngle = Mathf.Lerp(wheelFR.steerAngle, targetsteerangle, Time.deltaTime * speedturn);
    }
}

