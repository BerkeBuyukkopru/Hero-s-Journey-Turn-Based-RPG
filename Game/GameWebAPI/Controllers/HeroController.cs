using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BLL;
using Ortak_Katman;
using System.Web.Http;

namespace GameWebAPI.Controllers
{
    public class HeroController : ApiController
    {
        private HeroService _heroService;
        public class HeroBody
        {
            public int TemplateId { get; set; }
            public string HeroName { get; set; }
            public int HeroId { get; set; }
            public int HeroLevel { get; set; }
        }

        [HttpGet]
        public IHttpActionResult GetHero(int heroId)
        {
            if (heroId <= 0)
            {
                return BadRequest("Geçersiz kahraman id.");
            }

            try
            {
                // HeroService için gerekli parametreleri sağlıyoruz
                _heroService = new HeroService();
                Hero hero = _heroService.GetHero(heroId);
                if (hero == null)
                {
                    return Ok(new { success = false, message = "Kahraman Bulunamadı." });
                }
                return Ok(new { success = true, hero = hero });
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        [HttpPost]
        public IHttpActionResult InitializeHeroFromTemplate([FromBody] HeroBody heroBody)
        {
            if (heroBody == null || heroBody.TemplateId <= 0)
            {
                return BadRequest("Geçersiz kahraman şablonu.");
            }

            if (string.IsNullOrWhiteSpace(heroBody.HeroName))
            {
                return BadRequest("Kahraman adı boş olamaz.");
            }

            heroBody.HeroName = heroBody.HeroName.Trim();
            if (heroBody.HeroName.Length > 20)
            {
                return BadRequest("Kahraman adı en fazla 20 karakter olabilir.");
            }

            try
            {
                // HeroService için gerekli parametreleri sağlıyoruz
                _heroService = new HeroService();
                var hero = _heroService.InitializeHeroFromTemplate(heroBody.TemplateId, heroBody.HeroName);
                if (hero == null)
                {
                    return NotFound();
                }

                return Ok(new { success = true, message = "Kahraman Başarıyla Eklendi.", hero = hero });
            }
            catch (Exception e)
            {
                // Hata mesajı loglanabilir veya uygun bir şekilde işlenebilir
                return InternalServerError(e);
            }
        }

        [HttpPut]
        public IHttpActionResult UpdateHeroStats([FromBody] HeroBody heroBody)
        {
            if (heroBody == null || heroBody.HeroId <= 0 || heroBody.HeroLevel <= 0)
            {
                return BadRequest("Geçersiz kahraman veya seviye bilgisi.");
            }

            try
            {
                _heroService = new HeroService();
                var hero = _heroService.UpdateHeroStats(heroBody.HeroId, heroBody.HeroLevel);
                if (hero == null)
                {
                    return NotFound();
                }

                return Ok(new { success = true, message = "Kahraman Başarıyla Güncellendi." ,hero=hero });
            }
            catch (Exception e)
            {
                // Hata mesajı loglanabilir veya uygun bir şekilde işlenebilir
                return InternalServerError(e);
            }
        }
    }
}
