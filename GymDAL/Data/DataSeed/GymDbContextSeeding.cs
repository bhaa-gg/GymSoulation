using GymDAL.Data.Contexts;
using GymDAL.Entities;
using System.Numerics;
using System.Text.Json;

namespace GymPL.DataSeed
{
    public class GymDbContextSeeding
    {
        public static bool SeedData(GymDBContext context)
        {
            try {

                var hasPlans = context.Plans.Any();
                var hasCategories = context.Categories.Any();
                if (hasCategories && hasCategories) return false;

                if (!hasPlans)
                {


                    var plans = ReadDataFromJsons<Plan>("plans.json");
                    if (plans.Any()) context.Plans.AddRange(plans);
                }

                if (!hasCategories)
                {
                    var categories = ReadDataFromJsons<Category>("categories.json");
                    if (categories.Any()) context.Categories.AddRange(categories);
                }

                return context.SaveChanges() > 0;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during data seeding: {ex.Message}");
                return false;
            }
        }  



        private static List<T> ReadDataFromJsons<T>(string path)
        {
            var FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Files", path);

            if(!File.Exists(FilePath)) throw new FileNotFoundException();


            string Data = File.ReadAllText(FilePath);


            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<List<T>>(Data, options)  ?? new List<T>();
        }
    }
}
