using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            var viewModel = new Template();

            using (var ctx = new UrbanDecathlonContext())
            {
                var defaultTemplate = ctx.Templates.FirstOrDefault(x => x.Id == 1);

                viewModel = ctx.Templates.Where(x => x.Id == 1).Include(x => x.Events.Select(y => y.Positions.Select(p => p.Athlete))).FirstOrDefault();

                

                foreach (var viewModelEvent in defaultTemplate.Events)
                {
                    viewModelEvent.Positions = viewModelEvent.Positions.ToList();
                }
            }
            
            return View(viewModel);
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
                if (template.Id > 0)
                {
                    var existingTemplate = context.Templates.FirstOrDefault(x => x.Id == template.Id);

                    // Remove existing athletes and events
                    foreach (var athlete in existingTemplate.Athletes.ToList())
                    {
                        context.Athletes.Remove(athlete);
                    }

                    foreach (var existingEvent in existingTemplate.Events.ToList())
                    {
                        foreach (var existingPosition in existingEvent.Positions.ToList())
                        {
                            context.Positions.Remove(existingPosition);
                        }

                        context.Events.Remove(existingEvent);
                    }

                    context.SaveChanges();

                    // Add new athletes
                    foreach (var athlete in template.Athletes)
                    {
                        existingTemplate.Athletes.Add(athlete);
                    }
                    
                    context.SaveChanges();

                    foreach (var newEvent in template.Events)
                    {
                        foreach (var position in newEvent.Positions)
                        {
                            position.Athlete = template.Athletes.FirstOrDefault(x => x.Name == position.Athlete.Name);
                        }

                        existingTemplate.Events.Add(newEvent);
                    }
                    
                    context.SaveChanges();                 

                    context.Entry(existingTemplate).State = EntityState.Modified;
                }
                else
                {
                    context.Templates.Add(template);
                }                
                                
                context.SaveChanges();
            }

                

            

            return Json(new { success = true });
        }
    }
}