using Dawn_Winery.Data;
using Dawn_Winery.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Globalization;

namespace Dawn_Winery.Controllers
{
    public class HomeController : Controller
    {

        private readonly AppDbContext _appDbContext;
        private readonly Prolog.Prolog _prolog;

        public HomeController( AppDbContext context)
        {
            _appDbContext = context;
            _prolog = new Prolog.Prolog();

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
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
        public ActionResult GenerateRecipes(string wineName)
        {
            // Veritabanından üzüm bilgilerini çek
            string[] hids = _appDbContext.RawMaterial.Select(rm => rm.Hid).ToArray();
            string[] stocks = _appDbContext.RawMaterial.Select(rm => rm.Stock.ToString()).ToArray();
            string names = string.Join(",", hids[0]);
            for(int i = 1; i< 6; i++)
            {
                 names = names + "," + string.Join(",", hids[i]);
            }
             
            string[] stringNumbers = new string[6];
            string ton = string.Join(",", stocks[0]);
            for (int i = 1; i < 6; i++)
            {
                ton = ton + "," + string.Join(",", stocks[i]);
             }

            // Diziyi virgülle ayırarak birleştir
 
            var result = _prolog.make_recipes(names, ton);

             

            if (result != null)
            {
                // Çözümü çöz ve Receipe nesnelerini oluştur
                for (int i = 0; i < result.Item1.Length; i++)
                {
                    var receipe = new Receipe
                    {
                        Rname = "AAA",
                        Type = false,
                        Grape1 = result.Item1[i][0],
                        G1Kilo = result.Item2[i][0],
                        Grape2 = result.Item1[i][1],
                        G2Kilo = result.Item2[i][1],
                        Grape3 = result.Item1[i][2],
                        G3Kilo = result.Item2[i][2],
                        Grape4 = result.Item1[i][3],
                        G4Kilo = result.Item2[i][3],
                        Grape5 = result.Item1[i][4],
                        G5Kilo = result.Item2[i][4],
                        Grape6 = result.Item1[i][5],
                        G6Kilo = result.Item2[i][5],
                         SO2 = result.Item3[i]
                    };

                    // Oluşturulan Receipe nesnesini veritabanına ekleyin
                    _appDbContext.Receipe.Add(receipe);
                }

                // Değişiklikleri kaydet
                _appDbContext.SaveChanges();
            }
            // View'e yönlendir
            return View("Uretim");
        }

        [AllowAnonymous]
        public IActionResult Uretim()
        {
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