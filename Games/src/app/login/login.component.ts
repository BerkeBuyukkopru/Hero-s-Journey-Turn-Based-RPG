import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { HeroService } from '../services/hero.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  heroName: string = '';
  templateId: number = 1;
  noticeTitle = '';
  noticeMessage = '';

  constructor(private heroService: HeroService, private router: Router) {}

  onLogin(): void {
    const trimmedHeroName = this.heroName.trim();

    if (!trimmedHeroName) {
      this.showNotice('İsim gerekli', 'Lütfen bir kahraman adı giriniz.');
      return;
    }

    if (trimmedHeroName.length > 20) {
      this.showNotice('İsim çok uzun', 'Kahraman adı en fazla 20 karakter olabilir.');
      return;
    }

    this.heroService.InitializeHeroFromTemplate(this.templateId, trimmedHeroName)
      .subscribe(
        (returnModel: any) => {
          const heroId = returnModel.hero.HeroId;

          this.router.navigate(['/game'], { queryParams: { heroId: heroId } });
        },
        (error: any) => {
          this.showNotice('Bir hata oluştu', this.getErrorMessage(error));
        }
      );
  }

  showNotice(title: string, message: string): void {
    this.noticeTitle = title;
    this.noticeMessage = message;
  }

  closeNotice(): void {
    this.noticeTitle = '';
    this.noticeMessage = '';
  }

  private getErrorMessage(error: any): string {
    if (error && error.error && error.error.Message) {
      return error.error.Message;
    }

    if (error && error.message) {
      return error.message;
    }

    return 'Bilinmeyen hata';
  }
}
