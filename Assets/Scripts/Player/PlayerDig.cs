using System.Collections;

using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerDig : MonoBehaviour
{
    private PlayerMovement playerMovement;
    [SerializeField] private Animator animator;

    [SerializeField] private float digDistance;

    [SerializeField] private float destroyableObjectSpawnTime;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Transform tileSelector;

    public float normalDigTime = 1, fastDigTime = 0.5f;
    private float digTime;


    private bool dig;

    public Vector3 deadBoxSize = new Vector3(0.4f, 0.7f);
    private Vector2 deadBoxPosition;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        ChangeDigTime(normalDigTime);
    }
    public void ChangeDigTime(float value)
    {
        digTime = value;
    }
    void Update()
    {

        

        Vector3 dir = transform.right;
        if (transform.localScale.x > 0) dir = transform.right;
        if (transform.localScale.x < 0) dir = -transform.right;

        Vector3 pos = transform.position+ dir * 0.5f;
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.down, digDistance);
        Debug.DrawRay(pos, Vector2.down* digDistance);

        bool isGrounded = animator.GetBool("IsGrounded");

        if (hit.collider!=null &&!CheckDir() && isGrounded)
        {
            bool destroyable = false;
            Vector3Int point = Vector3Int.zero;
            Tilemap tileMap = null;
            if (hit.collider.TryGetComponent(out Tilemap _tilemap))
            {
                tileMap = _tilemap;
                point = tileMap.WorldToCell(tileSelector.position);
                Sprite sprite = tileMap.GetSprite(point);
                if (sprite)
                {
                    if (sprite.name == Const.DestroyableObjectName) destroyable = true;
                }
            }
            if (hit.collider != null && destroyable)
            {
                if (playerMovement.IsCrouching()) return;

                if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
                {
                    //playerMovement.transform.position = tileSelector.position+Vector3.up-dir/2;
                    dig = true;
                    playerMovement.SetMove(false);
                    animator.SetTrigger("Dig");

                    if (digTime == fastDigTime)
                        SoundManager.Instance.PlayOneShot("DigFast");
                    else SoundManager.Instance.PlayOneShot("Dig");

                    StartCoroutine(ResetDig(tileMap,point));
                }
            }
        }

    }
  
    private void OnDrawGizmos()
    {
        Vector3 dir = transform.right;
        if (transform.localScale.x > 0) dir = transform.right;
        if (transform.localScale.x < 0) dir = -transform.right;
        Vector3 pos = transform.position + dir * 0.5f;
        Gizmos.DrawRay(pos, Vector2.down * digDistance);
        if(playerMovement!=null)
            Gizmos.DrawWireCube(deadBoxPosition, deadBoxSize);
    }
    private IEnumerator ResetDig(Tilemap tilemap,Vector3Int pos)
    {
        yield return new WaitForSeconds(digTime);
        TileBase tileBase= tilemap.GetTile(pos);
        tilemap.SetTile(pos, null);

        StartCoroutine(CreateDeletedObject(tileBase,pos,tilemap));
        animator.Play("Idle");
        dig = false;
        playerMovement.SetMove(true);
    }

    private IEnumerator CreateDeletedObject(TileBase tileBase,Vector3Int pos,Tilemap tilemap)
    {
        yield return new WaitForSeconds(destroyableObjectSpawnTime);
        Vector2 worldPosition = tilemap.CellToWorld(pos);
        deadBoxPosition = worldPosition+new Vector2(0.5f,0.5f);//tilemap bound'u sol alttan baþlýyor(standard bound sol üstten baþlýyor)
        var hits = Physics2D.BoxCastAll(deadBoxPosition, deadBoxSize, 0, Vector2.zero);
        foreach (var hit in hits)
        {
            if(hit.collider.TryGetComponent(out PlayerHealth playerHealth))
            {
                Debug.Log(worldPosition);
                playerHealth.Respawn();
            }
        }
        tilemap.SetTile(pos, tileBase);
    }


    private bool CheckDir()
    {
        Vector3 pos = transform.position - Vector3.up * 0.5f;
        Vector3 dir = transform.right;
        if (transform.localScale.x > 0) dir = transform.right;
        if (transform.localScale.x < 0) dir = -transform.right;

        RaycastHit2D hit = Physics2D.Raycast(pos, dir, 1, layerMask);
        Debug.DrawRay(pos, dir * 1, hit.collider != null ? Color.red : Color.green  );
        if (hit.collider!=null)//&&hit.collider.CompareTag(Const.DestroyableObjectName)
        {
            return true;
        }
        return false;
    }

}
