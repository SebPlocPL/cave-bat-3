using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Cameras;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;

public class MobileObjectMovement : MonoBehaviour
{
    [SerializeField]
    BiteScript biteScrpt;

    [SerializeField]
    Transform posFromVillage;

    [SerializeField]
    SphereCollider fightCollider;

    [SerializeField]
    TextMeshProUGUI levelNameTxt;

    [SerializeField]
    HorseMove hrsMvScrpt;

    [SerializeField]
    GameObject bird;
    private bool hrsDrnkingBlood;

    [SerializeField]
    GameObject owlPic;
    private Coroutine owlRtnCoroutine;

    [SerializeField]
    Transform owl;

    [SerializeField]
    TextMeshProUGUI owlCountDownText;

    [SerializeField]
    RectTransform birdLifeBar;

    [SerializeField]
    GameObject birdLifeBarBack;

    private float birdLife;

    [SerializeField]
    RectTransform lifeBar;
    private Image lifebarImg;
    private float life = 10f;

    [SerializeField]
    RectTransform staminaBar;

    private float stamina = 100f;

    [SerializeField]
    Transform soundWaves;

    [SerializeField]
    Transform soundWavesStraight;

    [SerializeField]
    MoveOnCollision mvOnCllsn;

    [SerializeField]
    GameObject goBtn;

    [SerializeField]
    GameObject batLight;

    [SerializeField]
    GameObject lookBtn;

    [SerializeField]
    GameObject landFlyBtn;

    [SerializeField]
    TextMeshProUGUI landFlyBtnTxt;

    [SerializeField]
    TextMeshProUGUI vampBatBtnTxt;

    [SerializeField]
    TextMeshProUGUI goBtnTxt;

    [SerializeField]
    GameObject backBtn;

    [SerializeField]
    TextMeshProUGUI backBtnTxt;

    [SerializeField]
    GameObject batShdw;

    [SerializeField]
    GameObject vampLght;

    [SerializeField]
    float groundCheckDistance = 0.1f; // Distance to check for ground

    [SerializeField]
    LayerMask groundLayer; // Layer(s) considered as ground

    [SerializeField]
    TextMeshProUGUI huntCountTxt;

    [SerializeField]
    GameObject flyPic;
    private int huntCount;
    private bool go;
    private bool isGrounded;
    private bool backwards;

    private float strafeNow;

    [SerializeField]
    float minRotation = -89f;

    [SerializeField]
    float maxRotation = 89f;

    [SerializeField]
    float speed = 5f;

    [SerializeField]
    float accel = 3f;
    private float originalSpeed;

    [SerializeField]
    float turnSpeed = 60f;

    [SerializeField]
    float turnFlySpeed = 120f;

    [SerializeField]
    float flyUpForce = 5f;
    public bool vampOn;

    [SerializeField]
    float crawlSpeedBat;

    [SerializeField]
    float crawlSpeedVamp;
    private float crawlSpeed;

    [SerializeField]
    float crawlAccel = 5f;
    private float originalCrawlSpeed;

    //private bool vampOn;
    private float crawlSpeedNow;

    [SerializeField]
    float rotationResetDuration = 0.5f;

    [SerializeField]
    GameObject jumpButton;

    [SerializeField]
    TextMeshProUGUI textFastBtn;

    [SerializeField]
    Renderer[] batRndr;

    [SerializeField]
    Renderer vampRndr;

    [SerializeField]
    Animator aniContrBat;

    [SerializeField]
    Animator aniContrVamp;

    [SerializeField]
    CapsuleCollider crawlCollider;

    [SerializeField]
    CapsuleCollider vampCollider;

    [SerializeField]
    BoxCollider flyCollider;

    [SerializeField]
    BoxCollider biteCollider;

    [SerializeField]
    Transform autoCam;

    [SerializeField]
    Transform autoCamPivot;

    [SerializeField]
    AutoCam autoCamScrpt;

    [SerializeField]
    Transform batBody;

    [SerializeField]
    Transform vampBody;

    [SerializeField]
    private Joystick joystick;

    private bool isJoystickTouched;
    private float resetStartTime;
    private bool isResettingRotation;

    private bool isLooking;

    private Rigidbody rb;

    private bool flyForward;

    private bool crawling;

    private bool fastOn;

    private bool isJumping;

    private GameObject[] enemies;
    private bool finished;
    public GameController gmCntrlScrpt;
    private bool owlComing;

    public bool birdAttking;

    private bool birdBitten;

    private GameObject birdDead;
    public bool attacking;

    private Scene scene;

