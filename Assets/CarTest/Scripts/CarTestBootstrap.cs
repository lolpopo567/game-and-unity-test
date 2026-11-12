using UnityEngine;

namespace CarTest {
 public sealed class CarTestBootstrap : MonoBehaviour {
  private CarController car; private GUIStyle hud;
  private Material MakeMat(Color c) { var m = new Material(Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard")); m.color=c; return m; }
  private void Start() {
   var asphalt=MakeMat(new Color(.16f,.19f,.23f)); var grass=MakeMat(new Color(.18f,.42f,.24f)); var yellow=MakeMat(new Color(1f,.72f,.12f)); var red=MakeMat(new Color(.85f,.16f,.12f));
   Cube("Grass field",new Vector3(0,-.4f,0),new Vector3(90,.5f,100),grass); Cube("Asphalt road",new Vector3(0,-.1f,0),new Vector3(12,.1f,86),asphalt);
   for(int z=-36;z<=36;z+=6) Cube("Center dash",new Vector3(0,-.04f,z),new Vector3(.18f,.025f,2.4f),yellow);
   for(int side=-1;side<=1;side+=2) { Cube("Road edge",new Vector3(side*6.1f,-.01f,0),new Vector3(.25f,.14f,86),yellow); for(int z=-35;z<=35;z+=5) Cube("Track barrier",new Vector3(side*9f,.1f,z),new Vector3(.6f,.7f,.7f),red); }
   Cube("Start line",new Vector3(0,-.02f,-27),new Vector3(11.8f,.04f,.35f),yellow); Cube("Finish line",new Vector3(0,-.01f,32),new Vector3(11.8f,.04f,.5f),red);
   var root=new GameObject("Player Car"); root.transform.position=new Vector3(0,.55f,-24); CubeChild(root,"Body",new Vector3(0,0,0),new Vector3(1.8f,.6f,3.2f),MakeMat(new Color(.1f,.45f,.95f))); CubeChild(root,"Cabin",new Vector3(0,.48f,.1f),new Vector3(1.3f,.5f,1.4f),MakeMat(new Color(.14f,.78f,.9f)));
   for(int x=-1;x<=1;x+=2) for(int z=-1;z<=1;z+=2) { var w=GameObject.CreatePrimitive(PrimitiveType.Cylinder); w.name="Wheel"; w.transform.SetParent(root.transform,false); w.transform.localPosition=new Vector3(x*.95f,-.22f,z*.95f); w.transform.localRotation=Quaternion.Euler(0,0,90); w.transform.localScale=new Vector3(.44f,.18f,.44f); w.GetComponent<Renderer>().material=MakeMat(new Color(.04f,.05f,.06f)); }
   car=root.AddComponent<CarController>();
   var cam=new GameObject("Chase Camera"); var camera=cam.AddComponent<Camera>(); camera.tag="MainCamera"; camera.fieldOfView=66; cam.AddComponent<AudioListener>(); cam.AddComponent<ChaseCamera>().Target=root.transform;
   RenderSettings.ambientLight=new Color(.58f,.62f,.68f); var light=new GameObject("Sun").AddComponent<Light>(); light.type=LightType.Directional; light.intensity=1.15f; light.transform.rotation=Quaternion.Euler(48,-32,0);
  }
  private void Cube(string n,Vector3 p,Vector3 s,Material m) { var o=GameObject.CreatePrimitive(PrimitiveType.Cube); o.name=n;o.transform.position=p;o.transform.localScale=s;o.GetComponent<Renderer>().material=m; }
  private void CubeChild(GameObject parent,string n,Vector3 p,Vector3 s,Material m) { var o=GameObject.CreatePrimitive(PrimitiveType.Cube);o.name=n;o.transform.SetParent(parent.transform,false);o.transform.localPosition=p;o.transform.localScale=s;o.GetComponent<Renderer>().material=m; }
  private void OnGUI() { if(hud==null) hud=new GUIStyle(GUI.skin.label){fontSize=18,fontStyle=FontStyle.Bold,normal={textColor=Color.white}}; GUI.color=new Color(0,0,0,.68f);GUI.Box(new Rect(16,16,350,150),GUIContent.none);GUI.color=Color.white;GUI.Label(new Rect(30,25,330,30),"SIMPLE DRIVE — TEST TRACK",new GUIStyle(hud){fontSize=24});GUI.Label(new Rect(30,62,320,25),$"Speed  {car.SpeedKph:000} km/h",hud);GUI.Label(new Rect(30,89,330,25),"WASD / arrows: drive and steer",hud);GUI.Label(new Rect(30,116,330,25),$"Space: handbrake   R: reset   Progress: {car.Progress:0}%",hud); }
 }
}