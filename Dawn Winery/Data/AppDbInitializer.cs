using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Dawn_Winery.Models;
using Microsoft.EntityFrameworkCore;

namespace Dawn_Winery.Data
{
    public class AppDbInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using(var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
                context.Database.Migrate();

                if (context.Receipes != null && !context.Receipes.Any())
                {
                    context.Receipes.AddRange(new List<Receipes>()
                    { new Receipes()
                    {
                        Type = true,
                        Grape1 = "prosecco",
                        G1Kilo = 375,
                        SO2 = 100,
                        Rname = "Vyn"
                    }
                    }
                        ); 
                    context.SaveChanges();
                }
                if (context.RawMaterials != null && !context.RawMaterials.Any())
                {
                    context.RawMaterials.AddRange(new List<RawMaterials>()
                    {new RawMaterials()
                    {
                        Hid = "airen",
                        Hname = "Airen",
                        Type = true,
                        Alcohol= 10,
                        Sweet = 2,
                        Acidity = 3,
                        Body = 2,
                        Tannin = 1, 
                        Stock = 500
                    }

                    }
                        );
                    context.SaveChanges();

                }
                if (context.EndProducts != null && !context.EndProducts.Any())
                {
                    context.EndProducts.AddRange(new List<EndProducts>()
                    { new EndProducts()
                    {
                        Mname = "Akabane",
                        Year = 2018,
                        Aging = 5,
                        Quality = 7,
                        Type = false,
                        Milil = 750,
                        Bottle = 300
                    }

                    }

                       );
                    context.SaveChanges();

                }
                if (context.Employees != null && !context.Employees.Any())
                {
                    context.Employees.AddRange(new List<Employee>()
                    { new Employee()
                    {
                        Id = 1,
                        Name = "Tugce",
                        Title = "Expert"
                    }

                    }

                       );
                    context.SaveChanges();

                }

            }
        }
    }
}