    private void Start()
    {
        scene = SceneManager.GetActiveScene();
        lifebarImg = lifeBar.GetComponent<Image>();
        rb = GetComponent<Rigidbody>();
        crawlSpeed = crawlSpeedBat;
        originalCrawlSpeed = crawlSpeed;
        originalSpeed = speed;
        soundWaves.gameObject.SetActive(true);
        if (SaveManager.saveGlob.level != 2)
        {
            soundWavesStraight.gameObject.SetActive(true);
        }
        StartCoroutine(SoundDetectorRtn());
        gmCntrlScrpt = GameObject
            .FindGameObjectWithTag("GameController")
            .GetComponent<GameController>();
        if (SaveManager.saveGlob.level > 0)
        {
            staminaBar.gameObject.SetActive(true);
            textFastBtn.transform.parent.gameObject.SetActive(true);
            StartCoroutine(StaminaRtn());
        }
        if (SaveManager.saveGlob.level > 1)
        {
            landFlyBtn.SetActive(true);
            birdDead = Resources.Load("birdDead") as GameObject;
        }
        else
        {
            flyPic.SetActive(true);
            huntCountTxt.text = "0";
        }
        if (SaveManager.saveGlob.level == 3)
        {
            soundWavesStraight.gameObject.SetActive(false);
        }
        if (SaveManager.saveGlob.level == 4)
        {
            soundWavesStraight.gameObject.SetActive(false);
        }
        if (SaveManager.saveGlob.level < 5)
        {
            vampBatBtnTxt.transform.parent.gameObject.SetActive(false);
        }
        if (scene.name == "village")
        {
            if (SaveManager.saveGlob.level == 5)
            {
                soundWavesStraight.gameObject.SetActive(true);
            }
        }
        if (scene.name == "maze")
        {
            CrawlOnOff();
            landFlyBtn.SetActive(false);
            life = SaveManager.saveGlob.lifeNow;
            lifeBar.sizeDelta = new Vector2(life * 6f, 10f);
            soundWavesStraight.gameObject.SetActive(false);
        }
        if (scene.name == "crystalcave")
        {
            if (SaveManager.saveGlob.level == 4)
            {
                StartCoroutine(FirstChangeToVampRtn());
            }
            else if (SaveManager.saveGlob.level >= 5)
            {
                if (SaveManager.saveGlob.enterFrom == "village")
                {
                    transform.position = posFromVillage.position;
                    transform.rotation = posFromVillage.rotation;
                    VampOnOff();
                }
                if (SaveManager.saveGlob.enterFrom == "maze")
                {
                    aniContrBat.SetBool("wzlot", true);
                }
            }
            life = SaveManager.saveGlob.lifeNow;
            lifeBar.sizeDelta = new Vector2(life * 6f, 10f);
            soundWavesStraight.gameObject.SetActive(false);
        }

        if (scene.name == "entrancehall")
        {
            life = SaveManager.saveGlob.lifeNow;
            lifeBar.sizeDelta = new Vector2(life * 6f, 10f);
            soundWavesStraight.gameObject.SetActive(false);
            VampOnOff();
        }
    }

    IEnumerator FirstChangeToVampRtn()
    {
        yield return new WaitForSeconds(3f);
        VampOnOff();
    }

    public void TakeLife(float howMuch)
    {
        life -= howMuch;
        if (life <= 0f)
        {
            gmCntrlScrpt.EndLevel(false);
            life = 0f;
        }
        if (life > 100f)
        {
            life = 100f;
        }
        lifeBar.sizeDelta = new Vector2(life * 6f, 10f);
        if (howMuch > 0f)
        {
            StartCoroutine(BlinkLifebarPurpleRtn());
        }
        else
        {
            StartCoroutine(BlinkLifebarGreenRtn());
        }
    }

    IEnumerator BirdBiteOffRtn()
    {
        yield return new WaitForSeconds(4f);
        birdBitten = false;
    }

    IEnumerator OwlRtn()
    {
        for (int i = 30; i > 0; i--)
        {
            owlPic.SetActive(true);
            owlCountDownText.text = i.ToString();
            yield return new WaitForSeconds(0.6f);
            owlPic.SetActive(false);
            yield return new WaitForSeconds(0.4f);
        }
        finished = true;
        owl.gameObject.SetActive(true);
        for (int i = 10; i > 0; i--)
        {
            owl.Translate(0f, -0.1f, 0f);
            yield return new WaitForSeconds(0.05f);
        }
        gmCntrlScrpt.EndLevel(false);
    }

