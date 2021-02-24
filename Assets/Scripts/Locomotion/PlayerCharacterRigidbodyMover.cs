
using CharacterProperties;
using UnityEngine;

[RequireComponent(typeof(ICheckGrounded))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerCharacterRigidbodyMover : MonoBehaviour,ICharacterMover
{
    private ICheckGrounded _CheckGrounded;
    private Rigidbody _Rigidbody;
    private CharacterState _characterState;

    public ILocomotion Locomotion { get; private set; }

    public float GravityMultiplier=1f;

    public bool UseGravity { get {return _Rigidbody.useGravity;} set{_Rigidbody.useGravity=value;} }
    public Vector3 TotalVector { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public float SlipSpeed =1f;

    public Collision ColliderHit { get; private set; }
    public Vector3 BehindVector { get; private set; }

    private float hitDistance;
    public Vector3 totalUpdateMovePosition;
    private RaycastHit m_Hit;

    private void Awake()
    {
        _CheckGrounded= GetComponent<ICheckGrounded>();
        _Rigidbody= GetComponent<Rigidbody>();        
        _characterState = GetComponent<CharacterState>();
    }
    private void Start()
    {
        Locomotion= GetComponent<Character>()._Locomotion;        
    }
    private void Update()
    {        
        BehindVector = Quaternion.Euler(0,transform.eulerAngles.y,0)*(Vector3.back+Vector3.down).normalized;
    }
    private void FixedUpdate()
    {
        HandleIfCharacterIsStuckOnLedge();
        if(_Rigidbody.velocity.y<0f)
        {
            var v=_Rigidbody.velocity;
            v.y*=1.1f;
            _Rigidbody.velocity=v;
        }
        
    }

    private void HandleIfCharacterIsStuckOnLedge()
    {
        bool StuckinAir = !_CheckGrounded.isGrounded && _Rigidbody.velocity == Vector3.zero;
        var onLedge =Physics.Raycast(transform.position + Vector3.up*0.1f,BehindVector, out m_Hit,0.3f);
        bool stuckonledge=onLedge&&StuckinAir;
        if (stuckonledge)
        {
            Debug.Log("Commence Slipping");
            Vector3 Direction=transform.forward;
            Direction.y=0;
            Direction=Direction.normalized*Time.deltaTime*SlipSpeed;
            _Rigidbody.velocity+=(Direction);
        }
    }

    public void AddVelocity(Vector3 vInput)
    {
        _Rigidbody.velocity+=vInput;
    }

    public void OnUpdateAnimatorMove(Vector3 motion)
    {
        //_Rigidbody.MovePosition(transform.position+motion);
        transform.position=transform.position+motion;
        //totalUpdateMovePosition+=motion;
    }
    public void SetConstantMoveUpdate(Vector3 motion)
    {
        totalUpdateMovePosition+=motion;
    }
    public void SetVelocity(Vector3 vInput)
    {
        _Rigidbody.velocity=vInput;
    }
    public void SetGroundVelocity(float x, float z)
    {
        var v=new Vector3(x,_Rigidbody.velocity.y,z);
        _Rigidbody.velocity=v;
    }
    public void AddExtraMotion(Vector3 motion)
    {
        throw new System.NotImplementedException();
    }

    public void Jump(float height)
    {
        float force = (Mathf.Sqrt(height * -2f * Physics.gravity.y * GravityMultiplier));
        _Rigidbody.AddForce (new Vector3(0,force,0),ForceMode.VelocityChange);
        _Rigidbody.angularVelocity=Vector3.zero;
        _Rigidbody.isKinematic=false;
    }

    void OnCollisionEnter (Collision hit)
    {
        Debug.Log("Hit: "+hit.gameObject.name);
        ColliderHit= hit;
        hitDistance=(transform.position-ColliderHit.contacts[0].point).magnitude;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position+ Vector3.up*0.1f, BehindVector * 0.3f);
    }
}
