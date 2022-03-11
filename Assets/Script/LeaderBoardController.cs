using UnityEngine.UI;
using LootLocker.Requests;
using UnityEngine;

public class LeaderBoardController : MonoBehaviour
{
    public InputField memberID, playerScore;
    public int ID; //this refers to the lootlocker leaderboard ID


    /*CURRENTLY SET TO MANUALLY ENTERING THE SCORE. SHALL BE MODIFIED TO GET THE SCORE FROM OTHER SCRIPTS*/

    public void Start()

    {

        LootLockerSDKManager.StartSession("Player",(response)=>
        {
            if (response.success)
            {
                Debug.Log("Success in start");
            }

            else
            {
                Debug.Log("Fail in start");
            }


        }

        );
    } //start


    public void SubmitScore() {

        LootLockerSDKManager.SubmitScore(memberID.text, int.Parse(playerScore.text), ID, (response)=>

        {
            if (response.success)
            {
                Debug.Log("Success in submit score");
            }


            else
            {
                Debug.Log("Failure in submit score");
            }

        }

        );
    
    }



} //class
