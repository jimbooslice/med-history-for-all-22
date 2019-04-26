using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Vision.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
 using Google.Cloud.Translation.V2;


namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
           // string es_text = @"Historia social:\nFamilia: Su estado civil es divorciado.\nEducación / Trabajo: Paciente completado\neducación hasta la licenciatura, por ejemplo,\nBA, AB, BS, BBA. Su ocupacion es\nAsistente administrativo. Ella es\nActualmente no funciona. La ultima trabajaba\nen 2009. Ella no está en discapacidad. Ella\nNo está involucrado en acciones legales para el\ndolor. Ella tiene otra legal\nproblemas.\nExperiencias Adversas de la Infancia: Ella no\nexperimentar cualquier trastorno importante antes de\nla edad de 17. Ella experimentó algunos\ntrastorno grave después de la edad de 17 años.\nNo se sintió descuidado como un niño. Ella\nno experimentó dolor crónico como una\nniño. Ella no se siente amenazada en\nsu entorno actual. Ella nunca ha\nHa sido hospitalizado psiquiátricamente.\nSustancia: ella negó fumar. Ella negó\nbebiendo alcohol. Ella negó usar\ndrogas callejeras Ella apoyó a otros piensan\nElla tiene problemas con las drogas o el alcohol.\nElla respaldó haber recibido tratamiento.\npara el uso de sustancias.\n";
            var client = ImageAnnotatorClient.Create();
            var text = "";
            var foreignText = "";
            // TextAnnotation response = null;
            // Google.Cloud.Vision.V1.Image image = null;
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                var image = Image.FromStream(reader.BaseStream);
                var response = client.DetectDocumentText(image);
                
                TranslationClient tc = TranslationClient.Create();
                var tcresp = tc.TranslateText(response.Text, "en");
                var paragraphs = tcresp.TranslatedText.Split("\n");
                
                foreach (var p in paragraphs) {
                    Console.WriteLine(p);
                }

                text = tcresp.TranslatedText;
                foreignText = response.Text;
            }

            var delim = new Output()
            {
                Text = text,
                ForeignText = foreignText
            };
            return View("Output", delim);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Output(string delimitedText)
        {
            var text = new Output()
            {
                Text = delimitedText
            };
            ViewData["text"] = delimitedText;
            return View(text);
        }
    }
}