    IEnumerator SoundDetectorRtn()
    {
        if (SaveManager.saveGlob.level < 2)
        {
            enemies = GameObject.FindGameObjectsWithTag("fly");
            while (enemies.Length > 0)
            {
                yield return new WaitForSeconds(0.13f);
                if (SaveManager.saveGlob.level < 2)
                {
                    enemies = GameObject.FindGameObjectsWithTag("fly");
                    if (enemies.Length > 0)
                    {
                        Transform closestEnemy = GetClosestEnemy(enemies);
                        if (closestEnemy != null)
                        {
                            soundWaves.LookAt(closestEnemy);
                        }
                    }
                }
            }
        }
        if (SaveManager.saveGlob.level == 2)
        {
            while (true)
            {
                yield return new WaitForSeconds(0.13f);
                soundWaves.LookAt(hrsMvScrpt.transform.position + new Vector3(0f, 1f, 0f));
            }
        }
        if (SaveManager.saveGlob.level == 3)
        {
            Transform eggTrnsfrm = GameObject.FindGameObjectWithTag("egg").transform;
            while (eggTrnsfrm != null)
            {
                yield return new WaitForSeconds(0.13f);
                soundWaves.LookAt(eggTrnsfrm);
            }
            yield return new WaitForSeconds(0.13f);
            Transform birdTrnsfrm = GameObject.FindGameObjectWithTag("bird").transform;
            while (birdTrnsfrm != null)
            {
                yield return new WaitForSeconds(0.13f);
                soundWaves.LookAt(birdTrnsfrm);
            }
        }
        if (SaveManager.saveGlob.level == 4)
        {
            yield return new WaitForSeconds(0.1f);
            if (scene.name == "village")
            {
                Transform mazeEntrance = GameObject.FindGameObjectWithTag("maze").transform;
                while (true)
                {
                    yield return new WaitForSeconds(0.13f);
                    soundWaves.LookAt(mazeEntrance);
                }
            }
            if (scene.name == "maze")
            {
                enemies = GameObject.FindGameObjectsWithTag("scorpion");
                while (enemies.Length > 0)
                {
                    yield return new WaitForSeconds(0.13f);
                    enemies = GameObject.FindGameObjectsWithTag("scorpion");
                    if (enemies.Length > 0)
                    {
                        Transform closestEnemy = GetClosestEnemy(enemies);
                        if (closestEnemy != null)
                        {
                            soundWaves.LookAt(closestEnemy);
                        }
                    }
                }

                Transform mazeExit = GameObject.FindGameObjectWithTag("crystalcave").transform;
                while (true)
                {
                    yield return new WaitForSeconds(0.13f);
                    soundWaves.LookAt(mazeExit);
                }
            }
        }
        else if (SaveManager.saveGlob.level == 5)
        {
            yield return new WaitForSeconds(0.1f);
            if (scene.name == "village")
            {
                enemies = GameObject.FindGameObjectsWithTag("fly");
                while (enemies.Length > 0)
                {
                    yield return new WaitForSeconds(0.13f);
                    enemies = GameObject.FindGameObjectsWithTag("fly");
                    if (enemies.Length > 0)
                    {
                        Transform closestEnemy = GetClosestEnemy(enemies);
                        if (closestEnemy != null)
                        {
                            soundWaves.LookAt(closestEnemy);
                        }
                    }
                }
                Transform crystalCave = GameObject.FindGameObjectWithTag("crystalcave").transform;
                while (true)
                {
                    yield return new WaitForSeconds(0.13f);
                    soundWaves.LookAt(crystalCave);
                }
            }
            if (scene.name == "crystalcave")
            {
                Transform vamp = GameObject.FindGameObjectWithTag("vamp").transform;
                while (true)
                {
                    yield return new WaitForSeconds(0.13f);
                    soundWaves.LookAt(vamp);
                }
            }
        }
        soundWaves.gameObject.SetActive(false);
        soundWavesStraight.gameObject.SetActive(false);
    }

    public void Eat(string what)
    {
        if (what == "egg")
        {
            life = 100f;
            lifeBar.sizeDelta = new Vector2(600f, 10f);
            StartCoroutine(BlinkLifebarGreenRtn());
            bird.SetActive(true);
            birdLife = 100f;
            birdLifeBar.gameObject.SetActive(true);
            birdLifeBarBack.SetActive(true);
        }
        if (what == "fly")
        {
            if (SaveManager.saveGlob.level < 2)
            {
                huntCount += 1;
                huntCountTxt.text = huntCount.ToString();
            }
            life += 3f;
            if (life > 100f)
            {
                life = 100f;
            }
            lifeBar.sizeDelta = new Vector2(life * 6f, 10f);
            StartCoroutine(BlinkLifebarGreenRtn());

            if (SaveManager.saveGlob.level == 0 && huntCount >= 5 && !finished)
            {
                finished = true;
                gmCntrlScrpt.EndLevel(true);
            }
            if (SaveManager.saveGlob.level == 1 && huntCount >= 3 && !finished)
            {
                finished = true;
                gmCntrlScrpt.EndLevel(true);
            }
        }
        if (what == "horse")
        {
            life += 1;
            if (life > 100f)
            {
                life = 100f;
            }
            lifeBar.sizeDelta = new Vector2(life * 6f, 10f);
            StartCoroutine(BlinkLifebarGreenRtn());

            if (SaveManager.saveGlob.level == 2 && life >= 50 && !finished)
            {
                finished = true;
                gmCntrlScrpt.EndLevel(true);
            }
        }
        if (what == "bird")
        {
            if (!birdBitten)
            {
                birdBitten = true;
                birdLife -= 5f;
                birdLifeBar.sizeDelta = new Vector2(birdLife * 6f, 10f);
                if (birdLife <= 0f)
                {
                    Vector3 birdPos = bird.transform.position;
                    Quaternion birdRot = bird.transform.rotation;
                    bird.SetActive(false);
                    Instantiate(birdDead, birdPos, birdRot);
                    birdLifeBar.gameObject.SetActive(false);
                    birdLifeBarBack.SetActive(false);
                    if (SaveManager.saveGlob.level == 3 && !finished)
                    {
                        finished = true;
                        gmCntrlScrpt.EndLevel(true);
                    }
                }
                StartCoroutine(BirdBiteOffRtn());
            }
        }
        if (what == "scorpion" || what == "mouse")
        {
            life += 10;
            if (life > 100f)
            {
                life = 100f;
            }
            lifeBar.sizeDelta = new Vector2(life * 6f, 10f);
            StartCoroutine(BlinkLifebarGreenRtn());
        }
    }

    public void VampAttack(GameObject whom)
    {
        transform.LookAt(whom.transform, Vector3.up);
        if (Random.Range(0, 2) == 0)
        {
            aniContrVamp.SetTrigger("attack1");
        }
        else
        {
            aniContrVamp.SetTrigger("attack2");
        }
        StartCoroutine(VampAttackRtn(whom));
    }

