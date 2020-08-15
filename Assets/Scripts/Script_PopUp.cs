using System.Collections;
using TMPro;
using UnityEngine;

public class Script_PopUp : MonoBehaviour
{
    public GameObject PopUp;
    public TextMeshProUGUI ScoreAmount;
    public TextMeshProUGUI Text_GameOutcome;
    public TextMeshProUGUI PopUpButtonText;

    Vector3 ScaleXToZero = new Vector3(0, 1, 1 );       //  Somehow .Set() does not seem to work directly on objects
    Vector3 ScaleXIncrement = new Vector3(0.04F,0,0);   //  vectors are used instead

    Vector3 VisiblePos = new Vector3(0, 0, -10);
    Vector3 InvisiblePos = new Vector3(0, 15, -10);

    int StartingScore;                                  // stores score player starts with when starting new level
    void Start()
    {
        StartingScore = Settings.ScoreCount;
        SetPopUpInvisible();
        PopUp.transform.localScale = ScaleXToZero;
        ScoreAmount.text = "";
    } 
    public IEnumerator Event_ShowPopUp(bool Win)   // !Win ----> game was lost
    {
        SetPopUpVisible();                  // place in middle of the screen
        int countscore = StartingScore;     // for scorecounter effect

        if (Win)
        {
            Text_GameOutcome.text = "LEVEL COMPLETED";
            PopUpButtonText.text = "NEXT LEVEL";
        }
        else
        {
            Text_GameOutcome.text = "LEVEL FAILED";
            PopUpButtonText.text = "BACK TO MENU";
        }
                        
        while (PopUp.transform.localScale.x <= 1)
        {
            yield return null;
            PopUp.transform.localScale += ScaleXIncrement;
        }

        yield return new WaitForSeconds(1F);        
        if (Settings.ScoreCount > countscore)               // in rare cases where player did not score any points in level, 
                                                            // PopUp would not display score at all. This if statement prevents this situation
        {
            while (countscore < Settings.ScoreCount)
            {
                countscore += 5;                             // score is always dividable by 5, this speeds up scorecount
                ScoreAmount.text = countscore.ToString();
                yield return null;
            }
        }
        else // only case when Settings.ScoreCount == countscore is not gaining points at all on level
        {
            ScoreAmount.text = countscore.ToString();
            yield return null;
        }     

        if (Settings.Difficulty > 1)    // display this effect only if player had completed at least one level
        {
            yield return new WaitForSeconds(0.5F);
            ScoreAmount.text += " (+" + (countscore - StartingScore) + ")";
        }

    }

    void SetPopUpVisible()    // This is used instead setting PopUp to active / inactive since it messes with calling functions and coroutines
    {
        PopUp.transform.position = VisiblePos;
    }

    void SetPopUpInvisible()    // This is used instead setting PopUp to active / inactive since it messes with calling functions and coroutines
    {
        PopUp.transform.position = InvisiblePos;
    }
}
