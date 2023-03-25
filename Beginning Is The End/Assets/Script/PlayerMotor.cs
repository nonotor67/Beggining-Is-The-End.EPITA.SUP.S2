using Mirror.Examples.AdditiveLevels;
using UnityEngine;


public class PlayerMotor : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;

    //Vitesse de d�paclement
    private const float WalkSpeed = 5f;
    private const float SprintSpeed = 10f;
    private const float SprintTurnSpeed = 10f;
    private const float TurnSpeed = 5f;
    private const float JumpForce = 15f;
    
    //Inputs
    [SerializeField] private string inputFront = "z";
    [SerializeField] private string inputBack = "s";
    [SerializeField] private string inputLeft = "q";
    [SerializeField] private string inputRight = "d";
    private Player _player;
    private CapsuleCollider _capsuleCollider;
    GameObject Player_GFX;
    

    private void Start()
    {
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _player = GetComponent<Player>();
        Player_GFX = GameObject.Find("Player GFX");
    }

    private bool IsGrounded()
    {
        var distanceToGround = _capsuleCollider.bounds.extents.y;
        return Physics.Raycast(rb.transform.position, -Vector3.up, distanceToGround + 0.1f);
    }
    
    private bool IsNotTired(float ernegyCost)
    {
        return _player.energy > ernegyCost;
    }
    
    private void Update()
    {
        var ini = Player_GFX.transform.position;
        
        //Gestion des inpputs est des vitesse associ� (marche / court) sur l'axe z

        if (Input.GetKey(inputFront) && !Input.GetKey(inputRight) && !Input.GetKey(inputLeft) && !Input.GetKey(KeyCode.LeftShift))
        {
            rb.MovePosition(rb.transform.position + (WalkSpeed * Time.fixedDeltaTime * rb.transform.forward));
        }

        if (Input.GetKey(inputFront) && !Input.GetKey(inputRight) && !Input.GetKey(inputLeft) && Input.GetKey(KeyCode.LeftShift) && IsNotTired(0.1f) &&!IsNearLabo())
        {
            rb.MovePosition(rb.transform.position + (SprintSpeed * Time.fixedDeltaTime * rb.transform.forward));
            _player.UseEnergy(0.1f);
        }

        if (Input.GetKey(inputBack) && !Input.GetKey(inputRight) && !Input.GetKey(inputLeft) && !Input.GetKey(KeyCode.LeftShift))
        {
            rb.MovePosition(rb.transform.position + (-WalkSpeed * Time.fixedDeltaTime * rb.transform.forward ));
        }

        if (Input.GetKey(inputBack) && !Input.GetKey(inputRight) && !Input.GetKey(inputLeft) && Input.GetKey(KeyCode.LeftShift) && IsNotTired(0.1f) && !IsNearLabo())
        {
            rb.MovePosition(rb.transform.position + (-SprintSpeed * Time.fixedDeltaTime * rb.transform.forward));
            _player.UseEnergy(0.1f);
        }

        //Gestion du saut 

        var jumpVelocity = Vector3.zero;

        if (IsGrounded() && Input.GetKeyDown(KeyCode.Space) && IsNotTired(3) && !IsNearLabo())
        {
            rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
            _player.UseEnergy(3);
        }

        //Gestion des inpputs est des vitesse associ� (marche / court) sur l'axe x

        if (!Input.GetKey(inputFront) && Input.GetKey(inputRight) && !Input.GetKey(KeyCode.LeftShift))
        {
            rb.MovePosition(rb.transform.position + (TurnSpeed * Time.fixedDeltaTime * rb.transform.right));
        }

        if (!Input.GetKey(inputFront) && Input.GetKey(inputRight) && Input.GetKey(KeyCode.LeftShift) && IsNotTired(0.1f) && !IsNearLabo())
        {
            rb.MovePosition(rb.transform.position + (SprintTurnSpeed * Time.fixedDeltaTime * rb.transform.right));
            _player.UseEnergy(0.1f);
        }

        if (!Input.GetKey(inputFront) && Input.GetKey(inputLeft) && !Input.GetKey(KeyCode.LeftShift))
        {
            rb.MovePosition(rb.transform.position + (-TurnSpeed * Time.fixedDeltaTime * rb.transform.right));
        }

        if (!Input.GetKey(inputFront) && Input.GetKey(inputLeft) && Input.GetKey(KeyCode.LeftShift) && IsNotTired(0.1f) && !IsNearLabo())
        {
            rb.MovePosition(rb.transform.position + (-SprintTurnSpeed * Time.fixedDeltaTime * rb.transform.right));
            _player.UseEnergy(0.1f);
        }
        
        //Gestion des déplacements en diagonale
        
        if (Input.GetKey(inputFront) && Input.GetKey(inputRight) && !Input.GetKey(KeyCode.LeftShift))
        {
            rb.MovePosition(rb.transform.position + (WalkSpeed * Time.fixedDeltaTime * rb.transform.forward + TurnSpeed * Time.fixedDeltaTime * rb.transform.right));
        }
        
        if (Input.GetKey(inputFront) && Input.GetKey(inputRight) && Input.GetKey(KeyCode.LeftShift) && IsNotTired(0.1f) &&!IsNearLabo())
        {
            rb.MovePosition(rb.transform.position + (SprintSpeed * Time.fixedDeltaTime * rb.transform.forward + SprintTurnSpeed * Time.fixedDeltaTime * rb.transform.right));
            _player.UseEnergy(0.1f);
        }
        
        if (Input.GetKey(inputFront) && Input.GetKey(inputLeft) && !Input.GetKey(KeyCode.LeftShift))
        {
            rb.MovePosition(rb.transform.position + (WalkSpeed * Time.fixedDeltaTime * rb.transform.forward + -TurnSpeed * Time.fixedDeltaTime * rb.transform.right));
        }

        if (Input.GetKey(inputFront) && Input.GetKey(inputLeft) && Input.GetKey(KeyCode.LeftShift) && IsNotTired(0.1f) && !IsNearLabo())
        {
            rb.MovePosition(rb.transform.position + (SprintSpeed * Time.fixedDeltaTime * rb.transform.forward + -SprintTurnSpeed * Time.fixedDeltaTime * rb.transform.right));
            _player.UseEnergy(0.1f);
        }
        
        if (Input.GetKey(inputBack) && Input.GetKey(inputRight) && !Input.GetKey(KeyCode.LeftShift))
        {
            rb.MovePosition(rb.transform.position + (-WalkSpeed * Time.fixedDeltaTime * rb.transform.forward + TurnSpeed * Time.fixedDeltaTime * rb.transform.right));
        }
        
        if (Input.GetKey(inputBack) && Input.GetKey(inputRight) && Input.GetKey(KeyCode.LeftShift) && IsNotTired(0.1f) && !IsNearLabo())
        {
            rb.MovePosition(rb.transform.position + (-SprintSpeed * Time.fixedDeltaTime * rb.transform.forward + SprintTurnSpeed * Time.fixedDeltaTime * rb.transform.right));
            _player.UseEnergy(0.1f);
        }
        
        if (Input.GetKey(inputBack) && Input.GetKey(inputLeft) && !Input.GetKey(KeyCode.LeftShift))
        {
            rb.MovePosition(rb.transform.position + (-WalkSpeed * Time.fixedDeltaTime * rb.transform.forward + -TurnSpeed * Time.fixedDeltaTime * rb.transform.right));
        }
        
        if (Input.GetKey(inputBack) && Input.GetKey(inputLeft) && Input.GetKey(KeyCode.LeftShift) && IsNotTired(0.1f) && !IsNearLabo())
        {
            rb.MovePosition(rb.transform.position + (-SprintSpeed * Time.fixedDeltaTime * rb.transform.forward + -SprintTurnSpeed * Time.fixedDeltaTime * rb.transform.right));
            _player.UseEnergy(0.1f);
        }
        
        //gestion de la récupération d'énergie
        _player.AddEnergy(0.05f);

        Player_GFX.transform.position = ini;
    }
    
    //Detecte si le joeur est proche d'un object possedant le tag "Labo"
    private bool IsNearLabo()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 15);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Labo"))
            {
                return true;
            }
        }
        return false;
    }
}
