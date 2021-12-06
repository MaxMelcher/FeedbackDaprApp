
export class Session {
  public SessionId: string;
  public Title: string;

  constructor(sessionId: string, title: string) {
    this.SessionId = sessionId;
    this.Title = title;
  }
}