    IEnumerator VampAttackRtn(GameObject whom)
    {
        yield return new WaitForSeconds(0.5f);
        if (whom != null && vampOn)
        {
            if (biteScrpt.collidingObjects.Contains(whom))
            {
                if (whom.tag == "vamp")
                    whom.GetComponent<VampireScript>().TakeLife();
                if (whom.tag == "goblin")
                    whom.GetComponent<GoblinScript>().TakeLife();
            }
        }

        yield return new WaitForSeconds(0.5f);
        attacking = false;
    }

    IEnumerator BlinkLifebarGreenRtn()
    {
        lifebarImg.color = Color.green;
        yield return new WaitForSeconds(0.4f);
        lifebarImg.color = Color.red;
    }

    IEnumerator BlinkLifebarPurpleRtn()
    {
        lifebarImg.color = Color.magenta;
        yield return new WaitForSeconds(0.4f);
        lifebarImg.color = Color.red;
    }

    public void LookOnOff(bool how)
    {
        if (how)
        {
            autoCamScrpt.SetTarget(null);
        }
        else
        {
            autoCamScrpt.SetTarget(transform);
        }
        isLooking = how;
    }

    public void GoOnOff()
    {
        if (!go)
        {
            backwards = false;
            go = true;
            backBtnTxt.text = "BACK";
            goBtnTxt.text = "STOP";
        }
        else
        {
            go = false;
            goBtnTxt.text = "GO";
        }
    }

    public void BackOnOff()
    {
        if (!backwards)
        {
            go = false;
            backwards = true;
            backBtnTxt.text = "STOP";
            goBtnTxt.text = "GO";
        }
        else
        {
            backwards = false;
            backBtnTxt.text = "BACK";
        }
    }

    public void FastOnOff()
    {
        if (fastOn)
        {
            fastOn = false;
            speed = originalSpeed;
            crawlSpeed = originalCrawlSpeed;
            textFastBtn.color = Color.grey;
            if (!crawling)
            {
                aniContrBat.speed = 1f;
            }
        }
        else
        {
            fastOn = true;
            speed *= 2f;
            crawlSpeed *= 2f;
            textFastBtn.color = Color.red;
            if (!crawling)
            {
                aniContrBat.speed = 2f;
            }
        }
    }

