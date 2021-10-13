using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash_ : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float trashValue;
    Trashcans_ trashCans;
    TrashSpawner_ trashSpawner_;
    Rigidbody2D rb;
    TrashSound_ trashSound;
    [SerializeField] bool physicsOn;

    [SerializeField] SpriteRenderer trashSpriteRenderer;
   

   [SerializeField]float gravityScaleWhenFalling = 1, gravityScaleWhenHit = 3;
    bool hasBeenMissed = false;
    // Start is called before the first frame update
    void Start()
    {
        //trashSpriteRenderer.sprite = trashSprites[Random.Range(0, trashSprites.Length)];
        rb = GetComponent<Rigidbody2D>();
        trashCans = FindObjectOfType<Trashcans_>();
        trashSpawner_ = FindObjectOfType<TrashSpawner_>();
        rb.gravityScale = gravityScaleWhenFalling;
        trashSound = GetComponent<TrashSound_>();
    }

    private void FixedUpdate()
    {
        if (!physicsOn)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.velocity = new Vector2(0, speed);
        }
        else rb.bodyType = RigidbodyType2D.Dynamic;

        if (!trashSpawner_.IsPlay()) Destroy(this.gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        if (!trashSpawner_.IsPlay() && !hasBeenMissed)
        {
            hasBeenMissed = true;
            CheckAndAddMissedTrashType();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        rb.gravityScale = gravityScaleWhenHit;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.tag == "PlasticTrashcan")
        {
            if (this.gameObject.tag == "Plastic")
            {
                Debug.Log("I AM PLASTIC ");
                trashCans.AddPoints(trashValue, 2);
                trashSound.TrashCorrectSound();
            }
            else
            {
                Debug.Log("WRONG TRASH IN PLASTIC BIN");

                CheckAndAddMissedTrashType();
            }
            Destroy(this.gameObject);
        }
        if (collision.tag == "MetalTrashcan")
        {
            if (this.gameObject.tag == "Metal")
            {
                Debug.Log("I AM METAL");
                trashCans.AddPoints(trashValue, 1);
                trashSound.TrashCorrectSound();
            }
            else
            {
                Debug.Log("WRONG TRASH IN PLASTIC BIN");

                CheckAndAddMissedTrashType();
            }
            Destroy(this.gameObject);
        }
        if (collision.tag == "GlassTrashcan")
        {


            if (this.gameObject.tag == "Glass")
            {
                Debug.Log("I AM GLASS ");
                trashCans.AddPoints(trashValue, 0);
                trashSound.TrashCorrectSound();
            }
            else
            {
                Debug.Log("WRONG TRASH IN PLASTIC BIN");

                CheckAndAddMissedTrashType();
            }
            Destroy(this.gameObject);
        }

        if (collision.tag == "Ground" && !hasBeenMissed)
        {
            hasBeenMissed = true;
            CheckAndAddMissedTrashType();
            trashSound.TrashMissedSound();
            // Debug.Log("GROUND MISS");
        }






        /*

        if (collision.tag == "PlasticTrashcan" && this.gameObject.tag != "Plastic")
        {
            // Debug.Log("WRONG TRASH IN PLASTIC BIN");

            CheckAndAddMissedTrashType();

            Destroy(this.gameObject);
        }
        if (collision.tag == "MetalTrashcan" && this.gameObject.tag != "Metal")
        {
            // Debug.Log("WRONG TRASH IN METAL BIN");
            CheckAndAddMissedTrashType();

            Destroy(this.gameObject);
        }
        if (collision.tag == "GlassTrashcan" && this.gameObject.tag != "Glass")
        {
            //  Debug.Log("WRONG TRASH IN GLASS BIN");
            CheckAndAddMissedTrashType();

            Destroy(this.gameObject);

        }*/
    }

    private void CheckAndAddMissedTrashType()
    {
        // Debug.Log("Missed " + this.gameObject.tag);
        Debug.Log("GENERAL MISS");
        trashSpawner_.AddMissedTrash();

        if (this.gameObject.tag == "Plastic")
        {
            trashSpawner_.AddMissedTrashTypeAmount(2);
        }
        else if (this.gameObject.tag == "Metal")
        {
            trashSpawner_.AddMissedTrashTypeAmount(1);
        }
        else
        {
            trashSpawner_.AddMissedTrashTypeAmount(0);
        }
    }

}
