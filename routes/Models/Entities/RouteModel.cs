using System;
using System.Collections.Generic;

namespace Routes.Models.Entities
{
    public partial class RouteModel
    {
        public int Id { get; set; }
        public string From { get; set; } = null!;
        public string To { get; set; } = null!;
        public int Value { get; set; }
    }
}
