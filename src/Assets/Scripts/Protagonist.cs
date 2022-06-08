using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


//player, includes user controls, restarting when dead, returning to title screen, updating log, handeling movement animations and shooting
public class Protagonist : MonoBehaviour {

    public GameObject ghost;
    public GameObject bullet;

	private float speed = 2;
    private float sprint = 1;

    //private int rewinds = 0;
    //private int maxRewinds = 1;

    public bool budhistMode = false;
    public bool timeDialation = false;
    public bool canSprint = true;
    public bool continueAudio = true; //should be set false, ony on first scene (that does not cross over the audio)

	private List<List<Sprite>> textures;
	private int state; //0 = idle, 1 = walk, 2 = attack
	private int sequence;
	private int sequenceIndex;

    private List<TimeCommand> log;

    public List<GameObject> knifeCollision;

	private const int sequenceLength = 1;

	private bool mouseDown;

    //for stats page
    private int bullets;
    private int kills;
    private int reverse;

    void Start() {
    	mouseDown = false;
    	sequence = 0;
    	state = 0;
    	sequenceIndex = 0;
    	List<Sprite> idle = new List<Sprite>();
    	for (int i = 0; i <= 19; i++) {
    		idle.Add((Sprite)Resources.LoadAll("Textures/handgun/idle/survivor-idle_handgun_"+i, typeof(Sprite))[0]);
    	}
    	List<Sprite> walk = new List<Sprite>();
    	for (int i = 0; i <= 19; i++) {
    		walk.Add((Sprite)Resources.LoadAll("Textures/handgun/move/survivor-move_handgun_"+i, typeof(Sprite))[0]);
    	}
    	List<Sprite> attack = new List<Sprite>();
    	for (int i = 0; i <= 14; i++) {
    		attack.Add((Sprite)Resources.LoadAll("Textures/knife/meleeattack/survivor-meleeattack_knife_"+i, typeof(Sprite))[0]);
    	}
        List<Sprite> shoot = new List<Sprite>();
        for (int i = 0; i <= 2; i++) {
            shoot.Add((Sprite)Resources.LoadAll("Textures/handgun/shoot/survivor-shoot_handgun_"+i, typeof(Sprite))[0]);
        }
    	textures = new List<List<Sprite>>() {idle, walk, attack, shoot};

    	GetComponent<SpriteRenderer>().sprite = textures[state][sequence];
        log = new List<TimeCommand>();
        knifeCollision = new List<GameObject>();
        //Time.timeScale = 0.3f;
    }

    public static Vector3 Normalize(Vector3 vector) {
        float m = Mathf.Atan(vector.y / vector.x);
        int invert = 1; int invert2 = 1;
        if (vector.x < 0 & vector.y > 0) {invert2 = -1; invert = -1;}
        if (vector.x < 0 & vector.y < 0) {invert2 = -1; invert = -1;}
        return new Vector3(Mathf.Cos(m)*invert, Mathf.Sin(m)*invert2, 0);
    }

    void Update() {
        if (!WorldScript.paused) {
            if (Input.GetKeyUp(KeyCode.Q) /*& rewinds < maxRewinds*/ & !budhistMode) {
                GameObject g = Instantiate(ghost, transform.position, transform.rotation);
                g.GetComponent<Ghost>().commands = log;
                g.GetComponent<Ghost>().commandsIndex = log.Count - 1;
                g.GetComponent<Ghost>().existsAfterTime = false;
                g.GetComponent<Ghost>().existsBeforeTime = false;
                g.GetComponent<Ghost>().StartRewind();
                g.GetComponent<Ghost>().player = this;
                log = new List<TimeCommand>();
                reverse ++; //increments reverse stat
            }
            if (Input.GetKeyUp(KeyCode.Space)) {
                state = 2;
                sequence = 0;
            }
            if (Input.GetMouseButtonDown(0) & !mouseDown) {
                mouseDown = true;
                state = 3;
                sequence = 0;

                Vector3 mouseNormalized = Normalize(Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
                GameObject b = Instantiate(bullet, transform.position + new Vector3(mouseNormalized.x * 0.4f, mouseNormalized.y * 0.4f), Quaternion.identity);
                b.GetComponent<Bullet>().velocity = mouseNormalized;
                if (budhistMode) { b.GetComponent<Bullet>().polarity = 2; }

                bullets++; //adds to stats of bullets fired
            }
            else if (Input.GetMouseButtonUp(0) & mouseDown) {
                mouseDown = false;
            }
            if (Input.GetKeyDown(KeyCode.LeftShift) & canSprint) {
                sprint = 2;
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift) & canSprint) {
                sprint = 1;
            }
        }
    }

