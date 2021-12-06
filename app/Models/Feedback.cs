namespace app.Models
{
    public class Feedback
    {
        public enum FeedbackValue
        {
            bad = 1,
            good = 2
        }

        public string SessionId { get; set; }
        public FeedbackValue Choice { get; set; }

    }
}