    IEnumerator StaminaRtn()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.511f);
            if (fastOn)
            {
                if (!crawling)
                {
                    if ((go || backwards) && stamina > 0f)
                    {
                        stamina -= 3f;
                        if (stamina < 0f)
                        {
                            stamina = 0f;
                        }
                        staminaBar.sizeDelta = new Vector2(stamina * 6f, 10f);
                    }
                    else if ((go || backwards) && stamina <= 0f)
                    {
                        life -= 1f;
                        if (life <= 0f)
                        {
                            if (!finished)
                            {
                                finished = true;
                                gmCntrlScrpt.EndLevel(false);
                            }
                        }
                        if (life < 0f)
                        {
                            life = 0f;
                        }
                        lifeBar.sizeDelta = new Vector2(life * 6f, 10f);
                        StartCoroutine(BlinkLifebarPurpleRtn());
                    }
                    else if (!go && !backwards)
                    {
                        if (stamina < 100f)
                        {
                            stamina += 0.5f;
                            if (stamina > 100f)
                            {
                                stamina = 100f;
                            }
                            staminaBar.sizeDelta = new Vector2(stamina * 6f, 10f);
                        }
                    }
                }
                else
                {
                    if ((isJoystickTouched && !isLooking) && stamina > 0f)
                    {
                        stamina -= 3f;
                        if (stamina < 0f)
                        {
                            stamina = 0f;
                        }
                        staminaBar.sizeDelta = new Vector2(stamina * 6f, 10f);
                    }
                    else if ((isJoystickTouched && !isLooking) && stamina <= 0f)
                    {
                        life -= 1f;
                        if (life <= 0f)
                        {
                            if (!finished)
                            {
                                finished = true;
                                gmCntrlScrpt.EndLevel(false);
                            }
                        }
                        if (life < 0f)
                        {
                            life = 0f;
                        }
                        lifeBar.sizeDelta = new Vector2(life * 6f, 10f);
                        StartCoroutine(BlinkLifebarPurpleRtn());
                    }
                    else if ((!isJoystickTouched || isLooking))
                    {
                        if (stamina < 100f)
                        {
                            stamina += 0.5f;
                            if (stamina > 100f)
                            {
                                stamina = 100f;
                            }
                            staminaBar.sizeDelta = new Vector2(stamina * 6f, 10f);
                        }
                    }
                }
            }
            else if (!go && !backwards)
            {
                if (stamina < 100f)
                {
                    stamina += 0.5f;
                    if (stamina > 100f)
                    {
                        stamina = 100f;
                    }
                    staminaBar.sizeDelta = new Vector2(stamina * 6f, 10f);
                }
            }
        }
    }

    public void VampOnOff()
    {
        if (vampOn)
        {
            aniContrBat.SetBool("stoi", false);
            aniContrBat.SetBool("idz", false);
            aniContrBat.SetBool("idzTyl", false);
            aniContrBat.SetBool("zlot", false);
            aniContrBat.SetBool("wzlot", true);
            aniContrBat.speed = 1f;
            crawling = false;
            vampOn = false;
            rb.useGravity = false;
            Vector3 impulse = Vector3.up * flyUpForce;
            rb.AddForce(impulse, ForceMode.Impulse);
            autoCamScrpt.m_FollowTilt = true;
            //autoCamScrpt.m_FollowVelocity = true;
            vampCollider.enabled = false;
            flyCollider.enabled = true;
            transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
            autoCamPivot.localPosition = new Vector3(0f, 0.15f, -0.07f);
            crawlSpeed = crawlSpeedBat;
            vampRndr.enabled = false;
            foreach (Renderer rndr in batRndr)
            {
                rndr.enabled = true;
            }
            jumpButton.SetActive(false);
            vampLght.SetActive(false);
            batShdw.SetActive(true);
            backBtn.SetActive(true);
            go = false;
            goBtn.SetActive(true);
            mvOnCllsn.targetZ = -0.281f;
            landFlyBtn.SetActive(true);
            rb.freezeRotation = false;
            lookBtn.SetActive(false);
            landFlyBtnTxt.text = "LAND";
            vampBatBtnTxt.text = "VAMP";
            fightCollider.enabled = false;
            biteCollider.enabled = true;
            batLight.SetActive(true);
        }
        else
        {
            crawling = true;
            vampOn = true;
            rb.useGravity = true;
            autoCamScrpt.m_FollowTilt = false;
            //autoCamScrpt.m_FollowVelocity = false;
            vampCollider.enabled = true;
            flyCollider.enabled = false;
            crawlCollider.enabled = false;
            autoCamPivot.localPosition = new Vector3(0f, 2.2f, -2.2f);
            crawlSpeed = crawlSpeedVamp;
            vampRndr.enabled = true;
            foreach (Renderer rndr in batRndr)
            {
                rndr.enabled = false;
            }
            jumpButton.SetActive(true);
            vampLght.SetActive(true);
            batShdw.SetActive(false);
            backBtn.SetActive(false);
            goBtn.SetActive(false);
            mvOnCllsn.targetZ = 1.81f;
            go = false;
            goBtnTxt.text = "GO";
            backwards = false;
            backBtnTxt.text = "BACK";
            landFlyBtn.SetActive(false);
            float currentYRotation = transform.rotation.eulerAngles.y;
            // Freeze rotation on the X and Z axes
            transform.rotation = Quaternion.Euler(0f, currentYRotation, 0f);
            rb.freezeRotation = true;
            lookBtn.SetActive(true);
            vampBatBtnTxt.text = "BAT";
            biteCollider.enabled = false;
            fightCollider.enabled = true;
            batLight.SetActive(false);
        }
    }

    public void CrawlOnOff()
    {
        if (crawling)
        {
            aniContrBat.SetBool("stoi", false);
            aniContrBat.SetBool("zlot", false);
            aniContrBat.SetBool("idzTyl", false);
            aniContrBat.SetBool("idz", false);
            aniContrBat.SetBool("wzlot", true);
            aniContrBat.speed = 1f;
            crawling = false;
            rb.useGravity = false;
            Vector3 impulse = Vector3.up * flyUpForce;
            //rb.
            rb.AddForce(impulse, ForceMode.Impulse);
            autoCamScrpt.m_FollowTilt = true;
            //autoCamScrpt.m_FollowVelocity = true;
            crawlCollider.enabled = false;
            flyCollider.enabled = true;
            transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
            jumpButton.SetActive(false);
            goBtn.SetActive(true);
            backBtn.SetActive(true);
            rb.freezeRotation = false;
            lookBtn.SetActive(false);
            landFlyBtnTxt.text = "LAND";
            batLight.SetActive(true);
        }
        else
        {
            aniContrBat.SetBool("wzlot", false);
            aniContrBat.SetBool("zlot", true);
            aniContrBat.speed = 0f;
            crawling = true;
            rb.useGravity = true;
            autoCamScrpt.m_FollowTilt = false;
            //autoCamScrpt.m_FollowVelocity = false;
            crawlCollider.enabled = true;
            flyCollider.enabled = false;
            jumpButton.SetActive(true);
            goBtn.SetActive(false);
            backBtn.SetActive(false);
            go = false;
            goBtnTxt.text = "GO";
            backwards = false;
            backBtnTxt.text = "BACK";
            lookBtn.SetActive(true);
            transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
            rb.freezeRotation = true;
            landFlyBtnTxt.text = "FLY";
            batLight.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.isTrigger)
        {
            // Check if the collision involves a collider on the specified layer
            if ((groundLayer.value & (1 << collision.gameObject.layer)) != 0)
            {
                if (crawling && !vampOn && !aniContrBat.GetBool("stoi"))
                {
                    aniContrBat.SetBool("wzlot", false);
                    aniContrBat.SetBool("zlot", false);
                    aniContrBat.SetBool("stoi", true);
                }
            }
            if (vampOn && collision.gameObject.tag == "horse")
            {
                hrsMvScrpt.HorseStartWalking();
            }
            if (collision.gameObject.tag == "bird" && !birdAttking)
            {
                birdAttking = true;
                if (!birdBitten)
                {
                    life -= 5f;
                    if (life <= 0f)
                    {
                        gmCntrlScrpt.EndLevel(false);
                        life = 0f;
                    }
                    lifeBar.sizeDelta = new Vector2(life * 6f, 10f);
                    StartCoroutine(BlinkLifebarPurpleRtn());
                }
                StartCoroutine(BirdAttkingRtn());
            }
        }
    }

    IEnumerator BirdAttkingRtn()
    {
        yield return new WaitForSeconds(5f);
        birdAttking = false;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (!vampOn && collision.gameObject.tag == "kill" && !finished && hrsMvScrpt.isMoving)
        {
            finished = true;
            gmCntrlScrpt.EndLevel(false);
        }
        if (!vampOn && collision.gameObject.tag == "maze")
        {
            if (SaveManager.saveGlob.level > 3)
            {
                levelNameTxt.text = "Entering The Scorpion Maze...";
                SaveManager.saveGlob.lifeNow = life;
                SceneManager.LoadScene("maze");
            }
        }
        if (collision.gameObject.tag == "crystalcave")
        {
            if (SaveManager.saveGlob.level == 4)
            {
                if (life >= 50f)
                {
                    levelNameTxt.text = "Entering The Crystal Cave...";
                    SaveManager.saveGlob.lifeNow = life;
                    SceneManager.LoadScene("crystalcave");
                }
                else
                {
                    StartCoroutine(EndTextInfoRtn("Life less than 50%!"));
                }
            }
            else
            {
                levelNameTxt.text = "Entering The Crystal Cave...";
                SaveManager.saveGlob.lifeNow = life;
                SaveManager.saveGlob.enterFrom = scene.name;
                SceneManager.LoadScene("crystalcave");
            }
        }
        if (vampOn && collision.gameObject.tag == "village" && SaveManager.saveGlob.level == 4)
        {
            gmCntrlScrpt.EndLevel(true);
        }
    }

    IEnumerator EndTextInfoRtn(string txt)
    {
        levelNameTxt.text = txt;
        yield return new WaitForSeconds(2f);
        gmCntrlScrpt.EndLevel(false);
    }

    public void DrinkHorseBlood()
    {
        if (!hrsMvScrpt.isMoving && !hrsDrnkingBlood && crawling && !vampOn)
        {
            hrsDrnkingBlood = true;
            StartCoroutine(HorseDrinkBloodRtn());
        }
    }

    IEnumerator HorseDrinkBloodRtn()
    {
        Vector3 initialBatPosition = transform.position;
        yield return new WaitForSeconds(0.5f);
        if (
            !hrsMvScrpt.isMoving
            && (Vector3.Distance(transform.position, initialBatPosition) < 0.05f)
            && crawling
            && !vampOn
        )
        {
            Eat("horse");
        }
        hrsDrnkingBlood = false;
    }

    private void FixedUpdate()
    {
        if ((!crawling && go) || (!crawling && backwards))
        {
            // Calculate the desired movement vector based on the forward direction of the object
            Vector3 movement = Vector3.zero;
            if (!backwards)
            {
                if (rb.velocity.magnitude <= speed)
                    rb.AddRelativeForce(Vector3.forward * accel);
            }
            else
            {
                if (rb.velocity.magnitude <= (speed / 2f))
                    rb.AddRelativeForce(-Vector3.forward * accel);
            }
            // Apply the movement to the rigidbody
            rb.MovePosition(rb.position + movement);

            if (fastOn)
            {
                aniContrBat.speed = 2f;
            }
            else
            {
                aniContrBat.speed = 1f;
            }
        }
        else if (flyForward && crawling)
        {
            if (crawlSpeedNow > 0f)
            {
                aniContrBat.SetBool("stoi", false);
                aniContrBat.SetBool("zlot", false);
                aniContrBat.SetBool("wzlot", false);
                aniContrBat.SetBool("idzTyl", false);
                if (!aniContrBat.GetBool("idz"))
                    aniContrBat.SetBool("idz", true);
                if (crawlSpeedNow > strafeNow)
                {
                    aniContrBat.speed = crawlSpeedNow / 8f;
                }
                else
                {
                    aniContrBat.speed = strafeNow / 8f;
                }
            }
            if (crawlSpeedNow < 0f)
            {
                aniContrBat.SetBool("stoi", false);
                aniContrBat.SetBool("zlot", false);
                aniContrBat.SetBool("wzlot", false);
                aniContrBat.SetBool("idz", false);
                if (!aniContrBat.GetBool("idzTyl"))
                    aniContrBat.SetBool("idzTyl", true);

                if (crawlSpeedNow < strafeNow)
                {
                    aniContrBat.speed = (-crawlSpeedNow) / 8f;
                }
                else
                {
                    aniContrBat.speed = (-strafeNow) / 8f;
                }
            }
            // Apply the movement to the rigidbody
            if (rb.velocity.magnitude <= crawlSpeed)
            {
                rb.AddRelativeForce(Vector3.forward * crawlSpeedNow);
                rb.AddRelativeForce(Vector3.right * strafeNow);
            }
        }
        else if (crawling && !isJumping)
        {
            aniContrBat.SetBool("zlot", false);
            aniContrBat.SetBool("wzlot", false);
            aniContrBat.SetBool("idz", false);
            aniContrBat.SetBool("idzTyl", false);
            if (!aniContrBat.GetBool("stoi"))
                aniContrBat.SetBool("stoi", true);
            aniContrBat.speed = 1f;
        }
        else if (!crawling)
        {
            aniContrBat.speed = 1f;
        }
    }

    public void Jump()
    {
        Vector3 impulse = Vector3.up * flyUpForce * 10f;
        if (vampOn)
        {
            impulse *= 2f;
        }
        isGrounded = CheckGround();
        if (isGrounded)
        {
            if (!isJumping)
            {
                isJumping = true;
                autoCamScrpt.SetTarget(null);
                rb.AddForce(impulse, ForceMode.Impulse);
                if (vampOn)
                {
                    aniContrVamp.SetTrigger("jump");
                }
                else
                {
                    aniContrBat.speed = 1f;
                    aniContrBat.SetBool("idz", false);
                    aniContrBat.SetBool("zlot", true);
                }
            }
            StopCoroutine(SetBackTargetOnCam());
            StartCoroutine(SetBackTargetOnCam());
        }
    }

    IEnumerator SetBackTargetOnCam()
    {
        yield return new WaitForSeconds(0.1f);
        isJumping = false;
        yield return new WaitForSeconds(0.2f);
        if (!isLooking && !vampOn)
        {
            autoCamScrpt.SetTarget(transform);
        }
        isGrounded = CheckGround();
        if (!isLooking && vampOn && isGrounded)
        {
            autoCamScrpt.SetTarget(transform);
        }
        yield return new WaitForSeconds(.5f);
        if (!isLooking)
        {
            autoCamScrpt.SetTarget(transform);
        }
    }

    private void Update()
    {
        if (SaveManager.saveGlob.level > 0 && !finished)
        {
            if (!crawling)
            {
                if (transform.position.y > 18f && !owlComing)
                {
                    owlComing = true;
                    owlCountDownText.text = "30";
                    owlRtnCoroutine = StartCoroutine(OwlRtn()); // Store the coroutine instance
                }
                if (transform.position.y < 18f && owlComing)
                {
                    owlComing = false;
                    if (owlRtnCoroutine != null)
                    {
                        StopCoroutine(owlRtnCoroutine); // Stop the specific coroutine instance
                        owlPic.SetActive(false);
                    }
                    owlCountDownText.text = "";
                }
            }
            else
            {
                if (owlComing)
                {
                    owlComing = false;
                    if (owlRtnCoroutine != null)
                    {
                        StopCoroutine(owlRtnCoroutine); // Stop the specific coroutine instance
                        owlPic.SetActive(false);
                    }
                    owlCountDownText.text = "";
                }
            }
        }
        // Check if the joystick is being touched
        isJoystickTouched = joystick.IsTouched;
        if (!crawling)
        {
            if (isJoystickTouched && !isLooking)
            {
                // Reset the rotation target to the current rotation
                StopResetRotationCoroutine();
            }
            else if (
                (!isJoystickTouched && !isResettingRotation)
                || (isJoystickTouched && !isResettingRotation && isLooking)
            )
            {
                // If the joystick is not touched and the target rotation is not the original rotation, start the rotation reset coroutine
                StartCoroutine(ResetRotationCoroutine());
            }

            if (isJoystickTouched && isLooking)
            {
                batBody.localRotation = Quaternion.Lerp(
                    batBody.localRotation,
                    Quaternion.Euler(0, 0, 0),
                    0.05f
                );
            }

            // Move and turn the object based on joystick input
            if (isJoystickTouched)
            {
                // Get the joystick input values
                float joystickInputY = joystick.m_VerticalVirtualAxis.GetValue;
                float joystickInputX = joystick.m_HorizontalVirtualAxis.GetValue;

                // Move the object forward based on the vertical input
                if (!isLooking)
                {
                    flyForward = true;
                }
                else
                {
                    flyForward = false;
                }

                // Turn the object on the X and Y axes based on the horizontal input
                float xRotation = joystickInputX * turnFlySpeed * Time.deltaTime;
                float yRotation = joystickInputY * turnFlySpeed * Time.deltaTime;
                // Apply the rotations
                if (!isLooking)
                {
                    if (xRotation < 1.5f && xRotation > -1.5f)
                        xRotation /= 4f;
                    if (yRotation < 1.5f && yRotation > -1.5f)
                        yRotation /= 4f;
                    if (xRotation < 3.5f && xRotation > -3.5f)
                        xRotation /= 3f;
                    if (yRotation < 3.5f && yRotation > -3.5f)
                        yRotation /= 3f;
                    transform.Rotate(-yRotation, xRotation, 0f);
                    transform.rotation = Quaternion.Euler(
                        transform.rotation.eulerAngles.x,
                        transform.rotation.eulerAngles.y,
                        0f
                    );
                    batBody.localRotation = Quaternion.Euler(0f, 0f, -joystickInputX * 50f);
                }
                else
                {
                    autoCam.Rotate(-yRotation, xRotation, 0f);
                    autoCam.rotation = Quaternion.Euler(
                        autoCam.rotation.eulerAngles.x,
                        autoCam.rotation.eulerAngles.y,
                        0f
                    );
                }
            }
            else
            {
                batBody.localRotation = Quaternion.Lerp(
                    batBody.localRotation,
                    Quaternion.Euler(0, 0, 0),
                    0.05f
                );
                flyForward = false;
            }
        }
        else
        { // crawling
            if (SystemInfo.supportsAccelerometer)
            {
                // Get the acceleration values from the accelerometer
                float accelerationX = Input.acceleration.x;
                float rotationY = accelerationX * turnSpeed;

                // Apply rotation to the object on the Y-axis
                if (Time.timeScale != 0)
                {
                    if (accelerationX < -0.03f || accelerationX > 0.03f)
                        transform.Rotate(Vector3.up, rotationY);
                }
            }

            if (isJoystickTouched)
            {
                // Get the joystick input values
                float joystickInputY = joystick.m_VerticalVirtualAxis.GetValue;
                float joystickInputX = joystick.m_HorizontalVirtualAxis.GetValue;

                // Turn the object on the X and Y axes based on the horizontal input
                float xRotation = joystickInputX * turnSpeed * Time.deltaTime;
                float yRotation = joystickInputY * turnSpeed * Time.deltaTime;

                // Apply the rotations
                if (!isLooking)
                {
                    //transform.Rotate(0f, xRotation, 0f);
                    flyForward = true;
                    crawlSpeedNow = joystickInputY * crawlAccel * Time.deltaTime;
                    strafeNow = joystickInputX * crawlAccel * Time.deltaTime;
                }
                else
                {
                    flyForward = false;
                    autoCam.Rotate(-yRotation, xRotation, 0f);
                    autoCam.rotation = Quaternion.Euler(
                        autoCam.rotation.eulerAngles.x,
                        autoCam.rotation.eulerAngles.y,
                        0f
                    );
                }
            }
            else
            {
                flyForward = false;
            }
            batBody.localRotation = Quaternion.Lerp(
                batBody.localRotation,
                Quaternion.Euler(0, 0, 0),
                0.05f
            );
            if (crawling)
            {
                float currentYRotation = transform.rotation.eulerAngles.y;
                // Freeze rotation on the X and Z axes

                transform.rotation = Quaternion.Euler(0f, currentYRotation, 0f);
            }
        }
        // limit turning
        if (!isLooking)
        {
            // Get the current rotation of the object
            Quaternion currentRotation = transform.rotation;

            // Convert the rotation to a normalized Euler representation
            Vector3 eulerRotation = currentRotation.eulerAngles;

            // Normalize the X-axis rotation to the range of -180 to 180 degrees
            eulerRotation.x = (eulerRotation.x > 180f) ? eulerRotation.x - 360f : eulerRotation.x;

            // Clamp the X-axis rotation to the specified limits
            eulerRotation.x = Mathf.Clamp(eulerRotation.x, minRotation, maxRotation);
            if (crawling)
            {
                // Normalize the Z-axis rotation to the range of -180 to 180 degrees
                eulerRotation.z =
                    (eulerRotation.z > 180f) ? eulerRotation.z - 360f : eulerRotation.z;

                // Clamp the Z-axis rotation to the specified limits
                eulerRotation.z = Mathf.Clamp(eulerRotation.z, minRotation, maxRotation);
            }
            // Convert the Euler rotation back to a Quaternion
            Quaternion clampedRotation = Quaternion.Euler(eulerRotation);

            // Apply the clamped rotation to the object
            transform.rotation = clampedRotation;
        }
        else
        {
            Quaternion currentRotation = autoCam.rotation;

            // Convert the rotation to a normalized Euler representation
            Vector3 eulerRotation = currentRotation.eulerAngles;

            // Normalize the X-axis rotation to the range of -180 to 180 degrees
            eulerRotation.x = (eulerRotation.x > 180f) ? eulerRotation.x - 360f : eulerRotation.x;

            // Clamp the X-axis rotation to the specified limits
            eulerRotation.x = Mathf.Clamp(eulerRotation.x, minRotation, maxRotation);
            // Normalize the Z-axis rotation to the range of -180 to 180 degrees
            eulerRotation.z = (eulerRotation.z > 180f) ? eulerRotation.z - 360f : eulerRotation.z;

            // Clamp the Z-axis rotation to the specified limits
            eulerRotation.z = Mathf.Clamp(eulerRotation.z, minRotation, maxRotation);

            // Convert the Euler rotation back to a Quaternion
            Quaternion clampedRotation = Quaternion.Euler(eulerRotation);

            // Apply the clamped rotation to the object
            autoCam.rotation = clampedRotation;
        }
    }

    private void StopResetRotationCoroutine()
    {
        if (isResettingRotation)
        {
            StopCoroutine(ResetRotationCoroutine());
            isResettingRotation = false;
        }
    }

    private System.Collections.IEnumerator ResetRotationCoroutine()
    {
        isResettingRotation = true;
        resetStartTime = Time.time;
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);

        while (Time.time < resetStartTime + rotationResetDuration)
        {
            float t = (Time.time - resetStartTime) / rotationResetDuration;
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, t);
            yield return null;
        }

        transform.rotation = endRotation;
        isResettingRotation = false;
    }

    private bool CheckGround()
    {
        RaycastHit hit;
        if (
            Physics.Raycast(
                transform.position,
                Vector3.down,
                out hit,
                groundCheckDistance,
                groundLayer
            )
        )
        {
            return true;
        }

        return false;
    }

    Transform GetClosestEnemy(GameObject[] enemies)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (GameObject potentialTarget in enemies)
        {
            if (potentialTarget != null)
            {
                Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = potentialTarget.transform;
                }
            }
        }

        return bestTarget;
    }
}
