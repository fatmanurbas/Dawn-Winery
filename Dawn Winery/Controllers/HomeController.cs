using Dawn_Winery.Data;
using Dawn_Winery.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;

namespace Dawn_Winery.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
         
        private readonly AppDbContext _appDbContext;
        private readonly Prolog.Prolog _prolog;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _appDbContext = context;
            _prolog = new Prolog.Prolog();
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            string[] hids = _appDbContext.RawMaterial.OrderByDescending(rm => rm.Quality).Select(rm => rm.Hid).ToArray();

            string[] stocks = _appDbContext.RawMaterial.OrderBy(rm => rm.Quality).Select(rm => rm.Stock.ToString("0.0", CultureInfo.InvariantCulture)).ToArray();
            string names =  hids[0];
            for (int i = 1; i < 5; i++)
            {
                names = names + ", " +  hids[i];
            }

            string ton =  stocks[0];
            for (int i = 1; i < 5; i++)
            {
                ton = ton + ", " + stocks[i];
            }

             Prolog.Prolog prologInstance = new Prolog.Prolog();

            string[] a;
            float[] b;
            int c;
            int d;
            var res = prologInstance.make_recipe("prosecco, gewurztraminer, malvasia, viognier, chardonnay, pinot_gris, semillon, chenin_blanc, vermentino, airen, cortese, garganega, riesling, macabeo, sauvignon_blanc, trebbiano, pinot_grigio, glera, moscato", "4.3, 5.0, 5.0, 6.0, 5.0, 5.0, 5.0, 5.0, 5.0, 5.0, 6.0, 5.0, 5.0, 5.0, 5.0, 5.0, 6.0, 5.0, 0.6");
            a = res.Item1;
            b = res.Item2;
            c = res.Item3;
            d = res.Item4;
         //   var result = prologInstance.color_predict(a[0] +","+ a[1], "0.2, 1.3");

            ViewData["Message"] = "Prolog Sonucu: " + a[0] +"__"+ b[0] + "__"+ a[1] + "__" + b[1] + "__" + c + "__" + d + "__" ;
            return View();
 
         }
        public IActionResult Menu()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult Receipes()
        {
            var datar = _appDbContext.Receipe.ToList();
            return View(datar);
        }

        public IActionResult Stocks()
        {
            return View();
        }
        [HttpPost]
        public IActionResult UpdateGrapeStock(string hid, int quantityToAdd)
        {
            var grape = _appDbContext.RawMaterial.FirstOrDefault(r => r.Hid == hid);

            if (grape != null)
            {
                // Mevcut stok miktarını artır
                grape.Stock += quantityToAdd;

                // Veritabanında değişiklikleri kaydet
                _appDbContext.SaveChanges();

                // Güncellenmiş stok miktarını JSON olarak döndür
                return Json(new { updatedStock = grape.Stock });
            }

            // Diğer işlemler ve yönlendirmeler buraya eklenebilir
            return RedirectToAction("Hammadde"); // Üzüm listesini gösteren bir sayfaya yönlendirme
        }

        [AllowAnonymous]
        public IActionResult Hammadde()
        {
            var datah = _appDbContext.RawMaterial.OrderBy(r => r.Hname).ToList();
            return View(datah);
        }
        [HttpPost]
        public IActionResult ProduceWine(string receipeId, int bottleCount)
        {
            var receipe = _appDbContext.Receipe.Find(receipeId);

            if (receipe != null)
            {
                // Şarap ismine göre mevcut şarapları kontrol et

                // Şarap üretildiğinde kullanılan üzümlerin stok değerlerini güncelleme
                var usedGrapes = new List<string> { receipe.Grape1, receipe.Grape2, receipe.Grape3, receipe.Grape4, receipe.Grape5, receipe.Grape6 };
                float reductionRate = 1;

                 foreach (var grapeName in usedGrapes)
                {
                    var grape = _appDbContext.RawMaterial.FirstOrDefault(g => g.Hid == grapeName);

                    if (grape != null)
                    {                                            
                        
                        if (grapeName == receipe.Grape1)
                        {
                            reductionRate = receipe.G1Kilo; 
                        }
                        else if (grapeName == receipe.Grape2)
                        {
                            reductionRate = receipe.G2Kilo ?? 0; 
                        }
                        else if (grapeName == receipe.Grape3)
                        {
                            reductionRate = receipe.G3Kilo ?? 0;
                        }
                        else if (grapeName == receipe.Grape4)
                        {
                            reductionRate = receipe.G4Kilo ?? 0;
                        }
                        else if (grapeName == receipe.Grape5)
                        {
                            reductionRate = receipe.G5Kilo ?? 0;
                        }
                        else if (grapeName == receipe.Grape6)
                        {
                            reductionRate = receipe.G6Kilo ?? 0;
                        }
                         
                        float requiredStock = bottleCount * reductionRate; // Üretim için gereken toplam miktar

                        if (grape.Stock < requiredStock)
                        {
                            return StatusCode(500, "Internal Server Error");
                        }
                        grape.Stock -= requiredStock;
                    }
                }
                var existingWine = _appDbContext.EndProduct.FirstOrDefault(w => w.Mname == receipe.Rname);

                if (existingWine != null)
                {
                    // Eğer varsa, mevcut şarap nesnesinin Bottle (şişe) değerini arttır
                    existingWine.Bottle += 1200; // veya istediğiniz miktarı ekleyin
                }

                _appDbContext.SaveChanges();

            }

            return RedirectToAction("Uretim");
        }

        [HttpPost]
        public ActionResult GenerateRecipes(string wineName, string color)
        {
            int colorp;
            string names = "";
            string ton = "";
            if(color == "Red")
            {
                string[] hids = _appDbContext.RawMaterial.Where(rm => rm.Type == false && rm.Stock > 0).OrderByDescending(rm => rm.Quality).Select(rm => rm.Hid).ToArray();

                string[] stocks = _appDbContext.RawMaterial.Where(rm => rm.Type == false && rm.Stock > 0).OrderBy(rm => rm.Quality).Select(rm => rm.Stock.ToString("0.0", CultureInfo.InvariantCulture)).ToArray();
                 names = hids[0];
                for (int i = 1; i < hids.Length; i++)
                {
                    names = names + ", " + hids[i];
                }

                 ton = stocks[0];
                for (int i = 1; i < stocks.Length; i++)
                {
                    ton = ton + ", " + stocks[i];
                }
                Prolog.Prolog prologInstance = new Prolog.Prolog();

                string[] a;
                float[] b;
                int c;
                int d;
                var res = prologInstance.make_recipe(names, ton);
                a = res.Item1;
                b = res.Item2;
                c = res.Item3;
                d = res.Item4;
                var result = prologInstance.aging(a, b);
                colorp = prologInstance.color_predict(a, b);
                //  ViewData["Message"] = "Prolog Sonucu: " + a[0] + "__" + b[0] + "__" + a[1] + "__" + b[1] + "__" + c + "__" + d + "__" + result;
                _appDbContext.Receipe.AddRange(new List<Receipe>()
                    { new Receipe()
                    {
                    Type = false,
                    SO2 = 150,
                    Rname = wineName,
                    Grape1 = a[0],
                    G1Kilo = b[0],
                    Grape2 = a.Length > 1 ? a[1] : null,  // a[1] null değilse, a[1]; null ise null
                    G2Kilo = b.Length > 1 ? b[1] : null,  // b[1] null değilse, b[1]; null ise null
                    Grape3 = a.Length > 2 ? a[2] : null,  // a[1] null değilse, a[1]; null ise null
                    G3Kilo = b.Length > 2 ? b[2] : null,  // b[1] null değilse, b[1]; null ise null
                    Grape4 = a.Length > 3 ? a[3] : null,  // a[1] null değilse, a[1]; null ise null
                    G4Kilo = b.Length > 3 ? b[3] : null,  // b[1] null değilse, b[1]; null ise null
                    Grape5 = a.Length > 4 ? a[4] : null,  // a[1] null değilse, a[1]; null ise null
                    G5Kilo = b.Length > 4 ? b[4] : null,  // b[1] null değilse, b[1]; null ise null
                    Grape6 = a.Length > 5 ? a[5] : null,  // a[1] null değilse, a[1]; null ise null
                    G6Kilo = b.Length > 5 ? b[5] : null,  // b[1] null değilse, b[1]; null ise null
                    Color = colorp
                    }
                    });

                _appDbContext.EndProduct.AddRange(new List<EndProduct>()
                    { new EndProduct()
                    {
                        Mname = wineName,
                        Year = 2024,
                        Aging = result,
                        Quality = c,
                        Type = false,
                        Milil = 750,
                        Bottle = 0,
                        Stock = d
                    }
                    });

                _appDbContext.SaveChanges();
            } else
            {
                string[] hids = _appDbContext.RawMaterial.Where(rm => rm.Type == true && rm.Stock > 0).OrderByDescending(rm => rm.Quality).Select(rm => rm.Hid).ToArray();

                string[] stocks = _appDbContext.RawMaterial.Where(rm => rm.Type == true && rm.Stock > 0).OrderBy(rm => rm.Quality).Select(rm => rm.Stock.ToString("0.0", CultureInfo.InvariantCulture)).ToArray();
                names = hids[0];
                for (int i = 1; i < hids.Length; i++)
                {
                    names = names + ", " + hids[i];
                }

                ton = stocks[0];
                for (int i = 1; i < stocks.Length; i++)
                {
                    ton = ton + ", " + stocks[i];
                }
                Prolog.Prolog prologInstance = new Prolog.Prolog();

                string[] a;
                float[] b;
                int c;
                int d;
                var res = prologInstance.make_recipe(names, ton);
                a = res.Item1;
                b = res.Item2;
                c = res.Item3;
                d = res.Item4;
                var result = prologInstance.aging(a, b);
                colorp = prologInstance.color_predict(a, b);

                //  ViewData["Message"] = "Prolog Sonucu: " + a[0] + "__" + b[0] + "__" + a[1] + "__" + b[1] + "__" + c + "__" + d + "__" + result;

                _appDbContext.Receipe.AddRange(new List<Receipe>()
                    { new Receipe()
                    {
                    Type = true,
                    SO2 = 100,
                    Rname = wineName,
                    Grape1 = a[0],
                    G1Kilo = b[0],
                    Grape2 = a.Length > 1 ? a[1] : null,  // a[1] null değilse, a[1]; null ise null
                    G2Kilo = b.Length > 1 ? b[1] : null,  // b[1] null değilse, b[1]; null ise null
                    Grape3 = a.Length > 2 ? a[2] : null,  // a[1] null değilse, a[1]; null ise null
                    G3Kilo = b.Length > 2 ? b[2] : null,  // b[1] null değilse, b[1]; null ise null
                    Grape4 = a.Length > 3 ? a[3] : null,  // a[1] null değilse, a[1]; null ise null
                    G4Kilo = b.Length > 3 ? b[3] : null,  // b[1] null değilse, b[1]; null ise null
                    Grape5 = a.Length > 4 ? a[4] : null,  // a[1] null değilse, a[1]; null ise null
                    G5Kilo = b.Length > 4 ? b[4] : null,  // b[1] null değilse, b[1]; null ise null
                    Grape6 = a.Length > 5 ? a[5] : null,  // a[1] null değilse, a[1]; null ise null
                    G6Kilo = b.Length > 5 ? b[5] : null,  // b[1] null değilse, b[1]; null ise null
                    Color = colorp
                    }
                    });

                _appDbContext.EndProduct.AddRange(new List<EndProduct>()
                    { new EndProduct()
                    {
                        Mname = wineName,
                        Year = 2024,
                        Aging = result,
                        Quality = c,
                        Type = true,
                        Milil = 750,
                        Bottle = 0,
                        Stock = d
                    }
                    });

                _appDbContext.SaveChanges();
            }

            TempData["MyNumber"] = colorp;

            // View'e yönlendir
            return RedirectToAction("Uretim", new { parameterName = colorp });
            }


        [HttpPost]
        public ActionResult GenerateBestRecipe(string wineName, string color)
        {
            int colorp;
            string names = "";
            string ton = "";
            if (color == "Red")
            {
                string[] hids = _appDbContext.RawMaterial.Where(rm => rm.Type == false && rm.Stock > 0).OrderByDescending(rm => rm.Quality).Select(rm => rm.Hid).ToArray();

                string[] stocks = _appDbContext.RawMaterial.Where(rm => rm.Type == false && rm.Stock > 0).OrderBy(rm => rm.Quality).Select(rm => rm.Stock.ToString("0.0", CultureInfo.InvariantCulture)).ToArray();
                names = hids[0];
                for (int i = 1; i < hids.Length; i++)
                {
                    names = names + ", " + hids[i];
                }

                ton = stocks[0];
                for (int i = 1; i < stocks.Length; i++)
                {
                    ton = ton + ", " + stocks[i];
                }
                Prolog.Prolog prologInstance = new Prolog.Prolog();

                string[] a;
                float[] b;
                int c;
                int d;
                var res = prologInstance.make_recipe_best(names, ton);
                a = res.Item1;
                b = res.Item2;
                c = res.Item3;
                d = res.Item4;
                var result = prologInstance.aging(a, b);
                colorp = prologInstance.color_predict(a, b);
                //  ViewData["Message"] = "Prolog Sonucu: " + a[0] + "__" + b[0] + "__" + a[1] + "__" + b[1] + "__" + c + "__" + d + "__" + result;
                _appDbContext.Receipe.AddRange(new List<Receipe>()
                    { new Receipe()
                    {
                    Type = false,
                    SO2 = 150,
                    Rname = wineName,
                    Grape1 = a[0],
                    G1Kilo = b[0],
                    Grape2 = a.Length > 1 ? a[1] : null,  // a[1] null değilse, a[1]; null ise null
                    G2Kilo = b.Length > 1 ? b[1] : null,  // b[1] null değilse, b[1]; null ise null
                    Grape3 = a.Length > 2 ? a[2] : null,  // a[1] null değilse, a[1]; null ise null
                    G3Kilo = b.Length > 2 ? b[2] : null,  // b[1] null değilse, b[1]; null ise null
                    Grape4 = a.Length > 3 ? a[3] : null,  // a[1] null değilse, a[1]; null ise null
                    G4Kilo = b.Length > 3 ? b[3] : null,  // b[1] null değilse, b[1]; null ise null
                    Grape5 = a.Length > 4 ? a[4] : null,  // a[1] null değilse, a[1]; null ise null
                    G5Kilo = b.Length > 4 ? b[4] : null,  // b[1] null değilse, b[1]; null ise null
                    Grape6 = a.Length > 5 ? a[5] : null,  // a[1] null değilse, a[1]; null ise null
                    G6Kilo = b.Length > 5 ? b[5] : null,  // b[1] null değilse, b[1]; null ise null
                    Color = colorp
                    }
                    });            

                _appDbContext.SaveChanges();
                _appDbContext.EndProduct.AddRange(new List<EndProduct>()
                    { new EndProduct()
                    {
                        Mname = wineName,
                        Year = 2024,
                        Aging = result,
                        Quality = c,
                        Type = false,
                        Milil = 750,
                        Bottle = 0,
                        Stock = d
                    }
                    });

                _appDbContext.SaveChanges();
            }
            else
            {
                string[] hids = _appDbContext.RawMaterial.Where(rm => rm.Type == true && rm.Stock > 0).OrderByDescending(rm => rm.Quality).Select(rm => rm.Hid).ToArray();

                string[] stocks = _appDbContext.RawMaterial.Where(rm => rm.Type == true && rm.Stock > 0).OrderBy(rm => rm.Quality).Select(rm => rm.Stock.ToString("0.0", CultureInfo.InvariantCulture)).ToArray();
                names = hids[0];
                for (int i = 1; i < hids.Length; i++)
                {
                    names = names + ", " + hids[i];
                }

                ton = stocks[0];
                for (int i = 1; i < stocks.Length; i++)
                {
                    ton = ton + ", " + stocks[i];
                }
                Prolog.Prolog prologInstance = new Prolog.Prolog();

                string[] a;
                float[] b;
                int c;
                int d;
                var res = prologInstance.make_recipe_best(names, ton);
                a = res.Item1;
                b = res.Item2;
                c = res.Item3;
                d = res.Item4;
                var result = prologInstance.aging(a, b);
                colorp = prologInstance.color_predict(a, b);

                //  ViewData["Message"] = "Prolog Sonucu: " + a[0] + "__" + b[0] + "__" + a[1] + "__" + b[1] + "__" + c + "__" + d + "__" + result;

                _appDbContext.Receipe.AddRange(new List<Receipe>()
                    { new Receipe()
                    {
                    Type = true,
                    SO2 = 100,
                    Rname = wineName,
                    Grape1 = a[0],
                    G1Kilo = b[0],
                    Grape2 = a.Length > 1 ? a[1] : null,  // a[1] null değilse, a[1]; null ise null
                    G2Kilo = b.Length > 1 ? b[1] : null,  // b[1] null değilse, b[1]; null ise null
                    Grape3 = a.Length > 2 ? a[2] : null,  // a[1] null değilse, a[1]; null ise null
                    G3Kilo = b.Length > 2 ? b[2] : null,  // b[1] null değilse, b[1]; null ise null
                    Grape4 = a.Length > 3 ? a[3] : null,  // a[1] null değilse, a[1]; null ise null
                    G4Kilo = b.Length > 3 ? b[3] : null,  // b[1] null değilse, b[1]; null ise null
                    Grape5 = a.Length > 4 ? a[4] : null,  // a[1] null değilse, a[1]; null ise null
                    G5Kilo = b.Length > 4 ? b[4] : null,  // b[1] null değilse, b[1]; null ise null
                    Grape6 = a.Length > 5 ? a[5] : null,  // a[1] null değilse, a[1]; null ise null
                    G6Kilo = b.Length > 5 ? b[5] : null,  // b[1] null değilse, b[1]; null ise null
                    Color = colorp
                    }
                    });

                _appDbContext.SaveChanges();
                _appDbContext.EndProduct.AddRange(new List<EndProduct>()
                    { new EndProduct()
                    {
                        Mname = wineName,
                        Year = 2024,
                        Aging = result,
                        Quality = c,
                        Type = true,
                        Milil = 750,
                        Bottle = 0,
                        Stock = d
                    }
                    });
                _appDbContext.SaveChanges();
            }

            TempData["MyNumber"] = colorp;

            // View'e yönlendir
            return RedirectToAction("Uretim", new { parameterName = colorp });
        }

        [HttpPost]
        public JsonResult GetColor(string primaryKey)
        {
            
                // Veritabanından color değerini çek
                var item = _appDbContext.Receipe.FirstOrDefault(x => x.Rname == primaryKey);

                if (item != null)
                { 
                    // Başarılı ise JSON formatında cevap gönder
                    return Json(new { success = true, color = item.Color, type = item.Type });
                }
                else
                {
                    // Başarısız ise JSON formatında hata mesajı gönder
                    return Json(new { success = false, message = "Veri bulunamadı" });
                }
            
        }

      
        [AllowAnonymous]
        public IActionResult Uretim(int parameterName)
        {
            int uretimVerisi = parameterName;

            // ViewBag veya ViewData ile veriyi görünüme aktarın
            ViewBag.UretimVerisi = uretimVerisi;
            var datau = _appDbContext.Receipe.ToList();
            return View(datau);
        }
        [AllowAnonymous]
        public IActionResult Mamul()
        {
            var datam = _appDbContext.EndProduct.ToList();
            return View(datam);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}