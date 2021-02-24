
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

    public Vector3 totalUpdateMovePosition;

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

    
}
