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

                if (context.Receipe != null && !context.Receipe.Any())
                {
                    context.Receipe.AddRange(new List<Receipe>()
                    { new Receipe()
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
                if (context.RawMaterial != null && !context.RawMaterial.Any())
                {
                    context.RawMaterial.AddRange(new List<RawMaterial>()
                    {new RawMaterial()
                    {
                        Hid = "airen",
                        Hname = "Airen",
                        Type = true,
                        Alcohol= 10,
                        Sweet = 2,
                        Acidity = 3,
                        Body = 2,
                        Tannin = 1, 
                        Stock = 500,
                        Quality = 4
                    }

                    }
                        );
                    context.SaveChanges();

                }
                if (context.EndProduct != null && !context.EndProduct.Any())
                {
                    context.EndProduct.AddRange(new List<EndProduct>()
                    { new EndProduct()
                    {
                        Mname = "Akabane",
                        Year = 2018,
                        Aging = 5,
                        Quality = 7,
                        Type = false,
                        Milil = 750,
                        Bottle = 300,
                        Stock = 200
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
