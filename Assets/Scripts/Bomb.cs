using UnityEngine;

public class Bomb : Spell
{

    #region Serialized Private Fields
    
    [SerializeField] private GameObject fuse;
    [SerializeField] private float explosionRadius;
    [SerializeField] private float explosionForce;
    [SerializeField] private GameObject explosionPrefab;
    
    #endregion

    #region Private Fields
    #endregion

    #region Public Fields
    #endregion

    #region Unity Functions

    private void OnEnable()
    {
        fuse.SetActive(false);
    }
    
    #endregion

    #region Public Functions

    public override void Place()
    {
        base.Place();
        
        fuse.SetActive(true);
    }
    
    #endregion

    #region Private Functions

    protected override void Trigger()
    {
        var enemies = Physics.OverlapSphere(transform.position, explosionRadius, enemyLayer);

        foreach (var enemy in enemies)
        {
            enemy.GetComponent<Enemy>().Die((enemy.transform.position - transform.position).normalized * explosionForce);
        }

        Instantiate(explosionPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    #endregion
}