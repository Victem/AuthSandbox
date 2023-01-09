﻿using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Identity.App.Helpers
{
    public class FormValueRequiredAttribute : ActionMethodSelectorAttribute
    {
        private readonly string _name;
        private static readonly IEnumerable<string> _httMethods = new[] { "GET", "HEAD", "DELETE", "TRACE" };
        public FormValueRequiredAttribute(string name)
        {
            _name = name;
        }

        public override bool IsValidForRequest(RouteContext context, ActionDescriptor action)
        {
            //if (string.Equals(context.HttpContext.Request.Method, "GET", StringComparison.OrdinalIgnoreCase) ||
            //    string.Equals(context.HttpContext.Request.Method, "HEAD", StringComparison.OrdinalIgnoreCase) ||
            //    string.Equals(context.HttpContext.Request.Method, "DELETE", StringComparison.OrdinalIgnoreCase) ||
            //    string.Equals(context.HttpContext.Request.Method, "TRACE", StringComparison.OrdinalIgnoreCase))
            
            if (_httMethods.Any(method => string.Equals(context.HttpContext.Request.Method, method, StringComparison.OrdinalIgnoreCase)))
            {
                return false;
            }

            if (string.IsNullOrEmpty(context.HttpContext.Request.ContentType))
            {
                return false;
            }

            if (!context.HttpContext.Request.ContentType.StartsWith("application/x-www-form-urlencoded", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return !string.IsNullOrEmpty(context.HttpContext.Request.Form[_name]);
        }
    }
}
