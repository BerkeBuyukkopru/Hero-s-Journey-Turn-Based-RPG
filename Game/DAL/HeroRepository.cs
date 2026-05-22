using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Ortak_Katman; //referans ekledik

//Kahraman verilerini burada alıp işlemler yapacağız.
namespace DAL
{
    public class HeroRepository
    {
        string connectionString = Database.ConnectionString; //veri tabanı adresimiz.

        public Hero GetHero(int heroId) //heroId parametresine göre veri al ve hero nesnesi olarak döndür.
        {
            using (SqlConnection baglanti = new SqlConnection(connectionString))
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("SELECT * FROM Tbl_Hero WHERE HeroId = @HeroId", baglanti);
                komut.Parameters.AddWithValue("@HeroId", heroId);
                SqlDataReader rd1 = komut.ExecuteReader();

                if (rd1.Read())
                {
                    return new Hero
                    {
                        HeroId = (int)rd1["HeroId"],
                        HeroName = (string)rd1["HeroName"],
                        HeroAttackMin = (int)rd1["HeroAttackMin"],
                        HeroAttackMax = (int)rd1["HeroAttackMax"],
                        HeroDefenceMin = (int)rd1["HeroDefenceMin"],
                        HeroDefenceMax = (int)rd1["HeroDefenceMax"],
                        HeroPotMin = (int)rd1["HeroPotMin"],
                        HeroPotMax = (int)rd1["HeroPotMax"],
                        HeroHealth = (int)rd1["HeroHealth"],
                        HeroLevel = (int)rd1["HeroLevel"],
                        HeroImagePath = (string)rd1["HeroImagePath"]
                    };
                }
                return null;
            }
        }

        public void AddHero(Hero hero)
        {
            using (SqlConnection baglanti = new SqlConnection(connectionString))
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("INSERT INTO Tbl_Hero (HeroName, HeroAttackMin, HeroAttackMax, HeroDefenceMin, HeroDefenceMax, HeroPotMin, HeroPotMax, HeroHealth, HeroLevel, HeroImagePath) VALUES (@HeroName, @HeroAttackMin, @HeroAttackMax, @HeroDefenceMin, @HeroDefenceMax, @HeroPotMin, @HeroPotMax, @HeroHealth, @HeroLevel, @HeroImagePath) select cast(SCOPE_IDENTITY() as int);", baglanti);

                komut.Parameters.AddWithValue("@HeroName", hero.HeroName);
                komut.Parameters.AddWithValue("@HeroAttackMin", hero.HeroAttackMin);
                komut.Parameters.AddWithValue("@HeroAttackMax", hero.HeroAttackMax);
                komut.Parameters.AddWithValue("@HeroDefenceMin", hero.HeroDefenceMin);
                komut.Parameters.AddWithValue("@HeroDefenceMax", hero.HeroDefenceMax);
                komut.Parameters.AddWithValue("@HeroPotMin", hero.HeroPotMin);
                komut.Parameters.AddWithValue("@HeroPotMax", hero.HeroPotMax);
                komut.Parameters.AddWithValue("@HeroHealth", hero.HeroHealth);
                komut.Parameters.AddWithValue("@HeroLevel", hero.HeroLevel);
                komut.Parameters.AddWithValue("@HeroImagePath", hero.HeroImagePath);

                //komut.ExecuteNonQuery();
                //int modified = (int)komut.ExecuteScalar();
                hero.HeroId = (int)komut.ExecuteScalar();
            }
        }

        public void UpdateHero(Hero hero) //hero nesnesini güncelleme
        {
            using (SqlConnection baglanti = new SqlConnection(connectionString))
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("UPDATE Tbl_Hero SET HeroName = @HeroName, HeroAttackMin = @HeroAttackMin, HeroAttackMax = @HeroAttackMax, HeroDefenceMin = @HeroDefenceMin, HeroDefenceMax = @HeroDefenceMax, HeroPotMin = @HeroPotMin, HeroPotMax = @HeroPotMax, HeroHealth = @HeroHealth, HeroLevel = @HeroLevel, HeroImagePath=@HeroImagePath WHERE HeroId = @HeroId", baglanti);

                komut.Parameters.AddWithValue("@HeroName", hero.HeroName);
                komut.Parameters.AddWithValue("@HeroAttackMin", hero.HeroAttackMin);
                komut.Parameters.AddWithValue("@HeroAttackMax", hero.HeroAttackMax);
                komut.Parameters.AddWithValue("@HeroDefenceMin", hero.HeroDefenceMin);
                komut.Parameters.AddWithValue("@HeroDefenceMax", hero.HeroDefenceMax);
                komut.Parameters.AddWithValue("@HeroPotMin", hero.HeroPotMin);
                komut.Parameters.AddWithValue("@HeroPotMax", hero.HeroPotMax);
                komut.Parameters.AddWithValue("@HeroHealth", hero.HeroHealth);
                komut.Parameters.AddWithValue("@HeroLevel", hero.HeroLevel);
                komut.Parameters.AddWithValue("@HeroId", hero.HeroId);
                komut.Parameters.AddWithValue("@HeroImagePath", hero.HeroImagePath);

                komut.ExecuteNonQuery();
                // Komutu çalıştır
            }
        }
    }
}
