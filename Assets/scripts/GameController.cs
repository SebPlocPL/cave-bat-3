using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Cameras;

public class GameController : MonoBehaviour
{
    [SerializeField]
    GameObject life50perc;

    [SerializeField]
    GameObject[] lvls;

    [SerializeField]
    Animator aniBatCntrlr;

    [SerializeField]
    GameObject introCam;

    [SerializeField]
    GameObject gameCam;

    [SerializeField]
    MobileObjectMovement mblObjMvScrpt;

    [SerializeField]
    GameObject introPnl;

    [SerializeField]
    GameObject gamePnl;

    [SerializeField]
    TextMeshProUGUI introTxt;

    [SerializeField]
    Color lightBlueClr;

    [SerializeField]
    Color lightRedClr;

    [SerializeField]
    GameObject flyBtn;

    [SerializeField]
    GameObject instructionPnl;

    [SerializeField]
    TextMeshProUGUI instructionTxt;

    [SerializeField]
    GameObject endPnl;

    [SerializeField]
    TextMeshProUGUI endTxt;

    bool finished;

    void Start()
    {
        Time.timeScale = 1f;
        StartCoroutine(IntroRtn());
    }

    public void VampIntro()
    {
        StartCoroutine(VampIntroRtn());
    }

    IEnumerator VampIntroRtn()
    {
        if (SaveManager.saveGlob.level == 4)
        {
            yield return new WaitForSeconds(2f);
            introTxt.text = "Thank you for coming.";
            yield return new WaitForSeconds(3f);
            introTxt.text = "I was captured in these stones for many years.";
            yield return new WaitForSeconds(5f);
            introTxt.text = "I used to be the ruler of this land...";
            yield return new WaitForSeconds(5f);
            introTxt.text = "...until one day other vampires betrayed me...";
            yield return new WaitForSeconds(5f);
            introTxt.text = "...and captured my spirit in these magical stones.";
            yield return new WaitForSeconds(5f);
            introTxt.text = "Together we will take this land back!";
            yield return new WaitForSeconds(5f);
            introTxt.text = "";
        }
        else if (SaveManager.saveGlob.level == 5)
        {
            yield return new WaitForSeconds(2f);
            introTxt.text = "The red ligh below the vampire...";
            yield return new WaitForSeconds(3f);
            introTxt.text = "will blink slowly, if you hit him once.";
            yield return new WaitForSeconds(5f);
            introTxt.text = "It will blink fast, after two hits.";
            yield return new WaitForSeconds(5f);
            introTxt.text = "When it blinks fast, we just need to...";
            yield return new WaitForSeconds(5f);
            introTxt.text = "hit him one more time to kill.";
            yield return new WaitForSeconds(5f);
            introTxt.text = "Change to the bat right after the hit, to get away.";
            yield return new WaitForSeconds(5f);
            introTxt.text = "To not get killed as a bat, we need to be at the same height...";
            yield return new WaitForSeconds(5f);
            introTxt.text = "or a little lower than the enemy before changing.";
            yield return new WaitForSeconds(5f);
            introTxt.text = "It's also possible to kill him faster, if you're lucky.";
            yield return new WaitForSeconds(5f);
            introTxt.text = "The vampire takes 34% of your life with hit!";
            yield return new WaitForSeconds(5f);
            introTxt.text = "";
        }
    }

