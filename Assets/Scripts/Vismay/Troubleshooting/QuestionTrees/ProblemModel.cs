public class ProblemModel
{
    public int id;
    public string question;
    public string[] solutions;
    public string[] choices;

    // Class constructor with multiple parameters
    public ProblemModel(int id, string question, string[] solutions, string[] choices)
    {
        this.id = id;
        this.question = question;
        this.solutions = solutions;
        this.choices = choices;
    }
}
