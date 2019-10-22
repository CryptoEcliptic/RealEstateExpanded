﻿using System;
using System.Collections.Generic;
using System.Text;

namespace HomeHunter.Services.Models.Image
{
    public class ImageChangeableServiceModel
    {
        public string Url { get; set; }

        public string RealEstateId { get; set; }

        public bool IsDeleted { get; set; }

        public int Sequence { get; set; }
    }
}
