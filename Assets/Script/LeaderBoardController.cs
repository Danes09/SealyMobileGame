using UnityEngine.UI;
using LootLocker.Requests;
using UnityEngine;

public class LeaderBoardController : MonoBehaviour
{
    public InputField memberID, playerScore;
    public int ID; //this refers to the lootlocker leaderboard ID

    public int MaxScores = 10; //number of scores that can be displayed

    public Text[] Entries;


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

    public void ShowScores()
    {
        LootLockerSDKManager.GetScoreList(ID, MaxScores, (response) => {

            if (response.success)
            {
                //LootLockerMember has become LootLockerLeaderboardMember

                LootLockerLeaderboardMember[] scores = response.items;
                //LootLockerMember[] scores = response.items;

                for (int i = 0; i < scores.Length; i++)
                {
                    Entries[i].text = (scores[i].rank + ".   " + scores[i].score);

                }

                if (scores.Length < MaxScores)
                {

                    for (int i = scores.Length; i < MaxScores; i++)
                    {
                        //Entries[i].text = (i + 1).ToString() + ".   none"; //no reason to convert to string
                        Entries[i].text = (i + 1) + "." + "  none";
                    }

                    if (scores.Length == MaxScores)
                    {

                        for (int i = scores.Length; i < MaxScores; i++)
                        {
                            //Entries[i].text = (i + 1).ToString() + ".   none"; //no reason to convert to string
                            Entries[i].text = (i + 1) + "."+ "none"; //fewer spaces to accommodate #10
                        }



                    }

                }

                else
                {
                    Debug.Log("Fail in show scores");
                }


            }
        }
        );
    } //show scores

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