    void FixedUpdate() {
        if (!WorldScript.paused) {
            if (Input.GetAxis("Vertical") != 0 | Input.GetAxis("Horizontal") != 0) {
                if (sprint == 2 & timeDialation) {
                    Time.timeScale = 0.4f;
                }
                else if (timeDialation) {
                    Time.timeScale = 0.8f;
                }
                float diagonal = 1;
                if (Input.GetAxis("Vertical") != 0 & Input.GetAxis("Horizontal") != 0) {
                    diagonal = Mathf.Sqrt(2);
                }
                float xSpeed = (Input.GetAxis("Horizontal") * Time.deltaTime * speed * sprint) / diagonal;
                float ySpeed = (Input.GetAxis("Vertical") * Time.deltaTime * speed * sprint) / diagonal;
                if (state == 0) {
                    state = 1;
                    sequenceIndex = 0;
                }
                transform.position = new Vector3(transform.position.x + xSpeed, transform.position.y + ySpeed);
            }
            else {
                if (timeDialation) {
                    Time.timeScale = 1f;
                }
                if (state == 1) {
                    state = 0;
                }
            }
            Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(mouse.y - transform.position.y, mouse.x - transform.position.x) * Mathf.Rad2Deg);

            if (sequenceIndex == sequenceLength) {
                if (state == 2 & sequence == 4) {
                    Knife();
                }
                if (sequence >= textures[state].Count) {
                    sequence = 0;
                    if (state == 2 | state == 3) {
                        state = 0;
                    }
                }
                GetComponent<SpriteRenderer>().sprite = textures[state][sequence];
                sequenceIndex = 0;
                log.Add(new TimeCommand(transform.position, transform.rotation, sprite: textures[state][sequence]));
                sequence++;
            }
            else {
                sequenceIndex++;
                log.Add(new TimeCommand(transform.position, transform.rotation));
            }
        }
    }

    private void Knife() {
        if (knifeCollision.Count > 0) {
            knifeCollision[0].GetComponent<ReversableBody>().Kill();
        }
    }

    public void UpdateStats(bool dead, string level) {
        List<string> stats = Util.Instance.ReadFile();
        string preserve = ""; //lines to not override
        int statBullets = 0;
        int statReverse = 0;
        int statKills = 0;
        int statDeaths = 0;
        string statLevels = "";
        foreach(string line in stats) {
            if(line.Length < 2 || line.Substring(0, 2) == "//") { //if line begins with '//' skip line
                preserve += line + "\n";
                continue;
            }
            string[] log = line.Split(' ');
            statBullets = int.Parse(log[0]) + bullets;
            statReverse = int.Parse(log[1]) + reverse;
            statKills = int.Parse(log[2]) + kills;
            statDeaths = int.Parse(log[3]) + (dead? 1 : 0); // add 1 to deaths if player died
            statLevels = log[4];
            if(level != null) {
                string[] levels = log[4].Substring(1, log[4].Length - 2).Split(',');
                bool isInList = false;
                foreach (string l in levels) {
                    if(l == level) {
                        isInList = true;
                    }
                }
                if (!isInList) {
                    statLevels = statLevels.Substring(0, statLevels.Length - 1) + level + ",]";
                }
            }
            break; //ignore lines after (there should be none)
        }
        Util.Instance.WriteFile(preserve+statBullets+" "+statReverse+" "+statKills+" "+statDeaths+" "+statLevels);
    }

    public void Kill(bool dead = true) { //called when player is killed, restarts the scene
        UpdateStats(dead, null);
        if(continueAudio) {
            FindObjectsOfType<AudioPlayer>()[0].Play();
            DontDestroyOnLoad(FindObjectsOfType<AudioPlayer>()[0].gameObject);
        }
        else {
            Destroy(FindObjectsOfType<AudioPlayer>()[0].gameObject);
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void AddKill() { //called from turrets killed by player
        kills++;
    }
}
