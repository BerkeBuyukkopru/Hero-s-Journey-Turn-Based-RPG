using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Ortak_Katman; // Referans ekledik.

//Düşman verilerini burada alıp işlemler yapacağız.
namespace DAL
{
    public class EnemyRepository
    {
        string connectionString = Database.ConnectionString; //SQL Bağlantı Adresimiz.
        public List<Enemy> GetEnemiesByLevel(int level) //List<Enemy> veri türünü döndür. Parametre olarak level kullan.
        {
            List<Enemy> enemies = new List<Enemy>(); //enemy listesi oluştur

            using (SqlConnection baglanti = new SqlConnection(connectionString))
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("SELECT * FROM Tbl_Enemy WHERE EnemyLevel = @EnemyLevel", baglanti);
                komut.Parameters.AddWithValue("@EnemyLevel", level);
                SqlDataReader rd = komut.ExecuteReader();

                while (rd.Read()) //eğer okunacak veri varsa
                {
                    enemies.Add(new Enemy
                    {
                        EnemyId = (int)rd["EnemyId"],
                        EnemyName = (string)rd["EnemyName"],
                        EnemyLevel = (int)rd["EnemyLevel"],
                        EnemyAttackMin = (int)rd["EnemyAttackMin"],
                        EnemyAttackMax = (int)rd["EnemyAttackMax"],
                        EnemyHealth = (int)rd["EnemyHealth"],
                        EnemyImagePath = (string)rd["EnemyImagePath"]
                    }); //Enemy listesine ekle.
                }
            }
            return enemies; //okunacak veri yoksa listeyi döndür.
        }
    }
}
