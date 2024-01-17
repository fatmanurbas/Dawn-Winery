using Dawn_Winery.Data;
using Dawn_Winery.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Dawn_Winery.Controllers
{
    public class HomeController : Controller
    {

        private readonly AppDbContext _appDbContext;

        public HomeController( AppDbContext context)
        {
            _appDbContext = context;
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
                int reductionRate = 1;

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
                        int requiredStock = bottleCount * reductionRate; // Üretim için gereken toplam miktar

                        if (grape.Stock < requiredStock)
                        {
                            return StatusCode(500, "Internal Server Error");
                        }
                        grape.Stock -= bottleCount * reductionRate;
                    }
                }
                var existingWine = _appDbContext.EndProduct.FirstOrDefault(w => w.Mname == receipe.Rname);

                if (existingWine != null)
                {
                    // Eğer varsa, mevcut şarap nesnesinin Bottle (şişe) değerini arttır
                    existingWine.Bottle += bottleCount; // veya istediğiniz miktarı ekleyin
                }

                _appDbContext.SaveChanges();

            }

            return RedirectToAction("Uretim");
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