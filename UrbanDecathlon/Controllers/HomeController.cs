using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UrbanDecathlon.DataAccess;
using UrbanDecathlon.Models;

namespace UrbanDecathlon.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Template defaultTemplate;

            using (var ctx = new UrbanDecathlonContext())
            {
                defaultTemplate = ctx.Templates.FirstOrDefault(x => x.Id == 1);
                
                foreach (var currentEvent in defaultTemplate.Events)
                {
                    for (int i = 0; i < defaultTemplate.Athletes.Count; i++)
                    {
                        currentEvent.Positions.Add(new Position
                        {
                            Athlete = defaultTemplate.Athletes[i],
                            Order = i + 1
                        });
                    }                    
                }           
            }
            
            return View(defaultTemplate);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public JsonResult Save(Template template)
        {
            using (var context = new UrbanDecathlonContext())
            {
                var templateToSave = new Template();

                if (template.Id > 0)
                {
                    templateToSave = context.Templates.FirstOrDefault(x => x.Id == template.Id);
                }

                //context.Templates.Add(template);

                //context.SaveChanges();

            }

                

            

            return Json(new { success = true });
        }
    }
}