    IEnumerator IntroRtn()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (SaveManager.saveGlob.level == 0)
        {
            yield return new WaitForSeconds(3f);
            introTxt.text = "Wake up!";
            aniBatCntrlr.SetBool("zwin", false);
            yield return new WaitForSeconds(1f);
            introTxt.color = lightBlueClr;
            introTxt.text = "Who was that?\n\n";
            yield return new WaitForSeconds(2f);
            introTxt.color = lightRedClr;
            introTxt.text = "You need to be strong!";
            yield return new WaitForSeconds(2f);
            introTxt.text = "Eat 5 fireflies.";
            yield return new WaitForSeconds(2f);
            introTxt.text = "";
            flyBtn.SetActive(true);
            instructionPnl.SetActive(true);
            instructionTxt.text =
                "Use the joystick on the left to steer the bat. Press GO to fly forward or BACK to fly backwards. Follow the orange soundwaves to find the firefly. Aling the orange and grey soundwaves to catch them.";
            lvls[0].SetActive(true);
        }
        else if (SaveManager.saveGlob.level == 1)
        {
            yield return new WaitForSeconds(3f);
            introTxt.text = "Wakie, wakie!";
            aniBatCntrlr.SetBool("zwin", false);
            yield return new WaitForSeconds(1f);
            introTxt.color = lightBlueClr;
            introTxt.text = "What?\n\n";
            yield return new WaitForSeconds(2f);
            introTxt.color = lightRedClr;
            introTxt.text = "Eat 3 more fireflies...";
            yield return new WaitForSeconds(2f);
            introTxt.text = "but watchout for an owl!";
            yield return new WaitForSeconds(2f);
            introTxt.text = "";
            flyBtn.SetActive(true);
            instructionPnl.SetActive(true);
            instructionTxt.text =
                "If you fly too high, an owl will hear you and will start hunting you. You have 30 seconds to fly lower. Luckly you can fly faster. Just slow down before your stamina runs out or you will start losing your life energy.";
            lvls[1].SetActive(true);
        }
        else if (SaveManager.saveGlob.level == 2)
        {
            yield return new WaitForSeconds(3f);
            introTxt.text = "It's already dark!";
            aniBatCntrlr.SetBool("zwin", false);
            yield return new WaitForSeconds(1f);
            introTxt.color = lightBlueClr;
            introTxt.text = "You again!?\n\n";
            yield return new WaitForSeconds(2f);
            introTxt.color = lightRedClr;
            introTxt.text = "You'll drink horse blood...";
            yield return new WaitForSeconds(2f);
            introTxt.text = "but stay away from its legs!";
            yield return new WaitForSeconds(2f);
            introTxt.text = "";
            flyBtn.SetActive(true);
            instructionPnl.SetActive(true);
            instructionTxt.text =
                "You can only drink horse blood, when it's not walking by landing on it and not moving. Stay away from its hooves, when it's walking or you'll die! When crawling, you turn by turning your phone. Fill up half of your life energy.";
            lvls[2].SetActive(true);
        }
        else if (SaveManager.saveGlob.level == 3)
        {
            yield return new WaitForSeconds(3f);
            introTxt.text = "You need more life energy!";
            aniBatCntrlr.SetBool("zwin", false);
            yield return new WaitForSeconds(3f);
            introTxt.color = lightBlueClr;
            introTxt.text = "For what?\n\n";
            yield return new WaitForSeconds(2f);
            introTxt.color = lightRedClr;
            introTxt.text = "You'll eat an egg tonight...";
            yield return new WaitForSeconds(3f);
            introTxt.text = "and you will fight an oilbird!";
            yield return new WaitForSeconds(3f);
            introTxt.text = "";
            flyBtn.SetActive(true);
            instructionPnl.SetActive(true);
            instructionTxt.text =
                "The owl will not hunt you, when you're not flying. You need to land in a nest to eat an egg. It gives full life energy but you need to fight a bird. Turn towards the attacking bird to bite it first. It's easier to fight it, when you fly. Fight until it's dead.";
            lvls[3].SetActive(true);
        }
        else if (SaveManager.saveGlob.level == 4)
        {
            if (scene.name == "village")
            {
                yield return new WaitForSeconds(3f);
                introTxt.text = "Today we will meet!";
                aniBatCntrlr.SetBool("zwin", false);
                yield return new WaitForSeconds(3f);
                introTxt.color = lightBlueClr;
                introTxt.text = "Where?\n\n";
                yield return new WaitForSeconds(2f);
                introTxt.color = lightRedClr;
                introTxt.text = "Go through The Scorpion Maze...";
                yield return new WaitForSeconds(3f);
                introTxt.text = "and find me in The Crystal Cave!";
                yield return new WaitForSeconds(3f);
                introTxt.text = "";
                flyBtn.SetActive(true);
                instructionPnl.SetActive(true);
                instructionTxt.text =
                    "Find The Scorpion Maze and go through it to find The Crystal Cave. Enter The Crystal Cave with at least half of your life energy to summon the owner of the mysterious voice!";
                lvls[4].SetActive(true);
            }
            if (scene.name == "crystalcave")
            {
                aniBatCntrlr.SetBool("wzlot", true);
                aniBatCntrlr.transform.parent.GetComponent<BoxCollider>().enabled = true;
                instructionTxt.text =
                    "You can feel the energy coming from the stones with the markings. You are growing... Find the exit to the village. You can use doors only in a vampire form.";
                lvls[0].SetActive(true);
            }
        }
        else if (SaveManager.saveGlob.level == 5)
        {
            if (scene.name == "village")
            {
                yield return new WaitForSeconds(3f);
                introTxt.text = "We need to fight a vampire!";
                aniBatCntrlr.SetBool("zwin", false);
                yield return new WaitForSeconds(3f);
                introTxt.color = lightBlueClr;
                introTxt.text = "Why?\n\n";
                yield return new WaitForSeconds(2f);
                introTxt.color = lightRedClr;
                introTxt.text = "To unlock new caves...";
                yield return new WaitForSeconds(3f);
                introTxt.text = "we need to get a key from him!";
                yield return new WaitForSeconds(3f);
                introTxt.text = "";
                flyBtn.SetActive(true);
                instructionPnl.SetActive(true);
                instructionTxt.text =
                    "Swiching between a bat and a vampire uses a little life energy. Catch the fireflies to charge your life energy and in a vampire form enter the mine using the door or crawl through the maze to get more life energy from scorpions.";
                lvls[0].SetActive(true);
                lvls[1].SetActive(true);
            }
            if (scene.name == "crystalcave")
            {
                instructionTxt.text =
                    "Vampires don't chase bats, but they kill bats flying close in front of them. Fly behind, change to the vampire, get close to attack, quicly change to the bat (best is to stand at the same height or lower than the enemy) and fly away. Attack 3 times to kill him to recharge fully your life.";
                lvls[1].SetActive(true);
            }
        }
        else if (SaveManager.saveGlob.level == 6)
        {
            if (scene.name == "village")
            {
                yield return new WaitForSeconds(3f);
                introTxt.text = "We need to fight goblins now!";
                aniBatCntrlr.SetBool("zwin", false);
                yield return new WaitForSeconds(3f);
                introTxt.color = lightBlueClr;
                introTxt.text = "Oh nooo!\n\n";
                yield return new WaitForSeconds(2f);
                introTxt.color = lightRedClr;
                introTxt.text = "To unlock new caves...";
                yield return new WaitForSeconds(3f);
                introTxt.text = "we need to get a key from them!";
                yield return new WaitForSeconds(3f);
                introTxt.text = "";
                flyBtn.SetActive(true);
                instructionPnl.SetActive(true);
                instructionTxt.text =
                    "Go to through The Crystal Cave to The Entrance Hall. There are 3 goblins! You can kill some mice in The Crystal Cave to charge your life energy. Remember, you can always go through The Scorpion Maze to The Crystal Cave for extra life.";
            }
            if (scene.name == "crystalcave")
            {
                instructionTxt.text =
                    "Jumping mice returned to this cave, since the vampire is gone. You can eat some to charge your life. To find The Entrance Hall, follow the tracks in the tunnel.";
                lvls[2].SetActive(true);
            }
            if (scene.name == "entrancehall")
            {
                instructionTxt.text =
                    "Goblins will follow you, when they can see you, even in your bat form. You need to hit them 3 times from the back in your vampire form. Stay low close to the ground to escape. They will bleed more every time you hit them.";
                lvls[0].SetActive(true);
            }
        }
        // every level maze
        if (scene.name == "maze")
        {
            aniBatCntrlr.SetBool("wzlot", true);
            aniBatCntrlr.transform.parent.GetComponent<BoxCollider>().enabled = true;
            instructionTxt.text =
                "Find the exit to The Crystal Cave. You will hear the exit only after all scorpions are gone. You need to bite them first to kill them. Turn towards scorpions.";
            if (SaveManager.saveGlob.level == 4)
            {
                lvls[0].SetActive(true);
                life50perc.SetActive(true);
            }
            else
            {
                lvls[0].SetActive(true);
                lvls[1].SetActive(true);
            }
        }
    }

    public void StartFlying()
    {
        StartCoroutine(WzlotRtn());
    }

    IEnumerator WzlotRtn()
    {
        for (int i = 10; i > 0; i--)
        {
            mblObjMvScrpt.transform.Translate(0f, 0f, 0.05f);
            yield return new WaitForSeconds(0.05f);
        }
        aniBatCntrlr.SetBool("wzlot", true);
        mblObjMvScrpt.transform.Rotate(0, 180, 0);
        mblObjMvScrpt.enabled = true;
        introPnl.SetActive(false);
        gamePnl.SetActive(true);
        aniBatCntrlr.transform.parent.GetComponent<BoxCollider>().enabled = true;

        introCam.SetActive(false);
        gameCam.SetActive(true);
    }

    public void EndLevel(bool win)
    {
        if (!finished)
        {
            finished = true;
            gamePnl.SetActive(false);
            endPnl.SetActive(true);
            if (win)
            {
                switch (Random.Range(0, 5))
                {
                    case 0:
                        endTxt.text = "Well done!";
                        break;
                    case 1:
                        endTxt.text = "Grrreat!";
                        break;
                    case 2:
                        endTxt.text = "Exellent!";
                        break;
                    case 3:
                        endTxt.text = "Lovely!";
                        break;
                    case 4:
                        endTxt.text = "Niccce!";
                        break;
                }
                SaveManager.saveGlob.level += 1;
                // SaveManager.saveGlob.saveDataToDisk();
            }
            else
            {
                switch (Random.Range(0, 5))
                {
                    case 0:
                        endTxt.text = "Oh nooo...";
                        break;
                    case 1:
                        endTxt.text = "Trrry again...";
                        break;
                    case 2:
                        endTxt.text = "Arghhh!";
                        break;
                    case 3:
                        endTxt.text = "You fool...";
                        break;
                    case 4:
                        endTxt.text = "Next time...";
                        break;
                }
            }
            Time.timeScale = 0f;
        }
    }
}
