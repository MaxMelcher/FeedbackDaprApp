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
        public int Choice { get; set; }

        public string test {get; set;} = "test";
    }
}