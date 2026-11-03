import { Component, inject, OnInit, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-root',
  styleUrl: './app.css',
  templateUrl: './app.html',
})
export class App implements OnInit {
  private http = inject(HttpClient);
  protected readonly title = signal('Dating app');
  protected members = signal<any>([]);

  ngOnInit() {
    this.http.get("https://localhost:5001/api/members").subscribe({
      next: data => { this.members.set(data) },
      error: err => { console.log(err); },
      complete: () => { console.log('complete'); },
    });
  }
}
