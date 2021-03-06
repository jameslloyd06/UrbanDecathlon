﻿using System;
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
                var loadName = TempData["loadName"] == null ? string.Empty : TempData["loadName"].ToString();
                var loadPin = TempData["loadPin"] == null ? string.Empty : TempData["loadPin"].ToString();

                if (string.IsNullOrEmpty(loadName))
                {
                    HttpCookie templateNameCookie = Request.Cookies["UDTemplateName"];
                    HttpCookie templatePinCookie = Request.Cookies["UDTemplatePin"];

                    loadName = templateNameCookie == null ? loadName : templateNameCookie.Value;
                    loadPin = templatePinCookie == null ? loadPin : templatePinCookie.Value;
                }
                
                var templateId = 1;

                if (!string.IsNullOrEmpty(loadName) && !string.IsNullOrEmpty(loadPin))
                {
                    var existingTemplate = ctx.Templates.FirstOrDefault(x => x.Name == loadName);

                    if (existingTemplate != null && existingTemplate.Password == loadPin)
                    {
                        templateId = existingTemplate.Id;

                        SetCookieDetails(loadName, loadPin);
                    }
                }

                viewModel = ctx.Templates.Where(x => x.Id == templateId).Include(x => x.Events.Select(y => y.Positions.Select(p => p.Athlete))).FirstOrDefault();
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
                var existingTemplate = new Template();

                if (template.IsNew)
                {
                    if (context.Templates.Any(x => x.Name == template.Name))
                    {
                        return Json(new { success = false, message = "That name already exists" });
                    }

                    existingTemplate.Name = template.Name;

                    existingTemplate.Password = template.Password;

                    existingTemplate.Athletes = new List<Athlete>();

                    existingTemplate.Events = new List<Event>();

                    context.Templates.Add(existingTemplate);                    
                }
                else
                {
                    existingTemplate = context.Templates.FirstOrDefault(x => x.Name == template.Name);

                    if (existingTemplate == null)
                    {
                        return Json(new { success = false, message = "Something crazy has gone wrong. Call Bill Gates" });
                    }

                    if (existingTemplate.Password != template.Password)
                    {
                        return Json(new { success = false, message = "Wrong pin! Unnnnluckkkyy" });
                    }
                }

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

                context.SaveChanges();                

                SetCookieDetails(existingTemplate.Name, existingTemplate.Password);
            }

            return Json(new { success = true });
        }
        
        private void SetCookieDetails(string name, string pin)
        {
            HttpCookie templateNameCookie = Request.Cookies["UDTemplateName"] ?? new HttpCookie("UDTemplateName");
            HttpCookie templatePinCookie = Request.Cookies["UDTemplatePin"] ?? new HttpCookie("UDTemplatePin");

            templateNameCookie.Value = name;
            templateNameCookie.Expires = DateTime.Now.AddYears(50);
            
            templatePinCookie.Value = pin;
            templatePinCookie.Expires = DateTime.Now.AddYears(50);

            Response.Cookies.Add(templateNameCookie);
            Response.Cookies.Add(templatePinCookie);
        }

        public ActionResult Load(string name, string password)
        {
            TempData["loadName"] = name;
            TempData["loadPin"] = password;

            return RedirectToAction("Index");
        }
    }
}