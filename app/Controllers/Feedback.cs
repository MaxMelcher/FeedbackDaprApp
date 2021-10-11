namespace app.Controllers
{
    public class Feedback
    {
        public enum FeedbackValue
        {
            bad = 1,
            good = 2
        }

        public int SessionId { get; set; }
        public FeedbackValue Value { get; set; }
    }
}