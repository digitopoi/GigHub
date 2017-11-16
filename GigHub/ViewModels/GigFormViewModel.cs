using GigHub.Controllers;
using GigHub.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace GigHub.ViewModels
{
    public class GigFormViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Venue { get; set; }

        [Required]
        [FutureDate]
        public string Date { get; set; }

        [Required]
        [ValidTime]
        public string Time { get; set; }

        [Required]
        public byte Genre { get; set; }

        public IEnumerable<Genre> Genres { get; set; }

        public string Heading { get; set; }

        public string Action
        {
            get
            {
                //  Lamda expression here represents a method 
                //  that takes c as an argument and returns an action result
                //  We can use Func(delegate) to represent that
                //  First argument = input to the anonymous method
                //  Second argument = return type
                //  I don't want to call this - just want to store it - so, use Expression
                Expression<Func<GigsController, ActionResult>> update = 
                    (c => c.Update(this));
                
                Expression<Func<GigsController, ActionResult>> create = 
                    (c => c.Create(this));

                //  select one of the expressions
                var action = (Id != 0) ? update : create;
                //  and extract it at runtime 
                return (action.Body as MethodCallExpression).Method.Name;

                //  Above replaces this (for maintainablity, eliminate 'magic strings':
                //
                //  return (Id != 0 ? "Update" : "Create");
                //
            }
        }

        public DateTime GetDateTime()
        {
            return DateTime.Parse(string.Format("{0} {1}", Date, Time));
        }
    }
}