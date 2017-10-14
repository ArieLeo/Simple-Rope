#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class SimpleRope : MonoBehaviour {


    [Range(0, 20)]
    public int NumberOfConnectors = 0;
    private int PrevNumberOfConnectors = -1;

    [Header("RopeObjects")]
    public GameObject RopeStartPoint;
    private Vector3 RopeStartPrevPos;
    public ConfigurableJoint RopeStartJoint;
    public GameObject[] RopePoints;
    public GameObject RopeEndPoint;
    public GameObject RopeEndPosition;
    private Vector3 RopeEndPrevPos;
    public Material RopeMaterial;
    [Range(0, 20)]
    public float RopeMaterialWidth = .06f;

    [Header("Joint Configurations")]
    public ConfigurableJointMotion xMotion = ConfigurableJointMotion.Limited;
    public ConfigurableJointMotion yMotion = ConfigurableJointMotion.Limited;
    public ConfigurableJointMotion zMotion = ConfigurableJointMotion.Limited;
    public ConfigurableJointMotion angularXMotion = ConfigurableJointMotion.Free;
    public ConfigurableJointMotion angularYMotion = ConfigurableJointMotion.Free;
    public ConfigurableJointMotion angularZMotion = ConfigurableJointMotion.Free;

    //If this is still here just ignore it. I can never remember the for loop syntax
    //for(int i = 0; i < Array.Length; i++)

    public void GenerateRope()
    {
        DestroyRopePoints();
        if (RopeStartPoint != null)
        {
            DestroyImmediate(RopeStartPoint);
        }
        if (RopeEndPoint != null)
        {
            DestroyImmediate(RopeEndPoint);
        }
        if (RopeEndPosition != null)
        {
            DestroyImmediate(RopeEndPosition);
        }

        PrevNumberOfConnectors = -1;

        //Create RopeStartPoint & Common RopeStart/EndPoint Items
        RopeStartPoint = Instantiate(gameObject, transform);
        RopeStartPoint.name = "RopeStartPoint";
        DestroyImmediate(RopeStartPoint.GetComponent<SimpleRope>());
        RopeStartPoint.AddComponent<Rigidbody>();
        RopeStartPoint.transform.localPosition = new Vector3(0, 0, 0);
        RopeStartPrevPos = RopeStartPoint.transform.position;

        //Create RopeEndPosition Object
        //RopeEndPoint = Instantiate(RopeStartPoint, transform);
        //RopeEndPoint.name = "RopeEndPoint";
        //RopeEndPoint.transform.localPosition = new Vector3(0, -1 - NumberOfConnectors, 0);
        //RopeEndPrevPos = RopeEndPoint.transform.position;
        
        RopeEndPosition = Instantiate(RopeStartPoint, transform);
        RopeEndPosition.name = "RopeEndPosition";
        RopeEndPosition.transform.localPosition = new Vector3(0, -1 - NumberOfConnectors, 0);
        RopeEndPrevPos = RopeEndPosition.transform.position;

        //Configure RopeStart gameobject & joint
        RopeStartJoint = RopeStartPoint.AddComponent<ConfigurableJoint>();
        RopeStartPoint.GetComponent<Rigidbody>().isKinematic = true;
        RopeStartPoint.GetComponent<Rigidbody>().useGravity = false;

        RopeStartJoint.xMotion = xMotion;
        RopeStartJoint.yMotion = yMotion;
        RopeStartJoint.zMotion = zMotion;
        RopeStartJoint.angularXMotion = angularXMotion;
        RopeStartJoint.angularYMotion = angularYMotion;
        RopeStartJoint.angularZMotion = angularZMotion;

        RopeStartJoint.secondaryAxis = new Vector3(0, 0, 1);

        //CreateRopeEndPoint
        RopeEndPoint = Instantiate(RopeStartPoint, transform);
        RopeEndPoint.name = "RopeEndPoint";
        RopeEndPoint.transform.localPosition = RopeEndPosition.transform.position;
        RopeEndPoint.GetComponent<ConfigurableJoint>().connectedBody = RopeEndPosition.GetComponent<Rigidbody>();
        RopeEndPoint.GetComponent<Rigidbody>().isKinematic = false;

        //LineRender
        SimpleRopeLineRenderPositioning SRLRP = RopeStartPoint.AddComponent<SimpleRopeLineRenderPositioning>();
        SRLRP.LineRenderer = RopeStartPoint.AddComponent<LineRenderer>();
        RopeStartPoint.GetComponent<LineRenderer>().material = RopeMaterial;
        RopeStartPoint.GetComponent<LineRenderer>().widthMultiplier = RopeMaterialWidth;
        RopeStartPoint.GetComponent<LineRenderer>().textureMode = LineTextureMode.Tile;


        RefreshRope();
    }

    public void RefreshRope()
    {
        if (NumberOfConnectors == 0)
        {
            DestroyRopePoints();
            if (RopeStartJoint != null)
            {
                RopeStartJoint.connectedBody = RopeEndPoint.GetComponent<Rigidbody>();
                RopeStartPoint.GetComponent<SimpleRopeLineRenderPositioning>().LineRenderStartTarget = RopeEndPosition.gameObject;
                RopeStartPoint.GetComponent<LineRenderer>().material = RopeMaterial;
                RopeStartPoint.GetComponent<LineRenderer>().widthMultiplier = RopeMaterialWidth;
            } 
        }
        else
        {
            if (NumberOfConnectors != PrevNumberOfConnectors)
            {
                DestroyRopePoints();

                //Stuff we need for transform positioning
                float RopeLength = Vector3.Distance(RopeStartPoint.transform.position, RopeEndPoint.transform.position);
                float PointDistance = RopeLength / (RopePoints.Length + 1);
                Vector3 RopeDirection = (RopeEndPoint.transform.position - RopeStartPoint.transform.position).normalized;

                for (int i = 0; i < RopePoints.Length; i++)
                {

                    //Create & Connect Joints
                    RopePoints[i] = Instantiate(RopeStartPoint, transform);
                    Rigidbody RopeBody = RopePoints[i].GetComponent<Rigidbody>();
                    RopeBody.isKinematic = false;
                    RopeBody.useGravity = true;
                    RopePoints[i].name = "Rope Point" + i.ToString();
                    if (i == 0)
                    {
                        RopeStartJoint.connectedBody = RopeBody;
                        RopeStartPoint.GetComponent<SimpleRopeLineRenderPositioning>().LineRenderStartTarget = RopePoints[i];
                    }
                    else if (i == RopePoints.Length - 1)
                    {
                        RopePoints[i - 1].GetComponent<ConfigurableJoint>().connectedBody = RopeBody;
                        RopePoints[i - 1].GetComponent<SimpleRopeLineRenderPositioning>().LineRenderStartTarget = RopePoints[i];
                        RopePoints[i].GetComponent<ConfigurableJoint>().connectedBody = RopeEndPoint.GetComponent<Rigidbody>();
                        RopePoints[i].GetComponent<SimpleRopeLineRenderPositioning>().LineRenderStartTarget = RopeEndPosition;
                    }
                    else
                    {
                        RopePoints[i - 1].GetComponent<ConfigurableJoint>().connectedBody = RopeBody;
                        RopePoints[i - 1].GetComponent<SimpleRopeLineRenderPositioning>().LineRenderStartTarget = RopePoints[i];
                    }
                    //Set RopePoint positions
                    RopePoints[i].transform.position = RopeStartPoint.transform.position + (RopeDirection * PointDistance * (i+1));   
                }
                if (RopePoints.Length == 1)
                {
                    RopePoints[0].GetComponent<ConfigurableJoint>().connectedBody = RopeEndPoint.GetComponent<Rigidbody>();
                    RopePoints[0].GetComponent<SimpleRopeLineRenderPositioning>().LineRenderStartTarget = RopeEndPosition;
                }
            }
            else
            {
                float RopeLength = Vector3.Distance(RopeStartPoint.transform.position, RopeEndPoint.transform.position);
                float PointDistance = RopeLength / (RopePoints.Length + 1);
                Vector3 RopeDirection = (RopeEndPoint.transform.position - RopeStartPoint.transform.position).normalized;
                for (int i = 0; i < RopePoints.Length; i++)
                {
                    RopePoints[i].transform.position = RopeStartPoint.transform.position + (RopeDirection * PointDistance * (i + 1));
                    RopePoints[i].GetComponent<LineRenderer>().material = RopeMaterial;
                    RopePoints[i].GetComponent<LineRenderer>().widthMultiplier = RopeMaterialWidth;
                }
                RopeStartPoint.GetComponent<LineRenderer>().material = RopeMaterial;
                RopeStartPoint.GetComponent<LineRenderer>().widthMultiplier = RopeMaterialWidth;
            }
        }
        //Set Prev connectors value to current - used to avoid instantiating when not needed.
        PrevNumberOfConnectors = NumberOfConnectors;
    }



    public void Update()
    {
        if (EditorApplication.isPlaying) return;
        if (RopeEndPoint != null) { RopeEndPoint.transform.position = RopeEndPosition.transform.position; }
        if ((RopeStartPoint != null && RopeStartPrevPos != RopeStartPoint.transform.position) || (RopeEndPoint != null && RopeEndPrevPos != RopeEndPosition.transform.position))
        {
            if (RopeStartPoint != null) { RopeStartPrevPos = RopeStartPoint.transform.position; }
            if (RopeEndPosition != null) { RopeEndPrevPos = RopeEndPosition.transform.position; }
            
            RefreshRope();
        }
    }

    public void DestroyRopePoints()
    {
        if (RopePoints != null && RopePoints.Length > 0 && RopePoints[0] != null)
        {
            foreach (GameObject RopePoint in RopePoints)
            {
                DestroyImmediate(RopePoint);
            }
        }
        RopePoints = new GameObject[NumberOfConnectors];
    }
}
#endif