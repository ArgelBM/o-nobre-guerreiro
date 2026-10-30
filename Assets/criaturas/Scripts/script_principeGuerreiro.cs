using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D), typeof(Animator))]
public class script_principeGuerreiro : MonoBehaviour
{
    [SerializeField] private float velocidade = 5f;
    [SerializeField] private float forcaPulo = 10f;

    private static readonly int CorrendoHash = Animator.StringToHash("Correndo");

    private Rigidbody2D corpo;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private float movimentoHorizontal;
    private bool estaNoChao;

    private void Awake()
    {
        corpo = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        corpo.freezeRotation = true;

        if (animator.runtimeAnimatorController == null)
        {
            Debug.LogError("Atribua controller_principeGuerreiro ao Animator do personagem.", this);
        }

        if (spriteRenderer == null)
        {
            Debug.LogError("Adicione um SpriteRenderer ao personagem ou a um de seus filhos.", this);
        }
    }

    private void Update()
    {
        movimentoHorizontal = Input.GetAxisRaw("Horizontal");
        if (spriteRenderer != null && movimentoHorizontal != 0f)
        {
            spriteRenderer.flipX = movimentoHorizontal < 0f;
        }

        if (animator.runtimeAnimatorController != null)
        {
            animator.SetBool(CorrendoHash, estaNoChao && movimentoHorizontal != 0f);
        }

        if (Input.GetButtonDown("Jump") && estaNoChao)
        {
            corpo.velocity = new Vector2(corpo.velocity.x, forcaPulo);
            estaNoChao = false;
        }
    }

    private void FixedUpdate()
    {
        corpo.velocity = new Vector2(movimentoHorizontal * velocidade, corpo.velocity.y);
        estaNoChao = false;
    }

    private void OnCollisionStay2D(Collision2D colisao)
    {
        for (int i = 0; i < colisao.contactCount; i++)
        {
            if (colisao.GetContact(i).normal.y > 0.5f)
            {
                estaNoChao = true;
                return;
            }
        }
    }
}
