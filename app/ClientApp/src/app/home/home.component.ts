import { Component } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';
import { MatTableModule } from '@angular/material/table';
import { Feedback } from './Feedback';
import { Session } from './Session';
import { MatDialog } from '@angular/material/dialog';
import { DialogOverviewExampleDialog } from './DialogOverviewExampleDialog';

export interface DialogData {
  title: string;
}

const sessions: Session[] = [
  { "SessionId": "1", "Title": "Awesome Session 1" },
  { "SessionId": "2", "Title": "Angular Speedrun 2" },
  { "SessionId": "3", "Title": "From Zero to Dapr Hero" },
  { "SessionId": "4", "Title": "36 hours later" },
];

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {
  displayedColumns: string[] = ['sessionId', 'title', 'actions'];
  dataSource = sessions;
  constructor(private http: HttpClient, public dialog: MatDialog) {
  }

  openDialog(): void {
    const dialogRef = this.dialog.open(DialogOverviewExampleDialog, {
      width: '600px',
      data: { title: "" }
    });

    dialogRef.afterClosed().subscribe(title => {
      console.log('The dialog was closed', title);

      var session = new Session("", title)
      this.createSession(session);
    });
  }

  createSession(session:Session) {
    console.log(session);
    this.http.post('/api/session/create', session)
      .pipe(
        catchError(this.handleError)
      ).subscribe();
  }

  submitFeedback(sessionId: string, value: string) {

    console.log("sessionId", sessionId);
    console.log("value", value);
    let feedback = new Feedback(sessionId, value);

    this.http.post('/api/feedback/submitFeedback', feedback)
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

