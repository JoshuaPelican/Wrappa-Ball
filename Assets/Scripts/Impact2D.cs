using UnityEngine;
using UnityEngine.Events;

public class Impact2D : MonoBehaviour
{
    public static readonly int[] ImpactTiers = { 5, 10, 25, 50, 100, int.MaxValue };

    [HideInInspector]
    public UnityEvent<ImpactData> OnImpact = new UnityEvent<ImpactData>();

    [SerializeField] Faction Faction;
    [SerializeField] float ImpactCooldown = 1;
    [SerializeField] AudioClip[] ImpactClips;

    Rigidbody2D rig;
    ParticleSystem impactSystem;

    Vector2 oldVelocity;
    float impactCooldown;

    bool isQuitting;

    private void Awake()
    {
        Init();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleCollision(collision);
    }

    private void FixedUpdate()
    {
        oldVelocity = rig.GetPointVelocity(rig.position);
    }

    private void Update()
    {
        impactCooldown -= Time.deltaTime;
    }

    private void OnApplicationQuit()
    {
        isQuitting = true;
    }

    private void OnDestroy()
    {
        if (isQuitting)
            return;

        impactSystem.transform.parent = null;
        impactSystem.transform.localScale *= 2;
        ParticleSystem.ShapeModule shape = impactSystem.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        impactSystem.Emit(Random.Range(12, 16));
        Destroy(impactSystem.gameObject, 0.75f);
    }

    void Init()
    {
        rig = GetComponent<Rigidbody2D>();
        impactSystem = GetComponentInChildren<ParticleSystem>();

        impactCooldown = 0;

        ParticleSystemRenderer particleRend = impactSystem.GetComponent<ParticleSystemRenderer>();
        SpriteRenderer myRend = GetComponentInChildren<SpriteRenderer>();

        Material particleMat = new Material(particleRend.material);
        particleMat.mainTexture = myRend.sprite.texture;
        particleRend.material = particleMat;

        ParticleSystem.MainModule main = impactSystem.main;
        main.startColor = myRend.color;
    }

    public void HandleCollision(Collision2D collision)
    {
        if (!collision.gameObject.TryGetComponent(out Impact2D other))
            return;

        if (impactCooldown > 0)
            return;

        ImpactData impact = new ImpactData(collision, oldVelocity.magnitude, Faction);

        if (ImpactClips.Length > 0 && impact.ImpactTier > 0)
            AudioManager.PlayAudioRandom(ImpactClips, 1, 1 + (impact.ImpactTier * 0.2f));

        other.Impact(impact);

        impactCooldown = ImpactCooldown;
    }

    public void Impact(ImpactData impact)
    {
        ContactPoint2D contact = impact.Collision.GetContact(0);
        impactSystem.transform.position = contact.point;
        impactSystem.transform.rotation = Quaternion.Euler(0, 0, Vector2.Angle(Vector2.up, contact.normal));
        impactSystem.Emit(Random.Range(impact.ImpactTier, impact.ImpactTier * 2));

        OnImpact.Invoke(impact);
    }
}

public struct ImpactData
{
    public Collision2D Collision;
    public float Force;
    public Faction Faction;
    public int ImpactTier;

    public ImpactData(Collision2D collision, float force, Faction faction)
    {
        Collision = collision;
        Force = force;
        Faction = faction;

        ImpactTier = 0;
        for (int i = 0; i < Impact2D.ImpactTiers.Length; i++)
        {
            if(force < Impact2D.ImpactTiers[i])
            {
                ImpactTier = i;
                break;
            }
        }
    }
}
