using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Ortak_Katman; //referans ekledik

namespace DAL
{
    public class HeroTemplateRepository
    {
        string connectionString = Database.ConnectionString;

        public HeroTemplate GetHeroTemplate(int HTId)
        {
            using (SqlConnection baglanti = new SqlConnection(connectionString))
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("SELECT * FROM Tbl_HeroTemplate WHERE HTId = @HTId", baglanti);
                komut.Parameters.AddWithValue("@HTId", HTId);
                SqlDataReader rd1 = komut.ExecuteReader();

                if (rd1.Read())
                {
                    return new HeroTemplate
                    {
                        HTId = (int)rd1["HTId"],
                        HTName = rd1["HTName"]as string,
                        HTAttackMin = (int)rd1["HTAttackMin"],
                        HTAttackMax = (int)rd1["HTAttackMax"],
                        HTDefenceMin = (int)rd1["HTDefenceMin"],
                        HTDefenceMax = (int)rd1["HTDefenceMax"],
                        HTPotMin = (int)rd1["HTPotMin"],
                        HTPotMax = (int)rd1["HTPotMax"],
                        HTHealth = (int)rd1["HTHealth"],
                        HTLevel = (int)rd1["HTLevel"],
                        HTImagePath = (string)rd1["HTImagePath"]
                    };
                }
                return null;
            }
        }
    }
}
