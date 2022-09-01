namespace AlcoAxe.Data
{

    public class SampleQuestion
    {
        public int Id { get; set; }

        public string QuestionString { get; set; }
        public GameType GameType { get; set; }
        public GameMode GameMode { get; set; }

    }

    public class Question
    {
        public int Id { get; set; }

        public SampleQuestion SampleQuestion { get; set; }

        public QuestionDate QuestionDate { get; set; }
        public int Position { get; set; }
        public Game Game { get; set; }
        public Player Player { get; set; }

    }

    public enum QuestionDate
    {
        Right = 1,
        Wrong = 0,
        NoDate = -1
    }

    public enum GameMode
    {
        Easy = 0,
        Medium = 1,
        HardCore = 2,
        Random = 4

    }
}