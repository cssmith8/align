using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMain : MonoBehaviour
{
    public InputActionReference leftStickReference = null;

    private Rigidbody rb;
    private GameObject xrOrigin;
    private PerspectiveRotate pr;

    [SerializeField]
    private float baseSpeed = 5f;

    [SerializeField]
    private GameObject empty;

    public List<GameObject> stars;

    public Vector3 castDirection;


    private float speedMultiplier = 1f;
    private TrailRenderer tr;

    private bool completedLevel = false;

    // Start is called before the first frame update
    void Start()
    {
        stars = new List<GameObject>();
        xrOrigin = GameObject.FindGameObjectWithTag("XROrigin");
        pr = xrOrigin.GetComponent<PerspectiveRotate>();
        rb = GetComponent<Rigidbody>();        

        tr = transform.GetChild(0).GetComponent<TrailRenderer>();
        tr.time = 0;

        GenerateLevel();
        transform.position = Spawnpoint.Instance.spawnpoint;
        Invoke("Respawn", 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -15f)
        {
            Respawn();
        }

        if (completedLevel) return;

        Vector2 input = leftStickReference.action.ReadValue<Vector2>();

        float angle = 0 - pr.angleMain;
        transform.eulerAngles = new Vector3(0f, angle, 0f);

        input *= speedMultiplier;

        transform.position += transform.forward * input.y / 100f * baseSpeed;
        transform.position += transform.right * input.x / 100f * baseSpeed;
        
    }

    private void Respawn()
    {
        transform.position = Spawnpoint.Instance.spawnpoint;
        rb.velocity = Vector3.zero;

        Spawnpoint.Instance.PlayParticles();
    }

    public int TryStar(GameObject star)
    {
        if (stars.Contains(star)) return -1;

        int length = stars.Count;

        if (length == 0)
        {
            stars.Add(star);
            Arrow.Instance.SetOrigin(star.transform.position);
            return 1;
        } else if (length == 1)
        {
            stars.Add(star);
            castDirection = star.transform.position - stars[0].transform.position;
            Arrow.Instance.SetDirection(castDirection);
            Arrow.Instance.SetGameObjectTarget(star);
            return 2;
        } else
        {
            Vector3 star0 = stars[0].transform.position;
            Vector3 star1 = stars[1].transform.position;
            Vector3 newStar = star.transform.position;

            Vector3 star1normal = star1 - star0;
            Vector3 newStarnormal = newStar - star0;

            if (star0.x != star1.x)
            {
                //use x position calculation
                float ratio = newStarnormal.x / star1normal.x;
                if (Approximate(star1normal.y * ratio, newStarnormal.y) && Approximate(star1normal.z * ratio, newStarnormal.z))
                {
                    //in line
                    if (castDirection.x > 0)
                    {
                        if (newStar.x > stars[stars.Count - 1].transform.position.x)
                        {
                            //positive x direction
                            stars.Add(star);
                            return stars.Count;
                        }
                    } else
                    {
                        if (newStar.x < stars[stars.Count - 1].transform.position.x)
                        {
                            //negative x direction
                            stars.Add(star);
                            return stars.Count;
                        }
                    }
                }
            } else if (star0.y != star1.y)
            {
                //use y position calculation
                float ratio = newStarnormal.y / star1normal.y;
                if (Approximate(star1normal.x * ratio, newStarnormal.x) && Approximate(star1normal.z * ratio, newStarnormal.z))
                {
                    //in line
                    if (castDirection.y > 0)
                    {
                        if (newStar.y > stars[stars.Count - 1].transform.position.y)
                        {
                            //positive y direction
                            stars.Add(star);
                            return stars.Count;
                        }
                    }
                    else
                    {
                        if (newStar.y < stars[stars.Count - 1].transform.position.y)
                        {
                            //negative y direction
                            stars.Add(star);
                            return stars.Count;
                        }
                    }
                }
            } else if (star0.z != star1.z)
            {
                //use z position calculation
                float ratio = newStarnormal.z / star1normal.z;
                if (Approximate(star1normal.x * ratio, newStarnormal.x) && Approximate(star1normal.y * ratio, newStarnormal.y))
                {
                    //in line
                    if (castDirection.z > 0)
                    {
                        if (newStar.z > stars[stars.Count - 1].transform.position.z)
                        {
                            //positive z direction
                            stars.Add(star);
                            return stars.Count;
                        }
                    }
                    else
                    {
                        if (newStar.z < stars[stars.Count - 1].transform.position.z)
                        {
                            //negative z direction
                            stars.Add(star);
                            return stars.Count;
                        }
                    }
                }
            } else
            {
                //both the first stars are in the same spot
                //error
                UnityEngine.Debug.Log("Error: First 2 stars have same location values");
            }
        }
        return -1;
    }

    private bool Approximate(float a, float b)
    {
        if (Mathf.Abs(a - b) < 0.1f)
        {
            return true;
        }
        return false;
    }

    private void CastPlayer()
    {
        Vector3 diff = stars[1].transform.position - stars[0].transform.position;
        Vector3 direction = diff.normalized;
        direction *= 1.4f * (stars.Count - 1);

        Music.Instance.PlaySequence(stars.Count);

        StartCoroutine(Cast(direction));

        GameObject[] gos = GameObject.FindGameObjectsWithTag("Phase");
        foreach (GameObject go in gos)
        {
            go.GetComponent<PhasePlane>().GlowActivate();
        }
    }

    IEnumerator Cast(Vector3 v)
    {
        

        float animTime = 0.5f;
        float castTime = 2f;

        float time1 = Time.timeSinceLevelLoad;
        float time2 = time1 + animTime;
        float time3 = time2 + castTime;
        float time4 = time3 + animTime;

        while (Time.timeSinceLevelLoad < time2)
        {
            speedMultiplier = (time2 - Time.timeSinceLevelLoad) * 2;
            //start anim
            yield return null;
        }
        rb.useGravity = false;
        tr.time = castTime;
        //rb.AddForce(v * 20f);
        while (Time.timeSinceLevelLoad < time3)
        {
            //cast
            rb.velocity = v * ((time3 - Time.timeSinceLevelLoad) / castTime);
            rb.velocity *= 0.7f;

            //trail
            //tr.time = Time.timeSinceLevelLoad - time3;
            yield return null;
        }
        StartCoroutine(EndTrail(castTime));
        rb.useGravity = true;
        while (Time.timeSinceLevelLoad < time4)
        {
            speedMultiplier = 1f - ((time4 - Time.timeSinceLevelLoad) * 2);
            //end anim
            //tr.time = (animTime - (Time.timeSinceLevelLoad - time4)) * (castTime / animTime);

            yield return null;
        }
        
        speedMultiplier = 1f;

        
    }

    IEnumerator EndTrail(float castTime)
    {
        float endTime = 0.3f;
        float time1 = Time.timeSinceLevelLoad;

        while (Time.timeSinceLevelLoad < time1 + endTime)
        {
            tr.time = (endTime - (Time.timeSinceLevelLoad - time1)) * (castTime / endTime);
            yield return null;
        }
        tr.time = 0;
    }

    public void ReleaseStars(bool cast = true)
    {
        if (cast)
        {
            if (stars.Count > 1)
            {
                CastPlayer();
            }
            
        }
        foreach (GameObject go in stars)
        {
            go.GetComponent<Star>().SetEmission(1f);
        }
        stars.Clear();
        Arrow.Instance.FadeOut();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //respawn if touching water
        if (collision.gameObject.tag == "Water")
        {
            Respawn();
        }

        //change scene if touching goal
        if (collision.gameObject.tag == "Goal" && Time.timeSinceLevelLoad > 1f)
        {
            if (completedLevel) return;
            completedLevel = true;
            //Music.Instance.setComplete(0);
            Music.Instance.PlaySequence(5, 0.09f, true);
            GameObject.FindGameObjectWithTag("Complete").GetComponent<Complete>().CompleteLevel();
            //GameObject.FindGameObjectWithTag("Fade").GetComponent<SceneFade>().FadeOut();
            //Invoke("LoadNext", 1.2f);
        }

        if (collision.gameObject.tag == "RedSwitch")
        {
            //delete all gameobjects tagged RedCube
            GameObject[] redCubes = GameObject.FindGameObjectsWithTag("RedCube");
            foreach (GameObject go in redCubes)
            {
                Destroy(go);
            }
            Music.Instance.PlayNote(4);
        }

        if (collision.gameObject.tag == "BlueSwitch")
        {
            //delete all gameobjects tagged BlueCube
            GameObject[] blueCubes = GameObject.FindGameObjectsWithTag("BlueCube");
            foreach (GameObject go in blueCubes)
            {
                Destroy(go);
            }
            Music.Instance.PlayNote(4);
        }

    }

    private void LoadNext()
    {
        SceneManager.LoadScene("Menu");
    }

    private void GenerateLevel()
    {
        Level level = SceneData.Instance.levels[SceneData.Instance.level];
        Instantiate(level.terrain);
        GameObject stars = Instantiate(empty, new Vector3(0, 15 + level.extraHeight, 0), Quaternion.identity);
        float lowestHeight = 20f;
        foreach (Vector3 v in level.stars)
        {
            GameObject go = Instantiate(SceneData.Instance.star, v, Quaternion.identity);
            go.transform.parent = stars.transform; 
            go.transform.localPosition = v;
            if (v.y < lowestHeight)
            {
                lowestHeight = v.y;
            }
        }
        if (level.stars.Length == 0) lowestHeight = 0;
        stars.transform.position += new Vector3(0, -lowestHeight, 0);
        GetComponent<Tutorials>().Display(level.tutorials);
    }
}
