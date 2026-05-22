using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Ortak_Katman; // Referans ekledik.

//Level verilerini burada alıp işlemler yapacağız.
namespace DAL
{
    public class LevelRepository
    {
        string connectionString = Database.ConnectionString; //veri tabanı adresimiz.
            public Level GetLevel(int levelId) //levelId parametresine göre Level veri tabanından çek.
            {
                using (SqlConnection baglanti = new SqlConnection(connectionString))
                {
                    baglanti.Open();
                    SqlCommand komut1 = new SqlCommand("SELECT * FROM Tbl_Level WHERE LevelId = @LevelId", baglanti);
                    komut1.Parameters.AddWithValue("@LevelId", levelId);
                    SqlDataReader rd1 = komut1.ExecuteReader();

                    if (rd1.Read()) //eğer okunacak değer varsa
                    {
                        return new Level //yeni bir level nesnesi oluştur ve ata
                        {
                            LevelId = (int)rd1["LevelId"],
                            LevelNumber = (int)rd1["LevelNumber"],
                            HeroStatMultiplier = rd1["HeroStatMultiplier"] != DBNull.Value ? Convert.ToSingle(rd1["HeroStatMultiplier"]) : 1.0f //sql veri tabanından "double" olarak geliyor. Floata çevirdik.
                        };
                    }
                    return null; //sonuç bulamazsa
                }
            }
    }
}
