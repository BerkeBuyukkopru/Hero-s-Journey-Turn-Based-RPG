import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class HeroService {
  private path = `${environment.apiBaseUrl}/hero/`;// Backend API URL's
  constructor(private httpClient:HttpClient) { }
  
  GetHero(heroId: number): Observable<any> {
    return this.httpClient.get(`${this.path}GetHero?heroId=${heroId}`, { responseType: 'text'}).pipe(map((response: any)=>{      
      return JSON.parse(response);
    }));   // Hero'yu id ile getir.
  }

  InitializeHeroFromTemplate(templateId: number, heroName: string): Observable<any> {
    
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    //Gönderilecek verninin başlığını tanımlıyoruz ve verinin JSON formatında gönderileceğini belirtiyoruz.
    const body = {templateId: templateId, heroName: heroName }; 
    //backend API'ye gönderilecek verinin tanımı.
    return this.httpClient.post(`${this.path}InitializeHeroFromTemplate`, JSON.stringify(body),{
      responseType: 'text', headers //post ile ekleme işlemi.
    }).pipe(map((response: any)=>{
      return JSON.parse(response);
    }));
  }

  UpdateHeroStats(heroId:number, heroLevel: number): Observable<any> {
    
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' }); 
    //Gönderilecek verninin başlığını tanımlıyoruz ve verinin JSON formatında gönderileceğini belirtiyoruz.
    const body = { heroId: heroId, heroLevel :heroLevel }; 
    //backend API'ye gönderilecek verinin tanımı.
    return this.httpClient.put(`${this.path}UpdateHeroStats`, JSON.stringify(body),{
      responseType: 'text', headers
    }).pipe(map((response: any)=>{
      
      return JSON.parse(response);
    })); //put ile güncelleme işlemi.
  }
}
