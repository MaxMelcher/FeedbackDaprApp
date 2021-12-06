
export class Feedback {
  public SessionId: string;
  public Choice: string;

  constructor(sessionId: string, feedbackValue: string) {
    this.Choice = feedbackValue;
    this.SessionId = sessionId;
  }
}
