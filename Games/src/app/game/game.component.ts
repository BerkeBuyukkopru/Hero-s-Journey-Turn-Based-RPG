import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HeroService } from '../services/hero.service';
import { EnemyService } from '../services/enemy.service';
import { levelmodel } from '../models/levelmodel';
import { enemymodel } from '../models/enemymodel';
import { heromodel } from '../models/heromodel';

interface BattleLogEntry {
  source: 'hero' | 'enemy' | 'system';
  text: string;
}

@Component({
  selector: 'app-game',
  templateUrl: './game.component.html',
  styleUrls: ['./game.component.css']
})
export class GameComponent implements OnInit {
  heroId: number;

  hero: heromodel;
  enemy: enemymodel;
  level: levelmodel;

  battleLogs: BattleLogEntry[] = [];
  turnPhase: 'loading' | 'player' | 'enemy' | 'modal' | 'finished' = 'loading';
  statusMessage = 'Savaş hazırlanıyor...';
  isHealButtonDisabled = false;

  modalTitle = '';
  modalMessage = '';
  modalAction: 'none' | 'nextLevel' | 'login' = 'none';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private heroService: HeroService,
    private enemyService: EnemyService
  ) { }

  ngOnInit(): void {
    this.level = new levelmodel();
    this.level.LevelNumber = 1;

    this.route.queryParams.subscribe(params => {
      this.heroId = Number(params['heroId']);

      if (!Number.isInteger(this.heroId) || this.heroId <= 0) {
        this.router.navigate(['/login']);
        return;
      }

      this.initializeHero();
      this.initializeEnemy();
    });
  }

  initializeHero(): void {
    this.heroService.GetHero(this.heroId).subscribe(
      (data: any) => {
        if (!data || !data.success || !data.hero) {
          this.showModal('Kahraman bulunamadı', 'Giriş sayfasına yönlendiriliyorsun...', 'login');
          return;
        }

        this.hero = data.hero;
        this.hero.MaxHealth = this.hero.HeroHealth;
        this.resetHealButton();
        this.activatePlayerTurnIfReady();
      },
      (error: any) => this.showModal('Kahraman yüklenemedi', this.getErrorMessage(error), 'login')
    );
  }

  initializeEnemy(): void {
    this.enemyService.GetRandomEnemyByLevel(this.level.LevelNumber).subscribe(
      (data: any) => {
        this.enemy = data.enemy;
        this.enemy.EnemyMaxHealth = this.enemy.EnemyHealth;
        this.addLog('system', `${this.enemy.EnemyName} karşına çıktı.`);
        this.activatePlayerTurnIfReady();
      },
      (error: any) => this.showModal('Düşman yüklenemedi', this.getErrorMessage(error), 'login')
    );
  }

  heroAttack(): void {
    if (!this.canAct()) {
      return;
    }

    const heroAttackPower = this.randomBetween(this.hero.HeroAttackMin, this.hero.HeroAttackMax);
    this.enemy.EnemyHealth = Math.max(this.enemy.EnemyHealth - heroAttackPower, 0);
    this.addLog('hero', `${this.enemy.EnemyName} adlı düşmana ${heroAttackPower} hasar verdin.`);

    if (this.enemy.EnemyHealth <= 0) {
      this.handleEnemyDefeated();
      return;
    }

    this.queueEnemyTurn();
  }

  defend(): void {
    if (!this.canAct()) {
      return;
    }

    this.addLog('hero', 'Savunmaya geçtin. Potun yeniden hazırlandı.');
    this.isHealButtonDisabled = false;
    this.queueEnemyTurn(true);
  }

  heroHeal(): void {
    if (!this.canAct()) {
      return;
    }

    if (this.isHealButtonDisabled) {
      this.addLog('system', 'Pot tekrar kullanılamaz. Savunma yaparak potunu hazırlayabilirsin.');
      return;
    }

    if (this.hero.HeroHealth >= this.hero.MaxHealth) {
      this.hero.HeroHealth = this.hero.MaxHealth;
      this.addLog('system', 'Sağlığın tamamen dolu.');
      return;
    }

    const healingPower = this.randomBetween(this.hero.HeroPotMin, this.hero.HeroPotMax);
    this.hero.HeroHealth = Math.min(this.hero.HeroHealth + healingPower, this.hero.MaxHealth);
    this.isHealButtonDisabled = true;
    this.addLog('hero', `Sağlık potu kullandın ve ${healingPower} can iyileştin.`);
  }

  closeModal(): void {
    const action = this.modalAction;

    this.modalTitle = '';
    this.modalMessage = '';
    this.modalAction = 'none';

    if (action === 'nextLevel') {
      this.advanceLevel();
      return;
    }

    if (action === 'login') {
      this.router.navigate(['/login']);
    }
  }

  isActionDisabled(): boolean {
    return this.turnPhase !== 'player' || !this.hero || !this.enemy;
  }

  private queueEnemyTurn(isDefending: boolean = false): void {
    this.turnPhase = 'enemy';
    this.statusMessage = 'Düşman saldırıya hazırlanıyor...';

    setTimeout(() => {
      this.enemyAttack(isDefending);
    }, 900);
  }

  private enemyAttack(isDefending: boolean): void {
    if (!this.hero || !this.enemy || this.turnPhase !== 'enemy') {
      return;
    }

    const enemyAttackPower = this.randomBetween(this.enemy.EnemyAttackMin, this.enemy.EnemyAttackMax);
    let effectiveDamage = enemyAttackPower;

    if (isDefending) {
      const defencePower = this.randomBetween(this.hero.HeroDefenceMin, this.hero.HeroDefenceMax);
      const guardedDamage = Math.floor(enemyAttackPower * 0.5);
      effectiveDamage = Math.max(guardedDamage - defencePower, 0);

      this.addLog('hero', `${defencePower} savunma puanı ve gard ile hasarı azalttın.`);

      if (effectiveDamage === 0) {
        const counterDamage = Math.max(Math.floor(defencePower / 2), 1);
        this.enemy.EnemyHealth = Math.max(this.enemy.EnemyHealth - counterDamage, 0);
        this.addLog('hero', `Tam blok yaptın ve ${counterDamage} karşı hasar verdin.`);

        if (this.enemy.EnemyHealth <= 0) {
          this.handleEnemyDefeated();
          return;
        }
      }
    }

    this.hero.HeroHealth = Math.max(this.hero.HeroHealth - effectiveDamage, 0);
    this.addLog('enemy', `${this.enemy.EnemyName} ${effectiveDamage} hasar verdi.`);

    if (this.hero.HeroHealth <= 0) {
      this.turnPhase = 'finished';
      this.showModal('Oyunu kaybettin', 'Giriş sayfasına yönlendiriliyorsun...', 'login');
      return;
    }

    this.turnPhase = 'player';
    this.statusMessage = 'Sıra sende.';
  }

  private handleEnemyDefeated(): void {
    this.turnPhase = 'modal';

    if (this.level.LevelNumber < 5) {
      this.showModal(`${this.enemy.EnemyName} yenildi`, 'Sonraki seviyeye geçiyorsun.', 'nextLevel');
      return;
    }

    this.showModal('Oyun bitti', 'Tüm düşmanları yok ettin. Giriş sayfasına yönlendiriliyorsun...', 'login');
  }

  private advanceLevel(): void {
    if (!this.hero || !this.level) {
      return;
    }

    this.turnPhase = 'loading';
    this.statusMessage = 'Yeni seviye hazırlanıyor...';
    this.battleLogs = [];
    this.level.LevelNumber++;

    this.heroService.UpdateHeroStats(this.hero.HeroId, this.level.LevelNumber).subscribe(
      (data: any) => {
        this.hero = data.hero;
        this.hero.MaxHealth = this.hero.HeroHealth;
        this.resetHealButton();
        this.initializeEnemy();
      },
      (error: any) => this.showModal('Seviye güncellenemedi', this.getErrorMessage(error), 'login')
    );
  }

  private activatePlayerTurnIfReady(): void {
    if (this.hero && this.enemy && this.turnPhase === 'loading') {
      this.turnPhase = 'player';
      this.statusMessage = 'Sıra sende.';
    }
  }

  private resetHealButton(): void {
    this.isHealButtonDisabled = false;
  }

  private canAct(): boolean {
    return this.turnPhase === 'player' && !!this.hero && !!this.enemy;
  }

  private showModal(title: string, message: string, action: 'none' | 'nextLevel' | 'login' = 'none'): void {
    this.modalTitle = title;
    this.modalMessage = message;
    this.modalAction = action;
    this.turnPhase = action === 'none' ? this.turnPhase : 'modal';
  }

  private addLog(source: 'hero' | 'enemy' | 'system', text: string): void {
    this.battleLogs = [{ source: source, text: text }].concat(this.battleLogs).slice(0, 6);
  }

  private randomBetween(min: number, max: number): number {
    return Math.floor(Math.random() * (max - min + 1)) + min;
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
