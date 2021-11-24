namespace app.Controllers
{
    public class Feedback
    {
        public enum FeedbackValue
        {
            bad = 1,
            good = 2
        }

        public string SessionId { get; set; }
        public string Choice { get; set; }

        public string test {get; set;} = "test";
    }
}