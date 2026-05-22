using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL; //referans ekledim
using Ortak_Katman; //referans ekledim

namespace BLL
{
    public class HeroService
    {
        private readonly HeroRepository _heroRepository; //HeroRepository sınıfının örneğini sakla.
        private readonly HeroTemplateRepository _heroTemplateRepository;
        private readonly LevelRepository _levelRepository; //LevelRepository sınıfının örneğini sakla.

        public HeroService()
        {
            _heroRepository = new HeroRepository();
            _heroTemplateRepository = new HeroTemplateRepository();
            _levelRepository = new LevelRepository();
        }

        public Hero InitializeHeroFromTemplate(int templateId, string heroName)
        {
   
            var template = _heroTemplateRepository.GetHeroTemplate(templateId);
            if (template == null)
            {
                return null;
            }

            var hero = new Hero
            {
                HeroName = heroName,  // Burada HeroName'i atıyoruz
                HeroAttackMin = template.HTAttackMin,
                HeroAttackMax = template.HTAttackMax,
                HeroDefenceMin = template.HTDefenceMin,
                HeroDefenceMax = template.HTDefenceMax,
                HeroPotMin = template.HTPotMin,
                HeroPotMax = template.HTPotMax,
                HeroHealth = template.HTHealth,
                MaxHealth = template.HTHealth,
                HeroLevel = template.HTLevel,
                HeroImagePath = template.HTImagePath
            };

            _heroRepository.AddHero(hero); // Hero'yu veritabanına ekle
            
            return hero;
        }

        public Hero UpdateHeroStats(int heroId, int heroLevel)
        {
            Hero hero = _heroRepository.GetHero(heroId);
            Level level = _levelRepository.GetLevel(heroLevel);
            if (hero == null || level == null)
            {
                return null;
            }

            hero.HeroAttackMin = (int)(hero.HeroAttackMin * level.HeroStatMultiplier);
            hero.HeroAttackMax = (int)(hero.HeroAttackMax * level.HeroStatMultiplier);
            hero.HeroDefenceMin = (int)(hero.HeroDefenceMin * level.HeroStatMultiplier);
            hero.HeroDefenceMax = (int)(hero.HeroDefenceMax * level.HeroStatMultiplier);
            hero.HeroPotMin = (int)(hero.HeroPotMin * level.HeroStatMultiplier);
            hero.HeroPotMax = (int)(hero.HeroPotMax * level.HeroStatMultiplier);
            hero.HeroHealth = (int)(hero.HeroHealth * level.HeroStatMultiplier);

            hero.HeroLevel = heroLevel;

            _heroRepository.UpdateHero(hero);
            return hero;
            
        }
        public Hero GetHero(int heroId)
        {
            return _heroRepository.GetHero(heroId); //heroıd ye göre döndürür  
        }
    }
}
