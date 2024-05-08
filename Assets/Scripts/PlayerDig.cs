using System.Collections;

using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerDig : MonoBehaviour
{
    private PlayerMovement playerMovement;
    [SerializeField] private Animator animator;

    [SerializeField] private float digDistance;

    [SerializeField] private GameObject destroyableObjectPrefab;
    [SerializeField] private float destroyableObjectSpawnTime;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Transform tileSelector;

    public float normalDigTime = 1, fastDigTime = 0.5f;
    private float digTime;


    private bool dig;


    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        digTime = normalDigTime;
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
                if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
                {
                    playerMovement.transform.position = tileSelector.position+Vector3.up-dir/2;
                    dig = true;
                    playerMovement.SetMove(false);
                    animator.SetTrigger("Dig");
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
    }
    private IEnumerator ResetDig(Tilemap tilemap,Vector3Int pos)
    {
        yield return new WaitForSeconds(digTime);
        TileBase tileBase= tilemap.GetTile(pos);
        tilemap.SetTile(pos, null);

        StartCoroutine(CreateDeletedObject(tileBase,pos,tilemap));
        dig = false;
        playerMovement.SetMove(true);
    }

    private IEnumerator CreateDeletedObject(TileBase tileBase,Vector3Int pos,Tilemap tilemap)
    {
        yield return new WaitForSeconds(destroyableObjectSpawnTime);
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
