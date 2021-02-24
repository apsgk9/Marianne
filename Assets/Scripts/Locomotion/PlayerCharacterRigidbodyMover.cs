
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
    public bool StuckinAir { get; private set; }

    private float hitDistance;
    public Vector3 totalUpdateMovePosition;

    //Stuck In Air/Ledge
    private RaycastHit m_Hit;
    private Vector3 BehindVector;
    private Vector3 FrontVector;
    private Vector3 LeftVector;
    private Vector3 RightVector;
    private const float MaxDistance = 0.4f;
    private const float AcclerationTolerance = 0.05f;//0.001f
    private const int DownMultiplier = 2;

    //capsule Collider
    private CapsuleCollider _capsuleCollider;

    private Vector3 _originalCenter;

    //Acceleration

    private float _previousVelocityMagnitude;
    private float _accelaration;
    private Vector3 offset=Vector3.up * 0.2f;

    private void Awake()
    {
        _CheckGrounded= GetComponent<ICheckGrounded>();
        _Rigidbody= GetComponent<Rigidbody>();        
        _characterState = GetComponent<CharacterState>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _originalCenter=_capsuleCollider.center;
        _previousVelocityMagnitude=_Rigidbody.velocity.magnitude;
    }
    private void Start()
    {
        Locomotion= GetComponent<Character>()._Locomotion;
    }
    private void FixedUpdate()
    {
        
        BehindVector = Quaternion.Euler(0,transform.eulerAngles.y,0)*(Vector3.back+Vector3.down/ DownMultiplier).normalized;
        FrontVector = Quaternion.Euler(0,transform.eulerAngles.y,0)*(Vector3.forward+Vector3.down/DownMultiplier).normalized;
        LeftVector = Quaternion.Euler(0,transform.eulerAngles.y,0)*(Vector3.right+Vector3.down/DownMultiplier).normalized;
        RightVector = Quaternion.Euler(0,transform.eulerAngles.y,0)*(Vector3.left+Vector3.down/DownMultiplier).normalized;    
        AdjustCapsuleWheninAir();
        HandleIfCharacterIsStuckOnLedge();
        if (_Rigidbody.velocity.y < 0f)
        {
            var v = _Rigidbody.velocity;
            v.y *= 1.1f;
            _Rigidbody.velocity = v;
        }

    }
    private void Update()
    {
        _accelaration= (_previousVelocityMagnitude-_Rigidbody.velocity.magnitude)/Time.deltaTime;
    }

    private void AdjustCapsuleWheninAir()
    {
        if (_CheckGrounded.isGrounded)
        {
            _capsuleCollider.center = _originalCenter;
        }
        else
        {
            _capsuleCollider.center = _originalCenter + Vector3.up * 0.1f;
        }
    }

    private void HandleIfCharacterIsStuckOnLedge()
    {
        bool noAcceleration = Mathf.Abs(_accelaration) < AcclerationTolerance;
        StuckinAir = !_CheckGrounded.isGrounded && noAcceleration;

        if(!StuckinAir)
            return; 
        var onLedgeBehind = Physics.Raycast(transform.position + offset, BehindVector, out m_Hit, MaxDistance);
        if (onLedgeBehind)
        {
            Debug.Log("onLedgeBehind");
            StartMoving(FrontVector);
        }
        var onLedgeFront =Physics.Raycast(transform.position + offset,FrontVector, out m_Hit,MaxDistance);
        if (onLedgeFront)
        {
            Debug.Log("onLedgeFront");
            StartMoving(BehindVector);
        }
        var onLedgeLeft =Physics.Raycast(transform.position + offset,LeftVector, out m_Hit,MaxDistance);
        if (onLedgeLeft)
        {
            Debug.Log("onLedgeLeft");
            StartMoving(RightVector);
        }
        var onLedgeRight =Physics.Raycast(transform.position + offset,RightVector, out m_Hit,MaxDistance);
        if (onLedgeRight)
        {
            Debug.Log("onLedgeRight");
            StartMoving(LeftVector);
        }
    }

    private void StartMoving(Vector3 toPushFrom)
    {
        Debug.Log("Commence Slipping");
        Vector3 Direction = toPushFrom;
        Direction.y = 0;
        Direction = Direction.normalized * Time.deltaTime * SlipSpeed;
        _Rigidbody.velocity += (Direction);
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
        ColliderHit= hit;
        hitDistance=(transform.position-ColliderHit.contacts[0].point).magnitude;
    }

    void OnDrawGizmos()
    {
        if(StuckinAir)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position+ offset, BehindVector * MaxDistance);
            Gizmos.DrawRay(transform.position+ offset, FrontVector * MaxDistance);
            Gizmos.DrawRay(transform.position+ offset, LeftVector * MaxDistance);
            Gizmos.DrawRay(transform.position+ offset, RightVector * MaxDistance);
        }
        
    }
}
