import { Injectable } from '@angular/core';
import { HttpClient } from  '@angular/common/http';  
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ItemService {
  SERVER_URL: string = environment.apiUrl; 
  constructor(private httpClient : HttpClient) { }

  public addItem(formData) {
      return this.httpClient.post<any>(`${this.SERVER_URL}/itemController/addItem`, formData, {  
        reportProgress: true,  
        observe: 'events'  
      });  
  }

  public getItems(): Observable<any> {
    return this.httpClient.get<any>(`${this.SERVER_URL}/itemController/getAllItems`);  
}
}
