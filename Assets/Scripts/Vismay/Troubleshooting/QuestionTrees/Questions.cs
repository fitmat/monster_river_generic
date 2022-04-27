using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TroubleShoot/Questions/QuestionList")]
public class Questions : ScriptableObject
{
    [SerializeField] List<QuestionModel> questions;

    public List<QuestionModel> GetQuestions()
    {
        return questions;
    }

    public void SetQuestions(List<QuestionModel> value)
    {
        questions = value;
    }

    [System.Serializable]
    public class QuestionModel
    {
        public int id;
        public string question;
        public string[] solutions;
        public string[] choices;
    }
}
