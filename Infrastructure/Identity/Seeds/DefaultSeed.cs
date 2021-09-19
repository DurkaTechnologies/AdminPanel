using Domain.Entities;
using Infrastructure.DbContexts;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Identity.Seeds
{
    public static class DefaultSeed
    {
        private static List<District> districts = new List<District>()
        {
            new District { Id = 1, Name = "Рівненський" },
            new District { Id = 2, Name = "Вараський" },
            new District { Id = 3, Name = "Саренський" },
            new District { Id = 4, Name = "Дубенський" },
        };

        private static List<Community> communities = new List<Community>()
        {
            new Community { DistrictId = 2, Name = "Антонівська" },
            new Community { DistrictId = 1, Name = "Бабинська" },
            new Community { DistrictId = 3, Name = "Березівська" },
            new Community { DistrictId = 1, Name = "Березнівська" },
            new Community { DistrictId = 1, Name = "Білокриницька" },
            new Community { DistrictId = 4, Name = "Бокіймівська" },
            new Community { DistrictId = 4, Name = "Боремельська" },
            new Community { DistrictId = 1, Name = "Бугринська" },
            new Community { DistrictId = 2, Name = "Вараська" },
            new Community { DistrictId = 4, Name = "Варковицька" },
            new Community { DistrictId = 1, Name = "Великомежиріцька" },
            new Community { DistrictId = 1, Name = "Великоомелянська" },
            new Community { DistrictId = 4, Name = "Вербська" },
            new Community { DistrictId = 3, Name = "Вирівська" },
            new Community { DistrictId = 3, Name = "Висоцька" },
            new Community { DistrictId = 2, Name = "Володимирецька" },
            new Community { DistrictId = 1, Name = "Головинська" },
            new Community { DistrictId = 1, Name = "Городоцька" },
            new Community { DistrictId = 1, Name = "Гощанська" },
            new Community { DistrictId = 4, Name = "Демидівська" },
            new Community { DistrictId = 1, Name = "Деражненська" },
            new Community { DistrictId = 4, Name = "Дубенська" },
            new Community { DistrictId = 3, Name = "Дубровицька" },
            new Community { DistrictId = 1, Name = "Дядьковицька" },
            new Community { DistrictId = 2, Name = "Зарічненська" },
            new Community { DistrictId = 1, Name = "Здовбицька" },
            new Community { DistrictId = 1, Name = "Здолбунівська" },
            new Community { DistrictId = 1, Name = "Зорянська" },
            new Community { DistrictId = 2, Name = "Каноницька" },
            new Community { DistrictId = 1, Name = "Клеванська" },
            new Community { DistrictId = 3, Name = "Клесівська" },
            new Community { DistrictId = 4, Name = "Козинська" },
            new Community { DistrictId = 1, Name = "Корецька" },
            new Community { DistrictId = 1, Name = "Корнинська" },
            new Community { DistrictId = 1, Name = "Костопільська" },
            new Community { DistrictId = 4, Name = "Крупецька" },
            new Community { DistrictId = 2, Name = "Локницька" },
            new Community { DistrictId = 1, Name = "Малинська" },
            new Community { DistrictId = 1, Name = "Малолюбашанська" },
            new Community { DistrictId = 3, Name = "Миляцька" },
            new Community { DistrictId = 4, Name = "Мирогощанська" },
            new Community { DistrictId = 1, Name = "Мізоцька" },
            new Community { DistrictId = 4, Name = "Млинівська" },
            new Community { DistrictId = 3, Name = "Немовицька" },
            new Community { DistrictId = 1, Name = "Олександрійська" },
            new Community { DistrictId = 4, Name = "Острожецька" },
            new Community { DistrictId = 1, Name = "Острозька" },
            new Community { DistrictId = 4, Name = "Підлозцівська" },
            new Community { DistrictId = 4, Name = "Повчанська" },
            new Community { DistrictId = 2, Name = "Полицька" },
            new Community { DistrictId = 4, Name = "Привільненська" },
            new Community { DistrictId = 4, Name = "Радивилівська" },
            new Community { DistrictId = 2, Name = "Рафалівська" },
            new Community { DistrictId = 1, Name = "Рівненська" },
            new Community { DistrictId = 3, Name = "Рокитнівська" },
            new Community { DistrictId = 3, Name = "Сарненська" },
            new Community { DistrictId = 4, Name = "Семидубська" },
            new Community { DistrictId = 4, Name = "Смизька" },
            new Community { DistrictId = 1, Name = "Соснівська" },
            new Community { DistrictId = 3, Name = "Старосільська" },
            new Community { DistrictId = 3, Name = "Степанська" },
            new Community { DistrictId = 4, Name = "Тараканівська" },
            new Community { DistrictId = 1, Name = "Шпанівська" },
            new Community { DistrictId = 4, Name = "Ярославицька" },
        };

        public static async Task SeedAsync(IdentityContext context)
        {
            foreach (var item in districts)
            {
                bool result = await context.Districts.AnyAsync(x => x.Name == item.Name);

                if (!result)
                {
                    IEnumerable<Community> toSet = communities.Where(x => x.DistrictId == item.Id);

                    item.Id = 0;
                    await context.Districts.AddAsync(item);
                    context.SaveChanges();

                    var id = (await context.Districts.ToListAsync()).FirstOrDefault(x => x.Name == item.Name).Id;

                    foreach (var setElement in toSet)
                        setElement.DistrictId = id;

                    context.Communities.AddRange(toSet);
                    context.SaveChanges();
                }
            }
        }
    }
}
