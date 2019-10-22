﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace HomeHunter.Infrastructure.ValidationAttributes
{

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class AllowedFileExtensionAttribute : ValidationAttribute
    {
        private List<string> AllowedExtensions { get; set; }

        public AllowedFileExtensionAttribute(string allowedExtensions)
        {
            AllowedExtensions = allowedExtensions.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        public override bool IsValid(object value)
        {
            foreach (var item in (List<IFormFile>)value)
            {
                IFormFile file = item as IFormFile;

                if (file != null)
                {
                    var fileName = file.FileName;

                    var result = AllowedExtensions.Any(y => fileName.EndsWith(y));
                    if (!result)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
