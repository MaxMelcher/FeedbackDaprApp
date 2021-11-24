import { Component } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {

  constructor(private http: HttpClient) {

  }

  submitFeedback(sessionId: string, value: string) {

    console.log("sessionId", sessionId);
    console.log("value", value);
    let feedback = new Feedback(sessionId, value);

    this.http.post('/api/weatherforecast/submitFeedback', feedback)
      .pipe(
        catchError(this.handleError)
      ).subscribe();

  }

  private handleError(error: HttpErrorResponse) {
    if (error.status === 0) {
      // A client-side or network error occurred. Handle it accordingly.
      console.error('An error occurred:', error.error);
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong.
      console.error(
        `Backend returned code ${error.status}, body was: `, error.error);
    }
    // Return an observable with a user-facing error message.
    return throwError(
      'Something bad happened; please try again later.');
  }
}

export class Feedback {
  public SessionId: string;
  public Choice: string;

  constructor(sessionId: string, feedbackValue: string) {
    this.Choice = feedbackValue;
    this.SessionId = sessionId;
  }
}
