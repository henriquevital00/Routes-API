using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Routes.Models.Dto
{
    public class RouteDTO
    {
        public string From { get; set; } = null!;
        public string To { get; set; } = null!;
        public int Value { get; set; }
    }
}
