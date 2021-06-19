﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeedbackPortal.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new FeedbackPortalContext(
            serviceProvider.GetRequiredService<
            DbContextOptions<FeedbackPortalContext>>()))
            {
                // Look for any movies.
                if (context.Product.Any() || context.Client.Any() || context.Employee.Any() || context.Feedback.Any())
                {
                    return; // DB has been seeded
                }
                context.Employee.AddRange(
                new Employee { /*Id = 1, */Name = "Rob Reiner", Number = "256-398", Email = "robreiner@fp.com", Rating = 0 },
                new Employee { /*Id = 2, */Name = "Ivan Reitman", Number = "789-632", Email = "ivanreitman@fp.com", Rating = 0 },
                new Employee { /*Id = 3, */Name = "Howard Hawks", Number = "125-874", Email = "howardhawks@fp.com", Rating = 0 }
                );
                context.SaveChanges();
                context.Client.AddRange(
                new Client { /*Id = 1, */Name = "Billy Crystal", Number = "125-489", Email = "billycrystal@fp.com" },
                new Client { /*Id = 2, */Name = "Meg Ryan", Number = "985-126", Email = "megryan@fp.com" },
                new Client { /*Id = 3, */Name = "Carrie Fisher", Number = "358-745", Email = "carriefisher@fp.com" },
                new Client { /*Id = 4, */Name = "Bill Murray", Number = "964-158", Email = "billmurray@fp.com" },
                new Client { /*Id = 5, */Name = "Dan Aykroyd", Number = "258-631", Email = "danaykroyd@fp.com" },
                new Client { /*Id = 6, */Name = "Sigourney Weaver", Number = "369-758", Email = "sigourneyweaver@fp.com" }
                );
                context.SaveChanges();
                context.Product.AddRange(
                new Product
                {
                    //Id = 1,
                    Name = "E-Banking",
                    Type = "WEB",
                    EmployeeId = context.Employee.Single(e => e.Name == "Rob Reiner").Id
                },
                new Product
                {
                    //Id = 2,
                    Name = "E-Banking",
                    Type = "Mobile",
                    EmployeeId = context.Employee.Single(e => e.Name == "Rob Reiner").Id
                },
                new Product
                {
                    //Id = 3,
                    Name = "YNAB (You Need A Budget)",
                    Type = "Desktop",
                    EmployeeId = context.Employee.Single(e => e.Name == "Ivan Reitman").Id
                },
                new Product
                {
                    //Id = 4,
                    Name = "PocketGuard",
                    Type = "Mobile",
                    EmployeeId = context.Employee.Single(e => e.Name == "Howard Hawks").Id
                },
                new Product
                {
                    //Id = 5,
                    Name = "Truebill",
                    Type = "WEB",
                    EmployeeId = context.Employee.Single(e => e.Name == "Ivan Reitman").Id
                },
                new Product
                {
                    //Id = 6,
                    Name = "Truebill",
                    Type = "Mobile",
                    EmployeeId = context.Employee.Single(e => e.Name == "Ivan Reitman").Id
                }
                );
                context.SaveChanges();
                context.Feedback.AddRange
                (
                new Feedback { Title = "First Feedback", Details = "Details about the first feedback", Type ="Complaint", ClientId = 1, ProductId = 1 },
                new Feedback { Title = "Second Feedback", Details = "Details about the second feedback", Type = "Compliment", ClientId = 2, ProductId = 2 },
                new Feedback { Title = "Third Feedback", Details = "Details about the third feedback", Type = "Sugestion", ClientId = 3, ProductId = 3 }
                );
                context.SaveChanges();
            }
        }
    }
}